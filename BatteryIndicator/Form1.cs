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

            OnTimer(null, null);

            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Interval = 10000; // 10 seconds
            timer.Elapsed += new ElapsedEventHandler(OnTimer);
            timer.Start();

            notifyIcon.Visible = true;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            float BatteryPercentage = SystemInformation.PowerStatus.BatteryLifePercent * 100;
            ChangeTooltip();

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

                Point position;
                Font font;
                if (BatteryPercentage == 100)
                {
                    position = new System.Drawing.Point(-10, 6);
                    font = new Font("Arial", 25);
                }
                else
                {
                    position = new System.Drawing.Point(-4, 3);
                    font = new Font("Arial", 30);
                }

                //Write text
                graphicImage.DrawString(BatteryPercentage.ToString(), font, new SolidBrush(System.Drawing.Color.White), position);
                Icon icon = System.Drawing.Icon.FromHandle(bitMapImage.GetHicon());
                notifyIcon.Icon = icon;

                //Clean house
                graphicImage.Dispose();
                bitMapImage.Dispose();
            }
            catch
            {
            }
        }

        private void ChangeTooltip()
        {
            int timeLeft = (int)SystemInformation.PowerStatus.BatteryLifeRemaining / 60;
            int hrLeft = timeLeft / 60;
            int minLeft = timeLeft % 60;
            notifyIcon.Text = "";
            if (timeLeft == 0)
            {
                return;
            }
            if (hrLeft != 0)
            {
                notifyIcon.Text += $"{hrLeft} hr ";
            }
            notifyIcon.Text += $"{minLeft} min remain";
        }
    }
}
