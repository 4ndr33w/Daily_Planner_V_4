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
        StrDataRepository strings_;
        Time_Range_For_AddNoteWndw_TimePicker time_picker_range = new Time_Range_For_AddNoteWndw_TimePicker();

        List<string> time_picker_source = new List<string>();

        ObservableCollection<Group_of_Notes> union_grps = new ObservableCollection<Group_of_Notes>();

        List<Group_of_Notes> grp_list = new List<Group_of_Notes>();
        
        ObservableCollection<Group_Panel_Data> grp_panel = new ObservableCollection<Group_Panel_Data>();
        List<Note_Data> note_data = new List<Note_Data>();
        ObservableCollection<Note_Template> note_template = new ObservableCollection<Note_Template>();

        Note_Data current_note = new Note_Data();
        Group_of_Notes current_grp = new Group_of_Notes();
        Group_of_Notes default_grp = new Group_of_Notes();
        public Add_Note_Window()
        {
            InitializeComponent();
            loas_defaults();
            time_picker_source = time_picker_range.Choose_Time_Range(Properties.Languages.Lang.note_time_range_1_hour);
            time_picker.Items.Clear();
            time_picker.ItemsSource = time_picker_source;
        }
        private void loas_defaults()
        {
            MainWindow Form1 = new MainWindow();

            union_grps = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);
            CmbBox_Task_Groups.ItemsSource = union_grps;
            CmbBox_Task_Groups.Items.Refresh();
        }
       
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            MainWindow Form1 = new MainWindow();
            Form1 = Owner as MainWindow;
            union_grps = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);
            CmbBox_Task_Groups.ItemsSource = union_grps;
        }

        private void new_task_Window_Close_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void new_task_Window_OK_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;
            union_grps = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);

            grp_panel = Form1.my_Grps_Panel;

            if (date_picker.Text == String.Empty) { date_picker.Text = DateTime.Now.ToShortDateString(); }
            else if (time_picker.Text == String.Empty) { time_picker.Text = DateTime.Now.ToShortTimeString(); }

            int cmbBx_grps_selected_index = CmbBox_Task_Groups.SelectedIndex > -1 ? SelectedInCmboBx_Group_Index(union_grps, CmbBox_Task_Groups) : -1;

            if (Form1.create_new_note == true)
            {
                current_note.Header = (txtBx_Header_name.Text != String.Empty || txtBx_Header_name.Text != Properties.Languages.Lang.Enter_Header_string) ? txtBx_Header_name.Text : Properties.Languages.Lang.Note_Default_Header;
                current_note.Executor = (txtBx_Executor.Text != String.Empty || txtBx_Executor.Text != Properties.Languages.Lang.new_note_executor_empty_string) ? txtBx_Executor.Text : Properties.Languages.Lang.Executor_string_Me;
                current_note.Date = Convert.ToDateTime(date_picker.Text + ' ' + time_picker.Text);
                current_note.Creation_Date = DateTime.Now;
                current_note.Urgency = chkBx_Urgency.IsChecked == true ? true : false;
                current_note.Note = (new TextRange(txtBx_Note_TextBox.Document.ContentStart, txtBx_Note_TextBox.Document.ContentEnd)).Text;

                current_grp = CmbBox_Task_Groups.SelectedItem == null ? default_grp : CmbBox_Task_Groups.SelectedItem as Group_of_Notes;

                current_note.Group = current_grp;
                current_note.Color = current_grp.Color;

                Form1.note_template.Add(new Note_Template(current_note));
                Form1.ListBx_Stack_Of_Notes.Items.Refresh();
                if (CmbBox_Task_Groups.SelectedItem == null)
                {
                    int counter = 0;
                    foreach (var grp in Form1.my_Grps_Panel.ToArray())
                    {
                        if (grp.Grp_equals(default_grp))
                        {
                            counter++;
                        }
                    }
                    if (counter == 0) { grp_panel /*Form1.my_Grps_Panel*/.Add(new Group_Panel_Data(default_grp)); }

                    Form1.my_Grps_Panel = grp_panel;
                }
            }
            Form1.XML_Serialization(Form1.note_template, StrDataRepository.directory);

            Form1.All_Counter_Fills();
            this.Hide();
        }

        private int SelectedInCmboBx_Group_Index (ObservableCollection<Group_of_Notes> grps_collection, ComboBox cmbBx_as_grp)
        {
            int index = -1;
            for (int i = 0; i < grps_collection.Count; i++)
            {
                if ((cmbBx_as_grp.SelectedItem as Group_of_Notes).Grp_equals(grps_collection[i]))
                {
                    index = i;
                }
            }
            return index;
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
            time_picker_source = time_picker_range.Choose_Time_Range(time_range_comboBox.Text);
            time_picker.ItemsSource = time_picker_source;
            time_picker.Items.Refresh();
        }

        private void time_range_comboBox_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            time_picker_source = time_picker_range.Choose_Time_Range(time_range_comboBox.Text);
            time_picker.ItemsSource = time_picker_source;
            time_picker.Items.Refresh();
        }

        private void time_range_comboBox_DropDownClosed(object sender, EventArgs e)
        {
            time_picker_source = time_picker_range.Choose_Time_Range(time_range_comboBox.Text);
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
                current_grp.Color = (CmbBox_Task_Groups.SelectedItem as Group_of_Notes).Color;
                current_grp.Execution_of = (CmbBox_Task_Groups.SelectedItem as Group_of_Notes).Execution_of;
                current_grp.Group_Name = (CmbBox_Task_Groups.SelectedItem as Group_of_Notes).Group_Name;
            }
            else return;
        }
    }
}
