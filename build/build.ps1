param (
	[switch]$Debug,
	[string]$VisualStudioVersion = "14.0",
	[switch]$SkipKeyCheck
)

# build the solution
$SolutionPath = "..\StyleCopAnalyzers.sln"

# make sure the script was run from the expected path
if (!(Test-Path $SolutionPath)) {
	$host.ui.WriteErrorLine('The script was run from an invalid working directory.')
	exit 1
}

. .\version.ps1

If ($Debug) {
	$BuildConfig = 'Debug'
} Else {
	$BuildConfig = 'Release'
}

If ($Version.Contains('-')) {
	$KeyConfiguration = 'Dev'
} Else {
	$KeyConfiguration = 'Final'
}

# download NuGet.exe if necessary
$nuget = '..\.nuget\NuGet.exe'
If (-not (Test-Path $nuget)) {
	If (-not (Test-Path '..\.nuget')) {
		mkdir '..\.nuget'
	}

	$nugetSource = 'http://nuget.org/nuget.exe'
	Invoke-WebRequest $nugetSource -OutFile $nuget
}

# build the main project
$msbuild = "${env:ProgramFiles(x86)}\MSBuild\$VisualStudioVersion\Bin\MSBuild.exe"

&$nuget 'restore' $SolutionPath
&$msbuild '/nologo' '/m' '/Verbosity:minimal' '/nr:false' '/t:rebuild' "/p:Configuration=$BuildConfig" "/p:VisualStudioVersion=$VisualStudioVersion" "/p:KeyConfiguration=$KeyConfiguration" $SolutionPath
if ($LASTEXITCODE -ne 0) {
	$host.ui.WriteErrorLine('Build failed, aborting!')
	exit $LASTEXITCODE
}
# Run unit tests
$xunit = "..\packages\xunit.runners.2.0.0-rc1-build2826\tools\xunit.console.exe"
&$xunit "..\StyleCop.Analyzers\StyleCop.Analyzers.Test\bin\$BuildConfig\StyleCop.Analyzers.Test.dll" -parallel none
if ($LASTEXITCODE -ne 0) {
	$host.ui.WriteErrorLine('Test failed, aborting!')
	exit $LASTEXITCODE
}

# By default, do not create a NuGet package unless the expected strong name key files were used
if (-not $SkipKeyCheck) {
	. .\keys.ps1

	foreach ($pair in $Keys.GetEnumerator()) {
		$assembly = Resolve-FullPath -Path "..\StyleCop.Analyzers\StyleCop.Analyzers\bin\$BuildConfig\StyleCop.Analyzers.dll"
		# Run the actual check in a separate process or the current process will keep the assembly file locked
		powershell -Command ".\check-key.ps1 -Assembly '$assembly' -ExpectedKey '$($pair.Value)' -Build '$($pair.Key)'"
		if ($LASTEXITCODE -ne 0) {
			Exit $p.ExitCode
		}
	}
}

if (-not (Test-Path 'nuget')) {
	mkdir "nuget"
}

&$nuget 'pack' "..\StyleCop.Analyzers\StyleCop.Analyzers\bin\$BuildConfig\StyleCop.Analyzers.nuspec" '-OutputDirectory' 'nuget' '-Prop' "Configuration=$BuildConfig" '-Version' "$Version" '-Symbols'
