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
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;
using System.Timers;
using System.Windows.Media;
using System.Globalization;

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Логика взаимодействия для My_Notification_Wndw.xaml
    /// </summary>
    public partial class My_Notification_Wndw : Window
    {
        private static My_Notification_Wndw _instance;
        DoubleAnimation _animation;
        public Note_Template note_notice = new Note_Template();
        public ObservableCollection<Note_Template> note_templates = new ObservableCollection<Note_Template>();

        public ObservableCollection<Note_Template> temp_note_data = new ObservableCollection<Note_Template>();
        public ObservableCollection<Note_Template> temp_completed_note_data = new ObservableCollection<Note_Template>();
        public Note_Template one_note = new Note_Template();

        private static System.Timers.Timer event_timer;

        private static string _header = "Header _ Header _ Header";
        private static SolidColorBrush _color;
        string _time = "00:00";
        private static string _date;




        public static My_Notification_Wndw Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new My_Notification_Wndw();
                }
                return _instance;
            }
        }

        public My_Notification_Wndw()
        {
            if (this.IsFocused)
            {
                return;
            }
            else
            {
                this.Top = (SystemParameters.WorkArea.Height - 120); // / 0x00000002; //top; //  / 0x00000002;
                this.Left = (SystemParameters.WorkArea.Width - 300); // / 0x00000002; //left;
                _animation = new DoubleAnimation(SystemParameters.WorkArea.Height, TimeSpan.FromSeconds(2));
                //My_Notification_Window_Parameters(color,  header, date_time);

                //if (Form1.note_Datas != null)
                //{
                //    note_notice = Form1.note_Datas[0];
                //}

                this.WindowState = WindowState.Minimized;
                this.Visibility = Visibility.Hidden;
                Delayed_Window_Closing();
            }
            InitializeComponent();
            _instance = this;
        }


        public void My_Notification_Window_Parameters(string color, string header, DateTime date_time)
        {
            _header = header;
            _color = (SolidColorBrush)new BrushConverter().ConvertFromString(color);
            _time = date_time.ToString("HH' ':' ' mm");
            _date = date_time.ToString("d MMMM", CultureInfo.CreateSpecificCulture(Properties.Settings.Default.languageCode));

            note_notice.Color = color;
            note_notice.Header = header;
            note_notice.Date = date_time;
            //Binding time_bind = new Binding();
            ////time_bind.ElementName = note_notice.ToString();
            ////time_bind.Path = Time;
            //time_bind.Source = note_notice.Time;
            //Time_TxtBx.SetBinding(note_notice, time_bind);
        }

        public async void TimeEvent()
        {
            try
            {
                await Task.Run(() =>
                {
                    event_timer = new System.Timers.Timer();
                    event_timer.Interval = 2000;
                    event_timer.AutoReset = true;
                    event_timer.Enabled = true;
                    event_timer.Elapsed += Closing_Window_Method;
                });
                //event_timer.Elapsed -=
            }
            catch (Exception)
            {
                return;
                //MessageBox.Show("Troubles");
            }
        }

        private void Notify_Wndw_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void close_notification_window_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this != null)
                {
                    //TimeInterval_MilliSeconds = 2000;
                    //AnimationClock clock = anim.CreateClock();
                    //this.ApplyAnimationClock(property, clock);
                    AnimationClock clock = _animation.CreateClock();
                    this.ApplyAnimationClock(Window.TopProperty, clock);
                    this.WindowState = WindowState.Minimized;
                    this.Close();
                }
            }
            catch (Exception)
            {
                return;
                //MessageBox.Show("Возникла ошибка");
            }
        }

        private void Complete_Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this != null)
                {
                    Complete_Note_Request_Window compl_wndw = new Complete_Note_Request_Window();
                    int index = -1;
                    note_notice.Color = this.Background.ToString();
                    note_notice.Date = Convert.ToDateTime((Date_TxtBx.Text + " " + Time_TxtBx.Text));
                    note_notice.Header = Header_TxtBx.Text;

                    MainWindow Form1 = new MainWindow();
                    Form1 = this.Owner as MainWindow;

                    if (Form1 != null && Form1.note_template != null)
                    {
                        temp_note_data = Form1.note_template;
                        for (int i = 0; i < temp_note_data.Count; i++)
                        {
                            if (note_notice.Color == temp_note_data[i].Color && note_notice.Date == temp_note_data[i].Date && note_notice.Header == temp_note_data[i].Header)
                            {
                                note_notice = Form1.note_template[i];
                                index = i;
                            }
                        }
                        compl_wndw.Complete_note_Method(note_notice, temp_note_data);
                        Form1.ListBx_Stack_Of_Notes.ItemsSource = temp_note_data;
                        Form1.ListBx_Stack_Of_Notes.Items.Refresh();

                        Complete_Note_Request_Window complete_note_wndw = new Complete_Note_Request_Window();
                        complete_note_wndw.Owner = this;
                        complete_note_wndw.Btn_OK_Click(sender, e);
                    }
                    AnimationClock clock = _animation.CreateClock();
                    this.ApplyAnimationClock(Window.TopProperty, clock);
                    this.WindowState = WindowState.Minimized;
                    this.Close();
                }
            }
            catch (Exception)
            {
                return;
                //MessageBox.Show("Having troubles");
            }
        }
        async void Delayed_Window_Closing()
        {
            try
            {
                if (this != null)
                {
                    await Task.Delay(/*120000*/120000);
                    AnimationClock clock = _animation.CreateClock();
                    this.ApplyAnimationClock(Window.TopProperty, clock);
                    this.WindowState = WindowState.Minimized;
                    this.Close();
                }
            }
            catch (Exception)
            {
                return;
                //MessageBox.Show("Возникла ошибка");
            }
        }

        public void Closing_Window_Method(Object source, ElapsedEventArgs e)
        {
            try
            {
                if (this != null)
                {
                    //AnimationClock clock = anim.CreateClock();
                    //this.ApplyAnimationClock(property, clock);
                    AnimationClock clock = _animation.CreateClock();
                    this.ApplyAnimationClock(Window.TopProperty, clock);
                    this.WindowState = WindowState.Minimized;
                    this.Close();
                }
            }
            catch (Exception)
            {
                return;
                //MessageBox.Show("Возникла ошибка");
            }
        }
    }
}
