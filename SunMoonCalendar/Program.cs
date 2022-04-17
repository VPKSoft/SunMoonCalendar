#region License
/*
SunMoonCalendar

Shows sun / moon rise and set information.
Copyright © 2016 VPKSoft, Petteri Kautonen

Contact: vpksoft@vpksoft.net

This file is part of SunMoonCalendar.

SunMoonCalendar is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

SunMoonCalendar is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with SunMoonCalendar.  If not, see <http://www.gnu.org/licenses/>.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace SunMoonCalendar
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            DBLangEngine.DBName = "SunMoonCalendar.sqlite";

            if (Utils.ShouldLocalize() != null) // Localize and exit.
            {
                new FormMain();
                new FormSettings();
                new FormJumpToTime();
                return;
            } 
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FormMain());
        }
    }
}
