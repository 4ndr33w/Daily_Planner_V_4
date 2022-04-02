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
using System.IO;
using System.Collections.ObjectModel;

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Логика взаимодействия для Complete_Note_Request_Window.xaml
    /// </summary>
    public partial class Complete_Note_Request_Window : Window
    {
        public Complete_Note_Request_Window()
        {
            InitializeComponent();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        public void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;
            ObservableCollection<Note_Template> temp_note_collection = new ObservableCollection<Note_Template>();
            Note_Template one_note = new Note_Template();

            one_note = Form1.ListBx_Stack_Of_Notes.SelectedItem as Note_Template;

            //int index = Form1.Selected_Note_Index(temp_note_collection);
            File.Delete(StrDataRepository.note_full_filePath);

            if  (Form1.ListBx_Stack_Of_Notes.ItemsSource == Form1.note_template)
            {
                temp_note_collection.Remove(one_note);
                //temp_note_collection = Form1.note_template;
                Form1.note_template = temp_note_collection;
            }

            if (Form1.ListBx_Stack_Of_Notes.ItemsSource != Form1.note_template)
            {
                temp_note_collection.Clear();
                for (int i = 0; i < Form1.ListBx_Stack_Of_Notes.Items.Count; i++)
                {
                    temp_note_collection.Add(Form1.ListBx_Stack_Of_Notes.Items[i] as Note_Template);
                }
                //index = Form1.Selected_Note_Index(temp_note_collection);
                int index = Form1.Selected_Note_Index(Form1.note_template);
                Form1.note_template.RemoveAt(index);
                temp_note_collection.Remove(one_note);
                Form1.ListBx_Stack_Of_Notes.ItemsSource = temp_note_collection;
            }
            one_note.Urgency = false;
            one_note.Status = Properties.Languages.Lang.Completed_Note_String;
            one_note.Status_Title = Properties.Languages.Lang.Completed_Note_String;
            Form1.completed_note_template.Add(new Note_Template(one_note));

            Form1.ListBx_Stack_Of_Notes.SelectedIndex = -1;
            Form1.All_Counter_Fills();

            this.Hide();
            
            Form1.XML_Serialization(Form1.completed_note_template, StrDataRepository.directory);
            Form1.XML_Serialization(Form1.note_template, StrDataRepository.directory);


        }
        public void Complete_note_Method(Note_Template one_note, ObservableCollection<Note_Template> data_collection)
        {
            int index = data_collection.IndexOf(one_note);
            MainWindow Form1 = this.Owner as MainWindow;
            one_note.Status_Title = Properties.Languages.Lang.Completed_Note_String;
            Form1.completed_note_template.Add(one_note);
            Form1.XML_Serialization(Form1.completed_note_template, StrDataRepository.directory);

            data_collection.RemoveAt(index);
            Form1.XML_Serialization(data_collection, StrDataRepository.directory);
            Form1.TxtBx_Completed_Notes_Counter.Text = Form1.completed_note_template.Count.ToString();

            if (Form1.ListBx_Stack_Of_Notes.ItemsSource != Form1.note_template)
            {
                data_collection.Clear();
                for (int i = 0; i < Form1.ListBx_Stack_Of_Notes.Items.Count; i++)
                {
                    data_collection.Add(Form1.ListBx_Stack_Of_Notes.Items[i] as Note_Template);
                }
                index = Form1.Selected_Note_Index(data_collection);
                Form1.note_template.Remove(data_collection[index]);
                data_collection.RemoveAt(index);
                Form1.ListBx_Stack_Of_Notes.ItemsSource = data_collection;
                Form1.ListBx_Stack_Of_Notes.SelectedIndex = -1;
            }
        }
    }
}
