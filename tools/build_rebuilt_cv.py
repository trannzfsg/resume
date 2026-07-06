import argparse
import re
from pathlib import Path

from docx import Document
from docx.enum.section import WD_SECTION
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.oxml import OxmlElement
from docx.oxml.ns import qn
from docx.shared import Inches, Pt, RGBColor


ROOT = Path(__file__).resolve().parents[1]

# compact_reference_guide with a named resume_density override. The override
# keeps a long senior career readable in 3 pages while preserving a plain,
# one-column ATS-safe layout.
TEAL = RGBColor(15, 118, 110)
NAVY = RGBColor(25, 45, 63)
CHARCOAL = RGBColor(36, 36, 36)
MUTED = RGBColor(84, 84, 84)
LINK_BLUE = RGBColor(5, 99, 193)

LINK_RE = re.compile(r"(https?://[^\s|]+|[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,})", re.I)


def set_font(run, *, size=9.5, bold=False, color=CHARCOAL, underline=False):
    run.font.name = "Calibri"
    run._element.get_or_add_rPr().get_or_add_rFonts().set(qn("w:ascii"), "Calibri")
    run._element.rPr.rFonts.set(qn("w:hAnsi"), "Calibri")
    run._element.rPr.rFonts.set(qn("w:eastAsia"), "Calibri")
    run.font.size = Pt(size)
    run.font.bold = bold
    run.font.color.rgb = color
    run.font.underline = underline


def set_spacing(paragraph, *, before=0, after=3, line=1.04):
    fmt = paragraph.paragraph_format
    fmt.space_before = Pt(before)
    fmt.space_after = Pt(after)
    fmt.line_spacing = line


def add_text_runs(paragraph, text, *, size=9.5, bold=False, color=CHARCOAL):
    position = 0
    for match in LINK_RE.finditer(text):
        if match.start() > position:
            set_font(paragraph.add_run(text[position : match.start()]), size=size, bold=bold, color=color)
        link_text = match.group(0)
        set_font(paragraph.add_run(link_text), size=size, bold=bold, color=LINK_BLUE, underline=True)
        position = match.end()
    if position < len(text):
        set_font(paragraph.add_run(text[position:]), size=size, bold=bold, color=color)


def add_markdown_runs(paragraph, text, *, size=9.5, color=CHARCOAL):
    position = 0
    for match in re.finditer(r"\*\*([^*]+)\*\*", text):
        if match.start() > position:
            add_text_runs(paragraph, text[position : match.start()], size=size, color=color)
        add_text_runs(paragraph, match.group(1), size=size, bold=True, color=color)
        position = match.end()
    if position < len(text):
        add_text_runs(paragraph, text[position:], size=size, color=color)


def parse_markdown(markdown):
    blocks = []
    paragraph = []
    bullets = []

    def flush_paragraph():
        nonlocal paragraph
        if paragraph:
            blocks.append(("p", paragraph))
            paragraph = []

    def flush_bullets():
        nonlocal bullets
        if bullets:
            blocks.append(("ul", bullets))
            bullets = []

    for raw_line in markdown.splitlines():
        line = raw_line.strip()
        if not line:
            flush_paragraph()
            flush_bullets()
            continue
        if line.startswith("- "):
            flush_paragraph()
            bullets.append(line[2:].strip())
            continue

        flush_bullets()
        heading = re.match(r"^(#{1,4})\s+(.+)$", line)
        if heading:
            flush_paragraph()
            blocks.append((f"h{len(heading.group(1))}", heading.group(2).strip()))
        else:
            paragraph.append(line)

    flush_paragraph()
    flush_bullets()
    return blocks


def configure_document(document):
    section = document.sections[0]
    section.start_type = WD_SECTION.NEW_PAGE
    section.page_width = Inches(8.27)
    section.page_height = Inches(11.69)
    section.top_margin = Inches(0.58)
    section.bottom_margin = Inches(0.58)
    section.left_margin = Inches(0.65)
    section.right_margin = Inches(0.65)
    section.header_distance = Inches(0.3)
    section.footer_distance = Inches(0.3)

    styles = document.styles
    styles["Normal"].font.name = "Calibri"
    styles["Normal"].font.size = Pt(9.5)
    styles["Normal"].font.color.rgb = CHARCOAL

    styles["Title"].font.name = "Calibri"
    styles["Title"].font.size = Pt(22)
    styles["Title"].font.bold = True
    styles["Title"].font.color.rgb = NAVY

    for name, size, color in (
        ("Heading 1", 11.5, TEAL),
        ("Heading 2", 10.8, NAVY),
        ("Heading 3", 10.1, CHARCOAL),
    ):
        styles[name].font.name = "Calibri"
        styles[name].font.size = Pt(size)
        styles[name].font.bold = True
        styles[name].font.color.rgb = color

    styles["List Bullet"].font.name = "Calibri"
    styles["List Bullet"].font.size = Pt(9.25)


def add_header_block(document, blocks, index):
    name = document.add_paragraph(style="Title")
    set_spacing(name, after=0, line=1.0)
    set_font(name.add_run(blocks[index][1]), size=22, bold=True, color=NAVY)
    index += 1

    if index >= len(blocks) or blocks[index][0] != "p":
        return index

    lines = blocks[index][1]
    if lines:
        subtitle = document.add_paragraph()
        set_spacing(subtitle, after=1, line=1.0)
        add_markdown_runs(subtitle, lines[0], size=10.4, color=CHARCOAL)
        for run in subtitle.runs:
            run.font.bold = True

    if len(lines) > 1:
        location = document.add_paragraph()
        set_spacing(location, after=1, line=1.0)
        add_markdown_runs(location, lines[1], size=9.1, color=MUTED)

    if len(lines) > 2:
        contact = document.add_paragraph()
        set_spacing(contact, after=6, line=1.0)
        add_markdown_runs(contact, " ".join(lines[2:]), size=8.7, color=MUTED)

    return index + 1


def add_section(document, text):
    paragraph = document.add_paragraph(style="Heading 1")
    set_spacing(paragraph, before=7, after=2.5, line=1.0)
    paragraph.paragraph_format.keep_with_next = True
    set_font(paragraph.add_run(text.upper()), size=11.5, bold=True, color=TEAL)


def add_employer(document, text):
    paragraph = document.add_paragraph(style="Heading 2")
    set_spacing(paragraph, before=5, after=0.5, line=1.0)
    paragraph.paragraph_format.keep_with_next = True
    add_markdown_runs(paragraph, text, size=10.8, color=NAVY)
    for run in paragraph.runs:
        run.font.bold = True


def add_role(document, text):
    paragraph = document.add_paragraph(style="Heading 3")
    set_spacing(paragraph, before=3.5, after=0, line=1.0)
    paragraph.paragraph_format.keep_with_next = True
    add_markdown_runs(paragraph, text, size=10.1, color=CHARCOAL)
    for run in paragraph.runs:
        run.font.bold = True


def add_body(document, text):
    paragraph = document.add_paragraph()
    if text.startswith("**Stack:**"):
        set_spacing(paragraph, after=2, line=1.0)
        paragraph.paragraph_format.keep_with_next = True
        add_markdown_runs(paragraph, text, size=8.85, color=MUTED)
    else:
        set_spacing(paragraph, after=4, line=1.04)
        add_markdown_runs(paragraph, text, size=9.5, color=CHARCOAL)


def add_bullet(document, text):
    paragraph = document.add_paragraph(style="List Bullet")
    paragraph.paragraph_format.left_indent = Inches(0.24)
    paragraph.paragraph_format.first_line_indent = Inches(-0.16)
    set_spacing(paragraph, after=1.6, line=1.025)
    add_markdown_runs(paragraph, text, size=9.25, color=CHARCOAL)


def set_core_properties(document):
    props = document.core_properties
    props.author = "Tran Zha"
    props.title = "Tran Zha - Engineering Manager Resume"
    props.subject = "Engineering Manager, Platform Modernisation and Technical Leadership"
    props.keywords = (
        "Engineering Manager, Software Engineering Manager, people leadership, hiring, "
        "performance reviews, platform modernisation, C# .NET, AWS, Kubernetes, DevOps, "
        "CI/CD, event-driven architecture, stakeholder management"
    )
    props.comments = "Generated from tranzha-rebuilt.md"


def build(source, output):
    document = Document()
    configure_document(document)
    blocks = parse_markdown(source.read_text(encoding="utf-8"))

    index = 0
    if blocks and blocks[0][0] == "h1":
        index = add_header_block(document, blocks, index)

    while index < len(blocks):
        kind, value = blocks[index]
        if kind == "h2":
            add_section(document, value)
        elif kind == "h3":
            add_employer(document, value)
        elif kind == "h4":
            add_role(document, value)
        elif kind == "p":
            add_body(document, " ".join(value))
        elif kind == "ul":
            for item in value:
                add_bullet(document, item)
        index += 1

    set_core_properties(document)
    output.parent.mkdir(parents=True, exist_ok=True)
    document.save(output)
    print(output)


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--source", type=Path, default=ROOT / "tranzha-rebuilt.md")
    parser.add_argument(
        "--output",
        type=Path,
        default=ROOT / "output" / "docx" / "tran-zha-engineering-manager-resume.docx",
    )
    args = parser.parse_args()
    build(args.source, args.output)


if __name__ == "__main__":
    main()
