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

namespace CockTailGuide
{
    /// <summary>
    /// Interaction logic for Window5.xaml
    /// </summary>
    public partial class Window5 : Window
    {
        //function take the user to particular video depending on the cocktail he has selected.
        public Window5(string query)
        {
            this.WindowState = System.Windows.WindowState.Maximized;
            InitializeComponent();
            try
            {
                
                videoBrowser.Navigate(query);
                
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
    }
}
