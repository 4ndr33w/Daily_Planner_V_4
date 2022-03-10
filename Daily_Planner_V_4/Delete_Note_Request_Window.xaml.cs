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

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;

            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.note_template)
            {
                Delete_Note_Method(Form1.note_template, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory);
            }
            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.expired_note_template)
            {
                Delete_Note_Method(Form1.expired_note_template, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory);
                Form1.TxtBx_Expired_Notes_Counter.Text = Form1.expired_note_template.Count.ToString();
            }
            if (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.expired_note_template)
            {
                Delete_Note_Method(Form1.completed_note_template, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory);
                Form1.TxtBx_Completed_Notes_Counter.Text = Form1.completed_note_template.Count.ToString();
            }
            else
            {
                if (Form1.ListBx_Stack_Of_Notes.Items != null/* && (Form1.ListBx_Grp_Of_My_Tasks.SelectedItem != null || Form1.ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null)*/)
                {
                    Delete_Note_Method(Form1.note_template/*.Where(Form1.find_grp_filter) as ObservableCollection<Note_Template>*/, Form1.ListBx_Stack_Of_Notes, StrDataRepository.directory);
                    Form1.ListBx_Stack_Of_Notes.Items.Refresh();
                }
            }
            Form1.ListBx_Stack_Of_Notes.Items.Refresh();
            Form1.ListBx_Grp_Of_My_Tasks.Items.Refresh();
            Form1.ListBx_Grp_Of_Delegated_Tasks.Items.Refresh();
            Form1.All_Counter_Fills();
            this.Hide();
        }
        private void Delete_Note_Method(ObservableCollection<Note_Template> note_collection, ListBox listBx, string directory)
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

            Form1.XML_Serialization(note_collection, directory);
        }
    }
}
