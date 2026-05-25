from docx import Document

path = r"C:\github\resume\Tran_Zha_CV.docx"
doc = Document(path)
texts = [p.text for p in doc.paragraphs if p.text.strip()]
search_text = "\n".join(texts).lower()
required = [
    "Tran Zha",
    "Australian permanent resident",
    "Technical Toolkit",
    "Drove domain modelling",
    "roadmap alignment",
]
for needle in required:
    if needle.lower() not in search_text:
        raise SystemExit(f"missing: {needle}")
print(f"paragraphs={len(doc.paragraphs)}")
print(f"first={texts[0]}")
print("smoke=ok")
