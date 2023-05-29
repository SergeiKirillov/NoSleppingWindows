using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MyLibenNetFramework;

namespace NoSleppingWindows
{
    
    public partial class frmMain : Form
    {
        WorkingReestr reestr = new WorkingReestr("NoSleep");
        public frmMain()
        {
            InitializeComponent();

            axWindowsMediaPlayer1.URL= "NoSleepWindows.mp4";
            reestr.SetStr("PlayVideoSmena", true.ToString());
        }

        private void axWindowsMediaPlayer1_PlayStateChange(object sender, AxWMPLib._WMPOCXEvents_PlayStateChangeEvent e)
        {
            if (((DateTime.Now.Hour == 7) || (DateTime.Now.Hour == 19) || (DateTime.Now.Hour == 17)) && (DateTime.Now.Minute < 2))
            {
                if (reestr.GetBool("PlayVideoSmena"))
                {
                    reestr.SetStr("PlayVideoSmena", false.ToString());
                }
            }


            if (axWindowsMediaPlayer1.playState == WMPLib.WMPPlayState.wmppsStopped)
            {
                if (reestr.GetBool("PlayVideoSmena"))
                {
                    axWindowsMediaPlayer1.Ctlcontrols.play();
                }
                else
                {
                    Application.Exit();
                }


            }
        }

        private void выходToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
