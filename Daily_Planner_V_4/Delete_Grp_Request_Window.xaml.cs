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
    /// Interaction logic for Delete_Grp_Request_Window.xaml
    /// </summary>
    public partial class Delete_Grp_Request_Window : Window
    {
        public Delete_Grp_Request_Window()
        {
            InitializeComponent();
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;
            StrDataRepository strings_ = new StrDataRepository(); ;

            ObservableCollection<Note_Template> temp_note_data = new ObservableCollection<Note_Template>();
            ObservableCollection<Group_Panel_Data> my_temp_grp_panel = new ObservableCollection<Group_Panel_Data>();
            ObservableCollection<Group_Panel_Data> deleg_temp_grp_panel = new ObservableCollection<Group_Panel_Data>();

            ObservableCollection<Group_of_Notes> union_temp_grp_data = new ObservableCollection<Group_of_Notes>();
            Group_of_Notes selected_grp = new Group_of_Notes();
            
            temp_note_data = Form1.note_template;

            if (Form1.ListBx_Grp_Of_My_Tasks.SelectedItem != null)
            {
                my_temp_grp_panel = Form1.my_Grps_Panel;
                selected_grp = Form1.ListBx_Grp_Of_My_Tasks.SelectedItem as Group_of_Notes;
                Delete_Grp_Method(selected_grp, my_temp_grp_panel, temp_note_data);
                
                Form1.note_template = temp_note_data;
                Form1.my_Grps_Panel = my_temp_grp_panel;
            }
            if (Form1.ListBx_Grp_Of_Delegated_Tasks.SelectedItem != null)
            {
                deleg_temp_grp_panel = Form1.delegated_Grps_Panel;
                selected_grp = Form1.ListBx_Grp_Of_Delegated_Tasks.SelectedItem as Group_of_Notes;
                Delete_Grp_Method(selected_grp, deleg_temp_grp_panel, temp_note_data);
                
                Form1.note_template = temp_note_data;
                Form1.delegated_Grps_Panel = deleg_temp_grp_panel;
            }
            //my_temp_grp_data = Grps_Collection_Convert_to_Serialize(Form1.my_Grps_Panel);
            //deleg_temp_grp_data = Grps_Collection_Convert_to_Serialize(Form1.delegated_Grps_Panel);
            union_temp_grp_data = Form1.Union_Grps_Method(my_temp_grp_panel, deleg_temp_grp_panel);


            string save_path = strings_.Directory;
            Form1.XML_Serialization(union_temp_grp_data, save_path);
            Form1.XML_Serialization(temp_note_data, save_path);
            Form1.Note_Counter_Fills();
            this.Hide();
        }

        private void Btn_Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }
        private ObservableCollection<Group_of_Notes> Grps_Collection_Convert_to_Serialize(ObservableCollection<Group_Panel_Data> grp_collection)
        {
            ObservableCollection<Group_of_Notes> temp_grps = new ObservableCollection<Group_of_Notes>();
            foreach (var grp in grp_collection)
            {
                temp_grps.Add(new Group_of_Notes(grp));
            }
            return temp_grps;
        }
        private void Delete_Grp_Method (Group_of_Notes selected_grp, ObservableCollection<Group_Panel_Data> grp_collection, ObservableCollection<Note_Template> note_collection)
        {
            foreach (var grp in grp_collection.ToArray())
            {
                if (selected_grp.Execution_of == "Me" || selected_grp.Execution_of == "Я")
                {
                    if (selected_grp.my_grps_equals(grp)) grp_collection.Remove(grp);
                }
                if (selected_grp.Execution_of == "Delegated" || selected_grp.Execution_of == "Поручено")
                {
                    if (selected_grp.deleg_grps_equals(grp)) grp_collection.Remove(grp);
                }
            }

            //foreach (var note in note_collection.ToArray())
            //{
            //    if (selected_grp.Grp_equals(note.Group)) note_collection.Remove(note);
            //}

           
        }
    }
}
