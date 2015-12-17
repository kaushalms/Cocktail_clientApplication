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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;
using System.Web;
using Xceed.Wpf.Toolkit;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;
using System.Reflection;
using System.Media;




namespace CockTailGuide
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        List<string> iList = new List<string>();
        List<string> selectedIngredients = new List<string>();
        List<Label> Labels = new List<Label>();
        List<IntegerUpDown> upBoxes = new List<IntegerUpDown>();
        List<ComboBox> ComboBoxes = new List<ComboBox>();
        List<string> userIngredients = new List<string>();
        List<string> usermeasures = new List<string>();
        List<int> userQuantities = new List<int>();
        System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
        MediaPlayer player = new MediaPlayer();
        recipe viewRecipe = new recipe();

        public MainWindow()
        {
            try
            {
                InitializeComponent();




                this.WindowState = System.Windows.WindowState.Maximized;
                

                List<string> ingList = new List<string>();
                getResponse("api/values/?a=title", ingList);
                foreach (string item in ingList)
                {

                    Recipes.Items.Add(item.ToString());
                }
            }
            catch (Exception ex)
            {
                Xceed.Wpf.Toolkit.MessageBox.Show(ex.Message.ToString(), "Error");
            }

        }

        //Function to get response from the web service based on URL
        public void getResponse(string sentUrl, List<string> myList)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri("http://localhost:60953/");
            
            client.DefaultRequestHeaders.Accept.Add(
               new MediaTypeWithQualityHeaderValue("application/xml"));

            HttpResponseMessage response = client.GetAsync(sentUrl).Result;
            if (response.IsSuccessStatusCode)
            {
                var listNames = response.Content.ReadAsAsync<IEnumerable<string>>().Result;


                if (listNames != null)
                {
                    string str = null;
                    for (var i = 0; i < listNames.Count(); i++)
                    {
                        str = str + listNames.ElementAt(i);
                        myList.Add(listNames.ElementAt(i));

                    }
                }
               
            }
            else
            {
                System.Windows.MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
            }
        }


        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Function to click button
        private void Confirm_Ingredients_Click(object sender, RoutedEventArgs e)
        {

            if (Ingredients_List.SelectedItems.Count == 0)
            {
                Ing_Error.FontSize = 20;
                Ing_Error.Foreground = Brushes.Red;
                Ing_Error.FontFamily = new FontFamily("Buxton Sketch");
                Ing_Error.Content = "Select Atleast one Ingredient";
            }
            else
            {
                foreach (var item in Ingredients_List.SelectedItems)
                {
                    selectedIngredients.Add(item.ToString());
                }
                populateValues();
            }


        }
        //Function for populating the quantity and measure fields based on ingredients selected
        private void populateValues()
        {
            int position = 0;

            foreach (string s in selectedIngredients)
            {

                Label newLabel = new Label();
                newLabel.Tag = s;
                newLabel.Margin = new Thickness(0, 0 + position * 80, 0, 0);
                newLabel.Width = 220;
                newLabel.Height = 40;
                newLabel.Content = s;
                newLabel.FontSize = 20;
                newLabel.Foreground = Brushes.AntiqueWhite;
                newLabel.FontFamily = new FontFamily("Buxton Sketch");
                Labels.Add(newLabel);
                IntegerUpDown newTextBox = new IntegerUpDown();
                newTextBox.Tag = s;
                newTextBox.Margin = new Thickness(200, 5 + position * 80, 0, 0);
                newTextBox.Width = 60;
                newTextBox.Height = 30;
                newTextBox.Minimum = 1;
                newTextBox.Maximum = 100;
                newTextBox.DefaultValue = 1;
                newTextBox.FontSize = 18;
                newTextBox.Foreground = Brushes.IndianRed;
                newTextBox.FontFamily = new FontFamily("Buxton Sketch");
                newTextBox.DisplayDefaultValueOnEmptyText = true;
                upBoxes.Add(newTextBox);
                ComboBox newComboBox = new ComboBox();
                newComboBox.Tag = s;
                newComboBox.Items.Add("oz");
                newComboBox.Items.Add("dash");
                newComboBox.Items.Add("peel");
                newComboBox.SelectedIndex = 0;
                newComboBox.FontSize = 18;
                newComboBox.Foreground = Brushes.IndianRed;
                newComboBox.FontFamily = new FontFamily("Buxton Sketch");
                newComboBox.Margin = new Thickness(350, 5 + position * 80, 0, 0);
                newComboBox.Width = 60;
                newComboBox.Height = 30;
                ComboBoxes.Add(newComboBox);
                MyGrid.Children.Add(newLabel);
                MyGrid.Children.Add(newTextBox);
                MyGrid.Children.Add(newComboBox);
                position = position + 1;

            }
            List<string> stepsList = new List<string>();
            getResponse("api/values/?a=step", stepsList);
            foreach (string item in stepsList)
            {
                Steps.Items.Add(item.ToString());


            }
            Ingredients_List.Visibility = Visibility.Hidden;
            Ing_Error.Visibility = Visibility.Hidden;
            Confirm_Ingredients.Visibility = Visibility.Hidden;
            Ingre_label.Visibility = Visibility.Hidden;
            Make.Visibility = Visibility.Visible;

            Steps.Visibility = Visibility.Visible;
            Steps_Label.Visibility = Visibility.Visible;

        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        //Function for make button
        private void Make_Click(object sender, RoutedEventArgs e)
        {
            if (Steps.SelectedItems.Count == 0)
            {
                Ing_Error.FontSize = 20;
                Ing_Error.Foreground = Brushes.Red;
                Ing_Error.FontFamily = new FontFamily("Buxton Sketch");
                Ing_Error.Content = "Select Atleast one Ingredient";
            }
            else
            {
                MyGrid.Visibility = Visibility.Hidden;
                Make.Visibility = Visibility.Hidden;
                Ing_Error.Visibility = Visibility.Hidden;
                ShakerImage.Visibility = Visibility.Visible;
                Steps_Label.Visibility = Visibility.Hidden;

                player.Open(new Uri(@"Shaker_Sound.wav", UriKind.Relative));
                player.Play();
                dispatcherTimer.Tick += dispatcherTimer_Tick;
                dispatcherTimer.Interval = new TimeSpan(0, 0, 3);
                dispatcherTimer.Start();
            }

        }

        //This function will get the recipe from web service if we give the title
        public recipe getRecipe(string sentUrl)
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
                viewRecipe = listNames;
                return viewRecipe;


            }
            else
            {
                System.Windows.MessageBox.Show("Error Code" + response.StatusCode + " : Message - " + response.ReasonPhrase);
                return null;
            }
        }

        //populating the correct recipe text box
        //the correct recipe is requested from web service based on recipe
        public void textBoxPopulation()
        {

            viewRecipe = getRecipe("api/values/?a=" + Recipes.SelectedItem + "&&z=1");
            Correct_Recipe.Text = null;
            Correct_Recipe.FontSize = 18;
            Correct_Recipe.Foreground = Brushes.Green;
            Correct_Recipe.FontFamily = new FontFamily("Buxton Sketch");
            Correct_Recipe.Text = Correct_Recipe.Text + "Correct Steps: " + Environment.NewLine + Environment.NewLine;
            Correct_Recipe.Text = Correct_Recipe.Text + "Title:  " + viewRecipe.title + Environment.NewLine;
            Correct_Recipe.Text = Correct_Recipe.Text + "Type:  " + viewRecipe.type + Environment.NewLine ;
            Correct_Recipe.Text = Correct_Recipe.Text + "Glass:  " + viewRecipe.glass + Environment.NewLine ;
            Correct_Recipe.Text = Correct_Recipe.Text + "Garnish:  " + viewRecipe.garnish + Environment.NewLine ;
            Correct_Recipe.Text = Correct_Recipe.Text + "Strength:  " + viewRecipe.strength + Environment.NewLine ;
            Correct_Recipe.Text = Correct_Recipe.Text + "Preparation:  " + viewRecipe.preparation + Environment.NewLine;
            Correct_Recipe.Text = Correct_Recipe.Text + Environment.NewLine + "Ingredients:  " + Environment.NewLine;
            for (int j = 0; j < viewRecipe.ingredients.Count; j++)
            {
                Correct_Recipe.Text = Correct_Recipe.Text + viewRecipe.ingredients[j].Ingredient + " ";
                Correct_Recipe.Text = Correct_Recipe.Text + viewRecipe.ingredients[j].quantity + " ";
                Correct_Recipe.Text = Correct_Recipe.Text + viewRecipe.ingredients[j].measure + Environment.NewLine;

            }
            string[] strStep = viewRecipe.step.Split(';');
            Correct_Recipe.Text = Correct_Recipe.Text + "Steps:  " + Environment.NewLine;
            for (int i = 0; i < strStep.Count(); i++)
            {
                Correct_Recipe.Text = Correct_Recipe.Text + strStep[i] + Environment.NewLine;
            }
   
        }
        //Timer function to show the gif image and sound the video
        private void dispatcherTimer_Tick(object sender, EventArgs e)
        {
            dispatcherTimer.Stop();
            player.Stop();
            ShakerImage.Visibility = Visibility.Hidden;
            Garnish.Visibility = Visibility.Visible;
            Steps_Label.Visibility = Visibility.Hidden;
            Glass_Label.Visibility = Visibility.Visible;
            Garnish_label.Visibility = Visibility.Visible;
            List<string> garnishList = new List<string>();
            getResponse("api/values/?a=garnish", garnishList);
            foreach (string item in garnishList)
            {
                Garnish.Items.Add(item.ToString());


            }
            Glass.Visibility = Visibility.Visible;
            List<string> glassList = new List<string>();
            getResponse("api/values/?a=glass", glassList);
            foreach (string item in glassList)
            {
                Glass.Items.Add(item.ToString());


            }


            Serve.Visibility = Visibility.Visible;
        }

        private void Serve_Click(object sender, RoutedEventArgs e)
        {
            EvaluateIngredient();
        }


        //Function to evauluate the recipe made by the user and displaying 
        //the recipe made by the user and the correct recipe along withe efficiency
        private void EvaluateIngredient()
        {
            string recipe = Recipes.SelectedItem.ToString();
            decimal ingScore = 0;
            decimal totalScore = 0;
            int wrongIngredients = 0;
            User_Recipe.FontSize = 18;
            User_Recipe.Foreground = Brushes.IndianRed;
            User_Recipe.FontFamily = new FontFamily("Buxton Sketch");
            Score.FontSize = 28;
            Score.Foreground = Brushes.LightGreen;
            Score.FontFamily = new FontFamily("Buxton Sketch");
            Correct_Recipe.Text = "Recipe: " + recipe + Environment.NewLine;
            User_Recipe.Text = User_Recipe.Text + "Your Steps: " + Environment.NewLine + Environment.NewLine;
            User_Recipe.Text = User_Recipe.Text + "Recipe: " + recipe + Environment.NewLine; 
            List<string> countString = new List<string>(); ;
            int count = 0;
            int correctIngredients = 0;
            int correctQuantity = 0;
            int correctMeasure = 0;
            int glassCheck = 0;
            int garnishCheck = 0;
            int stepCheck = 0;
            getResponse("/api/count/?a=" + recipe, countString);
            if (countString[0] != null)
            {
                count = Convert.ToInt32(countString[0]);
            }
            if (count != 0)
            {
                User_Recipe.Text = User_Recipe.Text + "Ingredients:" + Environment.NewLine;
                //80% of marks are alloted for correct ingredient selection,measure and quantity
                //Each ingredient is alloted equal share out of 80%
                //Marks for each ingredient is again divided into four parts
                ingScore = 80 / (count * 4);
                int pos = 0;
                foreach (string s in selectedIngredients)
                {


                    List<string> ingList = new List<string>();
                    //Verifying whether the selected ingredient is the actual recipe
                    getResponse("api/recipe/?title=" + recipe + "&&field=" + s, ingList);
                    if (ingList.Count != 0)
                    {
                        correctIngredients = correctIngredients + 1;
                        //Adding 2 parts out of 4 parts if ingredient is chosen correctly
                        totalScore = totalScore + ingScore * 2;
                        string[] words = ingList[0].Split(' ');
                        //Verifying whether the quantity selected is correct
                        if (upBoxes[pos].Value.ToString() == words[0])
                        {
                            totalScore = totalScore + ingScore;
                            correctQuantity = correctQuantity + 1;
                        }
                        //verifying whether the measure selected is correct
                        if (ComboBoxes[pos].SelectedValue.ToString() == words[1])
                        {
                            totalScore = totalScore + ingScore;
                            correctMeasure = correctMeasure + 1;
                        }


                    }
                    else
                    {
                        //Updated wrongIngredients count if user selected ingredient is not in recipe
                        wrongIngredients = wrongIngredients + 1;

                    }
                    User_Recipe.Text = User_Recipe.Text + " " + s + " " + upBoxes[pos].Value.ToString() + " " + ComboBoxes[pos].SelectedValue.ToString() + Environment.NewLine;
                    pos = pos + 1;

                }
                List<string> garnishString = new List<string>();
                getResponse("api/recipe/?titlevalue=" + recipe + "&&a=" + 1, garnishString);
                if (garnishString.Count == 3)
                {
                    //verifying whether the glass selected is correct
                    if (Glass.SelectedItem.ToString() == garnishString[1])
                    {
                        totalScore = totalScore + 5;
                        glassCheck = 1;
                    }
                    //verifying whether the garnish selected is correct
                    if (Garnish.SelectedItem.ToString() == garnishString[2])
                    {
                        totalScore = totalScore + 5;
                        garnishCheck = 1;
                    }
                    //verifying whether the steps selected is correct
                    if (Steps.SelectedItem.ToString() == garnishString[0])
                    {
                        stepCheck = 1;
                        totalScore = totalScore + 10;
                    }
                    User_Recipe.Text = User_Recipe.Text + "Steps: " + Steps.SelectedItem.ToString() + Environment.NewLine;
                    User_Recipe.Text = User_Recipe.Text + "Glass: " + Glass.SelectedItem.ToString() + Environment.NewLine;
                    User_Recipe.Text = User_Recipe.Text + "Garnish: " + Garnish.SelectedItem.ToString() + Environment.NewLine + Environment.NewLine;
                }



            }
            //Updating the User recipe and correct recipe text boxes
            User_Recipe.Text = User_Recipe.Text + "You have Identified " + correctIngredients + " out of " + count + " ingredients Correctly." + Environment.NewLine;
            User_Recipe.Text = User_Recipe.Text + "You have Identified " + correctQuantity + " quantities " + correctMeasure + " measures of ingredients Correctly." + Environment.NewLine;
            if (stepCheck == 1)
            {
                User_Recipe.Text = User_Recipe.Text + "You have Identified steps correctly" + Environment.NewLine;
            }
            else
            {
                User_Recipe.Text = User_Recipe.Text + "You have Identified steps incorrectly" + Environment.NewLine;
            }
            if (glassCheck == 1)
            {
                User_Recipe.Text = User_Recipe.Text + "You have Identified glass correctly" + Environment.NewLine;
            }
            else
            {
                User_Recipe.Text = User_Recipe.Text + "You have Identified glass incorrectly" + Environment.NewLine;
            }
            if (garnishCheck == 1)
            {
                User_Recipe.Text = User_Recipe.Text + "You have Identified garnish correctly" + Environment.NewLine;
            }
            else
            {
                User_Recipe.Text = User_Recipe.Text + "You have Identified garnish incorrectly" + Environment.NewLine;
            }
            User_Recipe.Text = User_Recipe.Text + "You have selected " + wrongIngredients + " wrong ingredients" + Environment.NewLine;
            totalScore = totalScore - wrongIngredients * 5;
            if (totalScore < 0)
            {
                totalScore = 0;
            }
            //showing the score to the user
            Score.Content = "Your Efficiency: " + totalScore + "%";
            textBoxPopulation();
            Correct_Recipe.Visibility = Visibility.Visible;
            User_Recipe.Visibility = Visibility.Visible;
            Play_Again.Visibility = Visibility.Visible;
            Score.Visibility = Visibility.Visible;
            Garnish.Visibility = Visibility.Hidden;
            Glass.Visibility = Visibility.Hidden;
            Serve.Visibility = Visibility.Hidden;
            Glass_Label.Visibility = Visibility.Hidden;
            Garnish_label.Visibility = Visibility.Hidden;
        }

        //function to start making the recipe
        //Thin function will display the Ingredients list
        private void Start_Click(object sender, RoutedEventArgs e)
        {
            Ingredients_List.Visibility = Visibility.Visible;
            Confirm_Ingredients.Visibility = Visibility.Visible;
            Ingre_label.Visibility = Visibility.Visible;
            Start.Visibility = Visibility.Hidden;
            Recipes.Visibility = Visibility.Hidden;
            Recipe_label.Visibility = Visibility.Hidden;
            List<string> ingList = new List<string>();
            getResponse("api/values/?a=ingredient", ingList);
            foreach (string item in ingList)
            {

                Ingredients_List.Items.Add(item.ToString());
            }



        }

        //Function for returning to main menu from play window

        private void Main_menu_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window2 lc = new Window2();
            lc.Owner = this;
            lc.WindowState = System.Windows.WindowState.Maximized;
            lc.Show();
        }

        //button for playing the game again
        //This button will start the game again
        private void Play_Again_Click(object sender, RoutedEventArgs e)
        {
            User_Recipe.Visibility = Visibility.Hidden;
            Correct_Recipe.Visibility = Visibility.Hidden;
            Score.Visibility = Visibility.Hidden;
            Play_Again.Visibility = Visibility.Hidden;

            Recipes.Visibility = Visibility.Visible;
            Start.Visibility = Visibility.Visible;
            Recipe_label.Visibility = Visibility.Visible;
            selectedIngredients.Clear();
            
            Ingredients_List.Items.Clear();
            Steps.Items.Clear();
            Recipes.SelectedIndex = 0;
            upBoxes.Clear();
            Labels.Clear();
            ComboBoxes.Clear();

        }


    }
}
