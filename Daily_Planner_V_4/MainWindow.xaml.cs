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
//using System.Windows.Forms;


namespace Daily_Planner_V_4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public strings_data_repository strings_ = new strings_data_repository();
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
        public void Load_Default_Data()
        {
            union_Grps = XML_Deserialization(strings_.Grps_Save_Path);
            Fill_Grps_From_Union(union_Grps);
            ListBx_Grp_Of_My_Tasks.ItemsSource = my_Grps_Panel;
            ListBx_Grp_Of_Delegated_Tasks.ItemsSource = delegated_Grps_Panel;
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
                    if (grp.Execution_of == "Я" || grp.Execution_of == "Me")
                    {
                        my_Grps_Panel.Add(new Group_Panel_Data(grp));
                    }
                    if (grp.Execution_of == "Delegated" || grp.Execution_of == "Поручено")
                    {
                        delegated_Grps_Panel.Add(new Group_Panel_Data(grp));
                    }
                }
            }
        }
        //public string Localization_return_source_value_Method(string localization_string)
        //{
        //    CompareInfo exec_compareInfo_en = new CultureInfo(langCode, false).CompareInfo;
        //    SortKey sort_key_en = exec_compareInfo_en.GetSortKey(localization_string);
        //    string[] trimmed_string = sort_key_en.ToString().Split(',');
        //    string value = trimmed_string[2];
        //    value = value.Substring(1);
        //    return value; // value;
        //}
        //public int Localization_Compare_Method(string programFieldString, string localizationFieldString)
        //{
        //    CompareInfo exec_compareInfo_en = new CultureInfo(langCode, false).CompareInfo;
        //    CompareInfo exec_compareInfo_ru = new CultureInfo("ru-RU", false).CompareInfo;

        //    SortKey sort_key_en = exec_compareInfo_en.GetSortKey(Localization_return_source_value_Method(programFieldString)/*programFieldString*/);
        //    SortKey sort_key_ru = exec_compareInfo_ru.GetSortKey(Localization_return_source_value_Method(localizationFieldString)/*localizationFieldString*/);


        //    //Properties.Settings.Default.languageCode = "en-GB";
        //    //string str = "";
        //    //StringComparer.InvariantCultureIgnoreCase(Properties.Languages.Lang.Executor_string_Me)
        //    //MessageBox.Show(Properties.Languages.Lang.Executor_string_Me) ;
        //        //sort_key_en.ToString() + "   " +  sort_key_ru.ToString());
        //    return SortKey.Compare(sort_key_en, sort_key_ru);
        //}
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
            //Properties.Languages.Lang.Executor_string_Me = CultureInfo.
            //Localization_Compare_Method(my_Grps_Panel[0].Execution_of, Properties.Languages.Lang.Executor_string_Me);
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
            try
            {
                string file_path = directory;
                if (!Directory.Exists(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                if (data_to_serialize is ObservableCollection<Group_of_Notes>) { file_path = directory + strings_.Grps_FileName; }
                if (data_to_serialize is ObservableCollection<Note_Data>) { file_path = directory + strings_.Note_Data_Save; }
                Xml_auxillary_serialize_Method (data_to_serialize, file_path);
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
        public ObservableCollection<Group_of_Notes> XML_Deserialization(string file_path)
        {
            ObservableCollection<Group_of_Notes> temp_data = new ObservableCollection<Group_of_Notes>();
            if (File.Exists(file_path))
            {
                XmlSerializer xml_serializer = new XmlSerializer(temp_data.GetType());
                Stream fStream = new FileStream(file_path, FileMode.Open, FileAccess.Read);
                temp_data = xml_serializer.Deserialize(fStream) as ObservableCollection<Group_of_Notes>;
                fStream.Close();
            }
            return temp_data;
        }

        private void Btn_Create_New_Note_Click(object sender, RoutedEventArgs e)
        {
            Add_Note_Window add_note_Wndw = new Add_Note_Window();
            add_note_Wndw.Owner = this;
            ListBx_Stack_Of_Notes.ItemsSource = note_template;
            create_new_note = true;
            strings_.CreateOrEditNote_Mode = "create";
            add_note_Wndw.Show();
        }

        private void ListBx_Grp_Of_My_Tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //for (int i = 0; i < ListBx_Grp_Of_My_Tasks.Items.Count; i++)
            //{
            //    my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_My_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //for (int i = 0; i < ListBx_Grp_Of_Delegated_Tasks.Items.Count; i++)
            //{
            //    delegated_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_Delegated_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}


            //foreach (var grp in my_Grps_Panel)
            //{
            //    grp.Delete_Grp_Btn_visibility = "Hidden";
            //}

            //foreach (var grp in delegated_Grps_Panel)
            //{
            //    grp.Delete_Grp_Btn_visibility = "Hidden";
            //}

            //foreach (var element in ListBx_Grp_Of_My_Tasks.Items)
            //{
            //    (element as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //foreach (var element in ListBx_Grp_Of_Delegated_Tasks.Items)
            //{
            //    (element as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //ListBx_Grp_Of_My_Tasks.ItemsSource = my_Grps_Panel;
            //if (ListBx_Grp_Of_My_Tasks.SelectedIndex > -1)
            //{
            //    (ListBx_Grp_Of_My_Tasks.SelectedItem as Group_Panel_Data).Delete_Grp_Btn_visibility = "Visible";
            //    //my_Grps_Panel[ListBx_Grp_Of_My_Tasks.SelectedIndex].Delete_Grp_Btn_visibility = "Visible";
            //}
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
        }

        private void ListBx_Grp_Of_Delegated_Tasks_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //for (int i = 0; i < ListBx_Grp_Of_My_Tasks.Items.Count; i++)
            //{
            //    my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_My_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //for (int i = 0; i < ListBx_Grp_Of_Delegated_Tasks.Items.Count; i++)
            //{
            //    delegated_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_Delegated_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //for (int i = 0; i < my_Grps_Panel.Count; i++)
            //{
            //    //if (my_Grps_Panel[i] != ListBx_Grp_Of_My_Tasks.Items[ListBx_Grp_Of_My_Tasks.SelectedIndex])
            //    //{
            //    //    my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    //}
            //    //my_Grps_Panel[i].Delete_Grp_Btn_visibility = my_Grps_Panel[i].Delete_Grp_Btn_visibility == "Hidden" ? "Hidden" : "Hidden";
            //    //my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //}
            //for (int i = 0; i < delegated_Grps_Panel.Count; i++)
            //{
            //    delegated_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //}


            //foreach (var grp in my_Grps_Panel)
            //{
            //    grp.Delete_Grp_Btn_visibility = "Hidden";
            //}
            //foreach (var grp in delegated_Grps_Panel)
            //{
            //    grp.Delete_Grp_Btn_visibility = "Hidden";
            //}
            //if (ListBx_Grp_Of_Delegated_Tasks.SelectedIndex > -1)
            //{
            //    (ListBx_Grp_Of_Delegated_Tasks.SelectedItem as Group_Panel_Data).Delete_Grp_Btn_visibility = "Visible";
            //    //delegated_Grps_Panel[ListBx_Grp_Of_Delegated_Tasks.SelectedIndex].Delete_Grp_Btn_visibility = "Visible";
            //}
            //ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ////if (ListBx_Grp_Of_My_Tasks.SelectedIndex < 0)
            ////{
            //    for (int i = 0; i < my_Grps_Panel.Count; i++)
            //    {
            //    (ListBx_Grp_Of_My_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //    //my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    }
            //}
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
        }

        private void ListBx_Grp_Of_My_Tasks_Unselected(object sender, RoutedEventArgs e)
        {
            return;
        }

        private void Btn_Show_Today_Notes_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < ListBx_Grp_Of_My_Tasks.Items.Count; i++)
            //{
            //    my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_My_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //for (int i = 0; i < ListBx_Grp_Of_Delegated_Tasks.Items.Count; i++)
            //{
            //    delegated_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_Delegated_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //if (ListBx_Grp_Of_My_Tasks.SelectedItem != null)
            //{
            //    my_Grps_Panel[ListBx_Grp_Of_My_Tasks.SelectedIndex].Delete_Grp_Btn_visibility = "Hidden";
            //    ListBx_Grp_Of_My_Tasks.ItemsSource = my_Grps_Panel;
            //    //(ListBx_Grp_Of_My_Tasks.SelectedItem as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //if (ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null)
            //{
            //    (ListBx_Grp_Of_Delegated_Tasks.SelectedItem as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //    delegated_Grps_Panel[ListBx_Grp_Of_Delegated_Tasks.SelectedIndex].Delete_Grp_Btn_visibility = "Hidden";
            //}
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
        }

        private void Btn_Show_All_Notes_Click(object sender, RoutedEventArgs e)
        {
            //for (int i = 0; i < ListBx_Grp_Of_My_Tasks.Items.Count; i++)
            //{
            //    my_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_My_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            //for (int i = 0; i < ListBx_Grp_Of_Delegated_Tasks.Items.Count; i++)
            //{
            //    delegated_Grps_Panel[i].Delete_Grp_Btn_visibility = "Hidden";
            //    (ListBx_Grp_Of_Delegated_Tasks.Items[i] as Group_Panel_Data).Delete_Grp_Btn_visibility = "Hidden";
            //}
            ListBx_Grp_Of_Delegated_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.SelectedIndex = -1;
            ListBx_Grp_Of_My_Tasks.Items.Refresh();
            ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
        }

        private void Btn_my_task_group_delete_group_Click(object sender, RoutedEventArgs e)
        {
           // ListBx_Grp_Of_My_Tasks.SelectedItem =


            if (ListBx_Grp_Of_My_Tasks.SelectedItem != null)
            {
                Delete_Grp_Request_Window delete_grp_Wndw = new Delete_Grp_Request_Window();
                delete_grp_Wndw.Owner = this;
                delete_grp_Wndw.Show();
            }
            //if (ListBx_Grp_Of_My_Tasks.SelectedItem != (sender as ComboBox).Items.CurrentItem) return;
            //if ((sender as ComboBoxItem).IsFocused == true && (sender as ComboBoxItem) != ListBx_Grp_Of_My_Tasks.SelectedItem && ListBx_Grp_Of_My_Tasks.SelectedItem != null)
            //{ return; }

        }

        private void Btn_delegated_task_group_delete_group_Click(object sender, RoutedEventArgs e)
        {
            if (ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null/* && (sender as ComboBoxItem) == ListBx_Grp_Of_Delegated_Tasks.SelectedItem*/)
            {
                Delete_Grp_Request_Window delete_grp_Wndw = new Delete_Grp_Request_Window();
                delete_grp_Wndw.Owner = this;
                delete_grp_Wndw.Show();
            }
            //if ((sender as ComboBoxItem).IsFocused == true && (sender as ComboBoxItem) != ListBx_Grp_Of_Delegated_Tasks.SelectedItem && ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null)
            //{ return; }
        }
    }
}
