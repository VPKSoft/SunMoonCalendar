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
using System.Globalization;
using VPKSoft.LangLib;
using VPKSoft.About;
using VPKSoft.PosLib;

namespace SunMoonCalendar
{
    public partial class FormMain : DBLangEngineWinforms
    {
        public FormMain()
        {
            // Add this form to be positioned..
            PositionForms.Add(this, PositionCore.SizeChangeMode.MoveTopLeft);
            InitializeComponent();
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("SunMoonCalendar.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("SunMoonCalendar.Messages");

            // create a list of moon phases by illumination of 0 to 1 with names
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.000, Max = 0.125 }, DBLangEngine.GetMessage("msgMoonNewMoon", "New moon|As in new moon")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.125, Max = 0.250 }, DBLangEngine.GetMessage("msgMoonWaxingCrescent", "Waxing Crescent|As in waxing crescent (moon)")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.250, Max = 0.375 }, DBLangEngine.GetMessage("msgMoonFirstQuarter", "First Quarter|As in first quarter (moon)")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.375, Max = 0.500 }, DBLangEngine.GetMessage("msgMoonWaxingGibbous", "Waxing Gibbous|As in waxing gibbous (moon)")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.500, Max = 0.675 }, DBLangEngine.GetMessage("msgMoonFullMoon", "Full Moon|As in full moon")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.675, Max = 0.750 }, DBLangEngine.GetMessage("msgMoonWaningGibbous", "Waning Gibbous|As in waning gibbous (moon)")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.750, Max = 0.875 }, DBLangEngine.GetMessage("msgMoonLastQuarter", "Last Quarter|As in last quarter (moon)")));
            MoonPhases.Add(new KeyValuePair<DoubleSpan, string>(new DoubleSpan { Min = 0.875, Max = 1.000 }, DBLangEngine.GetMessage("msgMoonWaningCrescent", "Waning Crescent|As in waning crescent (moon)")));

            // create a list of compass points and their names
            compassPointsAll.Add(new KeyValuePair<double, string>(  0.0, DBLangEngine.GetMessage("msgCompassPointNorthShort",     "N|As in a letter for the north compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>( 45.0, DBLangEngine.GetMessage("msgCompassPointNorthEastShort", "NE|As in a letters for the north-east compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>( 90.0, DBLangEngine.GetMessage("msgCompassPointEastShort",      "E|As in a letter for the east compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>(135.0, DBLangEngine.GetMessage("msgCompassPointSouthEastShort", "SE|As in a letters for the south-east compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>(180.0, DBLangEngine.GetMessage("msgCompassPointSouthShort",     "S|As in a letter for the south compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>(225.0, DBLangEngine.GetMessage("msgCompassPointSouthWestShort", "SW|As in a letters for the south-west compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>(270.0, DBLangEngine.GetMessage("msgCompassPointWestShort",      "W|As in a letter for the west compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>(270.0, DBLangEngine.GetMessage("msgCompassPointNorthWestShort", "NW|As in a letters for the north-west compass point")));
            compassPointsAll.Add(new KeyValuePair<double, string>(360.0, DBLangEngine.GetMessage("msgCompassPointNorthShort",     "N|As in a letter for the north compass point")));

            // 8 moon phase controls to a list
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase01, lbMoonPhase01));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase02, lbMoonPhase02));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase03, lbMoonPhase03));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase04, lbMoonPhase04));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase05, lbMoonPhase05));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase06, lbMoonPhase06));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase07, lbMoonPhase07));
            moonTable.Add(new KeyValuePair<PictureBox, Label>(pbMoonPhase08, lbMoonPhase08));

            // Set the sun and moon date labels with correct UTC offset value
            UpdateUTCTexts();

            // Get the latitude and longitude from the settings file.
            double lat, lon;

            LocationName = FormSettings.GetLatLonName(out lat, out lon);

            // Get the timezone to use 
            tzUse = FormSettings.GetTimeZoneInfo();

            Latitude = lat;
            Longitude = lon;

//            Latitude = 69.54; // Utsjoki
//            Longitude = 27.01; // Utsjoki

            // Show day specifics for the to day
            ShowDaySpecifics(DateTime.Now);

            // Create a table of future moon phases
            CreateMoonPhasesTable();
        }

        // Get a string representing the Twilights enum and it's time span
        private string GetTwilightDescription(TwilightDateType twilights)
        {
            switch (twilights.Twilight)
            {
                case Twilights.CivilTwilight: return DBLangEngine.GetMessage("msgCivilTwilight", "Civil twilight ({0:HH:mm} - {1:HH:mm})|As in civil twilight", twilights.StartTime, twilights.EndTime);
                case Twilights.NauticalTwilight: return DBLangEngine.GetMessage("msgNauticalTwilight", "Nautical twilight ({0:HH:mm} - {1:HH:mm})|As in nautical twilight", twilights.StartTime, twilights.EndTime);
                case Twilights.AstronomicalTwilight: return DBLangEngine.GetMessage("msgAstronomicalTwilight", "Astronomical twilight ({0:HH:mm} - {1:HH:mm})|As in astronomical twilight", twilights.StartTime, twilights.EndTime);
                case Twilights.Day: return DBLangEngine.GetMessage("msgTwilightDay", "Day ({0:HH:mm} - {1:HH:mm})|Day as in not actually a twilight", twilights.StartTime, twilights.EndTime);
                case Twilights.Night: return DBLangEngine.GetMessage("msgTwilightNight", "Night ({0:HH:mm} - {1:HH:mm})|Night as in not actually a twilight", twilights.StartTime, twilights.EndTime);
                case Twilights.None: return DBLangEngine.GetMessage("msgTwilightNone", "None|None as in not actually a twilight");
                default: return DBLangEngine.GetMessage("msgTwilightNone", "None|None as in not actually a twilight");
            }            
        }

        // future moon phase controls list
        private List<KeyValuePair<PictureBox, Label>> moonTable = new List<KeyValuePair<PictureBox,Label>>();

        // compass point list
        private List<KeyValuePair<double, string>> compassPointsAll = new List<KeyValuePair<double,string>>();

        // Timezone to use
        private static TimeZoneInfo tzUse = TimeZoneInfo.Local;

        // List of moon phases and their values
        private static List<KeyValuePair<DoubleSpan, string>> MoonPhases = new List<KeyValuePair<DoubleSpan, string>>();

        // UTC offset of a given time with TimeZoneInfo given in the settings
        private double GetUtcOffset(DateTime dt)
        {
            return tzUse.GetUtcOffset(dt).TotalHours;
        }

        // the selected month to show
        private DateTime dtCalCurrent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

        // the selected date, which info to show
        private DateTime dtCurrent = DateTime.Now;

        // Just a helper class to keep two double values
        private class DoubleSpan
        {
            public double Min { get; set; }
            public double Max { get; set; }
        }

        // Settings for latitude, longitufe and location name
        private double Latitude { get; set; }
        private double Longitude { get; set; }
        private string LocationName { get; set; }

        #region compass
        // Get a compass name and point by degree
        private KeyValuePair<double, string> GetCompassPointByDegree(double degree)
        {
            degree %= 360.0;
            if (degree < 0)
            {
                degree += 360;
            }

            double degree2 = 0;
            string compassPoint = string.Empty;
            if (degree >= 337.5 || degree < 22.5)
            {
                degree2 = degree >= 337.5 ? 360 - degree : degree;
                compassPoint = compassPointsAll[0].Value;
            }
            else if (degree >= 22.5 && degree < 67.5)
            {
                degree2 = degree - 45.0;
                compassPoint = compassPointsAll[1].Value;
            }
            else if (degree >= 67.5 && degree < 112.5)
            {
                degree2 = degree - 90.0;
                compassPoint = compassPointsAll[2].Value;
            }
            else if (degree >= 112.5 && degree < 157.5)
            {
                degree2 = degree - 135.0;
                compassPoint = compassPointsAll[3].Value;
            }
            else if (degree >= 157.5 && degree < 202.5)
            {
                degree2 = degree - 180.0;
                compassPoint = compassPointsAll[4].Value;
            }
            else if (degree >= 202.5 && degree < 247.5)
            {
                degree2 = degree - 225;
                compassPoint = compassPointsAll[5].Value;
            }
            else if (degree >= 247.5 && degree < 292.5)
            {
                degree2 = degree - 270;
                compassPoint = compassPointsAll[6].Value;
            }
            else if (degree >= 292.5 && degree < 337.5)
            {
                degree2 = degree - 315;
                compassPoint = compassPointsAll[7].Value;
            }

            return new KeyValuePair<double, string>(degree2, compassPoint);
        }
        #endregion

        // Gets a moon phase string by a given illumination value (0-1)
        private static string GetMoonPhaseString(double phase)
        {
            foreach (KeyValuePair<DoubleSpan, string> mp in MoonPhases)
            {
                if (phase >= mp.Key.Min && phase < mp.Key.Max)
                {
                    return mp.Value;
                }
            }
            return string.Empty;
        }

        // A class to visualize the moon
        Moon moon = new Moon();


        // A helper class to store info of a given date at given location
        private class TagInfo
        {
            public double MoonPhase;
            public DateTime Date;
            public int DayLenHours;
            public int DayLenMinutes;
            public string NoonTime;
            public string SunAngle;
            public string MoonPhaseStr;

            public TagInfo(double lat, double lon)
            {
                TagInfoCreate(DateTime.Now, lat, lon);
            }

            public TagInfo(DateTime dt, double lat, double lon)
            {
                TagInfoCreate(dt, lat, lon);
            }

            private void TagInfoCreate(DateTime dt, double lat, double lon)
            {
                dt = new DateTime(dt.Year, dt.Month, dt.Day, 0, 0, 0, DateTimeKind.Utc);
                Date = dt;
                MoonPhase = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonIllumination(dt).phase; 
                MoonPhaseStr = GetMoonPhaseString(MoonPhase);
                double dayLength = 0, angle = 0;
                DateTime srise, sset;
                DateTime noonTime = DateTime.MinValue;
                SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, lat, lon, out srise, out sset);
                if (srise != DateTime.MinValue && sset != DateTime.MinValue)
                {
                    dayLength = Math.Abs((sset - srise).TotalHours);
                }

                DateTime sriseLast = srise;
                for (double n = 0; n < 90; n++)
                {
                    SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, lat, lon, out srise, out sset, n);
                    if (srise == DateTime.MinValue && sriseLast != DateTime.MinValue && n > 0)
                    {
                        DateTime snoon, snoonLast;
                        SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, lat, lon, out snoonLast, out sset, n - 1.0);
                        for (double n2 = n - 1.0; n2 < n; n2 += 0.01)
                        {
                            SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, lat, lon, out snoon, out sset, n2);
                            if (snoon == DateTime.MinValue && snoonLast != DateTime.MinValue)
                            {
                                angle = n2;
                                noonTime = snoonLast;
                                break;
                            }
                            snoonLast = snoon;
                        }
                        break;
                    }
                    sriseLast = srise;
                }
    
                int h = 0, m = 0;
                if (dayLength > 0)
                {
                    h = (int)dayLength;
                    m = (int)((dayLength - (double)h) * 60.0);
                }
                DayLenHours = h;
                DayLenMinutes = m;
                SunAngle = string.Format("{0:F2}°", angle);


                NoonTime = noonTime == DateTime.MinValue ? "-" : noonTime.ToLocalTime(tzUse).ToString("HH:mm");
            }
        }
        

        // Creates a calendar (grid) with day-specific details. Both sun and moon..
        public void CreateCalendar(DateTime dt, bool showCurrent = false)
        {
            int month = dt.Month;
            int day = DateTime.Now.Day;
            dt = new DateTime(dt.Year, dt.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            Text = DBLangEngine.GetMessage("msgCaptionText", "Sun and moon calendar [{0} / {1}, Location: {2}]|As in the main window's caption with current selected month and current location name", dt.Month, dt.Year, LocationName);
            dt = dt.AddDays(-dt.DayOfWeekNum(true));
            DateTime dtSave = dt;

            tlpMoonCalendar.Controls.Clear();

            TagInfo[] tis = new TagInfo[35];

            // the moon, create a grid of detailed moon data of 35 days and align the dates with week numbers
            for (int i = 0; i < 35; i++)
            {
                dt = dt.AddDays(i == 0 ? 0 : 1);
                tis[i] = new TagInfo(dt, Latitude, Longitude);
                Label lb = new Label();
                lb.Text = dt.Day.ToString();

                TableLayoutPanel tlp = new TableLayoutPanel();
                tlp.RowCount = 2;
                tlp.ColumnCount = 2;
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                tlp.Controls.Add(lb, 0, 0);
                tlpMoonCalendar.Controls.Add(tlp, 1 + (i % 7), i / 7);


                tlp.Tag = tis[i];

                tlp.Cursor = Cursors.Hand;
                tlp.Click += dayClick;
                lb.Click += dayClick;

                if ((i % 7) == 0)
                {
                    Panel pnWeek = new Panel();
                    Label lbWeek = new Label();
                    lbWeek.Parent = pnWeek;
                    lbWeek.Location = new Point(0, 0);
                    lbWeek.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    lbWeek.Text = dt.WeekOfTheYear().ToString();
                    pnWeek.Dock = DockStyle.Fill;
                    tlpMoonCalendar.Controls.Add(pnWeek, (i % 7), i / 7);
                }

                tlp.Dock = DockStyle.Fill;
                lb.Location = new Point(0, 0);
                lb.AutoSize = false;
                lb.Size = new System.Drawing.Size(25, 13);
                lb.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                lb.AutoSize = true;
                tlp.BackColor = dt.Month == month ? Color.White : SystemColors.Control;
                Panel pnImage = new Panel();
                tlp.Controls.Add(pnImage, 1, 1);

                pnImage.Dock = DockStyle.Fill;
                pnImage.Paint += moon_Paint;
                pnImage.Click += dayClick;
                pnImage.Tag = tis[i].MoonPhase;

                DateTime mrise, mset;
                bool? alwaysUp, alwaysDown;
                SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonTimes(dt, Latitude, Longitude, out mrise, out mset, out alwaysUp, out alwaysDown);                
                Label lbMoonRiseSet = new Label();
                string mriseStr = mrise == DateTime.MinValue ? "-" : mrise.ToLocalTime(tzUse).ToString("HH:mm");
                string msetStr = mset == DateTime.MinValue ? "-" : mset.ToLocalTime(tzUse).ToString("HH:mm");
                lbMoonRiseSet.Text = string.Format("{0} / {1}", mriseStr, msetStr);
                lbMoonRiseSet.AutoSize = true;
                lbMoonRiseSet.Click += dayClick;
                lbMoonRiseSet.BackColor = Color.Transparent;
                tlp.Controls.Add(lbMoonRiseSet, 1, 0);



                if ((dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday) &&
                    dt.Month == month)
                {
                    tlp.BackColor = Color.NavajoWhite;
                }

                if (day == dt.Day && month == dt.Month && dt.Month == DateTime.Now.Month)
                {
                    tlp.BackColor = Color.PowderBlue;
                }
            }

            tlpSunCalendar.Controls.Clear();
            dt = dtSave;

            // sun, create a grid of detailed sun data of 35 days and align the dates with week numbers
            for (int i = 0; i < 35; i++)
            {
                dt = dt.AddDays(i == 0 ? 0 : 1);
                Label lb = new Label();
                lb.Text = dt.Day.ToString();

                TableLayoutPanel tlp = new TableLayoutPanel();
                tlp.RowCount = 2;
                tlp.ColumnCount = 2;
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                tlp.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
                tlp.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
                tlp.Controls.Add(lb, 0, 0);
                tlpSunCalendar.Controls.Add(tlp, 1 + (i % 7), i / 7);
                tlp.Tag = tis[i];


                tlp.Cursor = Cursors.Hand;
                tlp.Click += dayClick;
                lb.Click += dayClick;

                if ((i % 7) == 0)
                {
                    Panel pnWeek = new Panel();
                    Label lbWeek = new Label();
                    lbWeek.Parent = pnWeek;
                    lbWeek.Location = new Point(0, 0);
                    lbWeek.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                    lbWeek.Text = dt.WeekOfTheYear().ToString();
                    pnWeek.Dock = DockStyle.Fill;
                    tlpSunCalendar.Controls.Add(pnWeek, (i % 7), i / 7);
                }

                tlp.Dock = DockStyle.Fill;
                lb.Location = new Point(0, 0);
                lb.AutoSize = false;
                lb.Size = new System.Drawing.Size(25, 13);
                lb.Anchor = AnchorStyles.Top | AnchorStyles.Left;
                lb.AutoSize = true;
                tlp.BackColor = dt.Month == month ? Color.White : SystemColors.Control;

                Panel pnSunData = new Panel();
                tlp.Controls.Add(pnSunData, 1, 1);
                pnSunData.Dock = DockStyle.Fill;

                Label lbSunRiseSet = new Label();
                DateTime srise, sset;
                SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, Latitude, Longitude, out srise, out sset);
                string sriseStr = srise == DateTime.MinValue ? "-" : srise.ToLocalTime(tzUse).ToString("HH:mm");
                string ssetStr = sset == DateTime.MinValue ? "-" : sset.ToLocalTime(tzUse).ToString("HH:mm");
                lbSunRiseSet.Text = string.Format("{0} / {1}", sriseStr, ssetStr);
                lbSunRiseSet.AutoSize = true;
                pnSunData.Click += dayClick;
                lbSunRiseSet.Click += dayClick;
                tlp.Controls.Add(lbSunRiseSet, 1, 0);

                double dayLength = 0, angle = 0;
                DateTime noonTime = DateTime.MinValue;
                if (srise != DateTime.MinValue && sset != DateTime.MinValue)
                {
                    dayLength = Math.Abs((sset - srise).TotalHours);
                }


                Label lbNoon = new Label();
                DateTime sriseLast = srise;
                for (double n = 0; n < 90; n++)
                {
                    SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, Latitude, Longitude, out srise, out sset, n);
                    if (srise == DateTime.MinValue && sriseLast != DateTime.MinValue && n > 0)
                    {
                        DateTime snoon, snoonLast;
                        SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, Latitude, Longitude, out snoonLast, out sset, n - 1.0);
                        for (double n2 = n - 1.0; n2 < n; n2 += 0.01)
                        {
                            SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, Latitude, Longitude, out snoon, out sset, n2);
                            if (snoon == DateTime.MinValue && snoonLast != DateTime.MinValue)
                            {
                                angle = n2;
                                noonTime = snoonLast;
                                break;
                            }
                            snoonLast = snoon;
                        }
                        break;
                    }
                    sriseLast = srise;
                }
                lbNoon.Parent = pnSunData;
                lbNoon.AutoSize = true;
                int h = 0, m = 0;
                if (dayLength > 0)
                {
                    h = (int)dayLength;
                    m = (int)((dayLength - (double)h) * 60.0);
                }


                lbNoon.Text = DBLangEngine.GetMessage("msgSunInfoShort", "N: {0}{1}A: {2:F2}°{3}L: {4}:{5:D2} h|N as in Noon and A as in angle and L as in day length, h as in hours",
                    noonTime == DateTime.MinValue ? "-" : noonTime.ToLocalTime(tzUse).ToString("HH:mm"), Environment.NewLine, angle, Environment.NewLine, h, m);
                lbNoon.Click += dayClick;

                if ((dt.DayOfWeek == DayOfWeek.Saturday || dt.DayOfWeek == DayOfWeek.Sunday) &&
                    dt.Month == month)
                {
                    tlp.BackColor = Color.NavajoWhite;
                }

                if (day == dt.Day && month == dt.Month && dt.Month == DateTime.Now.Month)
                {
                    tlp.BackColor = Color.PowderBlue;
                }
            }
            if (showCurrent)
            {
                ShowDaySpecifics(dtCurrent, new TagInfo(dtCurrent, Latitude, Longitude));
            }
        }

        // some specific information for a day
        void ShowDaySpecifics(DateTime dt, TagInfo ti = null)
        {
            DateTime srise, sset;
            dt = ti == null ? dt : ti.Date;
            SunMoonCalcs.SunMoonCalcs.SunCalc.GetTimes(dt, Latitude, Longitude, out srise, out sset);
            string sriseStr = srise == DateTime.MinValue ? "-" : srise.ToLocalTime(tzUse).ToString("HH:mm");
            string ssetStr = sset == DateTime.MinValue ? "-" : sset.ToLocalTime(tzUse).ToString("HH:mm");
            lbSunRiseTime.Text = sriseStr;
            lbSunSetTime.Text = ssetStr;

            DateTime mrise, mset;
            bool? alwaysUp, alwaysDown;
            
            SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonTimes(dt, Latitude, Longitude, out mrise, out mset, out alwaysUp, out alwaysDown);
            if (mrise == DateTime.MinValue || mrise == DateTime.MaxValue)
            {
                lbMoonRiseTime.Text = DBLangEngine.GetMessage("msgObjectNoRise", "does not rise|as in the sun/moon doesn't rise");
            }
            else
            {
                lbMoonRiseTime.Text = mrise.ToLocalTime(tzUse).ToString("HH:mm");
            }

            if (mset == DateTime.MinValue || mset == DateTime.MaxValue)
            {
                lbMoonSetTime.Text = DBLangEngine.GetMessage("msgObjectNoSet", "does not set|as in the sun/moon doesn't set");
            }
            else
            {
                lbMoonSetTime.Text = mset.ToLocalTime(tzUse).ToString("HH:mm");
            }
            lbSelectedDateValue.Text = dt.ToShortDateString();

            if (ti != null)
            {
                lbDayLenValue.Text = string.Format("{0}:{1:D2}", ti.DayLenHours, ti.DayLenMinutes);
                lbNoonValue.Text = ti.NoonTime;
                lbSunAngleValue.Text = ti.SunAngle;
                lbMoonPhaseValue.Text = ti.MoonPhaseStr;
                lbMoonIlluminationValue.Text = string.Format("{0:F0}", (ti.MoonPhase > 0.5 ? 1.0 - ti.MoonPhase : ti.MoonPhase) * 200);
            }
            dtCurrent = dt;
            UpdateUTCTexts();
            DrawSunMoonDetails();
        }

        // An event to handle day selection with mouse from the calendar(s)
        void dayClick(object sender, EventArgs e)
        {
            Control c = (sender as Control);
            while (c != null)
            {
                if (c.GetType() == typeof(TableLayoutPanel))
                {
                    if (c.Tag == null)
                    {
                        continue;
                    }
                    TagInfo ti = (TagInfo)c.Tag;
                    ShowDaySpecifics(ti.Date, ti);
                    break;
                }
                c = c.Parent;
            }
        }

        // An event to keep the moon images on the moon calendar "fresh"
        void moon_Paint(object sender, PaintEventArgs e)
        {
            Panel pn = (sender as Panel);

            int wh = pn.Width > pn.Height ? pn.Height : pn.Width;
            moon.DrawSize = wh - 2;
            moon.DrawMoonPhase(e.Graphics, -90, (double)pn.Tag);
        }

        // When the form is shown
        private void FormMain_Load(object sender, EventArgs e)
        {
            CreateCalendar(dtCalCurrent, true);
        }

        // Go one month backward with the calendar(s)
        private void tsbBackMonth_Click(object sender, EventArgs e)
        {
            dtCalCurrent = dtCalCurrent.AddMonths(-1);
            CreateCalendar(dtCalCurrent);
        }

        // Select current date as the selected date
        private void tsbToday_Click(object sender, EventArgs e)
        {
            dtCalCurrent = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            dtCurrent = DateTime.Now;
            UpdateUTCTexts();
            CreateCalendar(dtCalCurrent, true);
        }

        // Go one month forward with the calendar(s)
        private void tsbForwardMonth_Click(object sender, EventArgs e)
        {
            dtCalCurrent = dtCalCurrent.AddMonths(1);
            CreateCalendar(dtCalCurrent);
        }

        // Show an about dialog with a license
        private void mnuAbout_Click(object sender, EventArgs e)
        {
            new FormAbout(this, "GPL", "https://www.gnu.org/licenses/lgpl.txt");
        }

        // Open settings dialog
        private void mnuSettings_Click(object sender, EventArgs e)
        {
            DBLangEngineWinforms frmSettings = new FormSettings();
            frmSettings.Owner = this;
            if (frmSettings.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                double lat, lon;
                LocationName = FormSettings.GetLatLonName(out lat, out lon);
                tzUse = FormSettings.GetTimeZoneInfo();
                Latitude = lat;
                Longitude = lon; 
                CreateCalendar(dtCalCurrent);
            }
        }

        // Radians to degrees conversion
        private double RtD(double r)
        {
            return (r * 180.0 / Math.PI) % 360.0;
        }

        #region extensionclasses
        // An extension class for MoonAzAltDistPa class with date
        private class MoonAzAltDistPaDate : SunMoonCalcs.SunMoonCalcs.MoonCalc.MoonAzAltDistPa
        {
            public MoonAzAltDistPaDate(SunMoonCalcs.SunMoonCalcs.MoonCalc.MoonAzAltDistPa ap, DateTime dt): base()
            {
                this.altitude = ap.altitude;
                this.azimuth = ap.azimuth;
                this.distance = ap.distance;
                this.parallacticAngle = ap.parallacticAngle;
                this.dt = dt;
                this.mpa = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonIllumination(dt);
            }
            public DateTime dt;
            public SunMoonCalcs.SunMoonCalcs.MoonCalc.MoonFracPhaseAngle mpa = null;
        }

        // An extension class for AzAlt class with a date
        private class SunAzAltDate : SunMoonCalcs.SunMoonCalcs.SunCalc.AzAlt
        {
            public SunAzAltDate(SunMoonCalcs.SunMoonCalcs.SunCalc.AzAlt az, DateTime dt)
            {
                this.altitude = az.altitude;
                this.azimuth = az.azimuth;
                this.dt = dt;
            }

            public DateTime dt;
        }
        #endregion

        // twilight zone calculations and helper for drawing them on the misc tab page
        #region twilights
        private enum Twilights
        {
            Night,
            AstronomicalTwilight,
            CivilTwilight,
            NauticalTwilight,
            Day,
            None
        }

        private class ColorRect
        {
            public Rectangle Rectangle;
            public Color Color;
        }

        private class TwilightDateType
        {
            public Twilights Twilight { get; set; }
            public DateTime StartTime { get; set; }
            public DateTime EndTime { get; set; }
        }

        private class TwilightData
        {
            DateTime Start = DateTime.MinValue;
            DateTime End = DateTime.MinValue;
            Twilights Type = Twilights.None;
            double StartDegree = 0, EndDegree = 0;
            List<TwilightData> TwilightList = new List<TwilightData>();
            List<Color> TwilightColours = new List<Color>();

            public TwilightData()
            {
                NightStart = new TwilightData(Twilights.Night, -90, -18);
                NightEnd = new TwilightData(Twilights.Night, -18, -90);
                AstronomicalTwilightStart = new TwilightData(Twilights.AstronomicalTwilight, -18, -12);
                AstronomicalTwilightEnd = new TwilightData(Twilights.AstronomicalTwilight, -12, -18);
                NauticalTwilightStart = new TwilightData(Twilights.NauticalTwilight, -12, -6);
                NauticalTwilightEnd = new TwilightData(Twilights.NauticalTwilight, -6, -12);
                CivilTwilightStart = new TwilightData(Twilights.CivilTwilight, -6, 0);
                CivilTwilightEnd = new TwilightData(Twilights.CivilTwilight, 0, -6);
                DayStartEnd = new TwilightData(Twilights.Day, 0, 0);

                TwilightList.Add(NightStart); // 0
                TwilightList.Add(AstronomicalTwilightStart); // 1
                TwilightList.Add(NauticalTwilightStart); // 2
                TwilightList.Add(CivilTwilightStart); // 3
                TwilightList.Add(DayStartEnd); // 4
                TwilightList.Add(CivilTwilightEnd); // 5
                TwilightList.Add(NauticalTwilightEnd); // 6
                TwilightList.Add(AstronomicalTwilightEnd); // 7
                TwilightList.Add(NightEnd); // 8

                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#1F252D")); // 0
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#263E66")); // 1
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#4773BB")); // 2
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#87A4D3")); // 3
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#DBE9FF")); // 4
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#87A4D3")); // 5
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#4773BB")); // 6
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#263E66")); // 7
                TwilightColours.Add(System.Drawing.ColorTranslator.FromHtml("#1F252D")); // 8
            }

            public int RectangleCount
            {
                get
                {
                    return TwilightColours.Count;
                }
            }

            public bool Empty
            {
                get
                {
                    return Start == DateTime.MinValue || End == DateTime.MinValue || Math.Abs((End - Start).TotalMinutes) < 1;

                }
            }

            private List<ColorRect> Rects = new List<ColorRect>();

            private void FillColorRects(int width, int height)
            {
                Rects = new List<ColorRect>();
                for (int i = 0; i < TwilightList.Count; i++)
                {
                    double totalSeconds = (MaxDate - MinDate).TotalSeconds;
                    TwilightData td = TwilightList[i];
                    double twilightSeconds = (td.End - td.Start).TotalSeconds;
                    double leftSpan = (td.Start - MinDate).TotalSeconds;
                    int w = (int)(((double)width) * twilightSeconds / totalSeconds);
                    int left = (int)(((double)width) * leftSpan / totalSeconds);
                    Rects.Add(new ColorRect { Color = TwilightColours[i], Rectangle = new Rectangle(new Point(left, 0), new Size(w, height)) });
                }

                for (int i = 0; i < Rects.Count - 1; i++)
                {
                    if (TwilightList[i].Empty)
                    {
                        Rects[i].Rectangle = Rectangle.Empty;
                        continue;
                    }
                    if (Rects[i].Rectangle.Right - 1 < Rects[i + 1].Rectangle.Left)
                    {
                        int addPoints = Rects[i + 1].Rectangle.Left - Rects[i].Rectangle.Right;
                        Rects[i].Rectangle.Size = new Size(Rects[i].Rectangle.Width + addPoints, Rects[i].Rectangle.Height);
                    }
                }

                if (!TwilightList[Rects.Count - 1].Empty)
                {
                    if (Rects[Rects.Count - 1].Rectangle.Right - 1 < width)
                    {
                        int addPoints = width - Rects[Rects.Count - 1].Rectangle.Right;
                        Rects[Rects.Count - 1].Rectangle.Size = new Size(Rects[Rects.Count - 1].Rectangle.Width + addPoints, Rects[Rects.Count - 1].Rectangle.Height);
                    }
                }
            }

            public ColorRect this[int index, int width, int height]
            {
                get
                {
                    FillColorRects(width, height);
                    return Rects[index];
                }
            }

            public TwilightDateType this[int index]
            {
                get
                {
                    return new TwilightDateType { Twilight = TwilightList[index].Type, StartTime = TwilightList[index].Start, EndTime = TwilightList[index].End };
                }
            }

            public TwilightDateType this[DateTime dt]
            {
                get
                {
                    for (int i = 0; i < TwilightList.Count; i++)
                    {
                        if (TwilightList[i].Empty)
                        {
                            continue;
                        }

                        if (dt >= TwilightList[i].Start && dt <= TwilightList[i].End)
                        {
                            return new TwilightDateType { Twilight = TwilightList[i].Type, StartTime = TwilightList[i].Start, EndTime = TwilightList[i].End };
                        }
                    }
                    return null;
                }
            }

            public TwilightData(Twilights type, double startDegree, double endDegree)
            {
                Type = type;
                StartDegree = startDegree;
                EndDegree = endDegree;
            }

            int appendIndex = -1;

            DateTime AfterNoonTime = DateTime.MinValue;
            DateTime MinDate = DateTime.MaxValue;
            DateTime MaxDate = DateTime.MinValue;
            

            public void CalcAfterNoonDate(SunAzAltDate[] saz)
            {
                double maxAltitude = -90.0;
                for (int i = 0; i < saz.Length; i++)
                {
                    if (saz[i].altitude > maxAltitude)
                    {
                        maxAltitude = saz[i].altitude;
                        AfterNoonTime = saz[i].dt;
                    }
                }
            }

            public void DoAppendArray(SunAzAltDate[] saz)
            {
                for (int i = 0; i < saz.Length; i++)
                {
                    AppendValue(saz[i].altitude, saz[i].dt);
                }
            }

            public void AppendValue(double degree, DateTime dt)
            {
                if (dt < MinDate)
                {
                    MinDate = dt;
                }

                if (dt > MaxDate)
                {
                    MaxDate = dt;
                }

                bool afterNoon = dt >= AfterNoonTime;
                if (degree < -18.0)
                {
                    if (afterNoon)
                    {
                        appendIndex = 8;
                    }
                    else
                    {
                        appendIndex = 0;
                    }
                }
                else if (degree < -12.0)
                {
                    if (afterNoon)
                    {
                        appendIndex = 7;
                    }
                    else
                    {
                        appendIndex = 1;
                    }
                }
                else if (degree < -6.0)
                {
                    if (afterNoon)
                    {
                        appendIndex = 6;
                    }
                    else
                    {
                        appendIndex = 2;
                    }
                }
                else if (degree < 0.0)
                {
                    if (afterNoon)
                    {
                        appendIndex = 5;
                    }
                    else
                    {
                        appendIndex = 3;
                    }
                }
                else
                {
                    appendIndex = 4;
                }

                if (TwilightList[appendIndex].Start == DateTime.MinValue)
                {
                    TwilightList[appendIndex].Start = dt;
                }

                if (TwilightList[appendIndex].End == DateTime.MinValue)
                {
                    TwilightList[appendIndex].End = dt;
                }

                if (dt < TwilightList[appendIndex].Start)
                {
                    TwilightList[appendIndex].Start = dt;
                }

                if (dt > TwilightList[appendIndex].End)
                {
                    TwilightList[appendIndex].End = dt;
                }
            }

            public void EndAppend()
            {
                foreach (TwilightData td in TwilightList)
                {
                    td.Start = td.Start.ToLocalTime(tzUse);
                    td.End = td.End.ToLocalTime(tzUse);
                }
                AfterNoonTime = AfterNoonTime.ToLocalTime(tzUse);
                MinDate = MinDate.ToLocalTime(tzUse);
                MaxDate = MaxDate.ToLocalTime(tzUse);
            }

            TwilightData NightStart;
            TwilightData NightEnd;
            TwilightData AstronomicalTwilightStart;
            TwilightData AstronomicalTwilightEnd;
            TwilightData NauticalTwilightStart;
            TwilightData NauticalTwilightEnd;
            TwilightData CivilTwilightStart;
            TwilightData CivilTwilightEnd;
            TwilightData DayStartEnd;
        }
        #endregion

        // Draws details (the curves for sun and moon) and the twilight image. Data is passed using Tag properties for mouse move / click (details)
        private void DrawSunMoonDetails()
        {
            if (Utils.ShouldLocalize() != null)
            {
                return; // don't launch on localization!
            }

            Bitmap bm = new Bitmap(pnSunLocation.Size.Width, pnSunLocation.Size.Height);
            double dHeight = (double)pnSunLocation.Size.Height;
            double dWidth = (double)pnSunLocation.Size.Width;
            SunMoonCalcs.SunMoonCalcs.SunCalc.AzAlt az, azLast;
            DateTime drawDate = dtCurrent;

            string[] compassPoints = new string[] { DBLangEngine.GetMessage("msgCompassPointNorthShort", "N|As in a letter for the north compass point"),
                                                    DBLangEngine.GetMessage("msgCompassPointEastShort", "E|As in a letter for the east compass point"),
                                                    DBLangEngine.GetMessage("msgCompassPointSouthShort", "S|As in a letter for the south compass point"),
                                                    DBLangEngine.GetMessage("msgCompassPointWestShort", "W|As in a letter for the west compass point")};

            Font drawFont = new Font(FontFamily.GenericSansSerif, 10, GraphicsUnit.Pixel);

            using (Graphics g = Graphics.FromImage(bm))
            {
                g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), new Size(pnSunLocation.Size.Width, pnSunLocation.Size.Height)));
                DateTime dt = new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 0, 0, 0, DateTimeKind.Utc);

                // Draw main compass point lines on the sun image
                for (int i = 0; i <= 360; i += 90)
                {
                    int x = (int)((double)i / 360.0 * dWidth);

                    if (i != 0 && i != 360)
                    {
                        g.DrawLine(Pens.White, new Point(x, 25), new Point(x, pnSunLocation.Height));
                    }

                    if (i == 0)
                    {
                        g.DrawString(compassPoints[0], drawFont, Brushes.White, new PointF(x, 0));
                        g.DrawString(i + "°", drawFont, Brushes.White, new PointF(x, 10));
                    }
                    else if (i == 360)
                    {
                        string drawStr = compassPoints[0];
                        int offSet = (int)(g.MeasureString(drawStr, drawFont).Width);
                        g.DrawString(drawStr, drawFont, Brushes.White, new PointF(x - offSet, 0));
                        drawStr = i + "°";
                        offSet = (int)(g.MeasureString(drawStr, drawFont).Width);
                        g.DrawString(drawStr, drawFont, Brushes.White, new PointF(x - offSet, 10));
                    }
                    else
                    {
                        string drawStr = compassPoints[(i / 90)];
                        int offSet = (int)(g.MeasureString(drawStr, drawFont).Width / 2);
                        g.DrawString(drawStr, drawFont, Brushes.White, new PointF(x - offSet, 0));
                        drawStr = i + "°";
                        offSet = (int)(g.MeasureString(drawStr, drawFont).Width / 2);
                        g.DrawString(drawStr, drawFont, Brushes.White, new PointF(x - offSet, 10));
                    }
                }

                // Draw the degree lines above horizon (10 to 90 degrees) on the sun image
                for (int i = 0; i < 90; i += 10)
                {
                    int y = (int)((double)i / 90.0 * dHeight);
                    g.DrawLine(Pens.White, new Point(0, y), new Point(pnSunLocation.Width, y));
                    if (i != 0)
                    {
                        g.DrawString((90 - i) + "°", drawFont, Brushes.Aquamarine, new PointF(0, y));
                    }
                }

                azLast = SunMoonCalcs.SunMoonCalcs.SunCalc.GetPosition(dt, Latitude, Longitude);
                SunAzAltDate[] saz = new SunAzAltDate[360];

                // Plot the sun's path on the sun image and collect data for the twiligths and details for mouse moving on the image
                double offset = GetUtcOffset(new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 0, 0, 0, DateTimeKind.Local));
                dt = dt.AddHours(-offset);
                pnSunLocation.Tag = saz;
                int sunDataIndexZero = 0;
                for (int i = 0; i < 1440; i++)
                {
                    dt = dt.AddMinutes(i == 0 ? 0.0 : 1.0);
                    az = SunMoonCalcs.SunMoonCalcs.SunCalc.GetPosition(dt, Latitude, Longitude);

                    azLast.altitude = RtD(azLast.altitude);
                    azLast.azimuth = 180 + RtD(azLast.azimuth);
                    az.altitude = RtD(az.altitude);
                    az.azimuth = 180 + RtD(az.azimuth);
  
                    // fill by degree
                    int sunDataIndex = (int)az.azimuth;
                    
                    if (i == 0 && saz[sunDataIndex] == null)
                    {
                        sunDataIndexZero = sunDataIndex;
                        saz[sunDataIndex] = new SunAzAltDate(az, dt);
                        saz[sunDataIndex].altitude = az.altitude;
                        saz[sunDataIndex].azimuth = az.azimuth;
                    }
                    else if (i != 0 && sunDataIndexZero != sunDataIndex)
                    {
                        saz[sunDataIndex] = new SunAzAltDate(az, dt);
                        saz[sunDataIndex].altitude = az.altitude;
                        saz[sunDataIndex].azimuth = az.azimuth;
                    }

                    if (azLast.altitude > 0 && az.altitude > 0)
                    {
                        int x1 = (int)(azLast.azimuth / 360.0 * dWidth);
                        int y1 = (int)(dHeight - (azLast.altitude / 90.0 * dHeight));
                        int x2 = (int)(az.azimuth / 360.0 * dWidth);
                        int y2 = (int)(dHeight - (az.altitude / 90.0 * dHeight));

                        if ((x1 == 0 && x2 == pnSunLocation.Width - 1) ||
                            (x2 == 0 && x1 == pnSunLocation.Width - 1))
                        {
                            azLast = SunMoonCalcs.SunMoonCalcs.SunCalc.GetPosition(dt, Latitude, Longitude);
                            continue;
                        }

                        if (Math.Abs(x1 - x2) > 20)
                        {
                            azLast = SunMoonCalcs.SunMoonCalcs.SunCalc.GetPosition(dt, Latitude, Longitude);
                            continue;
                        }

                        g.DrawLine(Pens.Yellow, new Point(x1, y1), new Point(x2, y2));
                    }
                    azLast = SunMoonCalcs.SunMoonCalcs.SunCalc.GetPosition(dt, Latitude, Longitude);
                }

                // Create the twilights
                TwilightData twilights = new TwilightData();

                twilights.CalcAfterNoonDate(saz);

                twilights.DoAppendArray(saz);

                twilights.EndAppend();

                pnTwilights.Tag = twilights;
                // Draw the twilights
                DrawTwilights(pnTwilights);

                // Show sun specifics as clicked on the left-most corner of the sun curve image
                ShowSunSpecifics(0, pnSunLocation.Width, saz);

                // If showing today, we can show the twilight detail of current date time value
                if (twilights[DateTime.Now] == null)
                {
                    lbTwilightDescriptionValue.Text = "-";
                }
                else
                {
                    lbTwilightDescriptionValue.Text = GetTwilightDescription(twilights[DateTime.Now]);
                }
            }
            pnSunLocation.BackgroundImage = bm;


            MoonAzAltDistPaDate[] dts = new MoonAzAltDistPaDate[pnMoonLocation.Size.Width];
            pnMoonLocation.Tag = dts;

            // Plot the moons's path on the moon image and collect data for the details for mouse moving on the image

            bm = new Bitmap(pnMoonLocation.Size.Width, pnMoonLocation.Size.Height);
            using (Graphics g = Graphics.FromImage(bm))
            {
                dHeight = (double)pnMoonLocation.Size.Height;
                dWidth = (double)pnMoonLocation.Size.Width;
                g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), new Size(pnMoonLocation.Size.Width, pnMoonLocation.Size.Height)));
                DateTime dt = new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 0, 0, 0, DateTimeKind.Utc);

                g.DrawLine(Pens.White, new Point(0, pnMoonLocation.Height / 2), new Point(pnMoonLocation.Width, pnMoonLocation.Height / 2));
                double offSet = GetUtcOffset(new DateTime(drawDate.Year, drawDate.Month, drawDate.Day, 0, 0, 0, DateTimeKind.Local));
                dt = dt.AddHours(-offSet);
                SunMoonCalcs.SunMoonCalcs.MoonCalc.MoonAzAltDistPa mazLast = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonPosition(dt, Latitude, Longitude), maz;

                for (int i = 0; i < pnMoonLocation.Width; i++)
                {
                    dt = dt.AddMinutes(1440.0 / (double)pnMoonLocation.Width);
                    maz = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonPosition(dt, Latitude, Longitude);
                    mazLast.altitude = RtD(mazLast.altitude);
                    mazLast.azimuth = 180 + RtD(mazLast.azimuth);
                    maz.altitude = RtD(maz.altitude);
                    maz.azimuth = 180 + RtD(maz.azimuth);

                    dts[i] = new MoonAzAltDistPaDate(maz, dt);
                    mazLast = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonPosition(dt, Latitude, Longitude);
                }

                for (int i = 0; i < pnMoonLocation.Width; i++)
                {
                    int y1 = (int)((dHeight / 2) - (dts[i].altitude / 90.0 * dHeight / 2));
                    g.DrawLine(Pens.Gray, new Point(i, y1), new Point(i, y1 + 1));
                }
            }
            pnMoonLocation.BackgroundImage = bm;
            // Show moon specifics as clicked on the left-most corner of the moon curve image
            ShowMoonSpecifics(0, dts);
        }


        // Show specifics on mouse move of the moon curve image
        private void pnMoonLocation_MouseMove(object sender, MouseEventArgs e)
        {
            ShowMoonSpecifics(e.X, (MoonAzAltDistPaDate[])((Panel)(sender)).Tag);
        }

        // Show specifics on mouse move of the sun curve image
        private void pnSunLocation_MouseMove(object sender, MouseEventArgs e)
        {
            ShowSunSpecifics(e.X, ((Panel)sender).Width, (SunAzAltDate[])((Panel)sender).Tag);
        }

        #region specifics
        // visualize the mouse cursor location on the moon curve and show details on the mouse location
        private void ShowMoonSpecifics(int arrayIndex, MoonAzAltDistPaDate[] arraySpecifics)
        {
            pnMoonSlider.Left = arrayIndex;
            MoonAzAltDistPaDate spec = arraySpecifics[arrayIndex];
            lbMoonTimeValue.Text = string.Format("{0:HH:mm}", spec.dt.ToLocalTime(tzUse));
            lbMoonAltitudeValue.Text = string.Format("{0:F2}", spec.altitude);
            lbMoonIlluminationCurveValue.Text = string.Format("{0:F2} {1}", (spec.mpa.phase > 0.5 ? 1.0 - spec.mpa.phase : spec.mpa.phase) * 200, spec.mpa.phase >= 0.5 ? "<" : ">");
            pbMoonImage.Tag = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonIllumination(spec.dt).phase;
            double tmp = spec.mpa.angle - spec.parallacticAngle; // By subtracting the parallacticAngle from the angle one can get the zenith angle of the moons bright limb (anticlockwise). The zenith angle can be used do draw the moon shape from the observers perspective (e.g. moon lying on its back).
            tmp = RtD(tmp);

            PictureBox pn = pbMoonImage;

            int wh = pn.Width > pn.Height ? pn.Height : pn.Width;
            Bitmap bm = new Bitmap(wh, wh);
            moon.DrawSize = wh - 2;
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.FillRectangle(Brushes.White, new Rectangle(0, 0, wh, wh));
                moon.DrawMoonPhase(g, -90, SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonIllumination(spec.dt).phase);
            }

            lbMoonCompassPointValue.Text = GetCompassPointByDegree(spec.azimuth).Value + string.Format(" {0} {1:F0}° ({2:F0}°)", GetCompassPointByDegree(spec.azimuth).Key < 0 ? "-" : "+", Math.Abs(GetCompassPointByDegree(spec.azimuth).Key), spec.azimuth);

            pbMoonImage.Image = bm;
        }

        // visualize the mouse cursor location on the sun curve and show details on the mouse location
        private void ShowSunSpecifics(int mouseX, int width, SunAzAltDate[] saz)
        {
            pnSunSlider.Left = mouseX;
            int arrayIndex = (int)((double)mouseX / (double)width * 360.0);
            SunAzAltDate az = saz[arrayIndex];

            lbSunTimeValue.Text = string.Format("{0:HH:mm}", az.dt.ToLocalTime(tzUse));
            lbSunAltitudeValue.Text = string.Format("{0:F2}", az.altitude);
            lbSunCompassPointValue.Text = GetCompassPointByDegree(az.azimuth).Value + string.Format(" {0} {1:F0}° ({2:F0}°)", GetCompassPointByDegree(az.azimuth).Key < 0 ? "-" : "+", Math.Abs(GetCompassPointByDegree(az.azimuth).Key), az.azimuth);
        }

        // Creates the twilights imgae on a given panel
        private void DrawTwilights(Panel pn)
        {
            TwilightData td = (TwilightData)pn.Tag;
            int w = pn.Width, h = pn.Height;
            Bitmap bm = new Bitmap(w, h);
            using (Graphics g = Graphics.FromImage(bm))
            {
                g.FillRectangle(Brushes.Black, new Rectangle(new Point(0, 0), new Size(w, h)));
                for (int i = 0; i < td.RectangleCount; i++)
                {
                    ColorRect r = td[i, w, h];
                    g.FillRectangle(new SolidBrush(r.Color), r.Rectangle);
                }
            }
            pn.BackgroundImage = bm;
        }
        #endregion

        // Increases the selected date by one day and displays the specifics of that day
        private void tsbIncCurrentDate_Click(object sender, EventArgs e)
        {
            dtCurrent = dtCurrent.AddDays(1);
            UpdateUTCTexts();
            ShowDaySpecifics(dtCurrent, new TagInfo(dtCurrent, Latitude, Longitude));
        }

        // Decreases the selected date by one day and displays the specifics of that day
        private void tsbDecCurrentDate_Click(object sender, EventArgs e)
        {
            dtCurrent = dtCurrent.AddDays(-1);
            UpdateUTCTexts();
            ShowDaySpecifics(dtCurrent, new TagInfo(dtCurrent, Latitude, Longitude));
        }

        // Re-draw the misc tab images and texts on resize, Re-draw the future moon phases table
        private void FormMain_Resize(object sender, EventArgs e)
        {
            DrawSunMoonDetails();
            CreateMoonPhasesTable();
        }

        // Re-draw the misc tab images and texts on the tab's activation
        private void tcMain_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage == tpgMisc)
            {
                DrawSunMoonDetails();
            }
        }

        // Sets the selected date to a date given in a separate dialog and displays the specifics of that day
        private void mnuJumpToTime_Click(object sender, EventArgs e)
        {
            dtCurrent = FormJumpToTime.Execute(this, dtCurrent);
            UpdateUTCTexts();
            ShowDaySpecifics(dtCurrent, new TagInfo(dtCurrent, Latitude, Longitude));
        }

        // Displays the texts on sun and moon time labels and selected date UTC offset
        private void UpdateUTCTexts()
        {
            double utcOffset = GetUtcOffset(dtCurrent.ToLocalTime(tzUse));
            lbMoonTime.Text = DBLangEngine.GetMessage("msgUTCMoon", "Time, Moon (UTC {0} {1:F0})|A text describing a local time with UTC offset of the moon", utcOffset < 0 ? "-" : "+", Math.Abs(utcOffset)) + ":";
            lbSunTime.Text = DBLangEngine.GetMessage("msgUTCSun", "Time, Sun (UTC {0} {1:F0})|A text describing a local time with UTC offset of the sun", utcOffset < 0 ? "-" : "+", Math.Abs(utcOffset)) + ":";
        }

        // Create 8 future moon phases based on the current date
        private void CreateMoonPhasesTable()
        {
            DateTime dt = DateTime.Now.ToUniversalTime();
            int mPhaseIndex = 0; // the index of the image and label to fill the data on
            int phaseCycle = -1; // we don't want to repeat the same phase twice as the resolution is 1 minute
            for (int i = 0; i < 89280 && mPhaseIndex < 8; i++) // at least two months = 31 * 2 * (hours in a day = 24) * (minutes in an hour = 60);
            {
                double ph1 = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonIllumination(dt).phase;
                dt = dt.AddMinutes(1);
                double ph2 = SunMoonCalcs.SunMoonCalcs.MoonCalc.GetMoonIllumination(dt).phase;

                // The phase is always rising, with new moon we can assume that phase of abs(~0.0x - ~0.9x) > 0.5

                if ((Math.Abs(ph1 - ph2) > 0.5) && phaseCycle != 0)
                {
                    moonTable[mPhaseIndex].Key.Image = Properties.Resources.new_moon;
                    moonTable[mPhaseIndex].Value.Text = dt.ToLocalTime(tzUse).ToString("dd.MM.yyyy HH:mm");
                    mPhaseIndex++;
                    phaseCycle = 0;
                }
                else if ((ph2 >= 0.5 && ph1 <= 0.5) && phaseCycle != 1)
                {
                    moonTable[mPhaseIndex].Key.Image = Properties.Resources.moon_full;
                    moonTable[mPhaseIndex].Value.Text = dt.ToLocalTime(tzUse).ToString("dd.MM.yyyy HH:mm");
                    mPhaseIndex++;
                    phaseCycle = 1;
                }
                else if ((ph2 >= 0.25 && ph1 <= 0.25) && phaseCycle != 2)
                {
                    moonTable[mPhaseIndex].Key.Image = Properties.Resources.moon_first_quarter;
                    moonTable[mPhaseIndex].Value.Text = dt.ToLocalTime(tzUse).ToString("dd.MM.yyyy HH:mm");
                    mPhaseIndex++;
                    phaseCycle = 2;
                }
                else if ((ph2 >= 0.75 && ph1 <= 0.75) && phaseCycle != 3)
                {
                    moonTable[mPhaseIndex].Key.Image = Properties.Resources.moon_last_quarter;
                    moonTable[mPhaseIndex].Value.Text = dt.ToLocalTime(tzUse).ToString("dd.MM.yyyy HH:mm");
                    mPhaseIndex++;
                    phaseCycle = 3;
                }
            }
        }

        // The sun's or the moon's curve image size has changed so redraw
        private void detailImage_SizeChanged(object sender, EventArgs e)
        {
            DrawSunMoonDetails();
        }

        // Show a twilight name, it's starting and ending time on mouse click of the twilight panel
        private void pnTwilights_MouseClick(object sender, MouseEventArgs e)
        {
            Panel pn = (Panel)sender;
            TwilightData td = (TwilightData)pn.Tag;
            for (int i = 0; i < td.RectangleCount; i++)
            {
                if (td[i, pn.Width, pn.Height].Rectangle.Contains(e.Location))
                {
                    lbTwilightDescriptionValue.Text = GetTwilightDescription(td[i]);
                }
            }
        }
    }
}
