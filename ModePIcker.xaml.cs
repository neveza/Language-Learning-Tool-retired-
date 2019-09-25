using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage2 : Page
    {

        string GenerationChoice = " ";
        string PracticeTypeChoice = " ";

        public BlankPage2()
        {
            this.InitializeComponent();
            //PracticeType_Checked();

        }

        //private void PracticeType_Checked(object sender, RoutedEventArgs e)
        //{
        //   // RadioButton rb = sender as RadioButton;

        //    foreach (RadioButton item in PracticeTypeList.Items)
        //    {
        //        if (item.IsChecked == true)
        //        {
        //            //textBox.Text = item.Tag.ToString();
        //            PracticeTypeChoice = item.Tag.ToString();
        //        }
        //    }

        

        //    //if (rb != null && PracticeTypeBoarder != null)
        //    //{
        //    //    textBox.Text = rb.Tag.ToString();
        //    //}

        //}

        //private void GenerationType_Checked(object sender, RoutedEventArgs e)
        //{

        //    foreach (RadioButton item in GenerationTypeList.Items)
        //    {
        //        if (item.IsChecked == true)
        //        {
        //            GenerationChoice = item.Tag.ToString();
        //        }
        //    }


        //}

        private void ConfirmChoices()
        {

            foreach (RadioButton item in PracticeTypeList.Items)
            {
                if (item.IsChecked == true)
                {
                    //textBox.Text = item.Tag.ToString();
                    PracticeTypeChoice = item.Tag.ToString();
                }
            }


            foreach (RadioButton item in GenerationTypeList.Items)
            {
                if (item.IsChecked == true)
                {
                    GenerationChoice = item.Tag.ToString();
                }
            }



        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            var TheDictionary = (VocabularyDictionary.MasterDictionary)App.Current.Resources["TheDictionary"];
            //System.Diagnostics.Debug.WriteLine("ModePIcker Dictionary: " + TheDictionary.DictionaryCatalogue.Count());
            System.Diagnostics.Debug.WriteLine(PracticeTypeChoice, GenerationChoice);
            ConfirmChoices();
            Game.GameRules gameRules = new Game.GameRules(GenerationChoice, PracticeTypeChoice, TheDictionary);

            if (App.Current.Resources.ContainsKey("gameRules"))
            {
                App.Current.Resources.Add("GameRules", gameRules);
            }
            else
            {
                App.Current.Resources["GameRules"] = gameRules;
            }

            this.Frame.Navigate(typeof(MainPage), null);

        }
    }
}
