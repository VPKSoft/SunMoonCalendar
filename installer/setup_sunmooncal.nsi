; SunMoonCalendar
;
; Shows sun / moon rise and set information.
; Copyright (C) 2016 VPKSoft, Petteri Kautonen
; 
; Contact: vpksoft@vpksoft.net
; 
; This file is part of SunMoonCalendar.
; 
; SunMoonCalendar is free software: you can redistribute it and/or modify
; it under the terms of the GNU General Public License as published by
; the Free Software Foundation, either version 3 of the License, or
; (at your option) any later version.

; SunMoonCalendar is distributed in the hope that it will be useful,
; but WITHOUT ANY WARRANTY; without even the implied warranty of
; MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
; GNU General Public License for more details.

; You should have received a copy of the GNU General Public License
; along with SunMoonCalendar.  If not, see <http://www.gnu.org/licenses/>.


Name "SunMoonCalendar"

# General Symbol Definitions
!define REGKEY "SOFTWARE\$(^Name)"
!define VERSION 1.0.0.0
!define COMPANY VPKSoft
!define URL http://www.vpksoft.net

# MUI Symbol Definitions
!define MUI_ICON .\sunmooncal.ico
!define MUI_FINISHPAGE_NOAUTOCLOSE
!define MUI_STARTMENUPAGE_REGISTRY_ROOT HKLM
!define MUI_STARTMENUPAGE_NODISABLE
!define MUI_STARTMENUPAGE_REGISTRY_KEY ${REGKEY}
!define MUI_STARTMENUPAGE_REGISTRY_VALUENAME StartMenuGroup
!define MUI_STARTMENUPAGE_DEFAULTFOLDER "SunMoonCalendar"
!define MUI_UNICON .\un_sunmooncal.ico
!define MUI_UNFINISHPAGE_NOAUTOCLOSE
!define MUI_LANGDLL_REGISTRY_ROOT HKLM
!define MUI_LANGDLL_REGISTRY_KEY ${REGKEY}
!define MUI_LANGDLL_REGISTRY_VALUENAME InstallerLanguage
BrandingText "SunMoonCalendar"

#Include the LogicLib
!include 'LogicLib.nsh'
!include "x64.nsh"
!include "InstallOptions.nsh"

# Included files
!include Sections.nsh
!include MUI2.nsh

# Reserved Files
!insertmacro MUI_RESERVEFILE_LANGDLL

# Variables
Var StartMenuGroup

# Installer pages
!insertmacro MUI_PAGE_WELCOME
!insertmacro MUI_PAGE_LICENSE ..\COPYING
!insertmacro MUI_PAGE_DIRECTORY
!insertmacro MUI_PAGE_STARTMENU Application $StartMenuGroup
!insertmacro MUI_PAGE_INSTFILES
!insertmacro MUI_PAGE_FINISH
!insertmacro MUI_UNPAGE_CONFIRM
!insertmacro MUI_UNPAGE_INSTFILES

# Installer languages
!insertmacro MUI_LANGUAGE English
!insertmacro MUI_LANGUAGE Finnish

# Installer attributes
OutFile setup_sunmooncal.exe
InstallDir "$PROGRAMFILES64\SunMoonCalendar"
CRCCheck on
XPStyle on
ShowInstDetails hide
VIProductVersion 1.0.0.0
VIAddVersionKey /LANG=${LANG_ENGLISH} ProductName "SunMoonCalendar"
VIAddVersionKey /LANG=${LANG_ENGLISH} ProductVersion "${VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} CompanyName "${COMPANY}"
VIAddVersionKey /LANG=${LANG_ENGLISH} CompanyWebsite "${URL}"
VIAddVersionKey /LANG=${LANG_ENGLISH} FileVersion "${VERSION}"
VIAddVersionKey /LANG=${LANG_ENGLISH} FileDescription ""
VIAddVersionKey /LANG=${LANG_ENGLISH} LegalCopyright ""
InstallDirRegKey HKLM "${REGKEY}" Path
ShowUninstDetails hide

# Installer sections
Section -Main SEC0000
    SetOutPath $INSTDIR
    SetOverwrite on
		
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\COPYING
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\SunMoonCalendar.exe
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\System.Data.SQLite.dll
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.About.dll
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.LangLib.dll
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.PosLib.dll
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.SmallCityDatabase.dll
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.Utils.dll
	
	File ..\SunMoonCalendar\SunMoonCalcs.cs
	
    SetOutPath $INSTDIR\x64	
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\x64\SQLite.Interop.dll

    SetOutPath $INSTDIR\x86	
	File ..\SunMoonCalendar\bin\sunmooncalendar_release\x86\SQLite.Interop.dll

    SetOutPath $INSTDIR\licenses
	File ..\licenses\COPYING.LESSER.VPKSoft.About
	File ..\licenses\COPYING.LESSER.VPKSoft.LangLib
	File ..\licenses\COPYING.LESSER.VPKSoft.PosLib
	File ..\licenses\COPYING.LESSER.VPKSoft.SmallCityDatabase
	File ..\licenses\COPYING.LESSER.VPKSoft.Utils
	File ..\licenses\COPYING.VPKSoft.About
	File ..\licenses\COPYING.VPKSoft.LangLib
	File ..\licenses\COPYING.VPKSoft.PosLib
	File ..\licenses\COPYING.VPKSoft.SmallCityDatabase
	File ..\licenses\COPYING.VPKSoft.Utils
	File ..\SunMoonCalendar\COPYING.SunMoonCalsc.cs.txt

	
    SetOutPath "$LOCALAPPDATA\SunMoonCalendar"
	File ..\translation\SunMoonCalendar.sqlite
    
    Var /GLOBAL DotNet451_x86
    Var /GLOBAL DotNet451_x64
    
    ReadRegDWORD $DotNet451_x86 HKLM "SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full" Release
    ReadRegDWORD $DotNet451_x64 HKLM "HKEY_LOCAL_MACHINE\SOFTWARE\Wow6432Node\Microsoft\NET Framework Setup\NDP\v4\Full" Release

    SetOutPath $TEMP
    File .\NDP451-KB2859818-Web.exe
   
    ${If} ${RunningX64}
        ${If} DotNet451_x64 == ""
        ${AndIf} DotNet451_x64 != "378758"
        ${AndIf} DotNet451_x64 != "378675"
            ExecWait NDP451-KB2859818-Web.exe
        ${EndIf}
    ${Else}    
        ${If} DotNet451_x86 == ""
        ${AndIf} DotNet451_x86 != "378758"
        ${AndIf} DotNet451_x86 != "378675"
            ExecWait NDP451-KB2859818-Web.exe
        ${EndIf}
    ${EndIf}
   
    Delete $TEMP\NDP451-KB2859818-Web.exe
    
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\SunMoonCalendar.lnk" $INSTDIR\SunMoonCalendar.exe
    WriteRegStr HKLM "${REGKEY}\Components" Main 1
SectionEnd

Section -post SEC0001
    WriteRegStr HKLM "${REGKEY}" Path $INSTDIR
    SetOutPath $INSTDIR
    WriteUninstaller $INSTDIR\uninstall_sunmooncal.exe
    !insertmacro MUI_STARTMENU_WRITE_BEGIN Application
    SetOutPath $SMPROGRAMS\$StartMenuGroup
    CreateShortcut "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk" $INSTDIR\uninstall_sunmooncal.exe
    !insertmacro MUI_STARTMENU_WRITE_END
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayName "$(^Name)"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayVersion "${VERSION}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" Publisher "${COMPANY}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" URLInfoAbout "${URL}"
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" DisplayIcon $INSTDIR\uninstall_sunmooncal.exe
    WriteRegStr HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" UninstallString $INSTDIR\uninstall_sunmooncal.exe
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoModify 1
    WriteRegDWORD HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)" NoRepair 1
SectionEnd

# Macro for selecting uninstaller sections
!macro SELECT_UNSECTION SECTION_NAME UNSECTION_ID
    Push $R0
    ReadRegStr $R0 HKLM "${REGKEY}\Components" "${SECTION_NAME}"
    StrCmp $R0 1 0 next${UNSECTION_ID}
    !insertmacro SelectSection "${UNSECTION_ID}"
    GoTo done${UNSECTION_ID}
next${UNSECTION_ID}:
    !insertmacro UnselectSection "${UNSECTION_ID}"
done${UNSECTION_ID}:
    Pop $R0
!macroend

# Uninstaller sections
Section /o -un.Main UNSEC0000
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\SunMoonCalendar.lnk"

	Delete /REBOOTOK $INSTDIR\COPYING
	Delete /REBOOTOK $INSTDIR\SunMoonCalendar.exe
	Delete /REBOOTOK $INSTDIR\System.Data.SQLite.dll
	Delete /REBOOTOK $INSTDIR\VPKSoft.About.dll
	Delete /REBOOTOK $INSTDIR\VPKSoft.LangLib.dll
	Delete /REBOOTOK $INSTDIR\VPKSoft.PosLib.dll
	Delete /REBOOTOK $INSTDIR\VPKSoft.SmallCityDatabase.dll
	Delete /REBOOTOK $INSTDIR\VPKSoft.Utils.dll
	Delete /REBOOTOK $INSTDIR\x64\SQLite.Interop.dll
	Delete /REBOOTOK $INSTDIR\x86\SQLite.Interop.dll
	RmDir /REBOOTOK $INSTDIR\x64
	RmDir /REBOOTOK $INSTDIR\x86
	
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.LESSER.VPKSoft.About
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.LESSER.VPKSoft.LangLib
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.LESSER.VPKSoft.PosLib
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.LESSER.VPKSoft.SmallCityDatabase
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.LESSER.VPKSoft.Utils
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.VPKSoft.About
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.VPKSoft.LangLib
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.VPKSoft.PosLib
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.VPKSoft.SmallCityDatabase
	Delete /REBOOTOK $INSTDIR\licenses\COPYING.VPKSoft.Utils
	Delete /REBOOTOK $INSTDIR\SunMoonCalendar\COPYING.SunMoonCalsc.cs.txt
	RmDir /REBOOTOK $INSTDIR\licenses
	
	
	Delete /REBOOTOK $INSTDIR\SunMoonCalcs.cs	
	    
	Delete /REBOOTOK "$LOCALAPPDATA\SunMoonCalendar\SunMoonCalendar.sqlite"
	Delete /REBOOTOK "$LOCALAPPDATA\SunMoonCalendar\position.vnml"
	Delete /REBOOTOK "$LOCALAPPDATA\SunMoonCalendar\settings.vnml"
	RmDir /REBOOTOK $LOCALAPPDATA\SunMoonCalendar
	   
    DeleteRegValue HKLM "${REGKEY}\Components" Main
SectionEnd

Section -un.post UNSEC0001
    DeleteRegKey HKLM "SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\$(^Name)"
    Delete /REBOOTOK "$SMPROGRAMS\$StartMenuGroup\$(^UninstallLink).lnk"
    Delete /REBOOTOK $INSTDIR\uninstall_sunmooncal.exe
    DeleteRegValue HKLM "${REGKEY}" StartMenuGroup
    DeleteRegValue HKLM "${REGKEY}" Path
    DeleteRegKey /IfEmpty HKLM "${REGKEY}\Components"
    DeleteRegKey /IfEmpty HKLM "${REGKEY}"
    RmDir /REBOOTOK $SMPROGRAMS\$StartMenuGroup
    RmDir /REBOOTOK "$LOCALAPPDATA\SunMoonCalendar"
    RmDir /REBOOTOK $INSTDIR
SectionEnd

# Installer functions
Function .onInit
    InitPluginsDir
    !insertmacro MUI_LANGDLL_DISPLAY    
FunctionEnd

# Uninstaller functions
Function un.onInit
    ReadRegStr $INSTDIR HKLM "${REGKEY}" Path
    !insertmacro MUI_STARTMENU_GETFOLDER Application $StartMenuGroup
    !insertmacro MUI_UNGETLANGUAGE
    !insertmacro SELECT_UNSECTION Main ${UNSEC0000}
FunctionEnd

# Installer Language Strings
# TODO Update the Language Strings with the appropriate translations.

LangString ^UninstallLink ${LANG_ENGLISH} "Uninstall $(^Name)"
LangString ^UninstallLink ${LANG_FINNISH} "Poista $(^Name)"



