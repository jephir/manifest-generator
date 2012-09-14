# Manifest Generator

Produces manifest files for the `redress` application updater library.

## Usage Example

	ManifestGenerator.exe /ContentDirectory="C:\Build\Output" /ContentServerUri="http://example.com/content/" /OutputFile="C:\Apache\htdocs\content\" /InstallerPath="Bin\InstallRedist.exe" /ApplicationPath="Bin\App.exe" /ApplicationArguments="/Verbose"

## Arguments

All arguments are optional.

* `ContentDirectory` The path to the directory containing the files to analyze.
* `ContentServerUri` The URI of the content server.
* `OutputFile` The name of the manifest file to output (can include a directory).
* `InstallerPath` The relative path from the content directory to the application installer.
* `ApplicationPath` The relative path from the content directory to the application executable.
* `ApplicationArguments` A string of arguments to pass to the application when it is run.
* `IncludeManifest` If __true__ the generator will include any existing manifest file, if it exists, into the generated manifest.

## System Requirements

* [.NET Framework 3.5](http://www.microsoft.com/download/details.aspx?id=22)

## Copying

To the extent possible under law, the author(s) have dedicated all copyright and related and neighboring rights to this software to the public domain worldwide. This software is distributed without any warranty.
You should have received a copy of the CC0 Public Domain Dedication along with this software. If not, see <http://creativecommons.org/publicdomain/zero/1.0/>.