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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace HelloWorld
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        Game.GameRules gameRules;

        bool madeChoice = false;

        //int gameCount = 0;

        public MainPage()
        {
            this.InitializeComponent();

            //Hiragana_block.Text = " Hello World";

            gameRules = (Game.GameRules) App.Current.Resources["GameRules"];
            UpdatePage();

            ResultBlock.Visibility = Visibility.Collapsed;
            

            
        }

        private void UpdatePage()
        {
            ResultBlock.Text = " ";
            ResultBlock.Visibility = Visibility.Collapsed;
            CountBlock.Text = gameRules.getNumberofPlays.ToString();
            Question_Block.Text = gameRules.AnswerWord.getMembersAsArray[gameRules.Modes[gameRules.mode][0]];
            Hint_block.Text = gameRules.AnswerWord.getMembersAsArray[gameRules.Modes[gameRules.mode][1]];
            ButtonChoice1.Content = gameRules.Choice1.getMembersAsArray[gameRules.Modes[gameRules.mode][2]];
            ButtonChoice2.Content = gameRules.Choice2.getMembersAsArray[gameRules.Modes[gameRules.mode][2]];
            ButtonChoice3.Content = gameRules.Choice3.getMembersAsArray[gameRules.Modes[gameRules.mode][2]];

        }

        private void CheckCondition(VocabularyDictionary.Word Answer, VocabularyDictionary.Word Choice)
        {
            if (gameRules.doesMatch(Answer, Choice))
            {
                ResultBlock.Text = "Correct!";
                ResultBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Green);
                ResultBlock.Visibility = Visibility.Visible;
                madeChoice = true;
            }
            else
            {
                ResultBlock.Text = "Wrong!";
                
                ResultBlock.Foreground = new SolidColorBrush(Windows.UI.Colors.Red);
                ResultBlock.Visibility = Visibility.Visible;
                madeChoice = true;
            }
        }

        private void ResultBlock_PointerPressed(object sender, PointerRoutedEventArgs e)
        {

            var pt = e.GetCurrentPoint(ResultBlock);
            if (madeChoice == true)
            {
                if (pt.Properties.IsLeftButtonPressed)
                {
                    if (gameRules.getNumberofPlays < 10)
                    {
                        UpdatePage();
                        madeChoice = false;
                    }
                    else if (gameRules.getNumberofPlays >= 10)
                    {
                        madeChoice = false;
                        this.Frame.Navigate(typeof(BlankPage1), null);
                    }

                }
            }
            
        }


        private void ButtonChoice1_Click(object sender, RoutedEventArgs e)
        {
            CheckCondition(gameRules.AnswerWord, gameRules.Choice1);
            
        }

        private void ButtonChoice2_Click(object sende, RoutedEventArgs e)
        {
            CheckCondition(gameRules.AnswerWord, gameRules.Choice2);
        }

        private void ButtonChoice3_Click(object sende, RoutedEventArgs e)
        {
            CheckCondition(gameRules.AnswerWord, gameRules.Choice3);
        }

    }
}
