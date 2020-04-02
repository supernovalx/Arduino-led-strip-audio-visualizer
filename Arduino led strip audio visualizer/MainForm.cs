using NAudio.CoreAudioApi;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Arduino_led_strip_audio_visualizer
{
    public partial class MainForm : Form
    {
        private SerialPort port;
        public MainForm()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            rtbLog.Text += e.ToString() + "\n";
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);

            cbDevices.Items.AddRange(devices.ToArray());

            cbPort.Items.AddRange(SerialPort.GetPortNames());



            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (cbDevices.SelectedItem != null)
            {
                var device = (MMDevice)cbDevices.SelectedItem;
                pbMeter.Value = (int)(device.AudioMeterInformation.MasterPeakValue * 100);

                if (port != null && port.IsOpen)
                    port.WriteLine((pbMeter.Value - 10).ToString());
            }
        }

        private void cbPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            port = new SerialPort(cbPort.SelectedItem.ToString(), 9600);
            port.Open();
        }
    }
}
