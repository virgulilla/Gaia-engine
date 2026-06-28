$dotnetPath = Join-Path $PSScriptRoot ".dotnet\dotnet.exe"
& $dotnetPath @args
exit $LASTEXITCODE
