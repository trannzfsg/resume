const fs = require("fs");
const path = require("path");
const { spawnSync } = require("child_process");

const root = path.resolve(__dirname, "..");
const args = process.argv.slice(2);

function argValue(name, fallback) {
  const index = args.indexOf(name);
  if (index === -1) {
    return fallback;
  }
  const value = args[index + 1];
  if (!value || value.startsWith("--")) {
    throw new Error(`${name} requires a value`);
  }
  return value;
}

function resolveOutput(value) {
  return path.isAbsolute(value) ? value : path.join(root, value);
}

const mdPath = resolveOutput(argValue("--input", "tranzha.md"));
const htmlPath = resolveOutput(argValue("--html-output", "index.html"));
const pdfPath = resolveOutput(argValue("--pdf-output", "tranzha.pdf"));

const css = `:root{color-scheme:light;--page:#f7f8f5;--paper:#fff;--ink:#202124;--muted:#5f6368;--line:#d8ddd2;--accent:#0f766e}*{box-sizing:border-box}html{font-size:16px}body{margin:0;background:var(--page);color:var(--ink);font-family:Arial,Helvetica,sans-serif;line-height:1.42}.page{max-width:920px;margin:0 auto;padding:30px 20px 44px}.resume{background:var(--paper);border:1px solid var(--line);padding:44px}.actions{display:flex;justify-content:flex-end;margin-bottom:14px}.download{color:#fff;background:var(--accent);border-radius:6px;display:inline-block;font-size:.92rem;font-weight:700;padding:9px 13px;text-decoration:none}h1,h2,h3,h4{line-height:1.16;margin:0}h1{border-bottom:3px solid var(--accent);font-size:2.08rem;padding-bottom:10px}h2{border-top:1px solid var(--line);color:var(--accent);font-size:1.16rem;margin-top:20px;padding-top:11px}h3{font-size:1.02rem;margin-top:13px}h4{font-size:.98rem;margin-top:9px}p{margin:6px 0 0}.project-heading{font-weight:700}h1+p{color:var(--muted);font-size:.95rem;line-height:1.32}ul{margin:5px 0 0;padding-left:1.12rem}li{margin:3px 0}strong{color:var(--ink)}a{color:var(--accent)}@media(max-width:700px){.page{padding:18px 12px 32px}.resume{padding:28px 20px}h1{font-size:1.75rem}}@media print{html{font-size:13px}body{background:#fff;line-height:1.3}.page{max-width:none;padding:0}.actions{display:none}.resume{border:0;padding:0}h1{font-size:2rem;padding-bottom:6px}h2{font-size:1.13rem;margin-top:10px;padding-top:7px}h3{font-size:1.01rem;margin-top:8px}h4{font-size:.97rem;margin-top:6px}p{margin-top:3px}h1+p{font-size:.94rem;line-height:1.22}ul{margin-top:3px}li{margin:1.4px 0}h2,h3,h4,.date-line,.project-heading{break-after:avoid;page-break-after:avoid}h3,h4,li{break-inside:avoid;page-break-inside:avoid}}@page{margin:10mm;size:A4}`;
const defaultCss = `
:root { color-scheme: light; --page: #f3f6f5; --paper: #fff; --ink: #202b30; --muted: #4c5d64; --line: #d8e1df; --accent: #125e58; }
* { box-sizing: border-box; }
body { margin: 0; background: var(--page); color: var(--ink); font: 1rem/1.55 Arial, Helvetica, sans-serif; }
.page { max-width: 920px; margin: 0 auto; padding: 28px 20px 48px; }
.resume { background: var(--paper); border: 1px solid var(--line); padding: 44px 52px; }
.actions { display: flex; flex-wrap: wrap; justify-content: flex-end; gap: 8px; margin-bottom: 12px; }
.download { background: var(--paper); border: 1px solid var(--accent); border-radius: 5px; color: var(--accent); font-weight: 700; font-size: .9rem; padding: 8px 12px; text-decoration: none; }
.download:first-child { background: var(--accent); color: #fff; }
.download:hover { text-decoration: underline; }
h1, h2, h3, h4 { line-height: 1.2; margin: 0; }
h1 { font-size: 2.5rem; }
h2 { border-top: 1px solid var(--line); color: var(--accent); font-size: 1.18rem; margin-top: 26px; padding-top: 12px; }
h3 { font-size: 1.06rem; margin-top: 17px; }
h4 { font-size: 1rem; margin-top: 13px; }
p { margin: 7px 0 0; }
h1 + p { color: var(--muted); line-height: 1.5; overflow-wrap: anywhere; }
h1 + p strong { color: var(--ink); }
.date-line { color: var(--muted); font-size: .94rem; margin-top: 2px; }
.project-heading { font-weight: 700; }
ul { margin: 7px 0 0; padding-left: 1.18rem; }
li { margin: 4px 0; }
a { color: var(--accent); text-underline-offset: .17em; }
@media (max-width: 640px) {
  .page { padding: 14px 12px 24px; }
  .resume { padding: 24px 18px; }
  h1 { font-size: 2rem; }
}
@media print {
  html { font-size: 15px; }
  body { background: #fff; line-height: 1.4; }
  .page { max-width: none; padding: 0; }
  .resume { border: 0; padding: 0; }
  .actions { display: none; }
  h1 { font-size: 2.2rem; }
  h2 { margin-top: 16px; padding-top: 8px; }
  h3 { margin-top: 13px; }
  h4 { margin-top: 9px; }
  p, ul { margin-top: 5px; }
  li { margin: 3px 0; }
  h2, h3, h4, .date-line, .project-heading { break-after: avoid; }
  li { break-inside: avoid; }
}
@page { size: A4; margin: 16mm; }
`;
const rebuiltMode = args.includes("--rebuilt");
const selectedCss = rebuiltMode ? css : defaultCss;
const paginationCss = rebuiltMode
  ? `@media print{.stack-line{break-after:avoid;page-break-after:avoid}}`
  : "";

function escapeHtml(text) {
  return text
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function linkifyEscaped(text) {
  return text
    .replace(/\bhttps?:\/\/[^\s<]+/g, (url) => `<a href="${url}">${url}</a>`)
    .replace(/\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}\b/gi, (email) => `<a href="mailto:${email}">${email}</a>`);
}

function inlineMarkdown(text) {
  const links = /\[([^\]]+)\]\((https?:\/\/[^\s)]+)\)/g;
  const inlineText = (value) => linkifyEscaped(escapeHtml(value))
    .replace(/\*\*([^*]+)\*\*/g, "<strong>$1</strong>");
  let result = "";
  let position = 0;

  for (const match of text.matchAll(links)) {
    const [link, label, url] = match;
    result += inlineText(text.slice(position, match.index));
    result += `<a href="${escapeHtml(url)}">${escapeHtml(label)}</a>`;
    position = match.index + link.length;
  }

  return result + inlineText(text.slice(position));
}

function renderBlocks(markdown) {
  const blocks = [];
  let paragraph = [];
  let list = [];

  function flushParagraph() {
    if (paragraph.length) {
      const plain = paragraph.join(" ").replace(/\*\*/g, "").trim();
      const className = plain.startsWith("Selected initiative:")
        ? " class=\"project-heading\""
        : plain.startsWith("Stack:")
          ? " class=\"stack-line\""
          : /^(?:.+ \| )?[A-Z][a-z]{2} \d{4} to (Present|[A-Z][a-z]{2} \d{4})$/.test(plain)
            ? " class=\"date-line\""
            : "";
      const content = paragraph
        .map((line) => inlineMarkdown(line.replace(/\s+/g, " ").trim()))
        .join("<br>");
      blocks.push(`<p${className}>${content}</p>`);
      paragraph = [];
    }
  }

  function flushList() {
    if (list.length) {
      blocks.push(`<ul>\n${list.map((item) => `<li>${inlineMarkdown(item)}</li>`).join("\n")}\n</ul>`);
      list = [];
    }
  }

  for (const rawLine of markdown.split(/\r?\n/)) {
    const line = rawLine.trim();
    if (!line) {
      flushParagraph();
      flushList();
      continue;
    }

    if (line.startsWith("- ")) {
      flushParagraph();
      list.push(line.slice(2));
      continue;
    }

    flushList();
    if (line.startsWith("#### ")) {
      flushParagraph();
      blocks.push(`<h4>${inlineMarkdown(line.slice(5))}</h4>`);
    } else if (line.startsWith("### ")) {
      flushParagraph();
      blocks.push(`<h3>${inlineMarkdown(line.slice(4))}</h3>`);
    } else if (line.startsWith("## ")) {
      flushParagraph();
      blocks.push(`<h2>${inlineMarkdown(line.slice(3))}</h2>`);
    } else if (line.startsWith("# ")) {
      flushParagraph();
      blocks.push(`<h1>${inlineMarkdown(line.slice(2))}</h1>`);
    } else {
      paragraph.push(line);
    }
  }

  flushParagraph();
  flushList();
  return blocks.join("\n");
}

function buildHtml() {
  const markdown = fs.readFileSync(mdPath, "utf8");
  const body = renderBlocks(markdown);
  const downloadHref = path.relative(path.dirname(htmlPath), pdfPath).replace(/\\/g, "/");
  const wordDownload = mdPath === path.join(root, "tranzha.md") && htmlPath === path.join(root, "index.html")
    ? '<a class="download" href="tranzha.docx">Download Word</a>'
    : "";
  const html = `<!doctype html>
<html lang="en-AU">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Tran Zha | Engineering Manager</title>
<meta name="description" content="Résumé for Tran Zha, Engineering Manager with experience in software delivery and platform modernisation.">
<style>
${selectedCss}
${paginationCss}
</style>
</head>
<body><main class="page"><div class="actions"><a class="download" href="${escapeHtml(downloadHref)}">Download PDF</a>${wordDownload}</div><article class="resume">${body}</article></main></body>
</html>
`;
  fs.mkdirSync(path.dirname(htmlPath), { recursive: true });
  fs.writeFileSync(htmlPath, html);
}

async function buildPdf() {
  const fileUrl = `file://${htmlPath.replace(/\\/g, "/")}`;
  try {
    const { chromium } = require("playwright");
    const browser = await chromium.launch({ headless: true });
    const page = await browser.newPage();
    await page.goto(fileUrl, { waitUntil: "networkidle" });
    fs.mkdirSync(path.dirname(pdfPath), { recursive: true });
    await page.pdf({
      path: pdfPath,
      format: "A4",
      printBackground: true,
      margin: {
        top: rebuiltMode ? "10mm" : "16mm",
        right: rebuiltMode ? "10mm" : "16mm",
        bottom: rebuiltMode ? "10mm" : "16mm",
        left: rebuiltMode ? "10mm" : "16mm",
      },
    });
    await browser.close();
    return;
  } catch (error) {
    if (error.code !== "MODULE_NOT_FOUND") {
      throw error;
    }
  }

  const chromeCandidates = [
    "C:\\Program Files\\Google\\Chrome\\Application\\chrome.exe",
    "C:\\Program Files (x86)\\Microsoft\\Edge\\Application\\msedge.exe",
  ];
  const chrome = chromeCandidates.find((candidate) => fs.existsSync(candidate));
  if (!chrome) {
    throw new Error("Could not find Playwright or Chrome/Edge to generate PDF.");
  }
  fs.mkdirSync(path.dirname(pdfPath), { recursive: true });
  const result = spawnSync(chrome, [
    "--headless",
    "--disable-gpu",
    "--no-pdf-header-footer",
    "--print-to-pdf-no-header",
    `--print-to-pdf=${pdfPath}`,
    fileUrl,
  ], { stdio: "inherit" });
  if (result.status !== 0) {
    throw new Error(`PDF generation failed with status ${result.status}`);
  }
}

async function main() {
  buildHtml();
  if (process.argv.includes("--pdf")) {
    await buildPdf();
  }
}

main().catch((error) => {
  console.error(error);
  process.exit(1);
});
