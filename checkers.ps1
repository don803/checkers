$ErrorActionPreference = "Stop"

$scriptRoot = Split-Path -Parent $MyInvocation.MyCommand.Path
Set-Location $scriptRoot

dotnet run --project "src/Checkers.Cli/Checkers.Cli.csproj"
