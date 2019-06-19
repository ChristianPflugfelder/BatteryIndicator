using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers;

namespace BatteryIndicator
{
    public partial class Form1 : Form
    {
        private readonly NotifyIcon notifyIcon = new NotifyIcon();
        private float LastPercentage;
        public Form1()
        {
            InitializeComponent();

            float BatteryPercentage = SystemInformation.PowerStatus.BatteryLifePercent * 100;
            CreateIcon(BatteryPercentage);
            LastPercentage = BatteryPercentage;

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();

            notifyIcon.Visible = true;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            float BatteryPercentage = SystemInformation.PowerStatus.BatteryLifePercent * 100;

            if (LastPercentage != BatteryPercentage)
            {
                CreateIcon(BatteryPercentage);
                LastPercentage = BatteryPercentage;
            }
        }

        private void CreateIcon(float BatteryPercentage)
        {
            try
            {
                //Load the Image to be written on
                Bitmap bitMapImage = new Bitmap(50, 50);
                Graphics graphicImage = Graphics.FromImage(bitMapImage);

                //Write text
                graphicImage.DrawString(BatteryPercentage.ToString(), new Font("Arial", 25), new SolidBrush(System.Drawing.Color.White), new System.Drawing.Point(-5, 3));

                Icon icon = System.Drawing.Icon.FromHandle(bitMapImage.GetHicon());
                notifyIcon.Icon = icon;

                //Clean house
                graphicImage.Dispose();
                bitMapImage.Dispose();

                //Change Tooltip
                int timeLeft = (int)SystemInformation.PowerStatus.BatteryLifeRemaining / 60;
                int hrLeft = timeLeft / 60;
                int minLeft = timeLeft % 60;
                if (hrLeft != 0)
                {
                    notifyIcon.Text = $"{hrLeft} hr";
                }
                notifyIcon.Text += $"{minLeft} min";
            }
            catch
            {
            }
        }
    }
}
