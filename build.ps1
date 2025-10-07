# Build NuGet Package
# Usage: .\build.ps1

Write-Host "`n?? Building Resto.Front.Api.Console..." -ForegroundColor Cyan

# Download nuget.exe if not exists
if (-not (Test-Path ".\nuget.exe")) {
    Write-Host "?? Downloading nuget.exe..." -ForegroundColor Yellow
    Invoke-WebRequest -Uri "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -OutFile "nuget.exe"
}

# Clean
Remove-Item -Path ".\Resto.Front.Api.Console\bin\Release" -Recurse -Force -ErrorAction SilentlyContinue
Remove-Item -Path ".\PackageOutput" -Recurse -Force -ErrorAction SilentlyContinue

# Build
msbuild .\Resto.Front.Api.Console\Resto.Front.Api.Console.csproj /p:Configuration=Release /t:Rebuild /v:minimal /nologo

if ($LASTEXITCODE -ne 0) {
    Write-Host "? Build failed!" -ForegroundColor Red
    exit 1
}

# Create package
Write-Host "?? Creating NuGet package..." -ForegroundColor Yellow
New-Item -ItemType Directory -Force -Path ".\PackageOutput" | Out-Null
.\nuget.exe pack .\Resto.Front.Api.Console\Resto.Front.Api.Console.nuspec -OutputDirectory .\PackageOutput

if ($LASTEXITCODE -eq 0) {
    Write-Host "`n? Package created successfully!" -ForegroundColor Green
    Get-ChildItem -Path ".\PackageOutput\*.nupkg" | ForEach-Object {
        Write-Host "   $($_.Name) ($([math]::Round($_.Length/1KB,2)) KB)" -ForegroundColor White
    }
    Write-Host "`n?? To publish: .\publish.ps1 -ApiKey `"your-key`"`n" -ForegroundColor Cyan
} else {
    Write-Host "? Package creation failed!" -ForegroundColor Red
    exit 1
}
