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
    }
}