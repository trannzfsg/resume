from docx import Document

path = r"C:\github\resume\tranzha.docx"
doc = Document(path)
texts = [p.text for p in doc.paragraphs if p.text.strip()]
search_text = "\n".join(texts).lower()
required = [
    "Tran Zha",
    "tranzha83@gmail.com",
    "Software Engineering Manager",
    "Full Australian working rights",
    "Australian permanent resident",
    "How I Lead",
    "Harness engineering",
    "reusable workflows",
    "Loop engineering",
    "work myself out of the critical path",
    "Leadership and Technical Skills",
    "ISO 27001",
    "SOC 2 Type II",
    "PII data handling",
    "Loan Market Group",
    "Strangler Fig transition",
    "event sourcing",
    "scope and technical debt trade-offs",
    "domain modelling",
    "roadmap planning",
    "8 to 10 direct reports",
    "more than 30 engineers",
    "Scrum Master",
    "Managed cloud spend",
    "incident response",
    "Jul 2019 to Apr 2021",
    "Languages: English and Mandarin",
]
for needle in required:
    if needle.lower() not in search_text:
        raise SystemExit(f"missing: {needle}")
print(f"paragraphs={len(doc.paragraphs)}")
print(f"first={texts[0]}")
print("smoke=ok")
