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
using System.IO;

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Interaction logic for Delete_Note_Request_Window.xaml
    /// </summary>
    public partial class Delete_Note_Request_Window : Window
    {
        ObservableCollection<Note_Template> converted_from_listBox_collection = new ObservableCollection<Note_Template>();
        public Delete_Note_Request_Window()
        {
            InitializeComponent();
        }

        int data_count = 0;
        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;

            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.note_template)
            {
                data_count = Form1.note_template.Count;
                File.Delete(StrDataRepository.note_full_filePath);
                Delete_Note_Method(Form1.note_template, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory, data_count);
                //if (Form1.note_template.Count != -1)
                //    Form1.XML_Serialization(Form1.note_template, StrDataRepository.directory);
            }
            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.expired_note_template)
            {
                data_count = Form1.expired_note_template.Count;
                File.Delete(StrDataRepository.expired_note_full_filePath);
                Delete_Note_Method(Form1.expired_note_template, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory, data_count);
                Form1.TxtBx_Expired_Notes_Counter.Text = Form1.expired_note_template.Count.ToString();
                //if (Form1.expired_note_template.Count != -1)
                //    Form1.XML_Serialization(Form1.expired_note_template, StrDataRepository.directory);
            }
            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.completed_note_template)
            {
                data_count = Form1.completed_note_template.Count;
                File.Delete(StrDataRepository.completed_note_full_filePath);
                Delete_Note_Method(Form1.completed_note_template, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory, data_count);
                Form1.TxtBx_Completed_Notes_Counter.Text = Form1.completed_note_template.Count.ToString();
                //if (Form1.completed_note_template.Count != -1)
                //    Form1.XML_Serialization(Form1.completed_note_template, StrDataRepository.directory);
            }
            else
            {
                if (Form1.ListBx_Stack_Of_Notes.Items != null/* && (Form1.ListBx_Grp_Of_My_Tasks.SelectedItem != null || Form1.ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null)*/)
                {
                    data_count = Form1.ListBx_Stack_Of_Notes.Items.Count;
                    Delete_Note_Method(Form1.note_template/*.Where(Form1.find_grp_filter) as ObservableCollection<Note_Template>*/, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory, data_count);
                    Form1.ListBx_Stack_Of_Notes.Items.Refresh();
                }
            }
            Form1.ListBx_Stack_Of_Notes.Items.Refresh();
            Form1.ListBx_Grp_Of_My_Tasks.Items.Refresh();
            Form1.ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
            Form1.All_Counter_Fills();
            
            this.Hide();
        }
        private void Delete_Note_Method(ObservableCollection<Note_Template> note_collection, ListBox listBx, string directory, int note_count)
        {
            MainWindow Form1 = new MainWindow();
            Note_Template one_note = new Note_Template();
            one_note = listBx.SelectedItem as Note_Template;
            int note_index_in_collection = -1;

            //выполняется в случае когда отображение Note в ListBox'e не сортируется по группам... 
            //if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.expired_note_template || Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.completed_note_template || Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.note_template)
           // {
           if (listBx.SelectedItem != null)
            {
                if (note_collection != null && listBx.Items != null)
                {
                    foreach (var note in note_collection.ToArray())
                    {
                        if (one_note.Creation_Date == note.Creation_Date)
                        {
                            note_collection.Remove(one_note);
                        }
                    }
                }
            }
            Form1.ListBx_Stack_Of_Notes.ItemsSource = note_collection;

            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.expired_note_template) { Form1.expired_note_template = note_collection; }
            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.completed_note_template) { Form1.completed_note_template = note_collection; }
            else Form1.ListBx_Stack_Of_Notes.ItemsSource = Form1.note_template;

            if (note_count > 0)
            {
                Form1.XML_Serialization(note_collection, directory);
            }
            else
            {
                if (note_collection[0].Status == StrDataRepository.Status_Completed_En || note_collection[0].Status == StrDataRepository.Status_Completed_Ru)
                {
                    File.Delete(StrDataRepository.completed_note_full_filePath);
                    Form1.XML_Serialization(note_collection, directory);
                }
                if (note_collection[0].Status == StrDataRepository.Status_Expired_En || note_collection[0].Status == StrDataRepository.Status_Expired_Ru)
                {
                    File.Delete(StrDataRepository.expired_note_full_filePath);
                    Form1.XML_Serialization(note_collection, directory);
                }
                if (note_collection[0].Status != StrDataRepository.Status_Expired_En && note_collection[0].Status != StrDataRepository.Status_Expired_Ru &&
                    note_collection[0].Status != StrDataRepository.Status_Completed_En && note_collection[0].Status != StrDataRepository.Status_Completed_Ru)
                {
                    File.Delete(StrDataRepository.note_full_filePath);
                    Form1.XML_Serialization(note_collection, directory);
                }
            }
        }
    }
}
