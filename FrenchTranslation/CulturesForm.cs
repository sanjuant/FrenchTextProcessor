// Copyright © 2021, https://sanjuant.fr - All rights reserved. 
// https://github.com/sanjuant/FrenchTextProcessor

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using static FrenchTranslation.FrenchTranslation;

namespace FrenchTranslation
{
    public partial class CulturesForm : Form
    {
        static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);
        static readonly IntPtr HWND_NOTOPMOST = new IntPtr(-2);
        static readonly IntPtr HWND_TOP = new IntPtr(0);
        static readonly IntPtr HWND_BOTTOM = new IntPtr(1);
        const UInt32 SWP_NOSIZE = 0x0001;
        const UInt32 SWP_NOMOVE = 0x0002;
        const UInt32 TOPMOST_FLAGS = SWP_NOMOVE | SWP_NOSIZE;
        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);

        private readonly FrenchTranslation ft = FrenchTranslation.Instance;

        public CulturesForm()
        {
            InitializeComponent();
        }

        private void CulturesForm_Load(object sender, EventArgs e)
        {
            SetWindowPos(this.Handle, HWND_TOPMOST, 0, 0, 0, 0, TOPMOST_FLAGS);
            RefreshForm(ft.configCulture);
        }

        private void button_btn_save_Click(object sender, EventArgs e)
        {
            ft.RemoveCultureToList(ft.configCulture.Sturgians);
            ft.configCulture.Sturgians.Male = this.stu_textBox_male.Text;
            ft.configCulture.Sturgians.Female = this.stu_textBox_female.Text;
            ft.configCulture.Sturgians.MalePlural = this.stu_textBox_maleplural.Text;
            ft.configCulture.Sturgians.FemalePlural = this.stu_textBox_femaleplural.Text;
            ft.configCulture.Sturgians.Region = this.stu_textBox_region.Text;
            ft.AddCultureToList(ft.configCulture.Sturgians);

            ft.RemoveCultureToList(ft.configCulture.Vlandians);
            ft.configCulture.Vlandians.Male = this.vla_textBox_male.Text;
            ft.configCulture.Vlandians.Female = this.vla_textBox_female.Text;
            ft.configCulture.Vlandians.MalePlural = this.vla_textBox_maleplural.Text;
            ft.configCulture.Vlandians.FemalePlural = this.vla_textBox_femaleplural.Text;
            ft.configCulture.Vlandians.Region = this.vla_textBox_region.Text;
            ft.AddCultureToList(ft.configCulture.Vlandians);

            ft.RemoveCultureToList(ft.configCulture.Empire);
            ft.configCulture.Empire.Male = this.emp_textBox_male.Text;
            ft.configCulture.Empire.Female = this.emp_textBox_female.Text;
            ft.configCulture.Empire.MalePlural = this.emp_textBox_maleplural.Text;
            ft.configCulture.Empire.FemalePlural = this.emp_textBox_femaleplural.Text;
            ft.configCulture.Empire.Region = this.emp_textBox_region.Text;
            ft.AddCultureToList(ft.configCulture.Empire);

            ft.RemoveCultureToList(ft.configCulture.Aserai);
            ft.configCulture.Aserai.Male = this.ase_textBox_male.Text;
            ft.configCulture.Aserai.Female = this.ase_textBox_female.Text;
            ft.configCulture.Aserai.MalePlural = this.ase_textBox_maleplural.Text;
            ft.configCulture.Aserai.FemalePlural = this.ase_textBox_femaleplural.Text;
            ft.configCulture.Aserai.Region = this.ase_textBox_region.Text;
            ft.AddCultureToList(ft.configCulture.Aserai);

            ft.RemoveCultureToList(ft.configCulture.Khuzaits);
            ft.configCulture.Khuzaits.Male = this.khu_textBox_male.Text;
            ft.configCulture.Khuzaits.Female = this.khu_textBox_female.Text;
            ft.configCulture.Khuzaits.MalePlural = this.khu_textBox_maleplural.Text;
            ft.configCulture.Khuzaits.FemalePlural = this.khu_textBox_femaleplural.Text;
            ft.configCulture.Khuzaits.Region = this.khu_textBox_region.Text;
            ft.AddCultureToList(ft.configCulture.Khuzaits);

            ft.RemoveCultureToList(ft.configCulture.Battanians);
            ft.configCulture.Battanians.Male = this.bat_textBox_male.Text;
            ft.configCulture.Battanians.Female = this.bat_textBox_female.Text;
            ft.configCulture.Battanians.MalePlural = this.bat_textBox_maleplural.Text;
            ft.configCulture.Battanians.FemalePlural = this.bat_textBox_femaleplural.Text;
            ft.configCulture.Battanians.Region = this.bat_textBox_region.Text;
            ft.AddCultureToList(ft.configCulture.Battanians);

            ft.SaveData();
            this.Close();
        }

        private void button_btn_reset_Click(object sender, EventArgs e)
        {
            ConfigCulture configCulture = ft.GetBaseConfig();
            RefreshForm(configCulture);
        }

        private void button_btn_default_Click(object sender, EventArgs e)
        {
            this.stu_textBox_male.Text = "Sturgian";
            this.stu_textBox_female.Text = "Sturgian";
            this.stu_textBox_maleplural.Text = "Sturgians";
            this.stu_textBox_femaleplural.Text = "Sturgians";
            this.stu_textBox_region.Text = "Sturgia";

            this.vla_textBox_male.Text = "Vlandian";
            this.vla_textBox_female.Text = "Vlandian";
            this.vla_textBox_maleplural.Text = "Vlandians";
            this.vla_textBox_femaleplural.Text = "Vlandians";
            this.vla_textBox_region.Text = "Vlandia";

            this.emp_textBox_male.Text = "Impérial";
            this.emp_textBox_female.Text = "Impériale";
            this.emp_textBox_maleplural.Text = "Impériaux";
            this.emp_textBox_femaleplural.Text = "Impériales";
            this.emp_textBox_region.Text = "Empire";

            this.ase_textBox_male.Text = "Aserai";
            this.ase_textBox_female.Text = "Aserai";
            this.ase_textBox_maleplural.Text = "Aserai";
            this.ase_textBox_femaleplural.Text = "Aserai";
            this.ase_textBox_region.Text = "Aserai";

            this.khu_textBox_male.Text = "Khuzait";
            this.khu_textBox_female.Text = "Khuzait";
            this.khu_textBox_maleplural.Text = "Khuzaits";
            this.khu_textBox_femaleplural.Text = "Khuzaits";
            this.khu_textBox_region.Text = "Khuzait";

            this.bat_textBox_male.Text = "Battanian";
            this.bat_textBox_female.Text = "Battanian";
            this.bat_textBox_maleplural.Text = "Battanians";
            this.bat_textBox_femaleplural.Text = "Battanians";
            this.bat_textBox_region.Text = "Battania";
        }

        private void RefreshForm(ConfigCulture configCulture)
        {
            this.stu_textBox_male.Text = configCulture.Sturgians.Male;
            this.stu_textBox_female.Text = configCulture.Sturgians.Female;
            this.stu_textBox_maleplural.Text = configCulture.Sturgians.MalePlural;
            this.stu_textBox_femaleplural.Text = configCulture.Sturgians.FemalePlural;
            this.stu_textBox_region.Text = configCulture.Sturgians.Region;

            this.vla_textBox_male.Text = configCulture.Vlandians.Male;
            this.vla_textBox_female.Text = configCulture.Vlandians.Female;
            this.vla_textBox_maleplural.Text = configCulture.Vlandians.MalePlural;
            this.vla_textBox_femaleplural.Text = configCulture.Vlandians.FemalePlural;
            this.vla_textBox_region.Text = configCulture.Vlandians.Region;

            this.emp_textBox_male.Text = configCulture.Empire.Male;
            this.emp_textBox_female.Text = configCulture.Empire.Female;
            this.emp_textBox_maleplural.Text = configCulture.Empire.MalePlural;
            this.emp_textBox_femaleplural.Text = configCulture.Empire.FemalePlural;
            this.emp_textBox_region.Text = configCulture.Empire.Region;

            this.ase_textBox_male.Text = configCulture.Aserai.Male;
            this.ase_textBox_female.Text = configCulture.Aserai.Female;
            this.ase_textBox_maleplural.Text = configCulture.Aserai.MalePlural;
            this.ase_textBox_femaleplural.Text = configCulture.Aserai.FemalePlural;
            this.ase_textBox_region.Text = configCulture.Aserai.Region;

            this.khu_textBox_male.Text = configCulture.Khuzaits.Male;
            this.khu_textBox_female.Text = configCulture.Khuzaits.Female;
            this.khu_textBox_maleplural.Text = configCulture.Khuzaits.MalePlural;
            this.khu_textBox_femaleplural.Text = configCulture.Khuzaits.FemalePlural;
            this.khu_textBox_region.Text = configCulture.Khuzaits.Region;

            this.bat_textBox_male.Text = configCulture.Battanians.Male;
            this.bat_textBox_female.Text = configCulture.Battanians.Female;
            this.bat_textBox_maleplural.Text = configCulture.Battanians.MalePlural;
            this.bat_textBox_femaleplural.Text = configCulture.Battanians.FemalePlural;
            this.bat_textBox_region.Text = configCulture.Battanians.Region;
        }
    }
}
