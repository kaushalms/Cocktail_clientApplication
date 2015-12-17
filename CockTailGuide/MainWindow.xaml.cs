using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
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

namespace CockTailGuide
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        public Window2()
        {
            
            InitializeComponent();
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
        }

        //used to display view recipes button on main window to user
        private void View_recipies_Click(object sender, RoutedEventArgs e)
        {
            
            Window1 lc = new Window1();
            lc.Owner = this;
            lc.Show();
            this.Hide();

        }

        //used to display search recipes button on main window to user
        private void Search_Recipies_Click(object sender, RoutedEventArgs e)
        {



            try
            {
                Window3 lc = new Window3();
                Window1 w1 = new Window1();
                List<string> ingList = new List<string>();
                w1.getResponse("api/values/?a=ingredient", ingList);
                foreach (string s in ingList)
                    lc.listbox11.Items.Add(s);
                lc.Owner = this;
                lc.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
            
        }

        //used to display play button on the main window to user
        private void Play_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow lc = new MainWindow();
                lc.Owner = this;
                lc.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        
        //used to display add recipe button to user
        private void Add_recipies_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Window4 lc = new Window4();
                lc.Owner = this;
                lc.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
    }
}
