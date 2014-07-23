using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;

namespace SerialCommunication
{
    public partial class ServoTester : Form
    {
        private System.Timers.Timer timer1;
        private StringBuilder recievedB = new StringBuilder();
        private string recievedData;
        List<TextBox> tbPos = new List<TextBox>();
        List<HScrollBar> sbPos = new List<HScrollBar>();
        List<NumericUpDown> nudMin = new List<NumericUpDown>();
        List<NumericUpDown> nudMax = new List<NumericUpDown>();
        List<NumericUpDown> nudHome = new List<NumericUpDown>();
        bool init = true;
        bool block = false;
        int oldvalue = 0;

        public ServoTester()
        {
            InitializeComponent();

            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            DataGridViewButtonColumn btnSave = new DataGridViewButtonColumn();
            btnSave.Text = "Save";
            btnSave.HeaderText = "Save";
            btnSave.DataPropertyName = "Save";
            dataGridView1.Columns.Add(btnSave);

            DataGridViewButtonColumn btnMove = new DataGridViewButtonColumn();
            btnSave.Text = "Move";
            btnMove.HeaderText = "Move";
            btnSave.DataPropertyName = "Move";
            dataGridView1.Columns.Add(btnMove);

            tbPos.Add(textBox0);
            tbPos.Add(textBox1);
            tbPos.Add(textBox2);
            tbPos.Add(textBox3);
            tbPos.Add(textBox4);
            tbPos.Add(textBox5);
            tbPos.Add(textBoxWait);

            sbPos.Add(hScrollBar0);
            sbPos.Add(hScrollBar1);
            sbPos.Add(hScrollBar2);
            sbPos.Add(hScrollBar3);
            sbPos.Add(hScrollBar4);
            sbPos.Add(hScrollBar5);

            nudMin.Add(nudMin0);
            nudMin.Add(nudMin1);
            nudMin.Add(nudMin2);
            nudMin.Add(nudMin3);
            nudMin.Add(nudMin4);
            nudMin.Add(nudMin5);

            nudMax.Add(nudMax0);
            nudMax.Add(nudMax1);
            nudMax.Add(nudMax2);
            nudMax.Add(nudMax3);
            nudMax.Add(nudMax4);
            nudMax.Add(nudMax5);

            nudHome.Add(nudHome0);
            nudHome.Add(nudHome1);
            nudHome.Add(nudHome2);
            nudHome.Add(nudHome3);
            nudHome.Add(nudHome4);
            nudHome.Add(nudHome5);

            string[] theSerialPortNames = SerialPort.GetPortNames();
            foreach (string sp in theSerialPortNames)
            {
                cbComPort.Items.Add(sp);
            }

        }

        void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            timer1.Stop();
            this.Invoke(new EventHandler(ReadData));
        }

        private void ReadData(object s, EventArgs e)
        {
            recievedData = recievedB.ToString();
            recievedB.Remove(0, recievedB.Length - 1);

            // now we have the string for parsing
            int j = recievedB.Length;

            //labelCard.Text = "Read " + j.ToString() + " bytes...";
            textBoxData.Text += recievedData;

            // finally we have the data, we need to process it
            ProcessData();
        }

        private void serialPort1_DataReceived(object sender, System.IO.Ports.SerialDataReceivedEventArgs e)
        {
            this.Invoke(new EventHandler(AddRecieve));
        }

        private void AddRecieve(object s, EventArgs e)
        {
            string st = serialPort1.ReadExisting();
            recievedB.Append(st);

            timer1.Interval = 100;
            timer1.Start();
        }

        // actuall processing (parsing) of data
        private void ProcessData()
        {

        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            try
            {
                serialPort1.Close();
            }
            catch { }
        }


        private void serialPort1_ErrorReceived(object sender, System.IO.Ports.SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Serial Port Error!", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ibtnComPort.NormalImage = imageList1.Images[1];
            ibtnComPort.DownImage = imageList1.Images[1];
            ibtnComPort.HoverImage = imageList1.Images[1];

            serialPort1.PortName = Properties.Settings.Default.ComPort;
            serialPort1.Parity = Properties.Settings.Default.Parity;
            serialPort1.BaudRate = Properties.Settings.Default.BaudRate;
            serialPort1.DataBits = Properties.Settings.Default.DataBits;
            serialPort1.StopBits = Properties.Settings.Default.StopBits;

            cbComPort.SelectedIndex = cbComPort.FindStringExact(serialPort1.PortName);

            if (OpenPort() == true && serialPort1.IsOpen == true)
            {
                try
                {
                    GetSettings();
                    Properties.Settings.Default.ComPort = cbComPort.SelectedItem.ToString();
                    Properties.Settings.Default.Save();
                }
                catch
                {
                    recievedData = null;
                    OpenPort();
                }
            }

        }

        private void GetActualPos()
        {
            serialPort1.Write("INF\r");
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            string[] values;
            values = recievedData.Split(' ');

            for (int i = 0; i < 6; i++)
            {
                sbPos[i].Value = int.Parse(values[i].Trim());
                tbPos[i].Text = values[i];
            }

            tbPos[6].Text = values[6];

            recievedData = null;

        }

        private void GetMemData()
        {
            dataSet1.Tables[0].Rows.Clear();

            for (int i = 1; i <= 20; i++)
            {
                serialPort1.Write(string.Format("IME {0}\r", i));
                while (recievedData == null)
                {
                    Application.DoEvents();
                };

                string[] mem;
                mem = recievedData.Split(' ');

                DataRow row = dataSet1.Tables[0].NewRow();
                row["ID"] = i;
                row["Basis"] = mem[0];
                row["Schulter"] = mem[1];
                row["Ellbogen"] = mem[2];
                row["Hand"] = mem[3];
                row["Handdreh"] = mem[4];
                row["Greifer"] = mem[5];

                dataSet1.Tables[0].Rows.Add(row);

                recievedData = null;
            }

            dataGridView1.DataSource = dataSet1.Tables[0];

            for (int i = 0; i <= 8; i++)
            {
                dataGridView1.Columns[i].Width = 88;
            }

        }
        private void GetSettings()
        {
            EnableButtos(false);

            GetActualPos();

            serialPort1.Write("IMI\r");
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            string[] min;
            min = recievedData.Split(' ');

            for (int i = 0; i < 6; i++)
            {
                nudMin[i].Value = int.Parse(min[i].Trim());
            }

            recievedData = null;

            serialPort1.Write("IMA\r");
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            string[] max;
            max = recievedData.Split(' ');

            for (int i = 0; i < 6; i++)
            {
                nudMax[i].Value = int.Parse(max[i].Trim());
            }

            recievedData = null;

            serialPort1.Write("ISP\r");
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            string[] home;
            home = recievedData.Split(' ');

            for (int i = 0; i < 6; i++)
            {
                nudHome[i].Value = int.Parse(home[i].Trim());
            }

            recievedData = null;

            GetMemData();

            init = false;

            EnableButtos(true);
        }

        private bool OpenPort()
        {
            bool bReturn = true;
            try
            {
                if (serialPort1.IsOpen == true)
                {
                    serialPort1.Close();
                    ibtnComPort.NormalImage = imageList1.Images[1];
                    ibtnComPort.DownImage = imageList1.Images[1];
                    ibtnComPort.HoverImage = imageList1.Images[1];
                    cbComPort.Enabled = true;
                }
                else
                {

                    serialPort1.Open();

                    timer1 = new System.Timers.Timer(100);
                    timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);
                    GC.KeepAlive(timer1);

                    ibtnComPort.NormalImage = imageList1.Images[0];
                    ibtnComPort.DownImage = imageList1.Images[0];
                    ibtnComPort.HoverImage = imageList1.Images[0];
                    cbComPort.Enabled = false;
                }
            }
            catch
            {
                ibtnComPort.NormalImage = imageList1.Images[1];
                ibtnComPort.DownImage = imageList1.Images[1];
                ibtnComPort.HoverImage = imageList1.Images[1];

                bReturn = false;
                MessageBox.Show("Kann Port nicht öffnen", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return bReturn;

        }
        private void ibtnComPort_Click_1(object sender, EventArgs e)
        {
            if (OpenPort() == true && serialPort1.IsOpen == true)
            {
                try
                {
                    GetSettings();
                    oldvalue = int.Parse(tbPos[5].Text);
                    Properties.Settings.Default.ComPort = cbComPort.SelectedItem.ToString();
                    Properties.Settings.Default.Save();
                }
                catch
                {
                    recievedData = null;
                    OpenPort();
                    MessageBox.Show("Kein gültiger Servocontroller gefunden", "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void cbComPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbComPort.SelectedItem.ToString().Length > 0)
                serialPort1.PortName = cbComPort.SelectedItem.ToString();
        }

        private void hScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            decimal value = ((HScrollBar)sender).Value;
            int id = int.Parse(StringHelper.Right(((HScrollBar)sender).Name, 1));
            tbPos[id].Text = value.ToString();

            if (init == false && cbDirekt.Checked == true)
            {
                if (block == false)
                {
                    block = true;

                    if(id<5)
                        serialPort1.Write(string.Format("ONE {0} {1}\r", id, value));
                    

                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    recievedData = null;
                    block = false;
                }
            }

        }

        private void EnableButtos(bool Enable)
        {
            btnMove.Enabled = Enable;
            btnSetHome.Enabled = Enable;
            btnSetMax.Enabled = Enable;
            btnSetMin.Enabled = Enable;
            btnSetWait.Enabled = Enable;
            btnMoveHome.Enabled = Enable;
            btnMoveSequence.Enabled = Enable;
        }

        private void cbDirekt_CheckedChanged(object sender, EventArgs e)
        {
            if (cbDirekt.Checked == false)
            {
                EnableButtos(true);
            }
            else
            {
                EnableButtos(false);
            }
        }

        private void btnMove_Click(object sender, EventArgs e)
        {
            EnableButtos(false);
            serialPort1.Write(string.Format("PAS {0} {1} {2} {3} {4} {5} \r", hScrollBar0.Value, hScrollBar1.Value, hScrollBar2.Value, hScrollBar3.Value, hScrollBar4.Value, hScrollBar5.Value));
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            recievedData = null;
            EnableButtos(true);
        }

        private void nudMin_ValueChanged(object sender, EventArgs e)
        {
            if (init == false && cbDirekt.Checked == true)
            {
                if (block == false)
                {
                    block = true;

                    decimal value = ((NumericUpDown)sender).Value;
                    int id = int.Parse(StringHelper.Right(((NumericUpDown)sender).Name, 1));

                    serialPort1.Write(string.Format("SMI {0} {1}\r", id, value));
                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    recievedData = null;
                    block = false;
                }
            }
        }

        private void nudMax_ValueChanged(object sender, EventArgs e)
        {
            if (init == false && cbDirekt.Checked == true)
            {
                if (block == false)
                {
                    block = true;

                    decimal value = ((NumericUpDown)sender).Value;
                    int id = int.Parse(StringHelper.Right(((NumericUpDown)sender).Name, 1));

                    serialPort1.Write(string.Format("SMA {0} {1}\r", id, value));
                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    recievedData = null;
                    block = false;
                }
            }
        }

        private void nudHome_ValueChanged(object sender, EventArgs e)
        {
            if (init == false && cbDirekt.Checked == true)
            {
                if (block == false)
                {
                    block = true;

                    decimal value = ((NumericUpDown)sender).Value;
                    int id = int.Parse(StringHelper.Right(((NumericUpDown)sender).Name, 1));

                    serialPort1.Write(string.Format("SSP {0} {1}\r", id, value));
                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    recievedData = null;
                    block = false;
                }
            }
        }

        private void btnSetMin_Click(object sender, EventArgs e)
        {
            EnableButtos(false);
            if (block == false)
            {
                block = true;

                for (int i = 0; i < 6; i++)
                {
                    decimal value = nudMin[i].Value;
                    serialPort1.Write(string.Format("SMI {0} {1}\r", i, value));
                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                    {
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    recievedData = null;

                }
                recievedData = null;
                block = false;
            }
            EnableButtos(true);

        }

        private void btnSetMax_Click(object sender, EventArgs e)
        {
            EnableButtos(false);
            if (block == false)
            {
                block = true;

                for (int i = 0; i < 6; i++)
                {
                    decimal value = nudMax[i].Value;
                    serialPort1.Write(string.Format("SMA {0} {1}\r", i, value));
                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                    {
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    recievedData = null;

                }
                recievedData = null;
                block = false;
            }
            EnableButtos(true);

        }

        private void btnSetHome_Click(object sender, EventArgs e)
        {
            EnableButtos(false);
            if (block == false)
            {
                block = true;

                for (int i = 0; i < 6; i++)
                {
                    decimal value = nudHome[i].Value;
                    serialPort1.Write(string.Format("SSP {0} {1}\r", i, value));
                    while (recievedData == null)
                    {
                        Application.DoEvents();
                    };

                    if (recievedData != "\nACK\r\n")
                    {
                        MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    }
                    recievedData = null;

                }
                recievedData = null;
                block = false;
            }
            EnableButtos(true);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            textBoxData.Text = "";
        }

        private void btnSetWait_Click(object sender, EventArgs e)
        {
            EnableButtos(false);
            serialPort1.Write(string.Format("WAI {0}\r", textBoxWait.Text));
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            if (recievedData != "\nACK\r\n")
            {
                MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                recievedData = null;

                serialPort1.Write("HLD\r");
                while (recievedData == null)
                {
                    Application.DoEvents();
                };

                if (recievedData != "\nACK\r\n")
                {
                    MessageBox.Show(recievedData, "Fehler", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            recievedData = null;

            EnableButtos(true);
        }


        private void btnMoveHome_Click(object sender, EventArgs e)
        {
            EnableButtos(false);
            serialPort1.Write(string.Format("PAS {0} {1} {2} {3} {4} {5} \r", nudHome0.Value, nudHome1.Value, nudHome2.Value, nudHome3.Value, nudHome4.Value, nudHome5.Value));
            while (recievedData == null)
            {
                Application.DoEvents();
            };

            recievedData = null;
            GetActualPos();

            EnableButtos(true);

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            string id = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells["ID"].Value);
            string[] values = new string[6];

            for (int i = 0; i < 6; i++)
            {
                values[i] = Convert.ToString(dataGridView1.Rows[e.RowIndex].Cells[i + 3].Value);
            }

            if (e.ColumnIndex == 1)
            {
                EnableButtos(false);
                serialPort1.Write(string.Format("PAS {0} {1} {2} {3} {4} {5} \r", values[0], values[1], values[2], values[3], values[4], values[5]));
                while (recievedData == null)
                {
                    Application.DoEvents();
                };

                recievedData = null;
                GetActualPos();
            }

            else if (e.ColumnIndex == 0)
            {
                EnableButtos(false);

                serialPort1.Write(string.Format("WEP {0}\r", id));
                while (recievedData == null)
                {
                    Application.DoEvents();
                };
                recievedData = null;

                GetActualPos();
                GetMemData();


            }

            EnableButtos(true);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
        }

        private void btnMoveSequence_Click(object sender, EventArgs e)
        {
            EnableButtos(false);

            for(int i = 1; i<=10; i++)
            {
                serialPort1.Write(string.Format("MEP {0}\r", i));
                while (recievedData == null)
                {
                    Application.DoEvents();
                };

                recievedData = null;
            }

            for (int i = 10; i >= 1; i--)
            {
                serialPort1.Write(string.Format("MEP {0}\r", i));
                while (recievedData == null)
                {
                    Application.DoEvents();
                };

                recievedData = null;
            }

            GetActualPos();

            EnableButtos(true);
        }

        private void textBox_Leave(object sender, EventArgs e)
        {
            int value = -1;
            string text = ((TextBox)sender).Text;

            int.TryParse(text, out value);
            int id = int.Parse(StringHelper.Right(((TextBox)sender).Name, 1));

            if (value >= 0 && value <= 180)
            {
                sbPos[id].Value = value;
                hScrollBar_Scroll(sbPos[id], null);
            }

        }


    }
}