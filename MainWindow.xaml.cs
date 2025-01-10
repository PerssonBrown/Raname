using System.Windows;
using System.Windows.Input;
using Newtonsoft.Json.Linq;
using System.IO;

namespace Raname
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.IsManipulationEnabled = true;
            InitializeConfig();
        }

        private bool rolling = false;
        private Thread? rollingThread;

        private void RollingTextBox()
        {
            Random ran = new Random();
            while(rolling)
            {
                Dispatcher.Invoke(()=>
                {
                    RandomTextBlock.Text = nameList[ran.Next(nameList.Length - 1)];
                });
                Thread.Sleep(timeDelta);
            }
        }

        private bool configed = false;

        private void RandomBtn_Click(object sender, RoutedEventArgs e)
        {
            if (configed == false)
            {
                return;
            }
            if (rolling)
            {
                rolling = false;
                rollingThread?.Join();
                rollingThread = null;
            }
            else
            {
                rolling = true;
                rollingThread = new Thread(RollingTextBox)
                {
                    IsBackground = true
                };
                rollingThread.Start();
            }
        }

        private System.Windows.Point pressPosition;
        bool isDragMoved = false;

        private void Window_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            pressPosition = e.GetPosition(this);
        }

        private void Window_PreviewMouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (Mouse.LeftButton == MouseButtonState.Pressed && pressPosition != e.GetPosition(this))
            {
                isDragMoved = true;
                DragMove();
            }
        }

        private void Window_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isDragMoved)
            {
                RandomBtn.ReleaseMouseCapture();
                isDragMoved = false;
                e.Handled = true;
            }
        }

        private void Window_TouchDown(object sender, TouchEventArgs e)
        {
            pressPosition = e.GetTouchPoint(this).Position;
            this.CaptureTouch(e.TouchDevice);
        }

        private void Window_TouchMove(object sender, TouchEventArgs e)
        {
            isDragMoved = true;

            var currentPoint = e.GetTouchPoint(this).Position;

            var offsetX = currentPoint.X - pressPosition.X;
            var offsetY = currentPoint.Y - pressPosition.Y;

            this.Left += offsetX;
            this.Top += offsetY;
        }

        private void Window_TouchUp(object sender, TouchEventArgs e)
        {
            if (isDragMoved)
            {
                RandomBtn.ReleaseMouseCapture();
                this.ReleaseTouchCapture(e.TouchDevice);
                isDragMoved = false;
                e.Handled = true;
            }
        }

        private String[] nameList;
        private int timeDelta;

        private void InitializeConfig()
        {
            String currentDir = System.Environment.CurrentDirectory;
            Console.WriteLine(currentDir);
            string cfgfile = currentDir + "/config.json";
            if (File.Exists(cfgfile) == false)
            {
                Console.WriteLine("Config file not found");
                RandomTextBlock.Text = "No Config";
                return;
            }
            String file = File.ReadAllText(cfgfile);
            JObject jObject = JObject.Parse(file);
            // Parse name list
            JArray? jNameListArray = (JArray?)jObject["nameList"];
            if (jNameListArray == null)
            {
                goto badconfig;
            }
            nameList = jNameListArray.ToObject<String[]>();
            if (nameList.Length == 0)
            {
                goto badconfig;
            }
            //Parse setting
            JValue? jTimeDelta = (JValue?)jObject["timeDelta"];
            if (jTimeDelta == null)
            {
                goto badconfig;
            }
            if ( int.TryParse(jTimeDelta.ToString(), out timeDelta) == false)
            {
                goto badconfig;
            }
            RandomTextBlock.Text = "Ready";
            configed = true;
            return;

            badconfig:
            RandomTextBlock.Text = "Bad Config";
            return;
        }
    }
}