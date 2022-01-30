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
    /// Interaction logic for Add_Grp_Window.xaml
    /// </summary>
    public partial class Add_Grp_Window : Window
    {
        string _color; // = (Chosen_Color_Indicator.Background == null && Chosen_Color_Indicator.Background.ToString() != Colors.Transparent.ToString()) ? Colors.Salmon.ToString() : Chosen_Color_Indicator.Background.ToString();
        string _executor;
        string _grp_name;

        Group_of_Notes grp = new Group_of_Notes();
        Group_Panel_Data grp_panel = new Group_Panel_Data();
        List<Group_of_Notes> grp_coll = new List<Group_of_Notes>();
        ObservableCollection<Group_Panel_Data> grp_panel_coll = new ObservableCollection<Group_Panel_Data>();
        ObservableCollection<Group_of_Notes> union_grp_coll = new ObservableCollection<Group_of_Notes>();
        StrDataRepository strings_;
        public Add_Grp_Window()
        {
            InitializeComponent();
        }

        private void new_taskGroup_Close_Window_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
        }

        private void new_taskGroup_OK_Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;

            _color = Chosen_Color_Indicator.Background.ToString() == Colors.Transparent.ToString() ? Colors.Salmon.ToString() : Chosen_Color_Indicator.Background.ToString();
            _grp_name = Note_Group_Name_TextBox.Text == String.Empty ? Properties.Languages.Lang.Default_string : Note_Group_Name_TextBox.Text;
            this.grp.Color = _color; // Chosen_Color_Indicator.Background == null ? Colors.Salmon.ToString() : Chosen_Color_Indicator.Background.ToString();
            this.grp.Group_Name = _grp_name; // Note_Group_Name_TextBox.Text == String.Empty ? Properties.Languages.Lang.Default_string : Note_Group_Name_TextBox.Text;

            switch (Form1.strings_.GrpCreation_MyOrDelegated_Mode)
            {
                case "my_grp":
                    {
                        _executor = Properties.Languages.Lang.Executor_string_Me;
                        this.grp.Execution_of = _executor;
                        Form1.my_Grps_Panel.Add(new Group_Panel_Data(grp));
                        break;
                    }
                case "delegated_grp":
                    {
                        _executor = Properties.Languages.Lang.Executor_string_Delegated;
                        this.grp.Execution_of = _executor;
                        Form1.delegated_Grps_Panel.Add(new Group_Panel_Data(grp));
                        break;
                    }
            }
            this.Hide();
            union_grp_coll = Form1.Union_Grps_Method(Form1.my_Grps_Panel, Form1.delegated_Grps_Panel);
            Form1.XML_Serialization(union_grp_coll, strings_.Directory);
        }



        private void Drop_Color_Selection()
        {
            Color_Dark_Red_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Red_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Medium_Red_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Lighr_Red_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Dark_Orange_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Orange_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Medium_Orange_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Light_Orange_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Dark_Yellow_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Yellow_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Medium_Yellow_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Light_Yellow_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Dark_Green_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Green_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Medium_Green_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Light_Green_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Dark_Blue_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Blue_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Medium_Blue_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Light_Blue_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Dark_Violet_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Violet_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Medium_Violet_Button.BorderBrush = new SolidColorBrush(Colors.Black);
            Color_Light_Violet_Button.BorderBrush = new SolidColorBrush(Colors.Black);
        }
        private void ColorSelection(Button button)
        {
            Drop_Color_Selection();
            Chosen_Color_Indicator.Background = (button.Background != null && button.Background.ToString() != Colors.Transparent.ToString()) ? button.Background : Brushes.LightSalmon;
            button.BorderBrush = new SolidColorBrush(Colors.Aqua);
            Note_Group_Name_TextBox.Focus();
        }
        private void Color_Dark_Red_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Dark_Red_Button);
        }

        private void Color_Red_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Red_Button);
        }

        private void Color_Medium_Red_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Medium_Red_Button);
        }

        private void Color_Lighr_Red_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Lighr_Red_Button);
        }

        private void Color_Dark_Orange_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Dark_Orange_Button);
        }

        private void Color_Orange_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Orange_Button);
        }

        private void Color_Medium_Orange_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Medium_Orange_Button);
        }

        private void Color_Light_Orange_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Light_Orange_Button);
        }

        private void Color_Dark_Yellow_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Dark_Yellow_Button);
        }

        private void Color_Yellow_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Yellow_Button);
        }

        private void Color_Medium_Yellow_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Medium_Yellow_Button);
        }

        private void Color_Light_Yellow_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Light_Yellow_Button);
        }

        private void Color_Dark_Green_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Dark_Green_Button);
        }

        private void Color_Green_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Green_Button);
        }

        private void Color_Medium_Green_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Medium_Green_Button);

        }

        private void Color_Light_Green_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Light_Green_Button);
        }

        private void Color_Dark_Blue_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Dark_Blue_Button);
        }

        private void Color_Blue_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Blue_Button);
        }

        private void Color_Medium_Blue_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Medium_Blue_Button);
        }

        private void Color_Light_Blue_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Light_Blue_Button);
        }

        private void Color_Dark_Violet_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Dark_Violet_Button);
        }

        private void Color_Violet_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Violet_Button);
        }

        private void Color_Medium_Violet_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Medium_Violet_Button);
        }

        private void Color_Light_Violet_Button_Click(object sender, RoutedEventArgs e)
        {
            ColorSelection(Color_Light_Violet_Button);
        }
    }
}
