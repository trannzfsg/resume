import { readFile } from "node:fs/promises";
import path from "node:path";
import { fileURLToPath } from "node:url";
import { marked } from "marked";
import { PDFParse } from "pdf-parse";

const root = path.resolve(path.dirname(fileURLToPath(import.meta.url)), "..");
const markdownPath = path.join(root, "tranzha.md");
const htmlPath = path.join(root, "index.html");
const pdfPath = path.join(root, "tranzha.pdf");

const markdown = await readFile(markdownPath, "utf8");
const html = await readFile(htmlPath, "utf8");
const expectedText = textFromMarkdown(markdown);
const htmlText = normalizeText(stripTags(html));
const pdfText = await textFromPdf(pdfPath);

assertContains("HTML", htmlText, expectedText);
assertContains("PDF", pdfText, expectedText);

console.log("Verified index.html and tranzha.pdf contain the resume text from tranzha.md.");

function textFromMarkdown(markdown) {
  const tokens = marked.lexer(markdown, { gfm: true });
  const lines = [];
  collectText(tokens, lines);
  return lines.map(normalizeText).filter((line) => line.length > 0);
}

function collectText(tokens, lines) {
  for (const token of tokens) {
    if (["heading", "paragraph", "list_item", "text"].includes(token.type) && typeof token.text === "string") {
      lines.push(stripMarkdownSyntax(token.text));
      continue;
    }

    if (Array.isArray(token.items)) {
      collectText(token.items, lines);
      continue;
    }

    if (Array.isArray(token.tokens)) {
      collectText(token.tokens, lines);
    }
  }
}

function stripMarkdownSyntax(value) {
  return value
    .replace(/\*\*/g, "")
    .replace(/\*/g, "")
    .replace(/\[([^\]]+)\]\([^)]+\)/g, "$1")
    .replace(/`([^`]+)`/g, "$1");
}

function stripTags(value) {
  return decodeHtmlEntities(value
    .replace(/<style[\s\S]*?<\/style>/gi, " ")
    .replace(/<script[\s\S]*?<\/script>/gi, " ")
    .replace(/<[^>]+>/g, " ")
    .replace(/&nbsp;/g, " ")
    .replace(/&amp;/g, "&")
    .replace(/&lt;/g, "<")
    .replace(/&gt;/g, ">")
    .replace(/&quot;/g, '"'));
}

function decodeHtmlEntities(value) {
  return value.replace(/&#(\d+);/g, (_, code) => String.fromCharCode(Number(code)));
}

async function textFromPdf(pdfPath) {
  const parser = new PDFParse({ data: await readFile(pdfPath) });
  try {
    const result = await parser.getText();
    return normalizeText(result.text.replace(/--\s+\d+\s+of\s+\d+\s+--/g, " "));
  } finally {
    await parser.destroy();
  }
}

function assertContains(target, actualText, expectedLines) {
  const missing = expectedLines.filter((line) => !actualText.includes(line));

  if (missing.length > 0) {
    throw new Error(`${target} is missing resume text from tranzha.md:\n- ${missing.slice(0, 8).join("\n- ")}`);
  }
}

function normalizeText(value) {
  return value.replace(/(\S)-\r?\n\s*/g, "$1-").replace(/\s+/g, " ").trim();
}
