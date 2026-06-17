---
name: tailor-cv-for-job
description: Tailor Tran's main resume for a specific SEEK or LinkedIn job while staying truthful to the source resume. Use when Codex needs to analyse a job ad, compare it with C:\github\resume\tranzha.md, identify missing keywords or unsupported requirements, ask for clarification on material gaps, and create a separate job-specific Markdown resume such as tranzha-linkedin4425586749.md without overwriting the main resume or generating PDF/DOCX unless explicitly requested.
---

# Tailor CV For Job

## Overview

Create a job-specific Markdown resume variant from `C:\github\resume\tranzha.md`. Keep the resume accurate, improve keyword and evidence alignment with the job ad, and preserve `tranzha.md` as the reusable source resume.

## Inputs

Accept any of these job inputs:

- LinkedIn job URL or id, for example `https://www.linkedin.com/jobs/view/4425586749/` or `linkedin:4425586749`.
- SEEK job URL or id, for example `https://au.seek.com/job/92751565` or `seek:92751565`.
- A local job detail file under `C:\github\resume\job-search\job-details\`.
- Pasted job ad text.

If the ad cannot be accessed and no local detail file exists, ask the user to paste the job description before tailoring the resume.

## Required Reads

Before editing:

- Read `C:\github\resume\tranzha.md` as the source resume.
- Read the job ad or the matching detail file in `job-search/job-details\`.
- Read `job-search\to-be-applied.md` when the input is a queued job id, because it may contain location, salary, fit and risk context.
- Check `https://trannzfsg.github.io/resume/` before confirming a resume task is complete, per repo instructions.

## Workflow

1. Extract the job requirements:
   - Role title, company, platform id, location, work arrangement and contract/permanent status.
   - Hard requirements, preferred skills, technology keywords, leadership expectations, domain context and repeated wording.
   - The real role shape, especially whether it is people management, technical delivery, solution architecture, hands-on coding, project delivery, vendor management or stakeholder-facing work.

2. Compare the job with `tranzha.md`:
   - Mark each important requirement as `strong evidence`, `adjacent evidence`, `weak evidence`, or `unsupported`.
   - Capture missing keywords where the underlying experience exists but the wording is not prominent enough.
   - Capture material gaps where the ad appears to require something not supported by the resume.

3. Decide whether to ask before editing:
   - Ask the user to confirm only when a requirement is both important to the ad and unsupported by the resume, such as a must-have certification, clearance, named platform, domain, framework, daily coding language, or commercial responsibility.
   - Do not ask for confirmation for wording changes that are already supported by the resume.
   - Do not invent experience, dates, employers, certifications, titles, clearances, tools, team sizes or delivery outcomes.

4. Create a separate Markdown resume:
   - Never overwrite `C:\github\resume\tranzha.md`.
   - Default filename format is `tranzha-<platform><id>.md`, for example `tranzha-linkedin4425586749.md`.
   - If no platform id exists, use `tranzha-<company-title-slug>.md`.
   - Keep the file in `C:\github\resume` unless the user asks for another location.
   - Do not generate PDF, DOCX, HTML or Word output unless the user explicitly asks for that format.

5. Tailor conservatively:
   - Rewrite the headline and professional summary to match the role shape.
   - Reorder and reword Key Skills so job-critical keywords appear naturally near the top.
   - Emphasise matching experience in the most relevant roles; reduce emphasis on less relevant sections without deleting useful seniority evidence.
   - Bridge title mismatch plainly when useful, for example Engineering Manager to Technical Delivery Manager, by focusing on the technical delivery work behind the title.
   - Prefer concrete resume evidence over generic claims.
   - Use Australian English and Tran's direct, practical style.

6. Validate the tailored resume:
   - Confirm the new file exists and `tranzha.md` is unchanged.
   - Check for unsupported claims introduced during editing.
   - Check the tailored resume still reads as a coherent senior software engineering profile, not a keyword dump.

## Tailoring Rules

For technical delivery, solution architecture or platform roles:

- Emphasise .NET, C#, APIs, SQL, enterprise CRM/product platforms, solution design, service boundaries, integration patterns, CI/CD, environments, observability, release planning, data migration, vendor/stakeholder management and delivery risk.
- Use MyCRM Contact v2, Engagement model, Strangler Fig migration, CQRS, event-driven/event-sourcing patterns, backward-compatible projections, AWS/Kubernetes, CI/CD and acquisition/data migration evidence where relevant.
- Reduce broad people-management language when the ad does not ask for much people management, but keep enough leadership evidence to support cross-functional technical delivery.

For Engineering Manager or people leadership roles:

- Emphasise team coaching, 1:1s, hiring support, performance conversations, delivery ownership, roadmap tradeoffs, cross-team dependency management and stakeholder communication.
- Keep architecture and hands-on technical evidence as support for technical judgement, not as the whole story.

For product, data, cloud or AI-heavy roles:

- Emphasise only the parts that are true in the source resume.
- If the ad implies deep ownership that the source resume does not support, ask before finalising.

## Final Response

Keep the user-facing response short. Include:

- The new resume filename.
- A concise summary of what changed.
- Any gaps or clarification questions that remain.
- A note that `tranzha.md` was not overwritten.
