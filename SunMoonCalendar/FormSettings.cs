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
using VU = VPKSoft.Utils;
using VPKSoft.SmallCityDatabase;

namespace SunMoonCalendar
{
    public partial class FormSettings : DBLangEngineWinforms
    {
        private Cities cities;
        private static System.Globalization.CultureInfo ci = System.Globalization.CultureInfo.InvariantCulture;

        public FormSettings()
        {
            InitializeComponent();
            if (Utils.ShouldLocalize() != null)
            {
                DBLangEngine.InitalizeLanguage("SunMoonCalendar.Messages", Utils.ShouldLocalize(), false);
                return; // After localization don't do anything more.
            }
            DBLangEngine.InitalizeLanguage("SunMoonCalendar.Messages");
            cities = new Cities();

            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");
            tbLatitude.Text = vnml["latitude", "value", "61.68"].ToString();
            tbLongitude.Text = vnml["longitude", "value", "27.25"].ToString();
            tbLocationName.Text = vnml["location", "value", "Mikkeli"].ToString();

            var zones = TimeZoneInfo.GetSystemTimeZones();
            
            string tz = vnml["tz", "value", TimeZoneInfo.Local.Id].ToString();
            TimeZoneInfo selectInfo = TimeZoneInfo.Local;
            foreach (TimeZoneInfo info in zones)
            {
                cmbTimeZones.Items.Add(info);
                if (info.Id == tz)
                {
                    selectInfo = info;
                }
            }
            cmbTimeZones.SelectedItem = selectInfo;
        }

        public static string GetLatLonName(out double lat, out double lon)
        {
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");

            if (vnml["latitude", "value"] != null)
            {
                lat = double.Parse(vnml["latitude", "value"].ToString(), ci);
            }
            else
            {
                lat = (double)vnml["latitude", "value", 61.68];
            }

            if (vnml["longitude", "value"] != null)
            {
                lon = double.Parse(vnml["longitude", "value"].ToString(), ci);
            }
            else
            {
                lon = (double)vnml["longitude", "value", 61.68];
            }

            return vnml["location", "value", "Mikkeli"].ToString();
        }

        public static TimeZoneInfo GetTimeZoneInfo()
        {
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();
            vnml.Load(VU.Paths.GetAppSettingsFolder() + "settings.vnml");
            return TimeZoneInfo.FindSystemTimeZoneById(vnml["tz", "value", TimeZoneInfo.Local.Id].ToString());
        }


        private void tbFindLocation_TextChanged(object sender, EventArgs e)
        {
            List<CityLatLonCoordinate> cityList = cities.FindCityByName(tbFindLocation.Text);
            cmbSelectLocation.Items.Clear();
            cmbSelectLocation.Items.AddRange(cityList.ToArray());
            if (cmbSelectLocation.Items.Count > 0)
            {
                cmbSelectLocation.SelectedIndex = 0;
            }
        }

        private void cmbSelectLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSelectLocation.SelectedItem != null)
            {
                CityLatLonCoordinate coord = (CityLatLonCoordinate)cmbSelectLocation.SelectedItem;
                tbLocationName.Text = coord.CityName;
                tbLatitude.Text = coord.Latitude.ToString(ci);
                tbLongitude.Text = coord.Longitude.ToString(ci);
            }
        }

        private void btOK_Click(object sender, EventArgs e)
        {
            VU.VPKNml vnml = new VU.VPKNml();
            VU.Paths.MakeAppSettingsFolder();            
            vnml["latitude", "value"] = tbLatitude.Text;
            vnml["longitude", "value"] = tbLongitude.Text;
            vnml["location", "value"] = tbLocationName.Text;
            vnml["tz", "value"] = ((TimeZoneInfo)cmbTimeZones.SelectedItem).Id;
            vnml.Save(VU.Paths.GetAppSettingsFolder() + "settings.vnml");
        }

        private void setting_TextChanged(object sender, EventArgs e)
        {
            try 
            {
                double.Parse(tbLatitude.Text, ci);
                double.Parse(tbLongitude.Text, ci);
                btOK.Enabled = true && tbLocationName.Text != string.Empty;
            }
            catch
            {
                btOK.Enabled = false;
            }
        }

        private void lbTimeZone_Click(object sender, EventArgs e)
        {
            var zones = TimeZoneInfo.GetSystemTimeZones();

            cmbTimeZones.Items.Clear();

            TimeZoneInfo selectInfo = TimeZoneInfo.Local;
            foreach (TimeZoneInfo info in zones)
            {
                cmbTimeZones.Items.Add(info);
            }
            cmbTimeZones.SelectedItem = selectInfo;
        }
    }
}
