using System;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using KeypadConfigurator.Data;

namespace KeypadConfigurator.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
#if DEBUG
            this.AttachDevTools();
#endif
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            var portBox = this.Find<ComboBox>("PortBox");
            portBox.Items = KeypadConnectionManager.GetAvailablePorts();
        }
        
        private void applyButton_Click(object sender, RoutedEventArgs e)
        {
            var config = getConfigFromLayout();
        }

        private void Connect_OnClickButton_Click(object? sender, RoutedEventArgs e)
        {
            var portBox = this.Find<ComboBox>("PortBox");
            var propsPanel = this.Find<StackPanel>("PropsPanel");
            var connectButton = this.Find<Button>("Connect");
            var disconnectButton = this.Find<Button>("Disconnect");
            
            if (KeypadConnectionManager.Connected)
            {
                KeypadConnectionManager.Close();
                portBox.PlaceholderText = "Connect";
                portBox.IsEnabled = true;
                propsPanel.IsEnabled = false;
                disconnectButton.IsEnabled = false;
                connectButton.IsEnabled = true;
            }
            else
            {
                if (portBox.SelectedItem != null)
                {
                    KeypadConnectionManager.Initialize(portBox.SelectedItem.ToString());
                    portBox.PlaceholderText = "Disconnect";
                    portBox.IsEnabled = false;
                    propsPanel.IsEnabled = true;
                    disconnectButton.IsEnabled = true;
                    connectButton.IsEnabled = false;
                }
            }
        }

        private void resetButton_Click(object? sender, RoutedEventArgs e)
        {
            
        }

        private KeypadConfiguration? getConfigFromLayout()
        {
            var vidUpDown = this.Find<NumericUpDown>("VidUpDown");
            var pidUpDown = this.Find<NumericUpDown>("PidUpDown");
            var friendlyNameBox = this.Find<TextBox>("FriendlyNameBox");

            var leftPinUpDown = this.Find<NumericUpDown>("LeftPinUpDown");
            var leftDebounceUpDown = this.Find<NumericUpDown>("LeftDebounceUpDown");
            var leftKeyBox = this.Find<TextBox>("LeftKeyBox");
            
            var rightPinUpDown = this.Find<NumericUpDown>("RightPinUpDown");
            var rightDebounceUpDown = this.Find<NumericUpDown>("RightDebounceUpDown");
            var rightKeyBox = this.Find<TextBox>("RightKeyBox");

            KeypadConfiguration? config = null;
            
            try
            {
                config = new KeypadConfiguration
                {
                    Vid = Convert.ToUInt16(vidUpDown.Value),
                    Pid = Convert.ToUInt16(pidUpDown.Value),
                    FriendlyName = friendlyNameBox.Text,
                    LeftKey = new KeyConfiguration
                    {
                        Pin = Convert.ToByte(leftPinUpDown.Value),
                        KeyChar = leftKeyBox.Text[0],
                        DebounceTime = Convert.ToInt32(leftDebounceUpDown.Value)
                    },
                    RightKey = new KeyConfiguration
                    {
                        Pin = Convert.ToByte(rightPinUpDown.Value),
                        KeyChar = rightKeyBox.Text[0],
                        DebounceTime = Convert.ToInt32(rightDebounceUpDown.Value)
                    },
                };
            }
            catch (Exception)
            {
                
            }

            return config;
        }
        
        private void setConfigToLayout(KeypadConfiguration config)
        {
            
        }
    }
}