using NAudio.Wave;
using System;
using System.Windows;

namespace Clap_detection
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        WaveIn In;
        private float BigValue;
        private double MaxValue;
        private int Value = 80;
        void In_DataAvailable(object sender, WaveInEventArgs e)
        {
            for (int index = 0; index < e.BytesRecorded; index += 2)
            {
                short sample = (short)((e.Buffer[index + 1] << 8) | e.Buffer[index + 0]);
                float sample32 = sample / 32768f;
                CurValue.Content = sample32.ToString();
                if (BigValue < sample32)
                {
                    BigValue = sample32;
                    maxValue.Content = BigValue.ToString();
                    if (BigValue > MaxValue)
                    {
                        BigValue = 0;
                        MaxValue = 0;
                        In.StopRecording();
                        Start.IsEnabled = true;
                        ClapLabel.FontWeight = FontWeights.Bold;
                        ClapLabel.Content = "Clap detected!";
                    }
                }
            }
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            ClapLabel.FontWeight = FontWeights.Normal;
            ClapLabel.Content = "Clap not detected";
            Start.IsEnabled = false;
            MaxValue = Convert.ToDouble(Value) / 100;
            BigValue = 0;
            In = new WaveIn();
            int InDevices = WaveIn.DeviceCount;
            for (int InDevice = 0; InDevice < InDevices; InDevice++)
            {
                WaveInCapabilities deviceInfo = WaveIn.GetCapabilities(InDevice);
            }
            In.DeviceNumber = 0;
            In.DataAvailable += new EventHandler<WaveInEventArgs>(In_DataAvailable);
            int sampleRate = 8000;
            int channels = 1;
            In.WaveFormat = new WaveFormat(sampleRate, channels);
            In.StartRecording();
        }
    }
}

