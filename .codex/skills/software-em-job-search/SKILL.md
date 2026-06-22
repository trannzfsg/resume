---
name: software-em-job-search
description: Run and maintain Tran's Australian software Engineering Manager job search across SEEK and LinkedIn. Use when Codex needs to perform the daily software EM search, update C:\github\resume job-search files, screen SEEK/LinkedIn job ads, score role fit/location/employer review, add or remove candidates from the to-be-applied queue, record applied or not-applying decisions, create per-job detail notes, or run ad-hoc job discovery for Tran.
---

# Software EM Job Search

## Core Files

Use `C:\github\resume` as the workspace.

Read these before searching or changing the queue:

- `AGENTS.md`
- `job-search/applied-jobs.md`
- `job-search/to-be-applied.md`
- today's and yesterday's `memory/YYYY-MM-DD.md` files when present
- `MEMORY.md` when this is a direct/main session for Tran
- Tran's live resume at `https://trannzfsg.github.io/resume/`

Use `job-search/job-details/` for one detail file per queued role.

## Reference

Read `references/search-and-scoring.md` before running a daily search, doing a substantial ad-hoc search, or changing screening/scoring logic. For simple "record applied" or "record not applying" requests, the core files above are enough.

## Daily Search Workflow

1. Load context from the core files and the scoring reference.
2. Search SEEK and LinkedIn for software engineering leadership roles relevant to Brisbane, fully remote Australia/APAC, or 100% remote global/US/EU roles with a strong Engineering Manager title match. Also open the signed-in LinkedIn Jobs home page at `https://www.linkedin.com/jobs/` when browser/session access is available and review the personalised job sections such as top picks, recommended jobs, still hiring, and similar-to-viewed jobs. Capture direct LinkedIn IDs from relevant cards because these recommendations can surface profile-matched roles that public board search misses. If only the logged-out generic jobs page is accessible, note the caveat and continue with public search.
3. Open the full ad before scoring whenever possible. If the full ad is inaccessible, use public snippets only for conservative skip/review decisions.
4. Apply hard excludes from `job-search/applied-jobs.md` before scoring. Do not add hard-excluded roles to the queue with low scores.
5. Score surviving roles separately for `Fit`, `Location/work`, and `Employer review`.
6. Add suitable roles to `job-search/to-be-applied.md`, preserving the existing table format.
7. Create or update the matching `job-search/job-details/<platform-id-company-title>.md` file for each queued role.
8. Keep `job-search/to-be-applied.md` sorted by effective date descending, then fit score descending.
9. Run `node C:\Users\USER\.codex\skills\software-em-job-search\scripts\check_queue_order.js C:\github\resume\job-search\to-be-applied.md`.
10. Update today's memory with what changed, including added roles, skipped hard-exclude patterns, and any rule refinements.

## Recording Decisions

When Tran says a job was applied:

1. Add it under `Applied` in `job-search/applied-jobs.md`.
2. Remove it from `job-search/to-be-applied.md`.
3. Add a short note to today's memory.
4. Run the queue-order check.

When Tran says not applying:

1. Add it under `Do Not Show` with the real reason.
2. Remove it from `job-search/to-be-applied.md`.
3. If the reason is reusable, update the Screening Lessons in `applied-jobs.md`.
4. Add a short note to today's memory.
5. Run the queue-order check.

## Output Style

For daily runs, report only high-signal outcomes:

- New apply-first candidates
- Review candidates
- Important hard excludes or changed assumptions
- Any inaccessible-source caveat
- Verification result

Do not draft full cover letters by default. Provide cover-letter source notes only when useful, and use `seek-cover-letter` if the user asks for application notes or a finished letter.
