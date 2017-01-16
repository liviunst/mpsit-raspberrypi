using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Capture;
using Windows.Media.MediaProperties;
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace RaspberryDashboard
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private HttpClient _client = new HttpClient();
        private string _serverIp;
        private List<Image> images = new List<Image>();
        private MediaCapture _mediaCapture;
        private int _currentImageId;
        private List<Record> _records = new List<Record>();

        public MainPage()
        {
            this.InitializeComponent();

            InitializeWebCam();

            images.Add(Image1);
            images.Add(Image2);
            images.Add(Image3);
        }


#region WebCam

        private async void InitializeWebCam()
        {
            try
            {
                _mediaCapture = new MediaCapture();
                var settings = new MediaCaptureInitializationSettings
                {
                    StreamingCaptureMode = StreamingCaptureMode.Video
                };

                await _mediaCapture.InitializeAsync(settings);

                _mediaCapture.Failed += MediaCaptureFailed;
                _mediaCapture.RecordLimitationExceeded += MediaCaptureOnRecordLimitationExceeded;

                CameraLiveStream.Source = _mediaCapture;

                await _mediaCapture.StartPreviewAsync();
            }
            catch (Exception ex)
            {
                Logging.Text += ex.Message + Environment.NewLine;
            }
        }

        private void MediaCaptureOnRecordLimitationExceeded(MediaCapture sender)
        {
            Logging.Text += "CaptureLimitExceeded" + Environment.NewLine;
        }

        private void MediaCaptureFailed(MediaCapture sender, MediaCaptureFailedEventArgs erroreventargs)
        {
            Logging.Text += "WebCam failed" + Environment.NewLine;
        }

        private async void TakePhoto_Click(object sender, RoutedEventArgs e)
        {
            var photofile = await KnownFolders.PicturesLibrary.CreateFileAsync(
                                                                "mpsit_photo.jpg", 
                                                                CreationCollisionOption.GenerateUniqueName);

            ImageEncodingProperties imageProperties = ImageEncodingProperties.CreateJpeg();
            await _mediaCapture.CapturePhotoToStorageFileAsync(imageProperties, photofile);

            IRandomAccessStream photoStream = await photofile.OpenReadAsync();
            BitmapImage bitmap = new BitmapImage();
            bitmap.SetSource(photoStream);
            images[_currentImageId].Source = bitmap;
            _currentImageId = (_currentImageId + 1) % 3;
        }

#endregion

        private void CreateClient_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
