using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System.Threading;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Newtonsoft.Json;
using WinRTXamlToolkit.Controls.DataVisualization.Charting;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RaspberryDesktop
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private string _baseAddress = "http://localhost:9000/api/rasp/";
        private HttpClient _client = new HttpClient();
        private List<Temperature> _records = new List<Temperature>();

        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                await _client.PutAsync(_baseAddress + "command", new StringContent("take_picture"));
            }
            catch (Exception ex)
            {
                var message = ex.Message;
            }
        }

        private void MainPage_OnLoaded(object sender, RoutedEventArgs e)
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            ThreadPool.RunAsync(asyncAction =>
            {
                while (true)
                {
                    try
                    {
                        var result = _client.GetAsync(_baseAddress + "temperature").Result;

                        var data = result.Content.ReadAsStringAsync().Result;

                        var parts = data.Split('x');

                        var temp = new Temperature()
                        {
                            Id = int.Parse(parts[1]),
                            Value = double.Parse(parts[2])
                        };

                        Dispatcher.TryRunAsync(CoreDispatcherPriority.Normal, () =>
                        {
                            UpdateChart(temp);
                        });
                    }
                    catch (Exception ex)
                    {
                        var msg = ex.Message;
                    }
                    Task.Delay(10000).Wait();
                }
            });
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        private void UpdateChart(Temperature temp)
        {
            _records.Add(temp);

            var values = new List<Temperature>();
            int id = 0;
            foreach (var temperature in _records)
            {
                values.Add(new Temperature()
                {
                    Id = id,
                    Value = temperature.Value
                });
                id++;
            }

            var lineSeries = LineChart.Series[0] as LineSeries;
            lineSeries.ItemsSource = values;
        }
    }
}
