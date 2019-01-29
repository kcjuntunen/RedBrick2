!define DOTNET_VERSION "v4.0.30319"
!include version.nsh

Name "RedBrick Installer"
OutFile "InstallRedBrick.exe"

VIProductVersion ${VERSION}
VIAddVersionKey ProductVersion ${VERSION}
VIAddVersionKey ProductName "RedBrick"
VIAddversionkey FileVersion ${VERSION}
VIAddVersionKey FileDescription "Connect Solidworks to cutlist data."
VIAddVersionKey LegalCopyright  "2017-2018 K. C. Juntunen"
InstallDir "$PROGRAMFILES64\RedBrick\"
RequestExecutionLevel admin

Section
	SetOutPath $INSTDIR

	File ".\redlego.ico"
	File ".\bin\x64\Release\Redbrick2.dll.config"
	File ".\bin\x64\Release\System.dll"
	File ".\bin\x64\Release\System.Data.dll"
	; File ".\bin\x64\Release\System.Security.dll"
	File ".\bin\x64\Release\System.Xml.dll"
	File ".\bin\x64\Release\Machine Priority Control.dll"
	File ".\bin\x64\Release\FormatFixtureBook.dll"
	File ".\bin\x64\Release\EPPlus.dll"
	; File ".\bin\x64\Release\DrawingCompiler.dll"
	File ".\bin\x64\Release\itextsharp.dll"
	File ".\bin\x64\Release\itextsharp.xtra.dll"
	File ".\bin\x64\Release\itextsharp.pdfa.dll"
	; File ".\bin\x64\Release\ICSharpCode.SharpZipLib.dll"
	File ".\bin\x64\Release\ArchivePDF.dll"
	File ".\bin\x64\Release\HtmlAgilityPack.dll"
	; File ".\bin\x64\Release\Newtonsoft.Json.dll"
	File ".\bin\x64\Release\SolidWorks.Interop.sldworks.dll"
	File ".\bin\x64\Release\Redbrick2.dll"

	Push "$INSTDIR\Redbrick2.dll"
	Call RegisterDotNet
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"DisplayIcon" "$PROGRAMFILES64\Redbrick\redlego.ico"
	WriteRegDWORD HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"EstimatedSize" 16686
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"DisplayName" "Amstore Redbrick Property Manager"
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"DisplayVersion" ${VERSION}
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"Publisher" "Amstore Corp."
	WriteRegStr HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"UninstallString" "$\"$INSTDIR\RemoveRedbrick.exe$\""
	WriteUninstaller "$INSTDIR\RemoveRedBrick.exe"
SectionEnd

Section "Uninstall"
	Push "$INSTDIR\Redbrick_Addin.dll"
	Call un.RegisterDotNet

	DeleteRegKey HKLM "Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick"
	Delete "$INSTDIR\RemoveRedBrick.exe"
	Delete "$INSTDIR\Redbrick2.dll.config"
	; Delete "$INSTDIR\swTableType.dll"
	Delete "$INSTDIR\FormatFixtureBook.dll"
	Delete "$INSTDIR\EPPlus.dll"
	; Delete "$INSTDIR\DrawingCompiler.dll"
	Delete "$INSTDIR\itextsharp.dll"
	Delete "$INSTDIR\Machine Priority Control.dll"
	Delete "$INSTDIR\System.dll"
	Delete "$INSTDIR\System.Data.dll"
	Delete "$INSTDIR\System.Security.dll"
	Delete "$INSTDIR\System.Xml.dll"
	Delete "$INSTDIR\ArchivePDF.dll"
	Delete "$INSTDIR\HtmlAgilityPack.dll"
	Delete "$INSTDIR\Newtonsoft.Json.dll"
	Delete "$INSTDIR\SolidWorks.Interop.sldworks.dll"
	Delete "$INSTDIR\Redbrick2.dll"
	Delete "$INSTDIR\redlego.ico"
	RMDir	"$INSTDIR"
SectionEnd

Function .onInit
	SetRegView 64
	ReadRegStr $R0 HKLM \
			"Software\Microsoft\Windows\CurrentVersion\Uninstall\Redbrick" \
			"UninstallString"
	StrCmp $R0 "" done

	MessageBox MB_OKCANCEL|MB_ICONEXCLAMATION \
			"Redbrick is already installed. $\n$\nClick `OK` to remove the \
			previous version or `Cancel` to cancel this upgrade." \
			IDOK uninst
	Abort

	uninst:
		ClearErrors
		ExecWait '$R0 _?=$INSTDIR'

		IfErrors no_remove_uninstaller done

	no_remove_uninstaller:

	done:
	FunctionEnd

	Function RegisterDotNet

		Exch $R0
		Push $R1

		SetRegView 64
		ReadRegStr $R1 HKEY_LOCAL_MACHINE \
				"Software\Microsoft\.NETFramework" "InstallRoot"

		IfFileExists $R1\${DOTNET_VERSION}\regasm.exe FileExists
		MessageBox MB_ICONSTOP|MB_OK "Microsoft .NET Framework 4.0 was not detected!"
		Abort

		FileExists:
			ExecWait '"$R1\${DOTNET_VERSION}\regasm.exe" /codebase "$R0" /silent'

			Pop $R1
			Pop $R0
	FunctionEnd

	Function un.RegisterDotNet

		Exch $R0
		Push $R1

		SetRegView 64
		ReadRegStr $R1 HKEY_LOCAL_MACHINE \
				"Software\Microsoft\.NETFramework" "InstallRoot"

		IfFileExists $R1\${DOTNET_VERSION}\regasm.exe FileExists
		MessageBox MB_ICONSTOP|MB_OK "Microsoft .NET Framework 4.0 was not detected!"
		Abort

		FileExists:
			ExecWait '"$R1\${DOTNET_VERSION}\regasm.exe" "$R0" /unregister /silent'

			Pop $R1
			Pop $R0

	FunctionEnd
