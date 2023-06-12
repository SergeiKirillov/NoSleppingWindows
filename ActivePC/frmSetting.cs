using MyLibenNetFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ActivePC
{
    public partial class frmSetting : Form
    {
        WorkingReestr reestr;
        public frmSetting()
        {
            InitializeComponent();
            reestr = new WorkingReestr("ActivePC");
        }

       

        private void chkAutoStart_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAutoStart.Checked)
            {
                AutoStartReestr.SetAutostartWindows(true, Assembly.GetExecutingAssembly().Location, "ActivePC");
                lblAutoStart.Text = AutoStartReestr.strGetAutostartWindows("ActivePC");
            }
            else
            {
                AutoStartReestr.SetAutostartWindows(false, Assembly.GetExecutingAssembly().Location, "ActivePC");
                lblAutoStart.Text = "";
            }
        }

        private void frmSetting_Load(object sender, EventArgs e)
        {
            if (AutoStartReestr.GetAutostartWindows("ActivePC"))
            {
                chkAutoStart.Checked = true;
                lblAutoStart.Text = AutoStartReestr.strGetAutostartWindows("ActivePC");
            }
            else
            {
                chkAutoStart.Checked = false;
                lblAutoStart.Text = "";
            }
            chk5Brigada.Checked = reestr.GetBool("8Hour");
            chkSmena.Checked = reestr.GetBool("12Hour");

        }

              

        private void chk5Brigada_CheckedChanged(object sender, EventArgs e)
        {
            if (chk5Brigada.Checked) 
            {
                reestr.SetBool("8Hour", true);
            }
            else
            {
                reestr.SetBool("8Hour", false);
            }
        }

        private void chkSmena_CheckedChanged(object sender, EventArgs e)
        {
            if (chk5Brigada.Checked)
            {
                reestr.SetBool("12Hour", true);
            }
            else
            {
                reestr.SetBool("12Hour", false);
            }
        }
    }
}
