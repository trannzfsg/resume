# AGENTS.md

Repository-specific instructions for work in `C:\github\resume`.

## Safety and Scope

- Protect personal and private data; never expose it in public repository files.
- Work freely within this workspace, but ask before destructive actions or actions outside the workspace.
- Preserve unrelated user changes in the working tree.

## Session Context and Memory

- At the start of a direct session, read today's and yesterday's files under `memory/` when present, plus `MEMORY.md`.
- Record significant decisions, preferences and outcomes in the relevant daily memory file or `MEMORY.md`.
- When the user asks to remember something, write it down rather than relying on conversation context.
- Before debugging a recurring problem, check `ERROR_PATTERNS.md` when it exists.

## Durable Repository Guidance

- Put reusable project rules in `AGENTS.md`, tool-specific notes in `TOOLS.md`, and workflow guidance in the relevant skill.
- Prefer logic or authoritative data checks over hardcoded magic strings.

## Public To-Do List

- Keep `TO-DO.md` current for confirmed, completed or cancelled work.
- Because this repository is public, use generic and privacy-safe wording in `TO-DO.md`.
- Never include employer or recruiter names, specific job titles, job-board IDs, job-ad links, application or interview dates/stages, or individual role outcomes.
- Describe applications, role removals, closures, interview preparation and outcomes generically.
- Keep role-specific details only in appropriate non-public tracking or memory files, and sanitise exposed details encountered during future edits.

## Document Output Verification

- Keep render intermediates under `tmp/`: use `tmp/docx-render/` for conversion artifacts and `tmp/pdfs/` for PDF page renders. Do not create root-level render directories.
- When producing PDF and Word/DOCX outputs, visually verify only the final PDF. Render and inspect every PDF page with the bundled workspace runtime's `pypdfium2`.
- Do not render, open or visually inspect Word/DOCX output. This repository rule overrides generic document-skill DOCX verification instructions.

## Execution

- Continue implementation without confirmation unless blocked or additional authority is required; work on independent tasks if one path is blocked.
- Do not stop while a safe, in-scope path remains.
- For resume or website work, verify `https://trannzfsg.github.io/resume/` and any relevant published assets before declaring completion.
