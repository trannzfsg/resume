import importlib.util

for name in ("fitz", "pypdfium2", "pdf2image", "PIL"):
    print(f"{name}={bool(importlib.util.find_spec(name))}")
