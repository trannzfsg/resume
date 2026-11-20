from pathlib import Path

from docx import Document
from docx.opc.constants import RELATIONSHIP_TYPE

path = Path(__file__).resolve().parents[1] / "tranzha.docx"
doc = Document(path)
texts = [p.text for p in doc.paragraphs if p.text.strip()]
if texts[1] != "Engineering Manager":
    raise SystemExit("missing Engineering Manager headline")

search_text = "\n".join(texts).lower()
required = [
    "Tran Zha",
    "tranzha83@gmail.com",
    "Technical Delivery Manager",
    "City of Sydney | Jul 2026 to Present",
    "Full Australian working rights",
    "Professional Summary",
    "Engineering Manager with 15+ years in software engineering",
    "Professional Experience",
    "Loan Market Group",
    "8 to 10 direct reports",
    "4,000+ users in under three months",
    "Skills",
    "Applied AI",
    "multi-agent Codex workflow",
    "searchable knowledge base",
    "Education",
    "University of Auckland",
]
for needle in required:
    if needle.lower() not in search_text:
        raise SystemExit(f"missing: {needle}")

links = {
    rel.target_ref for rel in doc.part.rels.values()
    if rel.reltype == RELATIONSHIP_TYPE.HYPERLINK
}
for url in (
    "https://www.linkedin.com/in/tran-zha/",
    "https://github.com/trannzfsg",
    "https://trannzfsg.github.io/resume/",
):
    if url not in links:
        raise SystemExit(f"missing link: {url}")

print(f"paragraphs={len(doc.paragraphs)}")
print(f"first={texts[0]}")
print("smoke=ok")
