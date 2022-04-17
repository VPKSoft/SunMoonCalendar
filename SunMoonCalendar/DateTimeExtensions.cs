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
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace SunMoonCalendar
{
    public static class DateTimeExtensions
    {
        public static int DayOfWeekNum(this DateTime dt, bool startFromMonday, bool startZero = true)
        {
            int dayofweek = (int)dt.DayOfWeek;
            int addDay = startZero ? 0 : 1;

            if (startFromMonday && dayofweek == 0)
            {
                dayofweek = startZero ? 6 : 7;
            }

            return startZero ? dayofweek - 1 : dayofweek;
        }

        public static DateTime ToLocalTime(this DateTime dt, TimeZoneInfo tzi)
        {
            if (dt.Kind == DateTimeKind.Local)
            {
                return dt;
            }
            return TimeZoneInfo.ConvertTimeFromUtc(dt, tzi);
        }

        public static int WeekOfTheYear(this DateTime dt)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.CurrentInfo;
            Calendar cal = dfi.Calendar;

            return cal.GetWeekOfYear(dt, dfi.CalendarWeekRule, dfi.FirstDayOfWeek);
        }
    }
}
