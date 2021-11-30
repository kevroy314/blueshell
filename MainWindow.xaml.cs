using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace blueshell
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const uint WS_EX_TRANSPARENT = 0x00000020;
        const int GWL_EXSTYLE = (-20);
        const uint WS_EX_TOOLWINDOW = 0x00000080;
        public System.DateTime t0;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            if ((DateTime.Now - t0).TotalSeconds > (float)App.parsedArgs["duration"])
                Environment.Exit(0);
        }   

        private void Media_MediaEnded(object sender, RoutedEventArgs e)
        {
            media.Position = TimeSpan.Zero;
            media.Play();
        }

        [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        static extern uint GetWindowLongPtr(int hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, uint dwNewLong);

        public static void SetWindowExTransparent(IntPtr hwnd)
        {
            uint extendedStyle = GetWindowLongPtr(hwnd.ToInt32(), -20);
            SetWindowLong(hwnd, GWL_EXSTYLE, extendedStyle | WS_EX_TRANSPARENT | WS_EX_TOOLWINDOW);
        }
        protected override void OnSourceInitialized(EventArgs e)
        {
            base.OnSourceInitialized(e);
            var hwnd = new WindowInteropHelper(this).Handle;
            SetWindowExTransparent(hwnd);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            int pad = 16;
            this.Width = System.Windows.SystemParameters.PrimaryScreenWidth + pad;
            this.Height = System.Windows.SystemParameters.PrimaryScreenHeight + pad;
            this.Left = -pad / 2;
            this.Top = -pad / 2;
            this.Padding = new Thickness(0);
            this.Margin = new Thickness(0);
            this.Topmost = true;

            try
            {
                media.Source = new System.Uri((string)App.parsedArgs["src"]);
                media.IsMuted = (bool)App.parsedArgs["mute"];
                media.Opacity = (float)App.parsedArgs["opacity"];
                if ((bool)App.parsedArgs["loop"])
                {
                    media.MediaEnded += Media_MediaEnded;
                }
                t0 = DateTime.Now;
                System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
                dispatcherTimer.Start();
            }
            catch (Exception) { }
        }
    }
}
