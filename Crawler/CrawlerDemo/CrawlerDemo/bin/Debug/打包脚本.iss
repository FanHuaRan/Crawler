; �ű��� Inno Setup �ű��� ���ɣ�
; �йش��� Inno Setup �ű��ļ�����ϸ��������İ����ĵ���

#define MyAppName "��������"
#define MyAppVerName "�ҵĳ��� 1.5"
#define MyAppPublisher "Ran"
#define MyAppURL "http://www.example.com/"
#define MyAppExeName "CrawlerDemo.exe"

[Setup]
; ע: AppId��ֵΪ������ʶ��Ӧ�ó���
; ��ҪΪ������װ����ʹ����ͬ��AppIdֵ��
; (�����µ�GUID����� ����|��IDE������GUID��)
AppId={{57CE6D52-6E9C-4DE5-A6E6-D2C7F8E8218E}
AppName={#MyAppName}
AppVerName={#MyAppVerName}
AppPublisher={#MyAppPublisher}
AppPublisherURL={#MyAppURL}
AppSupportURL={#MyAppURL}
AppUpdatesURL={#MyAppURL}
DefaultDirName={pf}\{#MyAppName}
DefaultGroupName={#MyAppName}
OutputDir=C:\Users\Administrator\Desktop
OutputBaseFilename=setup
Compression=lzma
SolidCompression=yes

[Languages]
Name: "chinesesimp"; MessagesFile: "compiler:Default.isl"

[Tasks]
Name: "desktopicon"; Description: "{cm:CreateDesktopIcon}"; GroupDescription: "{cm:AdditionalIcons}"; Flags: unchecked; OnlyBelowVersion: 0,6.1

[Files]
Source: "E:\����\C#\С����\��������\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\����\C#\С����\��������\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\����\C#\С����\��������\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\����\C#\С����\��������\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.vshost.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\����\C#\С����\��������\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.vshost.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\����\C#\С����\��������\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.vshost.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion
; ע��: ��Ҫ���κι���ϵͳ�ļ���ʹ�á�Flags: ignoreversion��

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, "&", "&&")}}"; Flags: nowait postinstall skipifsilent

