; 脚本由 Inno Setup 脚本向导 生成！
; 有关创建 Inno Setup 脚本文件的详细资料请查阅帮助文档！

#define MyAppName "简易爬虫"
#define MyAppVerName "我的程序 1.5"
#define MyAppPublisher "Ran"
#define MyAppURL "http://www.example.com/"
#define MyAppExeName "CrawlerDemo.exe"

[Setup]
; 注: AppId的值为单独标识该应用程序。
; 不要为其他安装程序使用相同的AppId值。
; (生成新的GUID，点击 工具|在IDE中生成GUID。)
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
Source: "E:\程序\C#\小程序\网络爬虫\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\程序\C#\小程序\网络爬虫\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\程序\C#\小程序\网络爬虫\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.pdb"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\程序\C#\小程序\网络爬虫\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.vshost.exe"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\程序\C#\小程序\网络爬虫\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.vshost.exe.config"; DestDir: "{app}"; Flags: ignoreversion
Source: "E:\程序\C#\小程序\网络爬虫\CrawlerDemo\CrawlerDemo\bin\Debug\CrawlerDemo.vshost.exe.manifest"; DestDir: "{app}"; Flags: ignoreversion
; 注意: 不要在任何共享系统文件上使用“Flags: ignoreversion”

[Icons]
Name: "{group}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"
Name: "{commondesktop}\{#MyAppName}"; Filename: "{app}\{#MyAppExeName}"; Tasks: desktopicon

[Run]
Filename: "{app}\{#MyAppExeName}"; Description: "{cm:LaunchProgram,{#StringChange(MyAppName, "&", "&&")}}"; Flags: nowait postinstall skipifsilent

