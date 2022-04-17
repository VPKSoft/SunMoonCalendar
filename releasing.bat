:: SunMoonCalendar
:: 
:: Shows sun / moon rise and set information.
:: Copyright (C) 2016 VPKSoft, Petteri Kautonen
:: 
:: Contact: vpksoft@vpksoft.net
:: 
:: This file is part of SunMoonCalendar.
:: 
:: SunMoonCalendar is free software: you can redistribute it and/or modify
:: it under the terms of the GNU General Public License as published by
:: the Free Software Foundation, either version 3 of the License, or
:: (at your option) any later version.
:: 
:: SunMoonCalendar is distributed in the hope that it will be useful,
:: but WITHOUT ANY WARRANTY; without even the implied warranty of
:: MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
:: GNU General Public License for more details.
:: 
:: You should have received a copy of the GNU General Public License
:: along with SunMoonCalendar.  If not, see <http://www.gnu.org/licenses/>.


md .\SunMoonCalendar\bin\sunmooncalendar_release
md .\SunMoonCalendar\bin\sunmooncalendar_release\x64
md .\SunMoonCalendar\bin\sunmooncalendar_release\x86

copy .\SunMoonCalendar\bin\Release\SunMoonCalendar.exe .\SunMoonCalendar\bin\sunmooncalendar_release\SunMoonCalendar.exe
copy .\SunMoonCalendar\bin\Release\System.Data.SQLite.dll .\SunMoonCalendar\bin\sunmooncalendar_release\System.Data.SQLite.dll
copy .\SunMoonCalendar\bin\Release\VPKSoft.About.dll .\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.About.dll
copy .\SunMoonCalendar\bin\Release\VPKSoft.LangLib.dll .\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.LangLib.dll
copy .\SunMoonCalendar\bin\Release\VPKSoft.PosLib.dll .\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.PosLib.dll
copy .\SunMoonCalendar\bin\Release\VPKSoft.SmallCityDatabase.dll .\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.SmallCityDatabase.dll
copy .\SunMoonCalendar\bin\Release\VPKSoft.Utils.dll .\SunMoonCalendar\bin\sunmooncalendar_release\VPKSoft.Utils.dll
copy .\SunMoonCalendar\bin\Release\x64\SQLite.Interop.dll .\SunMoonCalendar\bin\sunmooncalendar_release\x64\SQLite.Interop.dll
copy .\SunMoonCalendar\bin\Release\x86\SQLite.Interop.dll .\SunMoonCalendar\bin\sunmooncalendar_release\x86\SQLite.Interop.dll
copy .\SunMoonCalendar\bin\Release\COPYING .\SunMoonCalendar\bin\sunmooncalendar_release\COPYING

pause