using Facebook;
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
    /// Interaction logic for Post.xaml
    /// </summary>
    public partial class Post : Window
    {
        FacebookClient fbC;
        string shareContent;
        public Post(FacebookClient fbClient,string Info)
        {
            try
            {
                InitializeComponent();
                fbC = fbClient;
                dynamic me = fbClient.Get("Me");
                shareContent = Info;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void Content_TextChanged(object sender, TextChangedEventArgs e)
        {
           
        }

        private void Share_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                fbC.Post("/me/feed", new { message = "hello" });
                this.Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
    }
}
