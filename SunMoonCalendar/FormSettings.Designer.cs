namespace SunMoonCalendar
{
    partial class FormSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSettings));
            this.lbFindLocation = new System.Windows.Forms.Label();
            this.tbFindLocation = new System.Windows.Forms.TextBox();
            this.cmbSelectLocation = new System.Windows.Forms.ComboBox();
            this.lbLocation = new System.Windows.Forms.Label();
            this.tbLocationName = new System.Windows.Forms.TextBox();
            this.lbLatitude = new System.Windows.Forms.Label();
            this.tbLatitude = new System.Windows.Forms.TextBox();
            this.tbLongitude = new System.Windows.Forms.TextBox();
            this.lbLongitude = new System.Windows.Forms.Label();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.lbTimeZone = new System.Windows.Forms.Label();
            this.cmbTimeZones = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // lbFindLocation
            // 
            this.lbFindLocation.AutoSize = true;
            this.lbFindLocation.Location = new System.Drawing.Point(12, 74);
            this.lbFindLocation.Name = "lbFindLocation";
            this.lbFindLocation.Size = new System.Drawing.Size(93, 13);
            this.lbFindLocation.TabIndex = 6;
            this.lbFindLocation.Text = "Find your location:";
            // 
            // tbFindLocation
            // 
            this.tbFindLocation.Location = new System.Drawing.Point(12, 90);
            this.tbFindLocation.Name = "tbFindLocation";
            this.tbFindLocation.Size = new System.Drawing.Size(334, 20);
            this.tbFindLocation.TabIndex = 7;
            this.tbFindLocation.TextChanged += new System.EventHandler(this.tbFindLocation_TextChanged);
            // 
            // cmbSelectLocation
            // 
            this.cmbSelectLocation.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSelectLocation.FormattingEnabled = true;
            this.cmbSelectLocation.Location = new System.Drawing.Point(12, 116);
            this.cmbSelectLocation.Name = "cmbSelectLocation";
            this.cmbSelectLocation.Size = new System.Drawing.Size(334, 21);
            this.cmbSelectLocation.TabIndex = 8;
            this.cmbSelectLocation.SelectedIndexChanged += new System.EventHandler(this.cmbSelectLocation_SelectedIndexChanged);
            // 
            // lbLocation
            // 
            this.lbLocation.AutoSize = true;
            this.lbLocation.Location = new System.Drawing.Point(12, 9);
            this.lbLocation.Name = "lbLocation";
            this.lbLocation.Size = new System.Drawing.Size(51, 13);
            this.lbLocation.TabIndex = 0;
            this.lbLocation.Text = "Location:";
            // 
            // tbLocationName
            // 
            this.tbLocationName.Location = new System.Drawing.Point(12, 25);
            this.tbLocationName.Name = "tbLocationName";
            this.tbLocationName.Size = new System.Drawing.Size(334, 20);
            this.tbLocationName.TabIndex = 1;
            this.tbLocationName.TextChanged += new System.EventHandler(this.setting_TextChanged);
            // 
            // lbLatitude
            // 
            this.lbLatitude.AutoSize = true;
            this.lbLatitude.Location = new System.Drawing.Point(9, 54);
            this.lbLatitude.Name = "lbLatitude";
            this.lbLatitude.Size = new System.Drawing.Size(48, 13);
            this.lbLatitude.TabIndex = 2;
            this.lbLatitude.Text = "Latitude:";
            // 
            // tbLatitude
            // 
            this.tbLatitude.Location = new System.Drawing.Point(108, 51);
            this.tbLatitude.Name = "tbLatitude";
            this.tbLatitude.Size = new System.Drawing.Size(64, 20);
            this.tbLatitude.TabIndex = 3;
            this.tbLatitude.TextChanged += new System.EventHandler(this.setting_TextChanged);
            // 
            // tbLongitude
            // 
            this.tbLongitude.Location = new System.Drawing.Point(282, 51);
            this.tbLongitude.Name = "tbLongitude";
            this.tbLongitude.Size = new System.Drawing.Size(64, 20);
            this.tbLongitude.TabIndex = 5;
            this.tbLongitude.TextChanged += new System.EventHandler(this.setting_TextChanged);
            // 
            // lbLongitude
            // 
            this.lbLongitude.AutoSize = true;
            this.lbLongitude.Location = new System.Drawing.Point(183, 54);
            this.lbLongitude.Name = "lbLongitude";
            this.lbLongitude.Size = new System.Drawing.Size(57, 13);
            this.lbLongitude.TabIndex = 4;
            this.lbLongitude.Text = "Longitude:";
            // 
            // btOK
            // 
            this.btOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOK.Location = new System.Drawing.Point(12, 191);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 11;
            this.btOK.Text = "OK";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Location = new System.Drawing.Point(271, 191);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(75, 23);
            this.btCancel.TabIndex = 12;
            this.btCancel.Text = "Cancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // lbTimeZone
            // 
            this.lbTimeZone.AutoSize = true;
            this.lbTimeZone.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lbTimeZone.Location = new System.Drawing.Point(9, 148);
            this.lbTimeZone.Name = "lbTimeZone";
            this.lbTimeZone.Size = new System.Drawing.Size(59, 13);
            this.lbTimeZone.TabIndex = 9;
            this.lbTimeZone.Text = "Time zone:";
            this.lbTimeZone.Click += new System.EventHandler(this.lbTimeZone_Click);
            // 
            // cmbTimeZones
            // 
            this.cmbTimeZones.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeZones.FormattingEnabled = true;
            this.cmbTimeZones.Location = new System.Drawing.Point(12, 164);
            this.cmbTimeZones.Name = "cmbTimeZones";
            this.cmbTimeZones.Size = new System.Drawing.Size(334, 21);
            this.cmbTimeZones.TabIndex = 10;
            // 
            // FormSettings
            // 
            this.AcceptButton = this.btOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ClientSize = new System.Drawing.Size(358, 226);
            this.Controls.Add(this.cmbTimeZones);
            this.Controls.Add(this.lbTimeZone);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbLongitude);
            this.Controls.Add(this.lbLongitude);
            this.Controls.Add(this.tbLatitude);
            this.Controls.Add(this.lbLatitude);
            this.Controls.Add(this.tbLocationName);
            this.Controls.Add(this.lbLocation);
            this.Controls.Add(this.cmbSelectLocation);
            this.Controls.Add(this.tbFindLocation);
            this.Controls.Add(this.lbFindLocation);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSettings";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Settings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbFindLocation;
        private System.Windows.Forms.TextBox tbFindLocation;
        private System.Windows.Forms.ComboBox cmbSelectLocation;
        private System.Windows.Forms.Label lbLocation;
        private System.Windows.Forms.TextBox tbLocationName;
        private System.Windows.Forms.Label lbLatitude;
        private System.Windows.Forms.TextBox tbLatitude;
        private System.Windows.Forms.TextBox tbLongitude;
        private System.Windows.Forms.Label lbLongitude;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbTimeZone;
        private System.Windows.Forms.ComboBox cmbTimeZones;
    }
}