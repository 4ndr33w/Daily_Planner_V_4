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

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public StrDataRepository strings_ = new StrDataRepository();
        public Note_Template current_note_template = new Note_Template();
        Note_Counter_Fills counter_fills_class = new Note_Counter_Fills();
        public ObservableCollection<Group_Panel_Data> my_Grps_Panel = new ObservableCollection<Group_Panel_Data>();
        public ObservableCollection<Group_Panel_Data> delegated_Grps_Panel = new ObservableCollection<Group_Panel_Data>();
        public ObservableCollection<Group_of_Notes> union_Grps = new ObservableCollection<Group_of_Notes>();

        public ObservableCollection<Note_Template> note_template = new ObservableCollection<Note_Template>();
        public ObservableCollection<Note_Template> expired_note_template = new ObservableCollection<Note_Template>();
        public ObservableCollection<Note_Template> completed_note_template = new ObservableCollection<Note_Template>();
        public bool create_new_note = false;
        string langCode = "en-GB";
        public MainWindow()
        {
            langCode = Properties.Settings.Default.languageCode;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.GetCultureInfo(langCode);
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(langCode);
            InitializeComponent();
            Load_Default_Data();
        }
        public void Hide_Btns_when_Selection_Changed()
        {
            foreach (var note in note_template)
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
            union_Grps = XML_Grps_Deserialization(StrDataRepository.directory);
            note_template = Fill_Note_Template_From_List_Data_and_sortByDate(   XML_Note_Deserialization(strings_.Note_Data_Save)  );
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
                if (data_to_serialize is ObservableCollection<Group_of_Notes>) { file_path = directory + strings_.Grps_Short_FileName; Xml_auxillary_serialize_Method(data_to_serialize, file_path); }
                //else if (data_to_serialize is ObservableCollection<Note_Data>) { file_path = directory + strings_.Note_Data_Save; }
                else if (data_to_serialize is ObservableCollection<Note_Template> && 
                    ((data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title == StrDataRepository.Status_Completed_En ||
                    (data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title == StrDataRepository.Status_Completed_Ru))
                {
                    file_path = directory + strings_.Completed_Note_Data_Save;
                    foreach (var note in data_to_serialize as ObservableCollection<Note_Template>)
                    {
                        temp_data.Add(new Note_Data(note));
                    }
                    Xml_auxillary_serialize_Method(temp_data, file_path);
                }

                else if (data_to_serialize is ObservableCollection<Note_Template> &&
                   ((data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title == StrDataRepository.Status_Expired_En ||
                   (data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title == StrDataRepository.Status_Expired_Ru))
                {
                    file_path = directory + strings_.Completed_Note_Data_Save;
                    foreach (var note in data_to_serialize as ObservableCollection<Note_Template>)
                    {
                        temp_data.Add(new Note_Data(note));
                    }
                    Xml_auxillary_serialize_Method(temp_data, file_path);
                }
                else if (data_to_serialize is ObservableCollection<Note_Template> &&
                 (data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title != StrDataRepository.Status_Expired_En &&
                 (data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title != StrDataRepository.Status_Expired_Ru &&
                 (data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title != StrDataRepository.Status_Completed_En &&
                 (data_to_serialize as ObservableCollection<Note_Template>)[0].Status_Title != StrDataRepository.Status_Completed_Ru ) 
                {
                    file_path = /*directory + */strings_.Note_Data_Save;
                    foreach (var note in data_to_serialize as ObservableCollection<Note_Template>)
                    {
                        temp_data.Add(new Note_Data(note));
                    }
                    Xml_auxillary_serialize_Method(temp_data, file_path);
                }

            }
            catch (Exception)
            {
                return;
                //System.Windows.MessageBox.Show(Properties.Languages.Lang.Saving_Error_Message);
            }
        }
        private void Xml_auxillary_serialize_Method <T> (ObservableCollection<T> data, string file_path)
        {
            if (File.Exists(file_path)) { File.Delete(file_path); }
            XmlSerializer xml_serializer = new XmlSerializer(data.GetType());
            Stream fstream = new FileStream(file_path, FileMode.Create, FileAccess.Write);
            xml_serializer.Serialize(fstream, data);
            fstream.Close();
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
                foreach (Note_Data item in data)
                {
                    temp_data.Add(new Note_Template(item));
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
            //ListBx_Stack_Of_Notes.ItemsSource = note_template;
            create_new_note = true;
            //strings_.CreateOrEditNote_Mode = "create";
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
            Hide_Btns_when_Selection_Changed();
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
            Hide_Btns_when_Selection_Changed();
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

            Hide_Btns_when_Selection_Changed();
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
                //Hide_Btns_when_Selection_Changed();
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
                        if (note.Expired == Properties.Languages.Lang.Expired_string || note.Expired == Properties.Languages.Lang.Completed_Note_String)
                        {
                            note.Btn_Hide_Compl_or_Exp_note_Visibility = StrDataRepository.Visibility_visible;
                            note.Mart_to_Complete_note_Visibility = StrDataRepository.Visibility_hidden;
                        }
                    }
                }
            }
            if (ListBx_Stack_Of_Notes.ItemsSource == expired_note_template && expired_note_template != null)
            {
                Hide_Btns_when_Selection_Changed();
                if (ListBx_Stack_Of_Notes.SelectedItem != null)
                {
                    expired_note_template[ListBx_Stack_Of_Notes.SelectedIndex].Delete_Btn_Visibility = StrDataRepository.Visibility_hidden;
                }
            }
            if (ListBx_Stack_Of_Notes.ItemsSource == completed_note_template && completed_note_template != null)
            {
                completed_note_template[ListBx_Stack_Of_Notes.SelectedIndex].Delete_Btn_Visibility = StrDataRepository.Visibility_hidden;
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
    }
}
