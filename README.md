# resume

This is an LLM-friendly repo for maintaining Tran Zha's resume, generated CV files and job-search notes.

## How to Use

Use natural language with Codex or another LLM to do the normal resume work:

- Tidy or rewrite `tranzha.md` while keeping the facts accurate.
- Regenerate the web resume, PDF and Word CV after edits.
- Tailor the CV or cover-letter notes for a specific job ad.
- Run job-search workflows and update private job notes.
- Capture useful context in local memory so future sessions understand preferences.

## Main Files

- `tranzha.md` - source resume text.
- `index.html` - generated web resume for GitHub Pages.
- `tranzha.pdf` - generated PDF resume.
- `tranzha.docx` - generated Word CV.

## Regenerate Outputs

```powershell
node tools/build_web_resume.js --pdf
python tools/build_word_cv.py
python tools/docx_smoke_check.py
```

After regenerating, visually check the PDF/Word output before publishing.

## Privacy

Job-search files and memories are intentionally ignored by git for privacy. Keep `memory/`, `job-search/`, `MEMORY*`, `INTERVIEW*` and render scratch files local unless there is a deliberate reason to share them.
