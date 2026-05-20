import { execFileSync } from "node:child_process";
import { existsSync, mkdtempSync, rmSync, writeFileSync } from "node:fs";
import { readFile } from "node:fs/promises";
import { tmpdir } from "node:os";
import path from "node:path";
import { fileURLToPath, pathToFileURL } from "node:url";
import { marked } from "marked";

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const sourcePath = path.join(root, "tranzha.md");
const htmlPath = path.join(root, "index.html");
const pdfPath = path.join(root, "tranzha.pdf");

const markdown = await readFile(sourcePath, "utf8");
const firstHeading = markdown.match(/^#\s+(.+)$/m)?.[1]?.trim() ?? "Tran Zha";
const description =
  markdown.match(/^##\s+Professional Summary\s+([\s\S]*?)(?:^##\s+)/m)?.[1]
    ?.replace(/\s+/g, " ")
    .trim() ?? "Tran Zha resume";

marked.use({
  gfm: true,
  breaks: false,
});

const content = marked.parse(markdown);
const html = renderPage({ title: firstHeading, description, content });

writeFileSync(htmlPath, html, "utf8");
writePdf(htmlPath, pdfPath);

console.log(`Built ${path.relative(root, htmlPath)} and ${path.relative(root, pdfPath)} from ${path.relative(root, sourcePath)}.`);

function renderPage({ title, description, content }) {
  return `<!doctype html>
<html lang="en">
  <head>
    <meta charset="utf-8">
    <meta name="viewport" content="width=device-width, initial-scale=1">
    <title>${escapeHtml(title)}</title>
    <meta name="description" content="${escapeHtml(description)}">
    <style>
      :root {
        color-scheme: light;
        --page: #f7f8f5;
        --paper: #ffffff;
        --ink: #202124;
        --muted: #5f6368;
        --line: #d8ddd2;
        --accent: #0f766e;
        --accent-soft: #e4f2ef;
      }

      * {
        box-sizing: border-box;
      }

      html {
        font-size: 16px;
      }

      body {
        margin: 0;
        background: var(--page);
        color: var(--ink);
        font-family: Arial, Helvetica, sans-serif;
        line-height: 1.55;
      }

      .page {
        max-width: 920px;
        margin: 0 auto;
        padding: 32px 20px 48px;
      }

      .resume {
        background: var(--paper);
        border: 1px solid var(--line);
        padding: 48px;
      }

      .actions {
        display: flex;
        justify-content: flex-end;
        margin-bottom: 14px;
      }

      .download {
        color: #ffffff;
        background: var(--accent);
        border-radius: 6px;
        display: inline-block;
        font-size: 0.92rem;
        font-weight: 700;
        padding: 9px 13px;
        text-decoration: none;
      }

      h1,
      h2,
      h3 {
        line-height: 1.18;
        margin: 0;
      }

      h1 {
        border-bottom: 3px solid var(--accent);
        font-size: 2.15rem;
        padding-bottom: 12px;
      }

      h2 {
        border-top: 1px solid var(--line);
        color: var(--accent);
        font-size: 1.2rem;
        margin-top: 30px;
        padding-top: 18px;
      }

      h3 {
        font-size: 1.02rem;
        margin-top: 20px;
      }

      p {
        margin: 10px 0 0;
      }

      h1 + p {
        color: var(--muted);
        font-size: 0.98rem;
      }

      ul {
        margin: 10px 0 0;
        padding-left: 1.2rem;
      }

      li {
        margin: 6px 0;
      }

      strong {
        color: var(--ink);
      }

      a {
        color: var(--accent);
      }

      @media (max-width: 700px) {
        .page {
          padding: 18px 12px 32px;
        }

        .resume {
          padding: 28px 20px;
        }

        h1 {
          font-size: 1.75rem;
        }
      }

      @media print {
        body {
          background: #ffffff;
        }

        .page {
          max-width: none;
          padding: 0;
        }

        .actions {
          display: none;
        }

        .resume {
          border: 0;
          padding: 0;
        }

        h2 {
          break-after: avoid;
          page-break-after: avoid;
        }

        h3,
        li {
          break-inside: avoid;
          page-break-inside: avoid;
        }
      }

      @page {
        margin: 14mm;
        size: A4;
      }
    </style>
  </head>
  <body>
    <main class="page">
      <div class="actions">
        <a class="download" href="tranzha.pdf">Download PDF</a>
      </div>
      <article class="resume">
${content
  .trim()
  .split("\n")
  .map((line) => `        ${line}`)
  .join("\n")}
      </article>
    </main>
  </body>
</html>
`;
}

function writePdf(htmlPath, pdfPath) {
  const browserPath = findBrowser();
  const profileDir = mkdtempSync(path.join(tmpdir(), "resume-pdf-"));

  try {
    execFileSync(
      browserPath,
      [
        "--headless=new",
        "--disable-gpu",
        "--no-sandbox",
        "--no-pdf-header-footer",
        `--user-data-dir=${profileDir}`,
        `--print-to-pdf=${pdfPath}`,
        pathToFileURL(htmlPath).href,
      ],
      { stdio: "pipe" },
    );
  } finally {
    rmSync(profileDir, { recursive: true, force: true });
  }
}

function findBrowser() {
  const candidates = [
    process.env.CHROME_PATH,
    "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe",
    "/Applications/Google Chrome.app/Contents/MacOS/Google Chrome",
    "/Applications/Microsoft Edge.app/Contents/MacOS/Microsoft Edge",
    "/usr/bin/google-chrome",
    "/usr/bin/chromium",
    "/usr/bin/chromium-browser",
  ].filter(Boolean);

  for (const candidate of candidates) {
    if (existsSync(candidate)) {
      return candidate;
    }
  }

  throw new Error("No Chrome or Edge executable found. Set CHROME_PATH to generate the PDF.");
}

function escapeHtml(value) {
  return value
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}
