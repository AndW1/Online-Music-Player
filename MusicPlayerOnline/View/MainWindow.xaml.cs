using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using MusicPlayerOnline.View;


namespace MusicPlayerOnline
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DispatcherTimer timer;
        bool isDragging = false;


        public MainWindow()
        {
            InitializeComponent();
            myMedia.MediaFailed += MyMedia_MediaFailed;

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(200);
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!isDragging)
                sliderPosition.Value = myMedia.Position.TotalSeconds;
        }

        private void MyMedia_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show(e.ErrorException.Message);    
        }

        private void windowPlayer_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

       
        private void myMedia_MediaOpened(object sender, RoutedEventArgs e)
        {
            if(myMedia.NaturalDuration.HasTimeSpan)
            {
                TimeSpan ts = myMedia.NaturalDuration.TimeSpan;
                sliderPosition.Maximum = ts.TotalSeconds;
                sliderPosition.SmallChange = 1;
                sliderPosition.LargeChange = Math.Min(10, ts.Seconds / 10);
            }
            //sliderPosition.Maximum = myMedia.NaturalDuration.TimeSpan.TotalSeconds;
            timer.Start();
        }

        private void sliderPosition_DragStarted(object sender, System.Windows.Controls.Primitives.DragStartedEventArgs e)
        {
            isDragging = true;
        }

        private void sliderPosition_DragCompleted(object sender, System.Windows.Controls.Primitives.DragCompletedEventArgs e)
        {
            isDragging = false;
            myMedia.Position = TimeSpan.FromSeconds(sliderPosition.Value);
        }
    }
}

