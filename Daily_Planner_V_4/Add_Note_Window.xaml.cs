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
using System.Collections.ObjectModel;

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Interaction logic for Add_Note_Window.xaml
    /// </summary>
    public partial class Add_Note_Window : Window
    {
        strings_data_repository strings_;
        Time_Range_For_AddNoteWndw_TimePicker time_picker_range = new Time_Range_For_AddNoteWndw_TimePicker();
        List<string> time_picker_1h_range = new List<string>();
        List<string> time_picker_30min_range = new List<string>();
        List<string> time_picker_10min_range = new List<string>();
        List<string> time_picker_5min_range = new List<string>();

        List<string> time_picker_source = new List<string>();

        ObservableCollection<Group_of_Notes> union_grps = new ObservableCollection<Group_of_Notes>();

        List<Group_of_Notes> grp_list = new List<Group_of_Notes>();
        
        ObservableCollection<Group_Panel_Data> grp_panel = new ObservableCollection<Group_Panel_Data>();
        List<Note_Data> note_data = new List<Note_Data>();
        ObservableCollection<Note_Template> note_template = new ObservableCollection<Note_Template>();
        public Add_Note_Window()
        {
            InitializeComponent();
            loas_defaults();
            time_picker_source = time_picker_1h_range;
            time_picker.Items.Clear();
            time_picker.ItemsSource = time_picker_source;
        }
        private void loas_defaults()
        {
            time_picker_1h_range = time_picker_range.Choose_Time_Range(/*"1 час"*/Properties.Languages.Lang.note_time_range_1_hour);
            time_picker_30min_range = time_picker_range.Choose_Time_Range(/*"30 мин"*/Properties.Languages.Lang.note_time_range_30_min);
            time_picker_10min_range = time_picker_range.Choose_Time_Range(/*"10 мин"*/Properties.Languages.Lang.note_time_range_10_min);
            time_picker_5min_range = time_picker_range.Choose_Time_Range(/*"5 мин"*/Properties.Languages.Lang.note_time_range_5_min);

            MainWindow Form1 = new MainWindow();

            union_grps = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);

            CmbBox_Task_Groups.ItemsSource = union_grps;
            CmbBox_Task_Groups.Items.Refresh();
        }
        private List<string> time_picker_ranges_return (string time_range_string)
        {
            MainWindow Form1 = new MainWindow();
            string time_picker_ignore_locale_text = Form1.Localization_return_source_value_Method(time_range_string);
            //int compare_result = Form1.Localization_Compare_Method(time_range_string, Properties.Languages.Lang.note_time_range_1_hour);
            if (Form1.Localization_Compare_Method(time_range_string, Properties.Languages.Lang.note_time_range_1_hour) == 0/*time_range_string == Properties.Languages.Lang.note_time_range_1_hour*/)
            {
                return time_picker_1h_range;
            }
            if (Form1.Localization_Compare_Method(time_range_string, Properties.Languages.Lang.note_time_range_30_min) == 0/*time_range_string == Properties.Languages.Lang.note_time_range_30_min*/)
            {
                return time_picker_30min_range;
            }
            if (Form1.Localization_Compare_Method(time_range_string, Properties.Languages.Lang.note_time_range_10_min) == 0)
            {
                return time_picker_10min_range;
            }
            if (Form1.Localization_Compare_Method(time_range_string, Properties.Languages.Lang.note_time_range_5_min) == 0)
            {
                return time_picker_5min_range;
            }
            else return null;
        }
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainWindow Form1 = new MainWindow();
            Form1 = Owner as MainWindow;
            union_grps = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);
            CmbBox_Task_Groups.ItemsSource = union_grps;


            //MainWindow Form1 = new MainWindow();
            //Form1 = Owner as MainWindow;
            //union_grps = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);
        }

        private void new_task_Window_Close_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void new_task_Window_OK_Button_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void txtBx_Header_name_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (txtBx_Header_name.Text == string.Empty || txtBx_Header_name.Text == Properties.Languages.Lang.Enter_Header_string)
            {
                txtBx_Header_name.Text = "";
            }
        }

        private void txtBx_Header_name_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBx_Header_name.Text == string.Empty || txtBx_Header_name.Text == Properties.Languages.Lang.Enter_Header_string)
            {
                txtBx_Header_name.Text = Properties.Languages.Lang.Enter_Header_string;
            }
        }

        private void date_picker_CalendarClosed(object sender, RoutedEventArgs e)
        {
            DateTime note_date_time = new DateTime();
            if (date_picker.Text == String.Empty)
            {
                date_picker.Text = DateTime.Now.ToShortDateString();
            }
            if (time_picker.Text == String.Empty)
            {
                time_picker.Text = DateTime.Now.ToShortTimeString();
            }
            note_date_time = Convert.ToDateTime((date_picker.Text + " " + time_picker.Text).ToString());
        }

        private void time_range_comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            time_picker_source = time_picker_ranges_return(time_range_comboBox.Text);
            time_picker.ItemsSource = time_picker_source;
            time_picker.Items.Refresh();
        }

        private void time_range_comboBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            time_picker_source = time_picker_ranges_return(time_range_comboBox.Text);
            time_picker.ItemsSource = time_picker_source;
            time_picker.Items.Refresh();
        }

        private void time_range_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            time_picker_source = time_picker_ranges_return(time_range_comboBox.Text);
            time_picker.ItemsSource = time_picker_source;
            time_picker.Items.Refresh();
        }

        private void txtBx_Executor_IsMouseCapturedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (txtBx_Executor.Text == string.Empty || txtBx_Executor.Text == Properties.Languages.Lang.new_note_executor_empty_string)
            {
                txtBx_Executor.Text = "";
            }
        }

        private void txtBx_Executor_LostFocus(object sender, RoutedEventArgs e)
        {
            if (CmbBox_Task_Groups.SelectedItem == null && txtBx_Executor.Text == string.Empty)
            {
                txtBx_Executor.Text = Properties.Languages.Lang.new_note_executor_empty_string;
            }
            else
            {
                int cmbBx_selected_index = CmbBox_Task_Groups.SelectedIndex;
                string executor = (txtBx_Executor.Text == "" || txtBx_Executor.Text == Properties.Languages.Lang.new_note_executor_empty_string) ? union_grps[cmbBx_selected_index].Execution_of : txtBx_Executor.Text;
                txtBx_Executor.Text = "";
                txtBx_Executor.Text = executor;
            }
        }

        private void CmbBox_Task_Groups_DropDownClosed(object sender, EventArgs e)
        {
            if (CmbBox_Task_Groups.SelectedIndex > -1)
            {
                int cmbBx_selected_index = CmbBox_Task_Groups.SelectedIndex;
                string executor = union_grps[cmbBx_selected_index].Execution_of;
                if (txtBx_Executor.Text == string.Empty || txtBx_Executor.Text == Properties.Languages.Lang.new_note_executor_empty_string)
                {
                    txtBx_Executor.Text = executor;
                }
            }
            else return;
        }
    }
}
