$ErrorActionPreference = "Stop"

$repoRoot = Split-Path -Parent $PSScriptRoot
$docxPath = Join-Path $repoRoot "tranzha.docx"
$pdfPath = Join-Path $repoRoot "tmp\docx-render\tranzha_word.pdf"

New-Item -ItemType Directory -Force -Path (Split-Path -Parent $pdfPath) | Out-Null

$word = $null
$doc = $null
try {
    $word = New-Object -ComObject Word.Application
    $word.Visible = $false
    $doc = $word.Documents.Open($docxPath, $false, $true)
    $doc.ExportAsFixedFormat($pdfPath, 17)
    Write-Output $pdfPath
}
finally {
    if ($doc -ne $null) {
        $doc.Close($false)
    }
    if ($word -ne $null) {
        $word.Quit()
    }
}
