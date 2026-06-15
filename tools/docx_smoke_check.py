from docx import Document

path = r"C:\github\resume\tranzha.docx"
doc = Document(path)
texts = [p.text for p in doc.paragraphs if p.text.strip()]
search_text = "\n".join(texts).lower()
required = [
    "Tran Zha",
    "tranzha83@gmail.com",
    "Australian permanent resident",
    "Technical Toolkit",
    "scope/debt trade-offs",
    "Drove domain modelling",
    "roadmap alignment",
    "~10 to 30+ engineers",
    "0 to 30+ engineers",
    "Scrum master",
    "Managed cloud spend",
    "incident response",
    "Jul 2019 - Apr 2021",
]
for needle in required:
    if needle.lower() not in search_text:
        raise SystemExit(f"missing: {needle}")
print(f"paragraphs={len(doc.paragraphs)}")
print(f"first={texts[0]}")
print("smoke=ok")
