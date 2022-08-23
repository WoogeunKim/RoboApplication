using System;
using System.Windows;
using System.Windows.Controls;

namespace AquilaErpWpfApp3
{
    /// <summary>
    /// Interaction logic for SplashScreenView.xaml
    /// </summary>
    public partial class SplashScreenView : UserControl
    {
        public SplashScreenView()
        {
            InitializeComponent();

            this.media01.Source = new Uri("http://www.robocon.ai/kor/videos/video" + new Random().Next(1, 9) + ".mp4");
            this.media01.Play();
        }

        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e) 
        { 
            this.media01.Stop();
            this.media01.Source = new Uri("http://www.robocon.ai/kor/videos/video" + new Random().Next(1, 9) + ".mp4");
            this.media01.Position = TimeSpan.FromSeconds(0);
            this.media01.Play(); 
        }

    }
}
