; Script generated by the Inno Setup Script Wizard.
; SEE THE DOCUMENTATION FOR DETAILS ON CREATING INNO SETUP SCRIPT FILES!

#define MyAppName "Video Player"
#define MyAppVersion "1.1"
#define MyAppPublisher "2late"
#define MyAppURL "https://2late.org"
#define MyAppExeName "VideoPlayer.exe"
#define MyAppAssocName "mp4 File"
#define MyAppAssocExt ".mp4"
#define MyAppAssocKey StringChange(MyAppAssocName, " ", "") + MyAppAssocExt

[Setup]
; NOTE: The value of AppId uniquely identifies this application. Do not use the same AppId value in installers for other applications.
; (To generate a new GUID, click Tools | Generate GUID inside the IDE.)
AppId={{C3BBD782-2B85-4C44-A533-E811A2B61DCD}
AppName={#MyAppName}
AppVersion={#MyAppVersion}
;AppVerName={#MyAppName} {#MyAppVersion}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={autopf}\{#MyAppName}
ChangesAssociations=yes
DisableProgramGroupPage=yes
; Uncomment the following line to run in non administrative install mode (install for current user only.)
;PrivilegesRequired=lowest
PrivilegesRequiredOverridesAllowed=dialog
OutputBaseFilename=Video Player Setup
Compression=lzma
SolidCompression=yes
WizardStyle=modern

[Languages]
Name: "english"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked

[Files]
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\{#MyAppExeName}"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\MaterialDesignColors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\MaterialDesignThemes.Wpf.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\Microsoft.Xaml.Behaviors.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\VideoPlayer.deps.json"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\VideoPlayer.dll"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\VideoPlayer.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "C:\Files\VSProjects\VideoPlayer\VideoPlayer\bin\Release\net7.0-windows\win-x64\VideoPlayer.runtimeconfig.json"; DestDir: "{app}"; Flags: ignoreversion
; NOTE: Don't use "Flags: ignoreversion" on any shared system files

[Registry]
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocExt}\OpenWithProgids"; ValueType: string; ValueName: "{#MyAppAssocKey}"; ValueData: ""; Flags: uninsdeletevalue
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}"; ValueType: string; ValueName: ""; ValueData: "{#MyAppAssocName}"; Flags: uninsdeletekey
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\DefaultIcon"; ValueType: string; ValueName: ""; ValueData: "{app}\{#MyAppExeName},0"
Root: HKA; Subkey: "Software\Classes\{#MyAppAssocKey}\shell\open\command"; ValueType: string; ValueName: ""; ValueData: """{app}\{#MyAppExeName}"" ""%1"""
Root: HKA; Subkey: "Software\Classes\Applications\{#MyAppExeName}\SupportedTypes"; ValueType: string; ValueName: ".myp"; ValueData: ""

[Icons]
Name: "{autoprograms}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{autodesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, '&', '&&')}}"; Flags: nowait postinstall skipifsilent

