---
name: seek-cover-letter
description: Prepare cover-letter source notes, application-question answers, role-fit bullets, and short personality-matched cover letters for Australian job applications using Tran's live resume at https://trannzfsg.github.io/resume/ and a SEEK or LinkedIn job ad URL. Use when the user asks for SEEK/LinkedIn cover letter help, application notes, key job-ad requirements, resume-to-ad match bullets, less AI-written drafting, application answers, HR-style application review, or concise role-fit judgement based on Tran's resume and a job ad.
---

# SEEK Cover Letter

## Operating Rules

Always read the live resume at `https://trannzfsg.github.io/resume/` and the given job ad before drafting or preparing notes. If the job ad cannot be accessed, ask the user to paste the ad text or key sections.

Keep the response to the user short and direct:

- If anything important is unclear, ask only the needed questions. Do not draft yet.
- By default, provide source notes the user can expand: key ad requirements, resume matches, useful angles, and gaps/questions.
- Draft a finished cover letter only when the user explicitly asks for a cover letter or final copy-paste draft.
- Include HR risk notes or CV change plans only when they materially improve the user's chance of getting an interview.
- Do not include process notes, hidden draft text, or long analysis in the final response.

## Inputs

Expect one of these:

- A SEEK URL in the form `https://au.seek.com/job/{job_id}`.
- A LinkedIn job URL in the form `https://www.linkedin.com/jobs/view/{job_id}`.
- A SEEK job ID.
- Pasted job ad text.

Use Tran's live resume as the profile source unless the user provides a different resume.

## Extract

From the resume, capture only facts that support this job:

- Current positioning: Engineering Manager, platform modernisation, technical leadership, Brisbane.
- 15+ years building software products, leading teams, and modernising complex platforms.
- Strengths: technical judgement, stakeholder alignment, coaching, delivery planning, architecture, CRM/product domains, legacy migration, cloud/platform work, data/reporting, AI-assisted engineering.
- Evidence from roles: LMG/MyCRM replatforming, Contact v2, Engagement model, commissions/reporting/data migration, AWS/Kubernetes/cloud modernisation, full-stack engineering.

From the job ad, extract:

- Role title, company, location, work arrangement, seniority, domain, and contract/permanent details.
- Sections such as `About you`, `What you'll bring`, `Responsibilities`, `Required skills`, `Experience`, and `Selection criteria`.
- Key attributes the employer appears to value, including stage, scale, domain, technical stack, leadership style, and delivery expectations.
- Any direct application questions in the ad.

## Ask First When Needed

Ask concise questions before drafting if:

- The ad asks about salary, work rights, notice period, relocation, security clearance, certifications, or availability and the resume does not answer it.
- The role requires a must-have skill or domain that is not clearly supported by the resume.
- The ad is inaccessible or missing key sections.
- The user asks for judgement that depends on preference, risk tolerance, or personal facts not in the resume.

Ask at most three questions. Make them practical and easy to answer.

## Style

Write like Tran:

- Short and concise.
- Direct and straight to the point.
- Strong personal style, but not loud.
- Keen, practical, and can-do.
- Plain words only. No fancy phrases.
- Australian English.
- Confident without exaggerating.
- Specific over generic.
- No corporate filler.
- Practical engineering management voice: people, projects, BAU, stakeholders and cross-team work all matter.
- Trade-off aware: delivery matters, but so does maintainability, system health and long-term effort.
- Candid and grounded: clear expectations, honest feedback, direct risks, real options and recommendations.
- Show leadership as useful work: coaching people, reducing friction, making decisions, aligning expectations and keeping teams moving.

Observed style from Tran's own Refactor Head of Engineering letter:

- Start warm and human, not formal. It is okay to say a role is exciting or feels unusually well matched when that is genuinely true.
- Use a career-story arc when the job calls for it: early startup engineer, small team growth, mergers/acquisitions, offshore/onshore expansion, then enterprise maturity.
- Show lived experience through plain specifics: garage/small office/remote work, being one of two engineers, becoming the only engineer, growing small teams, building 30+ offshore engineering/QA teams, launching platforms across Australia and New Zealand.
- Keep candid texture. Phrases like `Really fun times`, `learned a whole lot`, and `made plenty of mistakes along the way` sound more like Tran than polished corporate copy.
- Connect personality to role fit: likes carrying things end-to-end, broad trade-of-all-jacks ownership, solving complicated engineering problems, and helping coached teams become problem owners.
- For AI-forward roles, express genuine conviction and personal experimentation, but keep it grounded in practical engineering-team use rather than buzzwords.
- Close simply and keenly: excited by the opportunity, keen to chat, please let me know.
- Do not make every letter this long. Use the same ingredients selectively, especially for senior, founder-led, startup, scale-up, AI, or transformation roles where personal motivation matters.

Observed refinement from Tran's Arrow Software Development Lead letter:

- Keep candid gap-bridging when a requirement is adjacent rather than strongest. Example pattern: strongest hands-on background first, then honestly name the gap and ask to understand expectations.
- Use a small personal project as proof when bridging a gap. For mobile development, Tran can mention publishing a Flutter-based iOS math app for his child.
- It is okay to mention LLMs as a practical ramp-up aid, but avoid making it sound like a substitute for experience.
- Close with `Really keen to chat more, looking forward to hearing from you.` and `Best regards,` when the tone is warm and direct.

When writing leadership or Engineering Manager applications, prefer this personal management style:

- Teams: look after people; help them grow both personal capability and contribution to the project and company; align personal interests with business goals where possible; be candid, direct and honest when expectations are not being met.
- Projects: balance delivery and technical excellence; immediate business value matters, but so does maintainability; work with Product and UX to guide the project in the right direction, at the right pace, with the right trade-offs.
- BAU and continuous improvement: build processes, habits and toolsets that make it easier to keep users happy across technical, product and UX angles, while reducing long-term effort and repeated manual work.
- Stakeholders: align and adjust expectations; communicate key decisions, risks and contingency plans early; bring intentions, options and recommendations, not just problems.
- Cross-team work: collaborate on dependencies, blockers, shared work and architectural decisions; reduce friction, avoid double work and help teams move in the same direction.
- Engineering culture: make quality, ownership and learning normal, visible and expected.
- Decision making: make trade-offs explicit; separate facts from opinions; choose a direction when there is enough information, then adapt when reality changes.

For leadership letters, avoid sounding like a generic "people manager". Anchor claims in useful behaviours: coaching, expectation-setting, technical judgement, project trade-offs, operational health, stakeholder communication, dependency management and engineering standards.

To make writing less AI-written:

- Use concrete nouns from the ad and resume, not broad claims.
- Prefer one plain, specific point per sentence.
- Keep a few slightly personal, candid lines where appropriate; do not over-smooth every sentence.
- Avoid inflated adjectives, perfect symmetry, and three-item marketing rhythms unless the ad genuinely calls for them.
- Do not sound like a brochure. Sound like a practical Engineering Manager deciding whether the role is a real fit.

Avoid phrases like:

- `I am writing to express my interest`
- `esteemed organisation`
- `dynamic environment`
- `passionate about leveraging`
- `proven track record`
- `uniquely positioned`
- `synergy`

## Application Notes Shape

Use this by default, especially for daily job-search outputs or when Tran says he wants to draft the letter himself:

```text
Application notes

Key attributes sought
- [job-ad requirement]
- [job-ad requirement]

Resume matches to use
- [specific Tran evidence]
- [specific Tran evidence]

Angles Tran can expand
- [plain-language application angle]
- [stage/domain/scale transition angle if useful]

Gaps or questions
- [only material concerns]
```

For Refactor Head of Engineering-style roles, consider whether the ad is about moving from startup/product build mode toward enterprise-level platform, maturity, governance, scale, or capability. If yes, include the startup-to-enterprise transition as a key angle because Tran has relevant experience modernising and scaling business-critical platforms.

## Cover Letter Shape

Only use this section when the user explicitly asks for a finished cover letter. Default to 120-180 words unless the user asks otherwise.

Use this structure:

```text
Hi [Hiring Manager/team],

[Direct opening: role, why it fits, and current positioning.]

[Match 2-3 job requirements to resume evidence.]

[If the role is not Engineering Manager, bridge the title difference directly and explain why the experience fits.]

[Close with keen/can-do line.]

Regards,
Tran
```

Use `Hiring team` if no hiring manager name is available.

## Internal Draft-Review-Revise Workflow

Do not show the first draft to the user. Use it only as working material.

Follow this sequence:

1. Draft the initial cover letter from the resume and job ad.
2. Use a different session or subagent as a critical HR agency reviewer when available. Give it only the resume facts, the job ad facts, and the draft letter. Ask it to identify reasons the candidate may not progress to interview.
3. If a separate session or subagent is not available, run a separate internal pass with the mindset of a strict recruiter screening quickly for risk, gaps, weak evidence, over-seniority, mismatch, unclear motivation, missing keywords, work arrangement issues, and unanswered selection criteria.
4. Revise the cover letter to reduce those risks while staying truthful to the resume.
5. If the CV itself appears to weaken the application for this specific job, prepare a short CV change plan.

The HR review must look for:

- Missing must-have skills or keywords from the ad.
- Role-title mismatch, especially when applying outside `Engineering Manager`.
- Overqualified or under-hands-on signals.
- Unclear motivation for this specific role/company/domain.
- Too much management language for an individual contributor role.
- Too much technical detail for a people leadership role.
- Claims that are not backed by the resume.
- Gaps around work rights, location, salary, availability, certifications, clearance, or travel.
- Application questions that are not answered directly.

Use the review to improve the final letter. Do not make unsupported claims just to cover a gap.

## Role-Fit Rules

Tie the resume to the job ad requirements directly:

- Leadership roles: emphasise team coaching, delivery ownership, stakeholder alignment, hiring support, performance conversations, roadmap trade-offs, and delivery discipline.
- Engineering Manager roles: emphasise Tran's practical management definition: grow people, align interests with business goals, be candid, balance delivery with technical excellence, improve BAU/CI, communicate decisions/risks/options/recommendations, and reduce cross-team friction and duplicated work.
- Platform, architecture, or modernisation roles: emphasise MyCRM replatforming, Contact v2, Engagement model, Strangler Fig migration, CQRS, events, gRPC, clean architecture, service boundaries, and legacy risk reduction.
- Cloud, DevOps, or platform engineering roles: emphasise AWS, EKS/Kubernetes, Docker, CI/CD, GitHub Actions, Octopus, OpenTelemetry, logging, security audit support, and operational improvements.
- Data, reporting, or migration roles: emphasise commissions, business insights, acquisition migrations, Snowflake/BigQuery/ThoughtSpot/Sisense evaluation, ingestion, transformation, reporting, SSIS, Salesforce, and AWS jobs.
- Product or CRM roles: emphasise MyCRM, contacts, ownership, authorisation, reporting, workflow-heavy systems, PII handling, product discovery, and stakeholder alignment.
- AI-enabled roles: mention LLM use for discovery, prototyping, test ideas, documentation, technical analysis, and product experimentation, while keeping human review and engineering judgement central.

If the job title differs from `Engineering Manager`, include one plain sentence that bridges it, for example:

```text
My current title is Engineering Manager, but the fit here is the hands-on platform and delivery work behind that title: untangling legacy systems, setting technical direction, and keeping complex work moving.
```

Only use that sentence when it genuinely helps.

## Application Questions

If the ad contains questions, answer them before the cover letter under:

```text
Answers

Q: [question]
A: [short direct answer]

Cover letter

[letter]
```

Use resume evidence. If the answer needs personal confirmation, ask first and do not draft.

## Final Output

If information is missing, output questions only.

If the user asks for application notes or says they want to draft the letter themselves, output the `Application notes` shape only.

If the user explicitly asks for a finished cover letter and there are no material HR risks or CV changes, output the final revised cover letter only.

If the ad has questions, output:

- `Answers` section when the ad has questions.
- `Cover letter` section with the final revised letter.

If material HR risks remain after revision, add a short section after the letter:

```text
HR risks
- [risk and what to do about it]
```

If the CV should be changed for this application, add:

```text
CV change plan
- [specific change]
```

Keep `HR risks` and `CV change plan` to 1-3 bullets each. Do not include them if they are not useful.

Do not add notes such as `Here is your cover letter` or `You can copy and paste this`.
