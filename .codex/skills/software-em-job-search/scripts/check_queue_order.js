#!/usr/bin/env node

const fs = require("fs");
const path = require("path");

const queuePath = process.argv[2] || path.join("C:\\", "github", "resume", "job-search", "to-be-applied.md");
const fix = process.argv.includes("--fix");

const text = fs.readFileSync(queuePath, "utf8");
const lines = text.split(/\r?\n/);

const headerIndex = lines.findIndex((line) =>
  line.startsWith("| Status | Date posted | Date found | Company and title |")
);

if (headerIndex < 0 || !lines[headerIndex + 1] || !lines[headerIndex + 1].startsWith("|---")) {
  console.error("Could not find queue table header.");
  process.exit(1);
}

let endIndex = headerIndex + 2;
while (endIndex < lines.length && lines[endIndex].startsWith("|")) {
  endIndex += 1;
}

const rows = lines.slice(headerIndex + 2, endIndex).filter((line) => line.trim());

function parseRow(row, index) {
  const cells = row.split("|").map((cell) => cell.trim());
  const status = cells[1] || "";
  const datePosted = cells[2] || "";
  const dateFound = cells[3] || "";
  const effectiveDate = /^\d{4}-\d{2}-\d{2}$/.test(datePosted) ? datePosted : dateFound;
  const fitMatch = (cells[6] || "").match(/Fit\s+([0-9.]+)\/10/);
  const statusRank = status.includes("[ ] To apply") ? 0 : status.includes("[ ] Review") ? 1 : 2;
  return {
    row,
    index,
    status,
    statusRank,
    effectiveDate,
    fit: fitMatch ? Number(fitMatch[1]) : -1,
  };
}

const parsed = rows.map(parseRow);
const sorted = [...parsed].sort((a, b) => {
  if (a.statusRank !== b.statusRank) return a.statusRank - b.statusRank;
  if (a.effectiveDate !== b.effectiveDate) return b.effectiveDate.localeCompare(a.effectiveDate);
  if (a.fit !== b.fit) return b.fit - a.fit;
  return a.index - b.index;
});

const mismatches = parsed.filter((item, index) => item.row !== sorted[index].row);

if (mismatches.length === 0) {
  console.log(`SORT_OK rows=${rows.length}`);
  process.exit(0);
}

if (!fix) {
  console.error(`SORT_FAIL rows=${rows.length} mismatches=${mismatches.length}`);
  mismatches.slice(0, 5).forEach((item) => {
    console.error(`${item.status} ${item.effectiveDate} fit=${item.fit}: ${item.row}`);
  });
  process.exit(1);
}

const nextLines = [
  ...lines.slice(0, headerIndex + 2),
  ...sorted.map((item) => item.row),
  ...lines.slice(endIndex),
];

fs.writeFileSync(queuePath, nextLines.join("\n"), "utf8");
console.log(`SORT_FIXED rows=${rows.length}`);
