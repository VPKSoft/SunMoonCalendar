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
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VPKSoft.LangLib;

namespace SunMoonCalendar
{
    public partial class FormJumpToTime : DBLangEngineWinforms
    {
        public FormJumpToTime()
        {
            InitializeComponent();
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("SunMoonCalendar.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("SunMoonCalendar.Messages");
        }

        public static DateTime Execute(Form parent, DateTime dt)
        {
            FormJumpToTime fjt = new FormJumpToTime();
            fjt.Owner = parent;
            fjt.dtpSelectDate.Value = dt;
            if (fjt.ShowDialog() == DialogResult.OK)
            {
                return fjt.dtpSelectDate.Value;
            }
            else
            {
                return dt;
            }
        }
    }
}
