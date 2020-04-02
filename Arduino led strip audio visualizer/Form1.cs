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
    public partial class Form1 : Form
    {
        private SerialPort port;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MMDeviceEnumerator enumerator = new MMDeviceEnumerator();
            var devices = enumerator.EnumerateAudioEndPoints(DataFlow.All, DeviceState.Active);

            cbDevices.Items.AddRange(devices.ToArray());

            port = new SerialPort("COM5", 9600);
            port.Open();

            timer.Start();
        }

        private void timer_Tick(object sender, EventArgs e)
        {
            if (cbDevices.SelectedItem != null && port != null)
            {
                var device = (MMDevice)cbDevices.SelectedItem;
                pbMeter.Value = (int)(device.AudioMeterInformation.MasterPeakValue * 100);

                port.WriteLine((pbMeter.Value-10).ToString());
            }
        }
    }
}
