const fs = require("fs");
const path = require("path");
const { spawnSync } = require("child_process");

const root = path.resolve(__dirname, "..");
const mdPath = path.join(root, "tranzha.md");
const htmlPath = path.join(root, "index.html");
const pdfPath = path.join(root, "tranzha.pdf");

const css = `:root{color-scheme:light;--page:#f7f8f5;--paper:#fff;--ink:#202124;--muted:#5f6368;--line:#d8ddd2;--accent:#0f766e}*{box-sizing:border-box}html{font-size:16px}body{margin:0;background:var(--page);color:var(--ink);font-family:Arial,Helvetica,sans-serif;line-height:1.55}.page{max-width:920px;margin:0 auto;padding:32px 20px 48px}.resume{background:var(--paper);border:1px solid var(--line);padding:48px}.actions{display:flex;justify-content:flex-end;margin-bottom:14px}.download{color:#fff;background:var(--accent);border-radius:6px;display:inline-block;font-size:.92rem;font-weight:700;padding:9px 13px;text-decoration:none}h1,h2,h3{line-height:1.18;margin:0}h1{border-bottom:3px solid var(--accent);font-size:2.15rem;padding-bottom:12px}h2{border-top:1px solid var(--line);color:var(--accent);font-size:1.2rem;margin-top:30px;padding-top:18px}h3{font-size:1.02rem;margin-top:20px}p{margin:10px 0 0}h1+p{color:var(--muted);font-size:.98rem}ul{margin:10px 0 0;padding-left:1.2rem}li{margin:6px 0}strong{color:var(--ink)}a{color:var(--accent)}@media(max-width:700px){.page{padding:18px 12px 32px}.resume{padding:28px 20px}h1{font-size:1.75rem}}@media print{body{background:#fff}.page{max-width:none;padding:0}.actions{display:none}.resume{border:0;padding:0}h2{break-after:avoid;page-break-after:avoid}h3,li{break-inside:avoid;page-break-inside:avoid}}@page{margin:14mm;size:A4}`;

function escapeHtml(text) {
  return text
    .replace(/&/g, "&amp;")
    .replace(/</g, "&lt;")
    .replace(/>/g, "&gt;")
    .replace(/"/g, "&quot;");
}

function inlineMarkdown(text) {
  return escapeHtml(text).replace(/\*\*([^*]+)\*\*/g, "<strong>$1</strong>");
}

function renderBlocks(markdown) {
  const blocks = [];
  let paragraph = [];
  let list = [];

  function flushParagraph() {
    if (paragraph.length) {
      blocks.push(`<p>${inlineMarkdown(paragraph.join(" ").replace(/\s+/g, " ").trim())}</p>`);
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
    if (line.startsWith("### ")) {
      flushParagraph();
      blocks.push(`<h3>${inlineMarkdown(line.slice(4))}</h3>`);
    } else if (line.startsWith("## ")) {
      flushParagraph();
      blocks.push(`<h2>${inlineMarkdown(line.slice(3))}</h2>`);
    } else if (line.startsWith("# ")) {
      flushParagraph();
      blocks.push(`<h1>${inlineMarkdown(line.slice(2))}</h1>`);
    } else {
      paragraph.push(line.replace(/\s{2,}$/, ""));
    }
  }

  flushParagraph();
  flushList();
  return blocks.join("\n");
}

function buildHtml() {
  const markdown = fs.readFileSync(mdPath, "utf8");
  const body = renderBlocks(markdown);
  const html = `<!doctype html>
<html lang="en">
<head>
<meta charset="utf-8">
<meta name="viewport" content="width=device-width, initial-scale=1">
<title>Tran Zha</title>
<meta name="description" content="Tran Zha resume">
<style>
${css}
</style>
</head>
<body><main class="page"><div class="actions"><a class="download" href="tranzha.pdf">Download PDF</a></div><article class="resume">${body}</article></main></body>
</html>
`;
  fs.writeFileSync(htmlPath, html);
}

async function buildPdf() {
  const fileUrl = `file://${htmlPath.replace(/\\/g, "/")}`;
  try {
    const { chromium } = require("playwright");
    const browser = await chromium.launch({ headless: true });
    const page = await browser.newPage();
    await page.goto(fileUrl, { waitUntil: "networkidle" });
    await page.pdf({
      path: pdfPath,
      format: "A4",
      printBackground: true,
      margin: { top: "14mm", right: "14mm", bottom: "14mm", left: "14mm" },
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
  const result = spawnSync(chrome, [
    "--headless",
    "--disable-gpu",
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
