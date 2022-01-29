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

namespace Daily_Planner_V_4
{
    /// <summary>
    /// Interaction logic for Lang_Change_Message_Window.xaml
    /// </summary>
    public partial class Lang_Change_Message_Window : Window
    {
        public Lang_Change_Message_Window()
        {
            InitializeComponent();
        }

        private void Btn_OK_Click(object sender, RoutedEventArgs e)
        {
            MainWindow Form1 = this.Owner as MainWindow;
            this.Hide();
            Environment.Exit(0);
        }
    }
}
