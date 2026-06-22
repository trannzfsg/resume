# Search And Scoring

## Search Scope

Primary target: software Engineering Manager and adjacent software leadership roles for Tran, prioritising Brisbane and Australia/APAC remote but also including strong 100% remote global/US/EU Engineering Manager roles.

Useful search terms:

- Engineering Manager
- Software Engineering Manager
- Software Development Manager
- Head of Engineering
- Engineering Lead
- Technical Lead
- Technical Delivery Lead
- Platform Engineering Manager
- Cloud Services Manager
- Head of Technology
- .NET delivery manager
- Engineering Manager Product Experience
- Product Engineering Manager
- Engineering Manager Australia
- Manager Solutions Engineering
- Solutions Engineering Manager
- Sales Engineering Manager

Coverage note:

- LinkedIn public search can miss Australia-listed Engineering Manager roles that are not tagged Brisbane or Remote and may not surface even with exact company/title keywords. Include at least one broad Australia LinkedIn Engineering Manager pass, use product-experience/product-engineering EM keywords, and check official company careers or roles pages surfaced by web search. Do not treat "Product Experience" in an EM title as product-owner-heavy unless the ad says product ownership dominates.
- The signed-in LinkedIn Jobs home page at `https://www.linkedin.com/jobs/` is a separate discovery source, not just a search form. During daily runs, browse personalised sections such as top picks, recommended jobs, still hiring, and similar-to-viewed jobs when browser access is available; capture direct LinkedIn IDs and screen them normally. If only the logged-out generic page is accessible, record the caveat. Do not auto-add customer-facing sales/solutions engineering roles unless they survive the same fit, location, salary and hard-exclude screening.

Good location/work arrangements:

- Brisbane or Greater Brisbane remote/hybrid
- Fully remote Australia/APAC
- Australia remote where Brisbane is clearly eligible
- 100% remote global/US/EU roles when the role title is a strong software Engineering Manager match and the ad does not require office attendance. Queue these even when LinkedIn's main location is not Brisbane/Australia, but flag time zone, tax, visa/work-rights and country/state eligibility as review risks.

Hard location exclude:

- Any non-Brisbane hybrid, onsite, office-first, regular-office-days or ambiguous-office role.
- Sydney/Melbourne listings unless fully remote from Australia/Brisbane is explicit.
- Non-Australian listings that are hybrid, onsite, office-first or require relocation/office attendance.
- "Australia remote, or Sydney office" is acceptable only when remote is clearly a real option.
- Do not hard-exclude a strong Engineering Manager role solely because the company or LinkedIn job location is outside Brisbane/Australia when the ad has a real 100% remote signal and Australia/Brisbane eligibility is still genuinely open. If eligibility is unclear, queue as `[ ] Review` with the risk called out. If the company careers page or another authoritative employer source confirms the role is US-only, approved-US-states-only, US hybrid, or otherwise not available for Australia/Brisbane remote, hard-exclude it even if LinkedIn implies remote/worldwide.

## Hard Excludes

Skip before scoring:

- Already in `Applied`, `Do Not Show`, or a clear duplicate/repost.
- Non-Brisbane role without explicit fully remote eligibility, except strong software Engineering Manager roles with a real 100% remote global/US/EU signal and unresolved Australia/Brisbane eligibility; those should be reviewed, not hard-excluded, while work-rights or country/state eligibility needs confirmation. Hard-exclude roles confirmed by the company careers page or another authoritative employer source as US-only, approved-US-states-only, US hybrid, or otherwise non-Australia-eligible remote roles.
- Civil, mining, construction, hardware, physical-product, sales-only, support-only or non-software engineering.
- Roles where more than 50% of the work is hands-on development in a language/ecosystem Tran has little or no professional experience with. Hard-exclude heavy hands-on Elixir, Ruby, functional programming, JavaScript/TypeScript/Node/React/Vue, or Java backend roles unless the unfamiliar stack is clearly optional and the role is primarily leadership, .NET/object-oriented architecture, or DevOps/platform delivery.
- Product-owner-heavy roles where Product strategy, discovery, pricing, packaging or commercial validation dominates.
- Head of Data, BI, data warehouse, data lake or analytics-leadership roles unless software/platform engineering is dominant.
- Salary clearly below Tran's practical AUD 200k baseline unless there is a strong reason to review.

## Fit Score

Score highest when the role includes:

- People leadership for software developers, QA, BAs or platform/cloud engineers.
- Delivery ownership, technical direction, architecture, quality, DevOps or operational health.
- C#/.NET, APIs, SQL, AWS/cloud, CI/CD, observability or modern full-stack product engineering.
- Platform modernisation, migration, CRM/product systems, integrations, reporting or regulated data.

Reduce or cap fit when:

- The role is mostly individual contributor coding.
- The stack is mandatory JavaScript/Java without leadership/platform transferability.
- The role is hands-on in Elixir, Ruby, functional programming, JavaScript/TypeScript/Node/React/Vue, Java or another unfamiliar language ecosystem, but below the 50% hard-exclude threshold.
- Product ownership or data/BI specialisation dominates.
- It is generic project/program delivery without software engineering ownership.

## Location/Work Score

- 9-10: Brisbane remote, fully remote Australia/APAC, or remote-first with Brisbane clearly eligible.
- 8-9: 100% remote global/US/EU role with strong Engineering Manager fit and no office requirement, but time zone, tax, visa/work-rights or country/state eligibility may need checking.
- 7.5-8.5: Brisbane hybrid with manageable cadence.
- 6-7: Brisbane office-heavy, one WFH day, unclear cadence, or a commute/cadence concern.
- 0/exclude: non-Brisbane hybrid, onsite, office-first, regular office days or remote ambiguity.

## Employer Review Score

Score the actual employer, not only the recruiter.

- Use SEEK employer profile, LinkedIn signals, Glassdoor-style public review snippets, company scale/stability and visible engineering culture.
- Cap confidence when the client is anonymous behind a recruiter.
- Note weak review volume, poor review scores, hidden salary, churn or unclear leadership as risks.

## Queue Thresholds

Use `[ ] To apply` for strong candidates that usually satisfy:

- Fit >= 8
- Location/work >= 8
- No hard-exclude risk
- Salary alignment or strong compensation signal

Use `[ ] Review` for viable but uncertain candidates:

- Fit around 7-8
- A survivable gap or missing detail
- Needs live re-check, salary check, office-cadence check or employer review check
- Strong 100% remote Engineering Manager roles outside Australia where title/role fit is high but country/state eligibility, working hours, tax/payroll setup or visa/work rights need confirmation. Do not queue once the company careers page or another authoritative employer source confirms eligibility as US-only, US hybrid, approved-US-states-only, or unavailable for Australia remote.

Do not queue jobs that fail hard excludes or have weak fit plus weak location/employer signals.

## Detail File Shape

For each queued role, include:

- Primary ID and any duplicate IDs
- Date posted and date found
- Last seen in daily results
- Source report or search date
- Platform and link
- Company, title, location/work arrangement, employment type
- Fit, location/work and employer review scores
- Salary info
- Comments and reasons
- Risks, gaps and questions
- Suggested CV changes when useful
- Cover-letter source notes when useful

## Queue Order

Sort `job-search/to-be-applied.md` by:

1. Effective date descending. Use `Date posted` only when it is a real `YYYY-MM-DD`; otherwise use `Date found`.
2. Fit score descending when effective dates match.

Run the queue-order script after edits.
