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
    
    public partial class Window4 : Window
    {
        public Window4()
        {
            
            InitializeComponent();
            this.WindowState = System.Windows.WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                bool flag = true;
                int insideLoop1 = 0;
                int insideLoop2 = 0;
                int insideLoop3 = 0;
                if (Textbox1.Text.Trim().Length > 0 && textbox2.Text.Trim().Length > 0)
                {
                    insideLoop1 = 1;
                    
                    
                }
                else MessageBox.Show("Please enter valid title and type");
                if (listbox1.Items.Count > 0)
                {
                    insideLoop2 = 1;
                    foreach (string s in listbox1.Items)
                    {
                        if (s.Trim().Length == 0)
                        flag = false;
                        if(flag==false)
                        {
                           MessageBox.Show("Please enter valid Ingredients with quantity and measure");
                           break;

                        }

                    }
                }
                else MessageBox.Show("Please enter valid Ingredients with quantity and measure");
                if(listbox2.Items.Count>0)
                {
                    insideLoop3 = 1;
                    foreach (string s in listbox2.Items)
                    { 
                        if (s.Trim().Length == 0)
                            flag = false;
                        if (flag == false)
                        {
                            MessageBox.Show("Please enter valid steps");
                            break;

                        }
                    }
                }
                ;
                if (flag==true&&insideLoop1==1&&insideLoop2==1&&insideLoop3==1)
                {
                    postRequest();
                }
                else
                    MessageBox.Show("Please enter valid steps");
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }
        //This function will post the recipe to the we service
        //This function will take all the values updated in Add recipe window and post to web service
        public void postRequest()
        {
            try
            {
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri("http://localhost:60953/");

                client.DefaultRequestHeaders.Accept.Add(
                   new MediaTypeWithQualityHeaderValue("application/json"));
                recipe rec = new recipe();


                
                rec.title = Textbox1.Text;
                rec.garnish = textbox8.Text;
                rec.glass = textbox7.Text;
                rec.preparation = textbox10.Text;
                rec.step = null;
                foreach (string j in listbox2.Items)
                {
                    rec.step = rec.step + j + ";";
                }
                rec.step = rec.step.Substring(0, rec.step.Length - 1);
                
                rec.type = textbox2.Text;
                rec.strength = textbox9.Text;
                
                rec.ingredients = new List<ingredient>();

                foreach (string i in listbox1.Items)
                {
                    ingredient ing = new ingredient();
                    string[] str = i.Split(' ');
                    ing.Ingredient = str[0];
                    ing.measure = str[2];
                    ing.quantity = str[1];
                    rec.ingredients.Add(ing);
                }

                var response = client.PostAsJsonAsync("api/values", rec).Result;
                response.EnsureSuccessStatusCode();
                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Your Recipe has been added to our database");

                }
                else
                {
                    MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }


        //this function is used to add  ingredients in to listbox one by one
        private void Add_ingredient_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                if (textbox3.Text.Trim().Length > 0 && textbox4.Text.Trim().Length > 0 && textbox4.Text.Trim().Length > 0)
                {
                    ingredient ing1 = new ingredient();
                    ing1.Ingredient = textbox3.Text;
                    ing1.measure = textbox5.Text;
                    ing1.quantity = textbox4.Text;
                    string str = textbox3.Text + " " + textbox4.Text + " " + textbox5.Text;
                    listbox1.Items.Add(str);
                    textbox3.Text = null;
                    textbox4.Text = null;
                    textbox5.Text = null;
                }
                else
                    MessageBox.Show("Enter Valid ingredient with proper quantity and measure");

                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }

        //used to remove selected ingredients from the listbox
        private void Remove_Ingredient_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                while (listbox1.SelectedItems.Count > 0)
                {
                    var index = listbox1.Items.IndexOf(listbox1.SelectedItem);
                    listbox1.Items.RemoveAt(index);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

     
        //used to add steps in the listbox
        private void Add_steps_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                if (textbox6.Text.Trim().Length > 0)
                {
                    listbox2.Items.Add(textbox6.Text);
                    textbox6.Text = null;
                }

                else
                    MessageBox.Show("Please enter valid steps");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }

        //this function is used to remove all selected steps which user want to remove.
        private void Remove_steps_Click_1(object sender, RoutedEventArgs e)
        {
            try {
 
            while (listbox2.SelectedItems.Count > 0)
            {
                var index = listbox2.Items.IndexOf(listbox2.SelectedItem);
                listbox2.Items.RemoveAt(index);

            }
                }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Error");
            }
        }


        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window2 lc = new Window2();
            lc.Owner = this;
            lc.WindowState = System.Windows.WindowState.Maximized;
            lc.Show();
        }

        private void listbox2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
