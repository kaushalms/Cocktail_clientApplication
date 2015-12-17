using Facebook;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Xml;

namespace CockTailGuide
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window

    {
        
        recipe r = new recipe();
        private FacebookClient fbC = new FacebookClient();
        string recipeTitle = "abc";

        
        public Window1()
        {
            try {     
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            List<string> ingList = new List<string>();
            getResponse("api/values/?a=title", ingList);
            
            foreach (string item in ingList)
            {

                Recipe_List.Items.Add(item.ToString());
            }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        //used to display list of all cocktails in the combobox
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                r = getResponse1("api/values/?a=" + Recipe_List.SelectedItem + "&&z=1");
                textBoxPopulation(r);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        //used to display entire recipe information in the text box 
        public void textBoxPopulation(recipe r)
        {
            try
            {
                textbox50.Text = null;
                //string title = doc.SelectSingleNode("Recipe/title").InnerText.Trim();
                textbox50.Text = textbox50.Text + "Title:  " + r.title + Environment.NewLine + Environment.NewLine;
                recipeTitle = r.title;
                //string type = doc.SelectSingleNode("Recipe/type").InnerText.Trim();
                textbox50.Text = textbox50.Text + "Type:  " + r.type + Environment.NewLine + Environment.NewLine;
                //string glass = doc.SelectSingleNode("Recipe/glass").InnerText.Trim();
                textbox50.Text = textbox50.Text + "Glass:  " + r.glass + Environment.NewLine + Environment.NewLine;
                //string garnish = doc.SelectSingleNode("Recipe/garnish").InnerText.Trim();
                textbox50.Text = textbox50.Text + "Garnish:  " + r.garnish + Environment.NewLine + Environment.NewLine;
                //string strength = doc.SelectSingleNode("Recipe/strength").InnerText.Trim();
                textbox50.Text = textbox50.Text + "Strength:  " + r.strength + Environment.NewLine + Environment.NewLine;
                //string preparation = doc.SelectSingleNode("Recipe/preparation").InnerText.Trim();
                textbox50.Text = textbox50.Text + "Preparation:  " + r.preparation + Environment.NewLine + Environment.NewLine;
                //string step = doc.SelectSingleNode("Recipe/step").InnerText.Trim();
                string[] strStep = r.step.Split(';');
                textbox50.Text = textbox50.Text + "Steps:  " + Environment.NewLine;
                for (int i = 0; i < strStep.Count(); i++)
                {
                    textbox50.Text = textbox50.Text + strStep[i] + Environment.NewLine;
                }
                //var ingredients = doc.SelectNodes("Recipe/ingredients");
                //for (int i = 0; i < ingredients.Count; i++)
                //{
                textbox50.Text = textbox50.Text + Environment.NewLine + "Ingredients:  " + Environment.NewLine;
                for (int j = 0; j < r.ingredients.Count; j++)
                {
                    textbox50.Text = textbox50.Text + r.ingredients[j].Ingredient + " ";
                    textbox50.Text = textbox50.Text + r.ingredients[j].quantity + " ";
                    textbox50.Text = textbox50.Text + r.ingredients[j].measure + Environment.NewLine;

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }
        
        //used to get list of cocktails from webservice
        public recipe getResponse1(string sentUrl)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:60953/");
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = client.GetAsync(sentUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    //List<Recipe> 
                    var listNames = response.Content.ReadAsAsync<recipe>().Result;
                    r = listNames;
                    return r;
                    //var jsonString = response.Content.ReadAsStringAsync();
                    //jsonString.Wait();
                    //model = JsonConvert.DeserializeObject<List<Recipe>>(jsonString.Result);


                    //string str = null;
                    //for (var i = 0; i < listNames.Count(); i++)
                    //{
                    //    str = str + listNames.ElementAt(i);
                    //    //myList.Add(listNames.ElementAt(i));

                    //}

                    //doc.LoadXml(str);
                    //XmlNode mynode = doc.DocumentElement;



                }
                else
                {
                    System.Windows.MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                    return null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
                return null;
            }
        }

        //used to get list of recipe information from webservice
        public void getResponse(string sentUrl, List<string> myList)
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:60953/");
                //client.DefaultRequestHeaders.Add("appkey", "myapp_key");
                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/xml"));

                HttpResponseMessage response = client.GetAsync(sentUrl).Result;
                if (response.IsSuccessStatusCode)
                {
                    var listNames = response.Content.ReadAsAsync<IEnumerable<string>>().Result;


                    string str = null;
                    for (var i = 0; i < listNames.Count(); i++)
                    {
                        str = str + listNames.ElementAt(i);
                        myList.Add(listNames.ElementAt(i));

                    }
                    //XmlDocument doc = new XmlDocument();
                    //doc.LoadXml(str);
                    //XmlNode mynode = doc.DocumentElement;



                }
                else
                {
                    System.Windows.MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Daiquiri_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }
        
        //will show recipe information on the textbox to user once user selects on any image
        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            r = getResponse1("api/values/?a=1942 Martini&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=THE BASIL FIZZ&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=DAIQUIRI&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=B-52&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Brazilian&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Champagne&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=COSMOPOLITAN&&z=1");
            textBoxPopulation(r);
        }
        private void Button_Click_7(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=AN EARLY TASTE OF SPRING&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_8(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Gimlet&&z=1");
            textBoxPopulation(r);

        }

        private void Button_Click_9(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Kamikaze&&z=1");
            textBoxPopulation(r);

        }

        private void Button_Click_10(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Lagerita&&z=1");
            textBoxPopulation(r);

        }

        private void Button_Click_11(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=MANHATTAN&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_12(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Mojito&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_13(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=POINT REYES PUNCH&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_14(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=ROOSTER-TAIL&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_15(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Sex on the beach&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_16(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=SMASHING PUMPKIN&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_17(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=TOUGH TO HEAR&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_18(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Screwdriver&&z=1");
            textBoxPopulation(r);
        }

        private void Button_Click_19(object sender, RoutedEventArgs e)
        {
            r = getResponse1("api/values/?a=Dry Martini&&z=1");
            textBoxPopulation(r);
        }

        private void FacebookPost_Click(object sender, RoutedEventArgs e)
        {
            Authentication w1 = new Authentication(textbox50.Text);
            w1.Show();
        }


        //can share the content to facebook 
        private void Share_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FacebookMediaObject facebookUploader = new FacebookMediaObject { FileName = "Brazilian.jpg", ContentType = "image/jpg" };
                var bytes = File.ReadAllBytes("Brazilian.jpg");
                facebookUploader.SetValue(bytes);
                var postInfo = new Dictionary<string, object>();
                postInfo.Add("message", textbox50.Text);
                postInfo.Add("image", facebookUploader);
                fbC.Post("/me/feed", new { message = textbox50.Text });
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }


        //used to display videos depending on selected cocktail
        private void Video_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = null;
                switch (recipeTitle)
                {
                    case "Screwdriver":
                        query = "https://www.youtube.com/watch?v=ASuf8XVA1lk";
                        break;
                    case "Dry martini":
                        query = "https://www.youtube.com/watch?v=hYZKlE8qCrk";
                        break;
                    case "1942 martini":
                        query = "https://www.youtube.com/watch?v=EfrZL-q5Xf8";
                        break;
                    case "THE BASIL FIZZ":
                        query = "https://www.youtube.com/watch?v=bH7fy-C87as";
                        break;
                    case "MANHATTAN":
                        query = "https://www.youtube.com/watch?v=VG4Vr2C7Pog";
                        break;
                    case "Champagne":
                        query = "https://www.youtube.com/watch?v=9PjtvU-OMcM";
                        break;
                    case "Kamikaze":
                        query = "https://www.youtube.com/watch?v=Vdsh4HKRdEg";
                        break;
                    case "B-52":
                        query = "https://www.youtube.com/watch?v=uLdQitD47lw";
                        break;
                    case "AN EARLY TASTE OF SPRING":
                        query = "https://www.youtube.com/watch?v=lEUb6YeOXbE";
                        break;
                    case "Sex on the beach":
                        query = "https://www.youtube.com/watch?v=fCSbeWYVnnk";
                        break;
                    case "COSMOPOLITAN":
                        query = "https://www.youtube.com/watch?v=5FgUs8Ajcd0";
                        break;
                    case "Mojito":
                        query = "https://www.youtube.com/watch?v=xrJsVHr7YV4";
                        break;
                    case "POINT REYES PUNCH":
                        query = "https://www.youtube.com/watch?v=Chn_T1H-jWU";
                        break;
                    case "Lagerita":
                        query = "https://www.youtube.com/watch?v=YtFHywwfb7E";
                        break;
                    case "SMASHING PUMPKIN":
                        query = "https://www.youtube.com/watch?v=qe3YiqzTzO8";
                        break;
                    case "DAIQUIRI":
                        query = "https://www.youtube.com/watch?v=hwxKqv8CCzc";
                        break;
                    case "Brazilian":
                        query = "https://www.youtube.com/watch?v=R2PTgdrDaM4";
                        break;
                    case "Gimlet":
                        query = "https://www.youtube.com/watch?v=V7FPkHDZCXY";
                        break;
                    default:
                        MessageBox.Show("Video unavailable");
                        query = "Video unavailable";
                        break;


                }
                if (query != null && query != "Video unavailable")
                {
                    Window5 w1 = new Window5(query);
                    w1.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        //will take user to main menu after clicking on this button
        private void Button_Click_20(object sender, RoutedEventArgs e)
        {
            try
            {
                this.Hide();
                Window2 lc = new Window2();
                lc.Owner = this;
                lc.WindowState = System.Windows.WindowState.Maximized;
                lc.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

    }
}
