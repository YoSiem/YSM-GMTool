param(
    [string]$PublishDir = "C:\Users\patry\Desktop\YSM Tools\__Release\GM Tool"
)

$ErrorActionPreference = "Stop"
$RepoRoot = Split-Path -Parent $PSScriptRoot
$ProjectPath = Join-Path $RepoRoot "src\App.Desktop\App.Desktop.csproj"
$PublishedExePath = Join-Path $PublishDir "GM Tool.exe"

Get-Process |
    Where-Object {
        try {
            $_.Path -eq $PublishedExePath
        }
        catch {
            $false
        }
    } |
    Stop-Process -Force -ErrorAction SilentlyContinue

if (Test-Path $PublishDir) {
    Remove-Item -Path $PublishDir -Recurse -Force
}

New-Item -ItemType Directory -Path $PublishDir -Force | Out-Null

dotnet publish $ProjectPath `
    -c Release `
    -r win-x64 `
    --self-contained false `
    -p:PublishSingleFile=true `
    -p:CopyOutputSymbolsToPublishDirectory=false `
    -p:DebugType=None `
    -p:AllowedReferenceRelatedFileExtensions=none `
    -o "$PublishDir"

# Clean up any residual artifacts that might have escaped the publish flags
Get-ChildItem -Path $PublishDir -File -Recurse | Where-Object {
    $_.Extension -eq ".pdb" -or $_.Extension -eq ".xml"
} | Remove-Item -Force
