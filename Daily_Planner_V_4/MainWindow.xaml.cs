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
using System.Windows.Forms;


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
                    //if (/*Localization_Compare_Method*/(Localization_return_source_value_Method(grp.Execution_of) == Localization_return_source_value_Method(Properties.Languages.Lang.Executor_string_Me))/* == 0*/)
                    if (grp.Execution_of == "Я" || grp.Execution_of == "Me")
                    //if (grp.Execution_of == "Me" || grp.Execution_of == "Я")
                    {
                        my_Grps_Panel.Add(new Group_Panel_Data(grp));
                    }
                    if (grp.Execution_of == "Delegated" || grp.Execution_of == "Поручено")
                    {
                        delegated_Grps_Panel.Add(new Group_Panel_Data(grp));
                    }
                    
                    //if (grp.Execution_of == Localization_return_source_value_Method(Properties.Languages.Lang.Executor_string_Me))
                    //{
                    //    //my_Grps_Panel.Add(new Group_of_Notes(grp));
                    //    my_Grps_Panel.Add(new Group_Panel_Data(grp));
                    //}
                    //if (grp.Execution_of == Localization_return_source_value_Method(Properties.Languages.Lang.Executor_string_Delegated))
                    //{
                    //    //delegated_Grps_list.Add(new Group_of_Notes(grp));
                    //    delegated_Grps_Panel.Add(new Group_Panel_Data(grp));
                    //}
                }
                //Localization_Compare_Method(temp_data[0].Execution_of, Properties.Languages.Lang.Executor_string_Me);
                //MessageBox.Show(Localization_Compare_Method(Localization_return_source_value_Method(temp_data[0].Execution_of), Localization_return_source_value_Method(Properties.Languages.Lang.Executor_string_Me)).ToString()); // Localization_Compare_Method(Localization_return_source_value_Method(temp_data[0].Execution_of)), Localization_return_source_value_Method(Properties.Languages.Lang.Executor_string_Me)).ToString());

                //MessageBox.Show(Localization_return_source_value_Method(Properties.Languages.Lang.Executor_string_Me));
            }

        }
        public string Localization_return_source_value_Method(string localization_string)
        {
            CompareInfo exec_compareInfo_en = new CultureInfo(langCode, false).CompareInfo;
            SortKey sort_key_en = exec_compareInfo_en.GetSortKey(localization_string);
            string[] trimmed_string = sort_key_en.ToString().Split(',');
            string value = trimmed_string[2];
            value = value.Substring(1);
            return localization_string; // value;
        }
        public int Localization_Compare_Method(string programFieldString, string localizationFieldString)
        {
            CompareInfo exec_compareInfo_en = new CultureInfo(langCode, false).CompareInfo;
            CompareInfo exec_compareInfo_ru = new CultureInfo(langCode, false).CompareInfo;
            //exec_compareInfo_en.
            SortKey sort_key_en = exec_compareInfo_en.GetSortKey(Localization_return_source_value_Method(programFieldString)/*programFieldString*/);
            SortKey sort_key_ru = exec_compareInfo_ru.GetSortKey(Localization_return_source_value_Method(localizationFieldString)/*localizationFieldString*/);

            return SortKey.Compare(sort_key_en, sort_key_ru);
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

                System.Windows.MessageBox.Show(Properties.Languages.Lang.Saving_Error_Message);
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
            add_note_Wndw.Show();
        }
    }
}
