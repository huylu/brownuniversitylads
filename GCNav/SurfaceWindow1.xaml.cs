using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Surface;
using Microsoft.Surface.Presentation.Controls;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace GCNav
{
    /// <summary>
    /// Interaction logic for SurfaceWindow1.xaml
    /// </summary>
    /// 
    public partial class SurfaceWindow1 : SurfaceWindow
    {
        /// <summary>
        /// Default constructor.
        /// </summary>
        private StartCard _startCard;
        //private FilterTimelineBox filter;
        private DispatcherTimer _resetTimer = new DispatcherTimer();
       
        public SurfaceWindow1()
        {
            InitializeComponent();
            // Add handlers for window availability events
            AddWindowAvailabilityHandlers();

            backgroundTopImg.Visibility = Visibility.Collapsed; // jcchin
            //exitButton.Visibility = Visibility.Collapsed; // jcchin  // navigator exit

            this.SizeChanged += new SizeChangedEventHandler(SurfaceWindow1_SizeChanged);
            //this.SizeChanged += Map.WindowSizeChanged; // jcchin
            this.SizeChanged += nav.WindowSizeChanged;
            this.MouseUp += new MouseButtonEventHandler(MouseUp_Handler);

            _startCard = new StartCard();
            _startCard.HorizontalAlignment = HorizontalAlignment.Center;
            _startCard.VerticalAlignment = VerticalAlignment.Center;
            startCan.Children.Add(_startCard);

            panImg.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\panning_startup.bmp", UriKind.Absolute));
            // Logo pics!!
            //backgroundTopImg.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\cxo_logo_stacked.png", UriKind.Absolute));
            //backgroundTopImg.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\bode30.tif", UriKind.Absolute));
            ImageBrush brush = new ImageBrush();
            Image img = new Image();
            //img.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\wolbach_selection_bg_top.jpg", UriKind.Absolute));
            img.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\Galactic_Banner.png", UriKind.Absolute));
            brush.ImageSource = img.Source;
            backgroundTop.Background = brush;
            //backgroundTopImg.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\SDO_Logo_flat_lg.png", UriKind.Absolute));
            backgroundImg.Source = new BitmapImage(new Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\Data\\Startup\\catalog_chandra_bg.jpg", UriKind.Absolute));
         
            panImg.Width = 3598;
            panImg.Height = 1080;
            DoubleAnimation myAnimation = new DoubleAnimation();
            myAnimation.From = 0;
            myAnimation.To = -1080;
            myAnimation.AutoReverse = true;
            myAnimation.RepeatBehavior = RepeatBehavior.Forever;
            myAnimation.Duration = new Duration(TimeSpan.FromSeconds(45));
            TranslateTransform t = new TranslateTransform();
            panImg.HorizontalAlignment = HorizontalAlignment.Right;
            panImg.Opacity = 0.2;
            panCan.RenderTransform = t;
            t.BeginAnimation(TranslateTransform.XProperty, myAnimation);
            //nav.HandleImageSelected += Map.HandleImageSelectedEvent; // jcchin
            //nav.setMapWidth(Map.Width); // jcchin
            //filter = new FilterTimelineBox(); // jcchin
            //nav.filter = filter; // jcchin

            //map.Children.Add(filter); // jcchin
           
            this.SizeChanged += SurfaceWindow1_SizeChanged;

            _resetTimer.Interval = TimeSpan.FromSeconds(120);
            _resetTimer.Tick += new EventHandler(_resetTimer_Tick);

            //help.Visibility = Visibility.Visible;

            String[] c = Environment.GetCommandLineArgs();

            if (c.Length != 1)
            {
                if (c[1].Contains("noauthoring"))
                {
                    //ButtonPanel.Children.Remove(exitButton);  // navigator exit
                }
            }
        }

        //This adjusts the winodw size for screens of different resolutions
        void SurfaceWindow1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //Double canvasLeft = e.NewSize.Width / 2 - filter.ActualWidth / 2; // jcchin
            //nav.setMapWidth(Map.ActualWidth); // jcchin
            Double filterWidth = 420;
            if (e.NewSize.Width < 1600)
            {
                ScaleTransform tran = new ScaleTransform();
                tran.ScaleX = e.NewSize.Width / 1600;
                //filter.RenderTransform = tran; // jcchin
                //canvasLeft = e.NewSize.Width / 2 - filterWidth * tran.ScaleX / 2; // jcchin
                filterWidth = filterWidth * tran.ScaleX;
            }

            //Double scaleX= Map.tranScaleX; // jcchin
            //Double scaleY = Map.tranScaleY; // jcchin
            //Canvas.SetLeft(filter, canvasLeft); // jcchin
            //Canvas.SetZIndex(filter, 10); // jcchin
            //filter.Visibility = Visibility.Hidden; // jcchin

            //backRec.Width = map.Width*scaleX +10; // jcchin
            //backRec.Height = map.Height*scaleY + 30+10; // jcchin
            //Canvas.SetLeft(backRec, e.NewSize.Width *0.316); // jcchin
            //Canvas.SetZIndex(backRec, -10);  // jcchin
        }

        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            string message = "Are you sure you want to quit LADS?";
            string caption = "Quit LADS";
            System.Windows.Forms.MessageBoxButtons buttons = System.Windows.Forms.MessageBoxButtons.YesNo;
            System.Windows.Forms.DialogResult result;

            result = System.Windows.Forms.MessageBox.Show(message, caption, buttons);

            if (result == System.Windows.Forms.DialogResult.Yes)
            {

                Application.Current.Shutdown();
            }
        }

        /// <summary>
        /// Occurs when the window is about to close. 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Remove handlers for window availability events
            RemoveWindowAvailabilityHandlers();
        }

        /// <summary>
        /// Adds handlers for window availability events.
        /// </summary>
        private void AddWindowAvailabilityHandlers()
        {
            // Subscribe to surface window availability events
            ApplicationServices.WindowInteractive += OnWindowInteractive;
            ApplicationServices.WindowNoninteractive += OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable += OnWindowUnavailable;
        }

        /// <summary>
        /// Removes handlers for window availability events.
        /// </summary>
        private void RemoveWindowAvailabilityHandlers()
        {
            // Unsubscribe from surface window availability events
            ApplicationServices.WindowInteractive -= OnWindowInteractive;
            ApplicationServices.WindowNoninteractive -= OnWindowNoninteractive;
            ApplicationServices.WindowUnavailable -= OnWindowUnavailable;
        }

        /// <summary>
        /// This is called when the user can interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowInteractive(object sender, EventArgs e)
        {
            //TODO: enable audio, animations here
        }

        /// <summary>
        /// This is called when the user can see but not interact with the application's window.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowNoninteractive(object sender, EventArgs e)
        {
            //TODO: Disable audio here if it is enabled

            //TODO: optionally enable animations here
        }

        /// <summary>
        /// This is called when the application's window is not visible or interactive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnWindowUnavailable(object sender, EventArgs e)
        {
            //TODO: disable audio, animations here
        }

        /// <summary>
        /// The surface button serves 2 purposes:
        /// 1. Loading the images before the window finishes initializing causes resolution problem;
        /// 2. The about box shows up on start
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void surfaceButton1_Click(object sender, RoutedEventArgs e)
        {
            nav.startAll();
            surfaceButton1.Visibility = Visibility.Collapsed;
            _startCard.Visibility = Visibility.Hidden;
            if (nav.collectionEmpty())
            {
                EmptyCollectionControl popup = new EmptyCollectionControl();
                popup.HorizontalAlignment = HorizontalAlignment.Center;
                popup.VerticalAlignment = VerticalAlignment.Center;
                startCan.Children.Add(popup);

            }
            else
            {
                panImg.Visibility = Visibility.Hidden;
                //Map.loadMap(); // jcchin
                //Map.blur.Visibility = Visibility.Visible; // jcchin
                
                nav.filter.Visibility = Visibility.Visible; // jcchin
                nav.imageBorder.Visibility = Visibility.Visible; // jcchin
                nav.imageInfoBorder.Visibility = Visibility.Visible; // jcchin
                
                //backRec.Visibility = Visibility.Visible; // jcchin

                //exitButton.Visibility = Visibility.Visible; // navigator exit

                backgroundTopImg.Visibility = Visibility.Visible;
                _resetTimer.Start();
            }
        }

        public void MouseUp_Handler(object sender, EventArgs e)
        {
            nav.setTimelineMouseUpFalse();
        }

        private void SurfaceWindow_PreviewTouchDown(object sender, TouchEventArgs e)
        {
            _startCard.Visibility = Visibility.Collapsed;
            panImg.Visibility = Visibility.Collapsed;
            InstrLabel.Visibility = Visibility.Collapsed;
            _resetTimer.Stop();
            e.Handled = false;
        }

        private void SurfaceWindow_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            _startCard.Visibility = Visibility.Collapsed;
            panImg.Visibility = Visibility.Collapsed;
            InstrLabel.Visibility = Visibility.Collapsed;
            _resetTimer.Stop();
            e.Handled = false;
        }

        private void SurfaceWindow_PreviewTouchUp(object sender, TouchEventArgs e)
        {
            _resetTimer.Start();
            e.Handled = false;
        }

        private void SurfaceWindow_PreviewMouseUp(object sender, MouseButtonEventArgs e)
        {
            _resetTimer.Start();
            e.Handled = false;
        }

        void _resetTimer_Tick(object sender, EventArgs e)
        {
            _resetTimer.Stop();
            _startCard.Visibility = Visibility.Visible;
            panImg.Visibility = Visibility.Visible;
            InstrLabel.Visibility = Visibility.Visible;
        }

        private void SurfaceWindow_Deactivated(object sender, EventArgs e)
        {
            _resetTimer.Start();
        }

        private void help_MouseDown(object sender, MouseButtonEventArgs e)
        {
            helpWindow.Visibility = Visibility.Visible;
            //helpInstruction.Visibility = Visibility.Visible;
            //helpDone.Visibility = Visibility.Visible;
        }

        private void help_TouchDown(object sender, TouchEventArgs e)
        {
            helpWindow.Visibility = Visibility.Visible;
            //helpInstruction.Visibility = Visibility.Visible;
            //helpDone.Visibility = Visibility.Visible;
        }

        //private void helpDone_Click(object sender, RoutedEventArgs e)
        //{
        //    help.Visibility = Visibility.Visible;
        //    //helpInstruction.Visibility = Visibility.Hidden;
        //    //helpDone.Visibility = Visibility.Hidden;
        //}
    }
}