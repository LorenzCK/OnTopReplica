[Setup]
AppName=OnTopReplica
AppVersion=1.0
DefaultDirName={pf}\OnTopReplica
DefaultGroupName=OnTopReplica
OutputBaseFilename=OnTopReplica_Setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Files]
Source: "{#ProjectExePath}\\OnTopReplica.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "{#ProjectExePath}\\*.dll"; DestDir: "{app}"; Flags: ignoreversion recursesubdirs

[Tasks]
Name: "desktopicon"; Description: "Create a &desktop icon"; GroupDescription: "Additional icons:"; Flags: unchecked

[Icons]
Name: "{group}\OnTopReplica"; Filename: "{app}\OnTopReplica.exe"
Name: "{commondesktop}\OnTopReplica"; Filename: "{app}\OnTopReplica.exe"; Tasks: desktopicon

[Run]
Filename: "{app}\OnTopReplica.exe"; Description: "Launch OnTopReplica"; Flags: nowait postinstall skipifsilent
