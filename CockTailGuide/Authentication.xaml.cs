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
using Facebook;
using System.Windows.Navigation;

namespace CockTailGuide
{
    /// <summary>
    /// Interaction logic for Authentication.xaml
    /// </summary>
    
    public partial class Authentication : Window
    {
        string postString;
        private FacebookClient FBClient;
        public static string AccessToken { get; set; }
        public Authentication(string Info)
        {
            try
            {
                InitializeComponent();
                //WBrowser.Navigate(new Uri("https://graph.facebook.com/oauth/authorize?client_id=505410466306191&redirect_uri=http://www.facebook.com/connect/login_success.html&type=user_agent&display=popup").AbsoluteUri);
                WBrowser.Navigate(new Uri("https://graph.facebook.com/oauth/authorize?client_id=505410466306191&redirect_uri=http://www.facebook.com/connect/login_success.html&scope=publish_stream,manage_pages&perms=user_status&type=user_agent&display=popup").AbsoluteUri);
                postString = Info;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        private void WBrowser_OnNavigated(object sender, NavigationEventArgs e)
        {
            try
            {
                if (e.Uri.ToString().StartsWith("http://www.facebook.com/connect/login_success.html"))
                {
                    AccessToken = e.Uri.Fragment.Split('&')[0].Replace("#access_token=", "");
                    FBClient = new FacebookClient(AccessToken);

                    //InfoWindow InfW = new InfoWindow(FBClient);
                    //InfW.Show();
                    this.Hide();
                    //Post p = new Post(FBClient, postString);
                    //p.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
    }
}