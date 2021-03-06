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
using System.Threading;
using System.Globalization;
using System.Collections.ObjectModel;
using System.IO;
using System.Xml.Serialization;
using System.Resources;
using System.ComponentModel;
using System.Timers;

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private System.Windows.Forms.NotifyIcon _notifyIcon;
        private WindowState _stored_WindowState = WindowState.Normal;
        private static System.Timers.Timer event_timer;

        public My_Notification_Wndw Notify_Wndw;

        public StrDataRepository strings_ = new StrDataRepository();
        public Note_Template current_note_template = new Note_Template();
        Note_Counter_Fills counter_fills_class = new Note_Counter_Fills();
        public ObservableCollection<Group_Panel_Data> my_Grps_Panel = new ObservableCollection<Group_Panel_Data>();
        public ObservableCollection<Group_Panel_Data> delegated_Grps_Panel = new ObservableCollection<Group_Panel_Data>();
        public ObservableCollection<Group_of_Notes> union_Grps = new ObservableCollection<Group_of_Notes>();

        public ObservableCollection<Note_Template> note_template = new ObservableCollection<Note_Template>();
        public ObservableCollection<Note_Template> expired_note_template = new ObservableCollection<Note_Template>();
        public ObservableCollection<Note_Template> completed_note_template = new ObservableCollection<Note_Template>();
        public ObservableCollection<Note_Template> today_notes = new ObservableCollection<Note_Template>();

        public bool create_new_note = false;
        string langCode = "en-GB";
        public MainWindow()
        {
            langCode = Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);


            InitializeComponent();

            Notify_Wndw = My_Notification_Wndw.Instance;
            Load_Default_Data();
        }
        public void Hide_Btns_when_Selection_Changed(ObservableCollection<Note_Template> data)
        {
            foreach (var note in data)
            {
                note.Btn_Hide_Compl_or_Exp_note_Visibility = StrDataRepository.Visibility_hidden;
                note.Delete_Btn_Visibility = StrDataRepository.Visibility_hidden;
                note.Edit_Btn_Visibility = StrDataRepository.Visibility_hidden;
                note.Mart_to_Complete_note_Visibility= StrDataRepository.Visibility_hidden;
            }
        }
        public void Group_Counter_Fills(ObservableCollection<Note_Template> note_collection, ObservableCollection<Group_Panel_Data> grps_collection)
        {
            foreach (var grp in grps_collection)
            {
                grp.Filtered_notes_count = counter_fills_class.Group_Counter(note_collection, grp);
            }
        }
        public void All_Counter_Fills ()
        {
            if (TxtBx_All_Notes_Counter != null)
            {
                TxtBx_All_Notes_Counter.Text = note_template != null ? note_template.Count.ToString() : "0";
                TxtBlck_Today_Notes_Counter.Text = note_template != null ? counter_fills_class.Today_Counter(note_template).ToString() : "0";
                TxtBx_Expired_Notes_Counter.Text = expired_note_template != null ? expired_note_template.Count.ToString() : "0";
                TxtBx_Completed_Notes_Counter.Text = completed_note_template != null ? completed_note_template.Count.ToString() : "0";

                Group_Counter_Fills(note_template, my_Grps_Panel);
                Group_Counter_Fills(note_template, delegated_Grps_Panel);

                string day_and_month = DateTime.Today.ToString("MMM d", CultureInfo.CreateSpecificCulture(langCode)) + ", " + DateTime.Today.ToString("ddd", CultureInfo.CreateSpecificCulture(langCode));
                string month_ToUpper = "";
                string date = day_and_month;
                if (langCode == "ru-RU")
                {
                    month_ToUpper = day_and_month.ToUpper();
                    day_and_month.Replace(day_and_month[1], month_ToUpper[1]);
                    date = month_ToUpper[0] + day_and_month.Substring(1);
                }
                Today_Date_txtBx_of_Today_Notes_Btn.Text = date;
            }
        }
        public void Load_Default_Data()
        {
            _notifyIcon = new System.Windows.Forms.NotifyIcon();
            _notifyIcon.BalloonTipText = Properties.Languages.Lang.NotifyIcon_Minimized_Announce;
            _notifyIcon.Text = Properties.Languages.Lang.The_App_Name;
            _notifyIcon.BalloonTipTitle = Properties.Languages.Lang.The_App_Name;
            _notifyIcon.Icon = new System.Drawing.Icon("Icon.ico");
            _notifyIcon.Click += new EventHandler(NotifyIcon_Click);
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            union_Grps = XML_Grps_Deserialization(StrDataRepository.directory);
            note_template = Fill_Note_Template_From_List_Data_and_sortByDate(   XML_Note_Deserialization(strings_.Note_Data_Save)  );
            TimeSpan is_note_expired;
            foreach (var note in note_template)
            {
                is_note_expired = DateTime.Now - note.Date;
                if (is_note_expired.TotalSeconds > 0)
                {
                    note.Urgency = false;
                    note.Status = Properties.Languages.Lang.Expired_string;
                    note.Color = Colors.Transparent.ToString();
                    note.Group.Color = Colors.Transparent.ToString();
                    note.Foreground = Colors.White.ToString();
                    note.Urgency = false;
                    note.Status_Title = Properties.Languages.Lang.Expired_string;
                }
            }
            expired_note_template = Fill_Note_Template_From_List_Data_and_sortByDate( XML_Note_Deserialization(StrDataRepository.expired_note_full_filePath));
            completed_note_template = Fill_Note_Template_From_List_Data_and_sortByDate(XML_Note_Deserialization(StrDataRepository.completed_note_full_filePath));
            Fill_Grps_From_Union(union_Grps);
            ListBx_Grp_Of_My_Tasks.ItemsSource = my_Grps_Panel;
            ListBx_Grp_Of_Delegated_Tasks.ItemsSource = delegated_Grps_Panel;
            ListBx_Stack_Of_Notes.ItemsSource = note_template;
            All_Counter_Fills();
            ListBx_Stack_Of_Notes.Items.Refresh();
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
        }
        public void Fill_Grps_From_Union(ObservableCollection<Group_of_Notes> data)
        {
            ObservableCollection<Group_of_Notes> temp_data = new ObservableCollection<Group_of_Notes>();
            temp_data.Clear();
            temp_data = data;
            if (temp_data != null)
            {
                foreach (var grp in temp_data)
                {
                    if (grp.Execution_of == StrDataRepository.Executor_Me_Ru || grp.Execution_of == StrDataRepository.Executor_Me_En)
                    {
                        my_Grps_Panel.Add(new Group_Panel_Data(grp));
                    }
                    if (grp.Execution_of == StrDataRepository.Executor_Deleg_En || grp.Execution_of == StrDataRepository.Executor_Deleg_Ru)
                    {
                        delegated_Grps_Panel.Add(new Group_Panel_Data(grp));
                    }
                }
            }
        }
       
        public ObservableCollection<Group_of_Notes> Union_Grps_Method(ObservableCollection<Group_Panel_Data> my_grps, ObservableCollection<Group_Panel_Data> deleg_grps)
        {
            ObservableCollection<Group_of_Notes> temp_grps_coll = new ObservableCollection<Group_of_Notes>();
            foreach (var grp in my_Grps_Panel) temp_grps_coll.Add(new Group_of_Notes(grp));
            foreach (var grp in delegated_Grps_Panel) temp_grps_coll.Add(new Group_of_Notes(grp));
            return temp_grps_coll;
        }
        private void Language_Change_Method(string language)
        {
            Lang_Change_Message_Window lang_change_Wndw = new Lang_Change_Message_Window();
            lang_change_Wndw.Owner = this;
            Properties.Settings.Default.languageCode = language;
            Properties.Settings.Default.Save();
            lang_change_Wndw.Show();
        }
        private void menu_eng_lang_Click(object sender, RoutedEventArgs e)
        {
            Language_Change_Method("en-GB");
        }

        private void menu_ru_lang_Click(object sender, RoutedEventArgs e)
        {
            Language_Change_Method("ru-RU");
        }
        private void Menu_Close_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        public int Find_Index_Of_Equal_Grp_In_Collection(Group_of_Notes note_Grp, ObservableCollection<Group_of_Notes> note_Grp_Collection)
        {
            int index = -1;
            for (int i = 0; i < note_Grp_Collection.Count; i++)
            {
                if (note_Grp_Collection[i].Group_Name == note_Grp.Group_Name && note_Grp_Collection[i].Color == note_Grp.Color &&
                    ((note_Grp_Collection[i].Execution_of == StrDataRepository.Executor_Me_En) || (note_Grp_Collection[i].Execution_of == StrDataRepository.Executor_Me_Ru)))
                {
                    index = i;
                }

                if (note_Grp_Collection[i].Group_Name == note_Grp.Group_Name &&
                   note_Grp_Collection[i].Color == note_Grp.Color &&
                   ((note_Grp_Collection[i].Execution_of == StrDataRepository.Executor_Deleg_En) || (note_Grp_Collection[i].Execution_of == StrDataRepository.Executor_Deleg_Ru)))
                {
                    index = i;
                }
            }
            return index;
        }

        private void Btn_Add_Grp_My_Tasks_Click(object sender, RoutedEventArgs e)
        {
            Add_Grp_Window grp_create_wndw = new Add_Grp_Window();
            grp_create_wndw.Owner = this;

            strings_.GrpCreation_MyOrDelegated_Mode = "my_grp";
            grp_create_wndw.Show();
            ListBx_Grp_Of_My_Tasks.ItemsSource = my_Grps_Panel;
        }

        private void Btn_Add_New_Delegated_Task_Group_Click(object sender, RoutedEventArgs e)
        {
            Add_Grp_Window grp_create_wndw = new Add_Grp_Window();
            grp_create_wndw.Owner = this;

            strings_.GrpCreation_MyOrDelegated_Mode = "delegated_grp";
            grp_create_wndw.Show();
            ListBx_Grp_Of_Delegated_Tasks.ItemsSource = delegated_Grps_Panel;
        }
        public void XML_Serialization <T>(ObservableCollection<T> data_to_serialize, string directory)
        {
            ObservableCollection<Note_Data> temp_data = new ObservableCollection<Note_Data>();
            ObservableCollection<T> data = new ObservableCollection<T>();
            try
            {
                string file_path = directory;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }

                if (data_to_serialize.Count > -1)
                {
                    if (data_to_serialize is ObservableCollection<Group_of_Notes>)
                    {
                        file_path = directory + StrDataRepository.grps_short_fileName;
                        Xml_auxillary_serialize_Method(data_to_serialize, file_path);
                    }
                    if (data_to_serialize is ObservableCollection<Note_Template> &&
                        ((data_to_serialize as ObservableCollection<Note_Template>)[0].Status == StrDataRepository.Status_Completed_En ||
                        (data_to_serialize as ObservableCollection<Note_Template>)[0].Status == StrDataRepository.Status_Completed_Ru))

                    {
                        file_path = directory + StrDataRepository.completed_note_short_filePath;
                        foreach (var note in data_to_serialize as ObservableCollection<Note_Template>)
                        {
                            temp_data.Add(new Note_Data(note));
                        }
                        Xml_auxillary_serialize_Method(temp_data, file_path);
                    }

                    if (data_to_serialize is ObservableCollection<Note_Template> &&
                       ((data_to_serialize as ObservableCollection<Note_Template>)[0].Status == StrDataRepository.Status_Expired_En ||
                       (data_to_serialize as ObservableCollection<Note_Template>)[0].Status == StrDataRepository.Status_Expired_Ru))
                    {
                        file_path = directory + StrDataRepository.expired_note_short_filePath;
                        foreach (var note in data_to_serialize as ObservableCollection<Note_Template>)
                        {
                            temp_data.Add(new Note_Data(note));
                        }
                        Xml_auxillary_serialize_Method(temp_data, file_path);
                    }
                    if (data_to_serialize is ObservableCollection<Note_Template> &&
                     (data_to_serialize as ObservableCollection<Note_Template>)[0].Status != StrDataRepository.Status_Expired_En &&
                     (data_to_serialize as ObservableCollection<Note_Template>)[0].Status != StrDataRepository.Status_Expired_Ru &&
                     (data_to_serialize as ObservableCollection<Note_Template>)[0].Status != StrDataRepository.Status_Completed_En &&
                     (data_to_serialize as ObservableCollection<Note_Template>)[0].Status != StrDataRepository.Status_Completed_Ru)
                    {
                        file_path = directory + StrDataRepository.note_short_fileName;
                        foreach (var note in /*data_to_serialize*/ note_template as ObservableCollection<Note_Template>)
                        {
                            temp_data.Add(new Note_Data(note));
                        }
                        Xml_auxillary_serialize_Method(temp_data, file_path);
                    }
                }
                
            }
            catch (Exception)
            {
                //MessageBox.Show("Проблемы в основном методе сохранения");
                return;
            }
        }
        private void Xml_auxillary_serialize_Method <T> (ObservableCollection<T> data, string file_path)
        {
            try
            {
                if (File.Exists(file_path)) { File.Delete(file_path); }
                XmlSerializer xml_serializer = new XmlSerializer(data.GetType());
                Stream fstream = new FileStream(file_path, FileMode.Create, FileAccess.Write);
                xml_serializer.Serialize(fstream, data);
                fstream.Close();
            }
            catch (Exception)
            {
                //MessageBox.Show("Проблемы во вспомогательном методе сохранения");
                return;
            }
        }
        public ObservableCollection<Group_of_Notes> XML_Grps_Deserialization(string directory)
        {
            ObservableCollection<Group_of_Notes> temp_grps = new ObservableCollection<Group_of_Notes>();
            HashSet<Group_of_Notes> hashSet_Groups = new HashSet<Group_of_Notes>();
            List<Note_Data> temp_notes = new List<Note_Data>();
            List<Group_of_Notes> temp_notes_grps = new List<Group_of_Notes>();
            try
            {
                string grps_file_path = directory + StrDataRepository.grps_short_fileName;
                string note_file_path = directory + StrDataRepository.note_short_fileName;
                if (File.Exists(grps_file_path))
                {
                    XmlSerializer xml_serializer = new XmlSerializer(temp_grps.GetType());
                    Stream fStream = new FileStream(grps_file_path, FileMode.Open, FileAccess.Read);
                    temp_grps = xml_serializer.Deserialize(fStream) as ObservableCollection<Group_of_Notes>;
                    fStream.Close();
                }
                // загружаем группы из файла с заметками;   сравниваем с уже существующими группами

                if (File.Exists(note_file_path))
                {
                    XmlSerializer xml_serializer = new XmlSerializer(typeof(List<Note_Data>));
                    Stream fStream = new FileStream(note_file_path, FileMode.Open, FileAccess.Read);
                    temp_notes = xml_serializer.Deserialize(fStream) as List<Note_Data>;
                    fStream.Close();

                    int grp_counter = 0;
                    for (int i = 0; i < temp_notes.Count; i++)
                    {
                        for (int j = 0; j < temp_grps.Count; j++)
                        {
                            if (temp_notes[i].Group.Grp_equals(temp_grps[j]))
                            {
                                grp_counter++;
                            }
                        }
                        if (grp_counter == 0)
                        {
                            temp_grps.Add(new Group_of_Notes(temp_notes[i].Group));
                        }
                    }
                    return temp_grps;
                }

                return temp_grps;
            }
            catch (Exception)
            {
                return temp_grps;
            } 
        }
        public ObservableCollection<Note_Template> Fill_Note_Template_From_List_Data_and_sortByDate (List<Note_Data> data)
        {
            ObservableCollection<Note_Template> temp_data = new ObservableCollection<Note_Template>();
            if (data != null)
            {
                data.Sort(new Note_Data.Sort_By_Date());
                for (int i = 0; i < data.Count; i++)
                {
                    temp_data.Add(new Note_Template(data[i]));
                    if (data[i].Status == StrDataRepository.Status_Expired_En || temp_data[0].Status == StrDataRepository.Status_Expired_Ru)
                    {
                        temp_data[i].Status_Title = Properties.Languages.Lang.Expired_string;
                        temp_data[i].Color = Colors.Transparent.ToString();
                        temp_data[i].Group.Color = Colors.Transparent.ToString();
                        temp_data[i].Foreground = Colors.White.ToString();
                    }
                    if (data[i].Status == StrDataRepository.Status_Completed_En || temp_data[0].Status == StrDataRepository.Status_Completed_En)
                    {
                        temp_data[i].Status_Title = Properties.Languages.Lang.Completed_Note_String;
                    }
                }
                return temp_data;
            }
            return temp_data;
        }
        public List<Note_Data> XML_Note_Deserialization (string path)
        {
            List<Note_Data> temp_data = new List<Note_Data>();
            int last_slash_index = path.LastIndexOf(@"\");
            string directory = path.Substring(0, last_slash_index);
            try
            {
                if (!Directory.Exists(directory))
                {
                    return null;
                }
                else
                {
                    if (File.Exists(path))
                    {
                        XmlSerializer xml_serializer = new XmlSerializer(temp_data.GetType());
                        Stream fStream = new FileStream(path, FileMode.Open, FileAccess.Read);
                        temp_data = xml_serializer.Deserialize(fStream) as List<Note_Data>;
                        fStream.Close();
                        return temp_data;
                    }
                    
                    else return null;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Проблемы при загрузке файла с заметками");
                return null;
            }
        }

        private void Btn_Create_New_Note_Click(object sender, RoutedEventArgs e)
        {
            Add_Note_Window add_note_Wndw = new Add_Note_Window();
            add_note_Wndw.Owner = this;
            create_new_note = true;
            add_note_Wndw.Show();
        }

        private void ListBx_Grp_Of_My_Tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (note_template != null)
            {
                int index = ListBx_Grp_Of_My_Tasks.SelectedIndex;
                if (index > -1)
                {
                    ListBx_Stack_Of_Notes.ItemsSource = note_template.Where(find_grp_filter);
                    ListBx_Stack_Of_Notes.Items.Refresh();
                }
            }
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            Hide_Btns_when_Selection_Changed(note_template);
        }

        private void ListBx_Grp_Of_Delegated_Tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            if (note_template != null)
            {
                int index = ListBx_Grp_Of_Delegated_Tasks.SelectedIndex;
                if (index > -1)
                {
                    ListBx_Stack_Of_Notes.ItemsSource = note_template.Where(find_grp_filter);
                    ListBx_Stack_Of_Notes.Items.Refresh();
                }
            }
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            Hide_Btns_when_Selection_Changed(note_template);
        }
        public bool find_grp_filter(Note_Template arg)
        {
            bool result = false;
            if (arg.Group != null && ListBx_Grp_Of_My_Tasks.SelectedItem != null)
            {
                if (arg.Group.Grp_equals(ListBx_Grp_Of_My_Tasks.SelectedItem as Group_of_Notes)) result = true;
                else result = false;
            }
            if (arg != null && ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null)
            {
                if ((ListBx_Grp_Of_Delegated_Tasks.SelectedItem as Group_of_Notes).Grp_equals(arg.Group))
                {
                    result = true;
                }
                else result = false;
            } 
            return result;
        }

        private void ListBx_Grp_Of_My_Tasks_Unselected(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void Btn_Show_Today_Notes_Click(object sender, RoutedEventArgs e)
        {
            if (note_template != null)
            {
                ListBx_Stack_Of_Notes.ItemsSource = note_template.Where(a => a.Date.ToShortDateString() == DateTime.Today.ToShortDateString());
            }
            Hide_Btns_when_Selection_Changed(note_template);
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
        }

        private void Btn_Show_All_Notes_Click(object sender, RoutedEventArgs e)
        {
            if (note_template != null)
            {
                ListBx_Stack_Of_Notes.ItemsSource = note_template;
            }
            Hide_Btns_when_Selection_Changed(note_template);
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
        }
       
        private void Btn_my_task_group_delete_group_Click(object sender, RoutedEventArgs e)
        {
            if (ListBx_Grp_Of_My_Tasks.SelectedItem != null)
            {
                Delete_Grp_Request_Window delete_grp_Wndw = new Delete_Grp_Request_Window();
                delete_grp_Wndw.Owner = this;
                delete_grp_Wndw.Show();
            }
        }

        private void Btn_delegated_task_group_delete_group_Click(object sender, RoutedEventArgs e)
        {
            if (ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null/* && (sender as ComboBoxItem) == ListBx_Grp_Of_Delegated_Tasks.SelectedItem*/)
            {
                Delete_Grp_Request_Window delete_grp_Wndw = new Delete_Grp_Request_Window();
                delete_grp_Wndw.Owner = this;
                delete_grp_Wndw.Show();
            }
        }

        private void ListBx_Stack_Of_Notes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBx_Stack_Of_Notes.SelectedItem != null && ListBx_Stack_Of_Notes.ItemsSource != expired_note_template && ListBx_Stack_Of_Notes.ItemsSource != completed_note_template && note_template != null)
            {
                current_note_template = ListBx_Stack_Of_Notes.SelectedItem as Note_Template;
                foreach (var note in note_template)
                {
                    note.Btn_Hide_Compl_or_Exp_note_Visibility = StrDataRepository.Visibility_hidden;
                    note.Delete_Btn_Visibility = StrDataRepository.Visibility_hidden;
                    note.Edit_Btn_Visibility = StrDataRepository.Visibility_hidden;
                    note.Mart_to_Complete_note_Visibility = StrDataRepository.Visibility_hidden;

                    if (note.Creation_Date == current_note_template.Creation_Date)
                    {
                        note.Delete_Btn_Visibility = StrDataRepository.Visibility_visible;
                        note.Edit_Btn_Visibility = StrDataRepository.Visibility_visible;
                        note.Mart_to_Complete_note_Visibility = StrDataRepository.Visibility_visible;
                        if (note.Status_Title == Properties.Languages.Lang.Expired_string || note.Status_Title == Properties.Languages.Lang.Completed_Note_String)
                        {
                            note.Btn_Hide_Compl_or_Exp_note_Visibility = StrDataRepository.Visibility_visible;
                            note.Mart_to_Complete_note_Visibility = StrDataRepository.Visibility_hidden;
                        }
                    }
                }
            }
            if (ListBx_Stack_Of_Notes.ItemsSource == expired_note_template && expired_note_template != null && ListBx_Stack_Of_Notes.SelectedIndex > -1)
            {
                Hide_Btns_when_Selection_Changed(expired_note_template);
                if (ListBx_Stack_Of_Notes.SelectedItem != null)
                {
                    expired_note_template[ListBx_Stack_Of_Notes.SelectedIndex].Delete_Btn_Visibility = StrDataRepository.Visibility_visible;
                }
            }
            if (ListBx_Stack_Of_Notes.ItemsSource == completed_note_template && completed_note_template != null && ListBx_Stack_Of_Notes.SelectedIndex > -1)
            {
                Hide_Btns_when_Selection_Changed(completed_note_template);
                completed_note_template[ListBx_Stack_Of_Notes.SelectedIndex].Delete_Btn_Visibility = StrDataRepository.Visibility_visible;
            }
            ListBx_Stack_Of_Notes.Items.Refresh();
        }

        private void Btn_Delete_note_Click(object sender, RoutedEventArgs e)
        {
            if (ListBx_Stack_Of_Notes.SelectedItem != null)
            {
                Delete_Note_Request_Window delete_note_Wndw = new Delete_Note_Request_Window();
                delete_note_Wndw.Owner = this;
                delete_note_Wndw.Show();
            }
        }

        public int Selected_Note_Index(ObservableCollection<Note_Template> note_data_collection)
        {
            ObservableCollection<Note_Template> data = new ObservableCollection<Note_Template>();
            Note_Template current_note = new Note_Template();
            current_note = ListBx_Stack_Of_Notes.SelectedItem as Note_Template;
            data = note_data_collection;
            int index = -1;
            if (ListBx_Stack_Of_Notes.SelectedItem != null && data != null)
            {
                for (int i = 0; i < data.Count; i++)
                {
                    if (data[i].Creation_Date == current_note.Creation_Date) { index = i; }
                }
            }
            return index;
        }

        private void Btn_Hide_Expired_Complete_Note_Click(object sender, RoutedEventArgs e)
        {
            int index = -1;
            ObservableCollection<Note_Template> temp_filtered_data = new ObservableCollection<Note_Template>();
            if ((ListBx_Stack_Of_Notes.SelectedItem as Note_Template).Status_Title == StrDataRepository.Status_Expired_En || (ListBx_Stack_Of_Notes.SelectedItem as Note_Template).Status_Title == StrDataRepository.Status_Expired_Ru)
            {
                expired_note_template.Add(ListBx_Stack_Of_Notes.SelectedItem as Note_Template);
                XML_Serialization(expired_note_template, StrDataRepository.directory);
            }
            if ((ListBx_Stack_Of_Notes.SelectedItem as Note_Template).Status_Title == StrDataRepository.Status_Completed_En || (ListBx_Stack_Of_Notes.SelectedItem as Note_Template).Status_Title == StrDataRepository.Status_Completed_En)
            {
                completed_note_template.Add(ListBx_Stack_Of_Notes.SelectedItem as Note_Template);
                XML_Serialization(completed_note_template, StrDataRepository.directory);
            }
            index = Selected_Note_Index(note_template);
            note_template.RemoveAt(index);

            if (ListBx_Stack_Of_Notes.ItemsSource != note_template)
            {
                for (int i = 0; i < ListBx_Stack_Of_Notes.Items.Count; i++)
                {
                    temp_filtered_data.Add(ListBx_Stack_Of_Notes.Items[i] as Note_Template);
                }
                index = Selected_Note_Index(temp_filtered_data);
                temp_filtered_data.RemoveAt(index);
                ListBx_Stack_Of_Notes.ItemsSource = temp_filtered_data;
                index = Selected_Note_Index(note_template);
                note_template.RemoveAt(index);
            }
            //File.Delete(StrDataRepository.note_full_filePath);
            XML_Serialization(note_template, StrDataRepository.directory);
            All_Counter_Fills();
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
        }

        private void Btn_complete_task_Click(object sender, RoutedEventArgs e)
        {
            Complete_Note_Request_Window complete_note_wndw = new Complete_Note_Request_Window();
            complete_note_wndw.Owner = this;
            complete_note_wndw.Show();
        }

        private void Btn_Show_Expired_Notes_Click(object sender, RoutedEventArgs e)
        {
            Hide_Btns_when_Selection_Changed(expired_note_template);
            if (expired_note_template != null)
            {
                ListBx_Stack_Of_Notes.SelectedIndex = -1;
                ListBx_Stack_Of_Notes.ItemsSource = expired_note_template;

                ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
                ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            }
        }

        private void Btn_Show_Completed_Notes_Click(object sender, RoutedEventArgs e)
        {
            Hide_Btns_when_Selection_Changed(completed_note_template);
            if (completed_note_template != null)
            {
                ListBx_Stack_Of_Notes.SelectedIndex = -1;
                ListBx_Stack_Of_Notes.ItemsSource = completed_note_template;

                ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
                ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            }
        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            try
            {
                if (WindowState == WindowState.Minimized)
                {
                    Hide();
                    if (_notifyIcon != null)
                    {
                        _notifyIcon.ShowBalloonTip(4000);
                    }
                }
                else _stored_WindowState = WindowState;
            }
            catch (Exception)
            {

                return;
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            try
            {
                _notifyIcon.Dispose();
                _notifyIcon = null;
            }
            catch (Exception)
            {

                return;
            }
        }

        private void ShowTrayIcon(bool show)
        {
            if (_notifyIcon != null)
            {
                _notifyIcon.Visible = show;
            }
        }
        private void CheckTrayIcon()
        {
            ShowTrayIcon(!IsVisible);
        }
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            CheckTrayIcon();
        }
        private void NotifyIcon_Click(object sender, EventArgs e)
        {
            this.Show();
            WindowState = _stored_WindowState;
        }

        public void Run_Notify_OnTime_Event()
        {
            try
            {
                Thread newThread = new Thread(() => { TimeEvent(); })/* { IsBackground = true}*/;//(new ThreadStart(TimeEvent));
                newThread.SetApartmentState(ApartmentState.STA);

                newThread.Start();
                newThread.Interrupt();

                //TimeEvent();
            }
            catch (Exception)
            {
                return;

                // MessageBox.Show("Troubles");
            }
        }

        //private async void OnTimeEvent(Object source, ElapsedEventArgs e)
        //{
        //    try
        //    {
        //        await Task.Run(() =>
        //        {
        //            Application.Current.Dispatcher.Invoke((Action)delegate
        //            {
        //                //My_Notification_Wndw notification_Wndw = new My_Notification_Wndw();
        //                Notify_Wndw.WindowState = WindowState.Minimized;
        //                if ((note_template != null || note_template.Count > -1) && (!Notify_Wndw.IsVisible || Notify_Wndw.WindowState != WindowState.Normal))
        //                {
        //                    //if (!notification_Wndw.IsVisible || notification_Wndw.WindowState != WindowState.Normal)
        //                //    foreach (var today_note in today_notes.ToArray())
        //                //    {
        //                //        if ((Convert.ToDateTime(today_note.Date) - DateTime.Now).TotalSeconds > 300 && (Convert.ToDateTime(today_note.Date) - DateTime.Now).TotalSeconds < 304)
        //                //        {
        //                //            //_alert_sound.Load();
        //                //            //_alert_sound.Play();

        //                //            Notify_Wndw.WindowState = WindowState.Minimized;

        //                //            Notify_Wndw.Header_TxtBx.Text = today_note.Header;
        //                //            Notify_Wndw.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(today_note.Color));
        //                //            Notify_Wndw.Time_TxtBx.Text = today_note.Time;
        //                //            Notify_Wndw.Date_TxtBx.Text = today_note.Day_and_Month;
        //                //            Notify_Wndw.temp_completed_note_data = completed_note_template;
        //                //            Notify_Wndw.temp_note_data = today_notes;
        //                //            Notify_Wndw.note_notice = today_note;

        //                //            Notify_Wndw.Topmost = true;
        //                //            Notify_Wndw.Topmost = false;
        //                //            Notify_Wndw.WindowState = WindowState.Normal;
        //                //            Notify_Wndw.Visibility = Visibility.Visible;
        //                //            Notify_Wndw.Show();
        //                //            Notify_Wndw.Focus();
        //                //    }
        //                //    if ((today_note.Date - DateTime.Now).TotalSeconds < 0 && (today_note.Date - DateTime.Now).TotalSeconds > -3)
        //                //    {
        //                //        int index = -1;


        //                //            //_alert_sound.Load();
        //                //            //_alert_sound.Play();

        //                //            Notify_Wndw.WindowState = WindowState.Minimized;


        //                //            today_note.Color = Colors.Transparent.ToString();
        //                //            today_note.Foreground = Colors.White.ToString();
        //                //            today_note.Urgency = false;
        //                //            //today_note.Urgency_Expired_Complete_Title = String.Empty;
        //                //            today_note.Status = Properties.Languages.Lang.Expired_string;
        //                //            today_note.Status_Title = Properties.Languages.Lang.Expired_string;
        //                //            today_note.Expired_Foreground = Colors.White.ToString();
        //                //            today_note.Foreground = Colors.White.ToString();

        //                //            Notify_Wndw.Header_TxtBx.Text = today_note.Header;
        //                //            //notification_Wndw.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(today_note.Color));

        //                //            Notify_Wndw.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.Transparent.ToString()));// Colors.Transparent.ToString();
        //                //            Notify_Wndw.Time_TxtBx.Text = today_note.Time;
        //                //            Notify_Wndw.Date_TxtBx.Text = today_note.Day_and_Month;
        //                //            Notify_Wndw.temp_completed_note_data = completed_note_template;
        //                //            Notify_Wndw.temp_note_data = today_notes;
        //                //            Notify_Wndw.note_notice = today_note;
        //                //            Notify_Wndw.Header_TxtBx.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.White.ToString()));
        //                //            Notify_Wndw.Time_TxtBx.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.White.ToString()));
        //                //            Notify_Wndw.Date_TxtBx.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.White.ToString()));
        //                //            Notify_Wndw.expired_TxtBlck.Text = Properties.Languages.Lang.Expired_string;
        //                //            Notify_Wndw.expired_TxtBlck.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.White.ToString()));
        //                //            Notify_Wndw.close_notification_window.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.White.ToString()));
        //                //            Notify_Wndw.Complete_Btn.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(Colors.White.ToString()));// Colors.Transparent.ToString();
        //                //            for (int i = 0; i < note_template.Count; i++)
        //                //            {
        //                //                if (note_template[i].Equals(today_note))
        //                //                {
        //                //                    note_template[i] = today_note;

        //                //                    index = i;
        //                //                    return;
        //                //                }
        //                //            }
        //                //            ListBx_Stack_Of_Notes.Items.Refresh();

        //                //            Notify_Wndw.Topmost = true;
        //                //            Notify_Wndw.Topmost = false;
        //                //            Notify_Wndw.WindowState = WindowState.Normal;
        //                //            Notify_Wndw.Visibility = Visibility.Visible;
        //                //            Notify_Wndw.Topmost = true;
        //                //            Notify_Wndw.Topmost = false;
        //                //            Notify_Wndw.Show();
        //                //            Notify_Wndw.Focus();

        //                //            today_notes.Remove(today_note);
        //                //    }
        //                //        else return;
        //                //}
        //                }
        //                if (Notify_Wndw.IsVisible || Notify_Wndw.WindowState == WindowState.Normal)
        //                {
        //                    Notify_Wndw.Close();
        //                }
        //                Notify_Wndw.Topmost = true;
        //                //notification_Wndw.Topmost = false;
        //                Notify_Wndw.Focus();
        //                ListBx_Stack_Of_Notes.Items.Refresh();
        //            });
        //            if (!Dispatcher.CheckAccess())
        //            {
        //                Dispatcher.Invoke(() => ListBx_Stack_Of_Notes.Items.Refresh());
        //            }
        //        });
        //        if (!Dispatcher.CheckAccess())
        //        {
        //            Dispatcher.Invoke(() => ListBx_Stack_Of_Notes.Items.Refresh());
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public void TimeEvent()
        {
            event_timer = new System.Timers.Timer();
            event_timer.Interval = 3000;
            event_timer.AutoReset = true;
            event_timer.Enabled = true;
            //event_timer.Elapsed += OnTimeEvent;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            //if (notify != null)
            //{
            //    notify.Close();
            //}
        }
    }
}
