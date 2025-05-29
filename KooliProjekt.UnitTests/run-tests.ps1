dotnet tool install -g dotnet-reportgenerator-globaltool
$TestOutput = dotnet test --collect "XPlat Code Coverage" --results-directory ./BuildReports/UnitTests
$TestReportsParts = $TestOutput | Select-String coverage.cobertura.xml | ForEach-Object { $_.Line.Trim() }
$TestReportsCrappy = ($TestReportsParts -join ';')

$guid_regex = "[0-9a-f]{8}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{4}-[0-9a-f]{12}" 
$TestReports = $TestReportsCrappy -replace $guid_regex
$TestReports = $TestReports.Replace("//","/").Replace('\\','\') #.Replace("\\UnitTests","\\Coverage")

copy $TestReportsCrappy $TestReports
del $TestReportsCrappy

Get-ChildItem -Path ./BuildReports/UnitTests -Directory -Recurse | Remove-Item -Force  

reportgenerator "-reports:$TestReports" "-targetdir:.//BuildReports//Coverage" "-reporttype:Html" "-classfilters:-AspNetCoreGeneratedDocument.*"

if ($IsMacOS) {
    # On macOS, use the 'open' command
    $reportPath = (Get-Item "BuildReports/Coverage/index.htm").FullName
    $openCommand = "open '$reportPath'"
    Invoke-Expression $openCommand
} elseif ($IsLinux) {
    # On Linux, try xdg-open
    $reportPath = (Get-Item "BuildReports/Coverage/index.htm").FullName
    $openCommand = "xdg-open '$reportPath'"
    Invoke-Expression $openCommand
} else {
    # On Windows, use Start-Process
    Start-Process "BuildReports/Coverage/index.htm"
}
