using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        List<Station> stations;
        TabPage srcPage;

        public Form1()
        {
            InitializeComponent();
            srcPage = tabControl1.TabPages[0];
            tabControl1.TabPages.Clear();
            stations = new List<Station>();
        }

        private void addTabPage(string name, int index)
        {
            TabPage page = new TabPage();
            page.Text = name;

            Button getScrn = new Button();
            getScrn.Click += new System.EventHandler(this.getScreenshot);
            getScrn.Location = this.getScrn.Location;
            getScrn.Size = this.getScrn.Size;
            getScrn.Text = this.getScrn.Text;
            getScrn.Name = this.getScrn.Name + (stations.Count - 1).ToString();
            getScrn.Tag = stations.Count - 1;

            Button shutdown = new Button();
            shutdown.Click += new EventHandler(this.shutdownStation);
            shutdown.Location = this.shutdown.Location;
            shutdown.Size = this.shutdown.Size;
            shutdown.Text = this.shutdown.Text;
            shutdown.Name = this.shutdown.Name + (stations.Count - 1).ToString();
            shutdown.Tag = stations.Count - 1;

            Button reboot = new Button();
            reboot.Click += new EventHandler(this.rebootStation);
            reboot.Location = this.reboot.Location;
            reboot.Size = this.reboot.Size;
            reboot.Text = this.reboot.Text;
            reboot.Name = this.reboot.Name + (stations.Count - 1).ToString();
            reboot.Tag = stations.Count - 1;

            Button netPing = new Button();
            netPing.Click += new EventHandler(this.setNetping);
            netPing.Location = this.netPing.Location;
            netPing.Size = this.netPing.Size;
            netPing.Text = this.netPing.Text;
            netPing.Name = this.netPing.Name + (stations.Count - 1).ToString();
            netPing.Tag = stations.Count - 1;

            PictureBox screenBox = new PictureBox();
            screenBox.Size = this.screenBox.Size;
            screenBox.SizeMode = this.screenBox.SizeMode;
            screenBox.Name = this.screenBox.Name + (stations.Count - 1).ToString();
            screenBox.Image = this.screenBox.Image;
            screenBox.Location = this.screenBox.Location;
            screenBox.BackColor = Color.White;
            screenBox.Tag = stations.Count - 1;

            Label statusLabel = new Label();
            statusLabel.Location = this.statusLabel.Location;
            statusLabel.Text = "";
            statusLabel.AutoSize = this.statusLabel.AutoSize;
            statusLabel.Size = this.statusLabel.Size;
            statusLabel.TextAlign = this.statusLabel.TextAlign;
            statusLabel.Name = this.statusLabel.Name + (stations.Count - 1).ToString();
            statusLabel.Tag = stations.Count - 1;

            page.Controls.Add(getScrn);
            page.Controls.Add(shutdown);
            page.Controls.Add(reboot);
            page.Controls.Add(netPing);
            page.Controls.Add(screenBox);
            page.Controls.Add(statusLabel);

            tabControl1.TabPages.Add(page);
        }

        private async void getScreenshot(object sender, EventArgs e)
        {
            int i = Convert.ToInt32((sender as Button).Tag);
            PictureBox screenBox = tabControl1.TabPages[i].Controls["screenBox" + i] as PictureBox;
            screenBox.Image = Properties.Resources.loading;
            int timeout = 2000;


            var task = stations[i].getScreenshotAsync();
            if (await Task.WhenAny(task, Task.Delay(timeout)) == task)
            {
                screenBox.Image = task.Result;
                if (task.Result == null)
                {
                    updateStatus("Ошибка при получении скриншота!", i);
                }
            }
            else
            {
                updateStatus("Ошибка при получении скриншота!", i);
                screenBox.Image = null;
            }
            
            
        }

        private void settingButton_Click(object sender, EventArgs e)
        {
            Point ptLowerLeft = new Point(0, settingButton.Height);
            ptLowerLeft = settingButton.PointToScreen(ptLowerLeft);
            contextMenuStrip1.Show(ptLowerLeft);
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                string json = JsonConvert.SerializeObject(stations);
                File.WriteAllText("config.txt", json);
            }
            catch (Exception ex)
            {

            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                string json = File.ReadAllText("config.txt");
                var stations = JsonConvert.DeserializeObject<List<Station>>(json);
                foreach (var s in stations)
                {
                    Station station = new Station(s.hostName, s.port, s.netpingIp);
                    this.stations.Add(station);
                    addTabPage(s.hostName, this.stations.Count);
                    station.pingAsync();
                }
            }
            catch (Exception ex)
            {

            }
            updateTabPage(0);
        }

        private async void updateTabPage(int i)
        {
            TabPage currentTab = tabControl1.TabPages[i];

            updateStatus("", i);

            bool pingable = await stations[i].pingAsync();

            if (!pingable)
            {
                updateStatus("Машина не пингуется!", i);
            }
            else
            {
                (currentTab.Controls["getScrn" + i] as Button).PerformClick();
            }

            Button netpingButton = currentTab.Controls["netPing" + i.ToString()] as Button;
            netpingButton.Enabled = false;

            string netpingStatus = await stations[i].getNetpingStatusAsync();

            switch (netpingStatus)
            {
                case "on":
                    {
                        netpingButton.Enabled = true;
                        netpingButton.Text = "Выключить NetPing";
                        break;
                    }

                case "off":
                    {
                        netpingButton.Enabled = true;
                        netpingButton.Text = "Включить NetPing";
                        break;
                    }

                case "error":
                    {
                        netpingButton.Enabled = false;
                        updateStatus("Не удалось получить состояние NetPing!", i);
                        break;
                    }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int i = tabControl1.SelectedIndex;
            if (i == -1) return;
            updateTabPage(i);
        }

        private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
        {
            int i = e.Index;
            Brush br;
            if (stations[i].isPingable)
            {
                br = new SolidBrush(Color.LightGreen);
            } else
            {
                br = new SolidBrush(Color.PaleVioletRed);
            }
            e.Graphics.FillRectangle(br, e.Bounds);
            SizeF sz = e.Graphics.MeasureString(tabControl1.TabPages[i].Text, e.Font);
            e.Graphics.DrawString(tabControl1.TabPages[i].Text, e.Font, Brushes.Black, e.Bounds.Left + (e.Bounds.Width - sz.Width) / 2, e.Bounds.Top + (e.Bounds.Height - sz.Height) / 2 + 1);
        }

        private bool confirmDialog()
        {
            var confirmResult = MessageBox.Show("Вы уверены?", "Подтверждение", MessageBoxButtons.YesNo);
            return confirmResult == DialogResult.Yes;
        }

        private async void shutdownStation(object sender, EventArgs e)
        {
            if (!confirmDialog()) return;
            Button button = sender as Button;
            int to = Convert.ToInt32(button.Tag);
            bool success = await stations[to].shutdown();
            if (!success)
            {
                updateStatus("Ошибка при отправке запроса!", to);
            }                        
        }

        private async void rebootStation(object sender, EventArgs e)
        {
            if (!confirmDialog()) return;
            Button button = sender as Button;
            int to = Convert.ToInt32(button.Tag);
            bool success = await stations[to].reboot();
            if (!success)
            {
                updateStatus("Ошибка при отправке запроса!", to);
            }
        }

        private async void setNetping(object sender, EventArgs e)
        {
            if (!confirmDialog()) return;
            Button button = sender as Button;
            int to = Convert.ToInt32(button.Tag);
            string netpingStatus = await stations[to].setNetping();

            if (netpingStatus == "ok")
            {
                if (button.Text.StartsWith("Вы"))
                {
                    button.Text = "Включить NetPing";
                } else
                {
                    button.Text = "Выключить NetPing";
                }
            } else
            {

            }

        }
        
        private void addToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 settingsForm = new Form2();
            DialogResult res = settingsForm.ShowDialog();
            if (res == DialogResult.OK)
            {
                stations.Add(new Station(settingsForm.nameBox.Text, Convert.ToInt32(settingsForm.portBox.Text), settingsForm.netpingIpBox.Text));
                addTabPage(settingsForm.nameBox.Text, stations.Count - 1);
            }
        }

        private void deleteТекущуюToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int i = tabControl1.SelectedIndex;
            stations.RemoveAt(i);
            tabControl1.Controls.RemoveAt(i);
        }

        private void updateStatus(string status, int i)
        {
            Label statusLabel = tabControl1.TabPages[i].Controls["statusLabel" + i] as Label;
            if (status != "")
                statusLabel.Text += status + "\n";
            else
                statusLabel.Text = "";
        }
    }
}
