using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NoSleppingWindows
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();

            axWindowsMediaPlayer1.URL= "NoSleepWindows.mp4";
        }
    }
}
