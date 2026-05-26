from pathlib import Path

from docx import Document
from docx.enum.section import WD_SECTION
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Inches, Pt, RGBColor


OUT = Path(r"C:\github\resume\tranzha.docx")


BLUE = RGBColor(46, 116, 181)
CHARCOAL = RGBColor(34, 34, 34)
MUTED = RGBColor(85, 85, 85)


def set_spacing(paragraph, before=0, after=6, line=1.1):
    fmt = paragraph.paragraph_format
    fmt.space_before = Pt(before)
    fmt.space_after = Pt(after)
    fmt.line_spacing = line


def set_run(run, size=10.5, bold=False, color=CHARCOAL):
    run.font.name = "Calibri"
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Calibri")
    run.font.size = Pt(size)
    run.font.bold = bold
    run.font.color.rgb = color


def add_border(paragraph, color="D9E2F3", size="6"):
    p_pr = paragraph._p.get_or_add_pPr()
    borders = p_pr.find(qn("w:pBdr"))
    if borders is None:
        borders = OxmlElement("w:pBdr")
        p_pr.append(borders)
    bottom = OxmlElement("w:bottom")
    bottom.set(qn("w:val"), "single")
    bottom.set(qn("w:sz"), size)
    bottom.set(qn("w:space"), "6")
    bottom.set(qn("w:color"), color)
    borders.append(bottom)


def add_section(doc, title):
    p = doc.add_paragraph(style="Heading 1")
    set_spacing(p, before=10, after=5, line=1.0)
    run = p.add_run(title.upper())
    set_run(run, size=11.5, bold=True, color=BLUE)
    add_border(p)
    return p


def add_body_paragraph(doc, text, after=6):
    p = doc.add_paragraph()
    set_spacing(p, after=after)
    set_run(p.add_run(text), size=10.5)
    return p


def add_bullet(doc, text, bold_prefix=None):
    p = doc.add_paragraph(style="List Bullet")
    p.paragraph_format.left_indent = Inches(0.25)
    p.paragraph_format.first_line_indent = Inches(-0.25)
    p.paragraph_format.space_after = Pt(3)
    p.paragraph_format.line_spacing = 1.08
    if bold_prefix and text.startswith(bold_prefix):
        set_run(p.add_run(bold_prefix), size=10, bold=True)
        set_run(p.add_run(text[len(bold_prefix):]), size=10)
    else:
        set_run(p.add_run(text), size=10)
    return p


def add_role(doc, title, dates):
    p = doc.add_paragraph(style="Heading 2")
    set_spacing(p, before=7, after=1, line=1.0)
    set_run(p.add_run(title), size=10.5, bold=True, color=CHARCOAL)
    p2 = doc.add_paragraph()
    set_spacing(p2, before=0, after=4, line=1.0)
    set_run(p2.add_run(dates), size=9.5, bold=True, color=MUTED)


def add_project(doc, title):
    p = doc.add_paragraph(style="Heading 3")
    set_spacing(p, before=3, after=2, line=1.0)
    set_run(p.add_run(title), size=10, bold=True, color=CHARCOAL)


def set_core_properties(doc):
    props = doc.core_properties
    props.author = "Tran Zha"
    props.title = "Tran Zha CV"
    props.subject = "Engineering Manager, Platform Modernisation, Technical Leadership"
    props.keywords = "Engineering Manager, Technical Project Manager, Platform Modernisation, Data Migration, Integration, AWS, Kubernetes, Salesforce"
    props.comments = "Generated from trannzfsg.github.io/resume"


def build():
    doc = Document()
    section = doc.sections[0]
    section.start_type = WD_SECTION.NEW_PAGE
    section.page_width = Inches(8.5)
    section.page_height = Inches(11)
    section.top_margin = Inches(0.55)
    section.bottom_margin = Inches(0.55)
    section.left_margin = Inches(0.65)
    section.right_margin = Inches(0.65)
    section.header_distance = Inches(0.3)
    section.footer_distance = Inches(0.3)

    styles = doc.styles
    styles["Normal"].font.name = "Calibri"
    styles["Normal"].font.size = Pt(10.5)
    styles["Normal"].font.color.rgb = CHARCOAL
    styles["Title"].font.name = "Calibri"
    styles["Title"].font.size = Pt(22)
    styles["Title"].font.bold = True
    styles["Title"].font.color.rgb = RGBColor(31, 77, 120)
    styles["Heading 1"].font.name = "Calibri"
    styles["Heading 1"].font.size = Pt(11.5)
    styles["Heading 1"].font.bold = True
    styles["Heading 1"].font.color.rgb = BLUE
    styles["Heading 2"].font.name = "Calibri"
    styles["Heading 2"].font.size = Pt(10.5)
    styles["Heading 2"].font.bold = True
    styles["Heading 2"].font.color.rgb = CHARCOAL
    styles["Heading 3"].font.name = "Calibri"
    styles["Heading 3"].font.size = Pt(10)
    styles["Heading 3"].font.bold = True
    styles["Heading 3"].font.color.rgb = CHARCOAL
    styles["List Bullet"].font.name = "Calibri"
    styles["List Bullet"].font.size = Pt(10)

    name = doc.add_paragraph(style="Title")
    name.alignment = WD_ALIGN_PARAGRAPH.CENTER
    set_spacing(name, after=1, line=1.0)
    set_run(name.add_run("Tran Zha"), size=22, bold=True, color=RGBColor(31, 77, 120))

    subtitle = doc.add_paragraph()
    subtitle.alignment = WD_ALIGN_PARAGRAPH.CENTER
    set_spacing(subtitle, after=2, line=1.0)
    set_run(
        subtitle.add_run(
            "Engineering Manager | Platform Modernisation | Technical Leadership | Brisbane, Australia | Australian permanent resident"
        ),
        size=10.5,
        bold=True,
        color=CHARCOAL,
    )

    contact = doc.add_paragraph()
    contact.alignment = WD_ALIGN_PARAGRAPH.CENTER
    set_spacing(contact, after=7, line=1.0)
    set_run(
        contact.add_run(
            "info@tranzha.com | 0402 798 180 | linkedin.com/in/tranzha | github.com/trannzfsg | trannzfsg.github.io/resume"
        ),
        size=9.5,
        color=MUTED,
    )

    add_section(doc, "Professional Summary")
    add_body_paragraph(
        doc,
        "Engineering Manager with 15+ years of experience building software products, leading teams and modernising complex platforms. I help teams make sense of messy legacy systems, turn unclear problems into practical delivery plans, and keep business-critical work moving without losing sight of quality. Strong in technical judgement, stakeholder alignment, team coaching, delivery planning and long-term platform improvement.",
        after=4,
    )
    add_body_paragraph(
        doc,
        "I also utilise various LLM tools in both work and personal projects to speed up research, prototyping, documentation and analysis, while keeping human review and engineering judgement at the centre.",
        after=4,
    )

    add_section(doc, "Leadership & Technical Strengths")
    strengths = [
        ("Engineering leadership:", " Team coaching, delivery ownership, expectation-setting, hiring support, career development and performance conversations."),
        ("Platform modernisation:", " Strangler Fig migration, monolith decomposition, backward compatibility, facade/service boundaries and legacy risk reduction."),
        ("Architecture judgement:", " REST, CQRS, event-driven design, HTTP/gRPC, clean architecture, VSA, domain modelling and integration patterns."),
        ("Product & stakeholder alignment:", " Discovery, roadmap trade-offs, executive communication and cross-team dependency management."),
        ("Complex product domains:", " CRM platforms including contacts, ownership, authorisation, reporting, migrations, workflow-heavy systems and PII data handling."),
        ("AI-assisted engineering:", " Uses LLMs for discovery, prototyping, test ideas, documentation, technical analysis and rapid product experimentation."),
    ]
    for prefix, rest in strengths:
        add_bullet(doc, prefix + rest, bold_prefix=prefix)

    add_section(doc, "Professional Experience")
    add_role(doc, "LMG / Loan Market Group - Engineering Manager, MyCRM Replatforming", "Feb 2023 - Present")
    add_project(doc, "Contact Replatform - centralising contact mutation and retrieval | Jun 2025 - Present")
    for item in [
        "Leading the highest-complexity MyCRM replatform initiative: contacts are entangled across more than half of the monolith's business logic, with historical edge cases and patches spread across screens, workflows and services.",
        "Building Contact v2 with backward compatibility and future-facing architecture, while untangling fragmented product logic into centralised mutation/retrieval paths and clearer ownership/auth boundaries.",
        "Using a Strangler Fig approach: facade over the existing data store first, gradual migration of legacy consumers, then replacement of the underlying data model once behaviours are controlled.",
        "Positioning Contact as a reference architecture for future MyCRM work, applying CQRS, domain and integration events, gRPC internal communication, clean architecture, Vertical Slice Architecture and stronger service boundaries.",
        "Partnering with product, architecture and frontend teams to introduce a new Material UI-based design system while preserving legacy interoperability during migration.",
    ]:
        add_bullet(doc, item)

    add_project(doc, "Diversified Deals Replatform - generic engagement model for financial deals | Feb 2023 - Jun 2025")
    for item in [
        "Led replatforming of residential-focused deal capability into a generic Engagement model that can handle multiple product types to support business growth.",
        "Used the Engagement platform to launch asset finance, commercial and insurance deal journeys, expanding MyCRM beyond residential lending without duplicating core deal logic.",
        "Drove domain modelling, transition planning, roadmap alignment and stakeholder trade-offs across product, engineering and business groups.",
        "Reset expectations and delivery discipline in a struggling team, improving clarity, technical direction and execution focus across a long-running replatform program.",
    ]:
        add_bullet(doc, item)

    add_role(doc, "LMG / Loan Market Group - Engineering Manager, Commission System / Business Insights / Podium Data Migration", "Mar 2021 - Apr 2023")
    for item in [
        "Built and led teams across commissions, reporting/analytics and acquisition migration programs; shaped business cases, roadmaps, discovery and delivery plans for complex domains.",
        "Evaluated Snowflake, Starburst, ThoughtSpot, BigQuery, Sisense and related platforms; separated data lake/warehouse concerns and delivered ingestion, transformation and reporting pipelines.",
        "Led migration work after acquisitions, including one-off and overnight delta pipelines using SSIS, TaskFactory, Salesforce, Kubernetes jobs and AWS services; supported onshore growth from 10 to 30+ engineers.",
    ]:
        add_bullet(doc, item)

    add_role(doc, "NZFSG - Senior DevOps Engineer / Senior Engineer, MyCRM Launch and Cloud Modernisation", "Apr 2016 - Jun 2019")
    for item in [
        "Helped rebuild, launch and modernise MyCRM across Australia/New Zealand; migrated from Windows/IIS/on-premise to AWS, CI/CD and improved operational observability; coordinated security audit activity and supported offshore expansion from 0 to 20+ engineers and QA staff.",
        "Managed cloud spend, software licensing and vendor cost inputs for the MyCRM platform, balancing operational needs with budget and delivery priorities.",
        "Supported production operations, incident response and service reliability for business-critical CRM systems, using monitoring and support feedback to drive continuous improvement.",
    ]:
        add_bullet(doc, item)

    add_role(doc, "LMG / Loan Market Group - Engineering Manager, MyCRM Platform Modernisation", "Jul 2019 - Apr 2021")
    for item in [
        "Introduced OpenTelemetry, contextual logging, GitHub Actions and Octopus, improved local development workflows, and led containerisation and Kubernetes/EKS migrations.",
    ]:
        add_bullet(doc, item)

    add_role(doc, "LifeDirect / Inform Holdings / Trade Me - Software Engineer", "Jul 2008 - Mar 2016")
    add_bullet(
        doc,
        "Full-stack engineering across online insurance, internal admin, reporting, CRM and hosting operations; built quoting, application and underwriting workflows and mentored team members as the function grew.",
    )

    add_section(doc, "Technical Toolkit")
    add_body_paragraph(
        doc,
        "AWS, GCP, EKS/Kubernetes, Docker, GitHub Actions, Octopus, OpenTelemetry, Elasticsearch, Snowflake, BigQuery, ThoughtSpot, Firebase, Supabase, Serverless, C#/.NET, SQL Server, PostgreSQL, DDD, REST, CQRS, HTTP/gRPC, event-driven architecture, data lake/warehouse, ETL and data migration pipelines",
        after=4,
    )

    add_section(doc, "AI/LLM-Assisted Personal Projects")
    projects = [
        ("mqcyb.tranzha.com - Math Quiz / mqcyb:", " Flutter learning app previously released on the App Store; used AI assistance for rapid feature iteration, UI refinement and content generation support."),
        ("transtags.tranzha.com - Bank transaction categorisation and tax returns:", " React/Node.js product experiment for importing bank transactions, categorising spending and preparing tax-return exports; explored Supabase before pausing the project."),
        ("sports.tranzha.com - Community sports registration and mini CRM:", " Next.js/Node.js/Firebase app for registrations, participants and lightweight community operations management."),
        ("Roblox Autorun Obby:", " Roblox game/prototype using AI-assisted scripting, level iteration and gameplay experimentation."),
    ]
    for prefix, rest in projects:
        add_bullet(doc, prefix + rest, bold_prefix=prefix)

    add_section(doc, "Education, Coursework & Languages")
    add_body_paragraph(
        doc,
        "Postgraduate Diploma in Science, Computer Science - University of Auckland | Bachelor of Science, Computer Science - University of Auckland | Bachelor of Commerce, Finance and Economics - University of Auckland",
        after=3,
    )
    add_body_paragraph(
        doc,
        "AWS Solutions Architect Associate coursework completed (A Cloud Guru); Kubernetes Administrator coursework completed (Udemy); Machine Learning and Deep Learning coursework completed (Coursera/deeplearning.ai); Certified Scrum Master | English, Mandarin | References available on request",
        after=0,
    )

    set_core_properties(doc)
    doc.save(OUT)


if __name__ == "__main__":
    build()
