﻿using System;
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
    /// Interaction logic for Window3.xaml
    /// </summary>
    public partial class Window3 : Window
    {
        recipe r = new recipe();
        public Window3()
        {
            
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
            
        }

        //used to get list of recipes containing selected ingredients from web service
        public void getResponse(string sentUrl, List<string> myList)
        {
            try { 
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Ingredients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Load_Recipe_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //displays Get cocktail button on search recipe window
        private void button11_Click(object sender, RoutedEventArgs e)
        {
            try { 
            dropdown11.SelectedItem = null;
            dropdown11.Items.Clear();

            r = null;
          
                
            if (listbox11.SelectedItems.Count > 2)
            {
                MessageBox.Show("Please select up to 2 ingredients");

            }
            else
            {
                List<string> ingList = new List<string>();
                dropdown11.Items.Clear();
                if (listbox11.SelectedItems.Count > 1)
                {

                    getResponse("api/parent/?a=" + listbox11.SelectedItems[0] + "&&b=" + listbox11.SelectedItems[1], ingList);
                    if(ingList.Count()==0)
                        MessageBox.Show("Sorry, there are no recipe in the database that matches with your selection");
                    else
                    {
                        foreach (string s in ingList)
                        dropdown11.Items.Add(s);
                    }
                }
                else
                {
                    getResponse("api/parent/?a=" + listbox11.SelectedItems[0], ingList);
                    if(ingList.Count()==0)
                        MessageBox.Show("Sorry, there are no recipe in the database that matches with your selection");
                    else
                    {
                        foreach (string s in ingList)
                        dropdown11.Items.Add(s);
                    }
                }
            }
                }
            catch (Exception ex)
            {
                if(ex is ArgumentOutOfRangeException)
                 MessageBox.Show("Please select atleast 1 or up to 2 ingredients");
                else
                MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }

        private void listbox12_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        private void button12_Click(object sender, RoutedEventArgs e)
        {
            //while (listbox12.SelectedItems.Count > 0)
            //{
            //    var index = listbox12.Items.IndexOf(listbox12.SelectedItem);
            //    listbox12.Items.RemoveAt(index);

            //}
        }

        private void dropdown11_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {


        }

        //used to display particular recipe details in the textbox
        private void button12_Click_1(object sender, RoutedEventArgs e)
        {
            try { 
            //XmlDocument doc = new XmlDocument();
            //List<string> list = new List<string>();
            Window1 w1 = new Window1();
            r = w1.getResponse1("api/values/?a=" + dropdown11.SelectedItem + "&&z=1");

            //path is file location
            textbox51.Text = null;
            //string title = doc.SelectSingleNode("Recipe/title").InnerText.Trim();
            textbox51.Text = textbox51.Text + "Title:  " + r.title + Environment.NewLine + Environment.NewLine;
            //string type = doc.SelectSingleNode("Recipe/type").InnerText.Trim();
            textbox51.Text = textbox51.Text + "Type:  " + r.type + Environment.NewLine + Environment.NewLine;
            //string glass = doc.SelectSingleNode("Recipe/glass").InnerText.Trim();
            textbox51.Text = textbox51.Text + "Glass:  " + r.glass + Environment.NewLine + Environment.NewLine;
            //string garnish = doc.SelectSingleNode("Recipe/garnish").InnerText.Trim();
            textbox51.Text = textbox51.Text + "Garnish:  " + r.garnish + Environment.NewLine + Environment.NewLine;
            //string strength = doc.SelectSingleNode("Recipe/strength").InnerText.Trim();
            textbox51.Text = textbox51.Text + "Strength:  " + r.strength + Environment.NewLine + Environment.NewLine;
            //string preparation = doc.SelectSingleNode("Recipe/preparation").InnerText.Trim();
            textbox51.Text = textbox51.Text + "Preparation:  " + r.preparation + Environment.NewLine + Environment.NewLine;
            //string step = doc.SelectSingleNode("Recipe/step").InnerText.Trim();
            string[] strStep = r.step.Split(';');
            textbox51.Text = textbox51.Text + "Steps:  " + Environment.NewLine;
            for (int i = 0; i < strStep.Count(); i++)
            {
                textbox51.Text = textbox51.Text + strStep[i] + Environment.NewLine;
            }
            //var ingredients = doc.SelectNodes("Recipe/ingredients");
            //for (int i = 0; i < ingredients.Count; i++)
            //{
            textbox51.Text = textbox51.Text + Environment.NewLine + "Ingredients:  " + Environment.NewLine;
            for (int j = 0; j < r.ingredients.Count; j++)
            {
                textbox51.Text = textbox51.Text + r.ingredients[j].Ingredient + " ";
                textbox51.Text = textbox51.Text + r.ingredients[j].quantity + " ";
                textbox51.Text = textbox51.Text + r.ingredients[j].measure + Environment.NewLine;

            }
            //}
                }
            catch (Exception ex)
            {
                if (ex is NullReferenceException)
                    MessageBox.Show("Please select atleast one ingredient from Ingredients List first and then choose Cocktail from dropdown to view the recipe ");
                else
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        private void textbox51_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        //will take user to main menu after clicking on this button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window2 lc = new Window2();
            lc.Owner = this;
            lc.WindowState = System.Windows.WindowState.Maximized;
            lc.Show();
        }
    }
}