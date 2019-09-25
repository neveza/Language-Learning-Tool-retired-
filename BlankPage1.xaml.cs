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
    public sealed partial class BlankPage1 : Page
    {
        public BlankPage1()
        {
            this.InitializeComponent();

            var TheDictionary = (VocabularyDictionary.MasterDictionary)App.Current.Resources["TheDictionary"];
            VocabularyDictionary.DataManagement.Save("DataFile", TheDictionary);
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            this.Frame.Navigate(typeof(BlankPage2), null);

            
        }

        private async void button1_Click(object sender, RoutedEventArgs e)
        {
            
            var openFile = new Windows.Storage.Pickers.FileOpenPicker();
            openFile.ViewMode = Windows.Storage.Pickers.PickerViewMode.Thumbnail;
            openFile.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.PicturesLibrary;
            openFile.FileTypeFilter.Add(".ods");


            
            Windows.Storage.StorageFile file = await openFile.PickSingleFileAsync();

           

            if (file != null)
            {
                // Application now has read/write access to the picked file
                var fileStream = file.OpenStreamForReadAsync();
               

                var TheDictionary = (VocabularyDictionary.MasterDictionary)App.Current.Resources["TheDictionary"];

                var t = System.Threading.Tasks.Task.Run(() => TheDictionary.LoadODS(fileStream.Result));
                t.Wait();

                VocabularyDictionary.DataManagement.Save("DataFile", TheDictionary);

                App.Current.Resources["TheDictionary"] = TheDictionary;

            }
            else
            {
                //this.textBlock.Text = "Operation cancelled.";
            }

        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {

            App.Current.Exit();


        }
    }
}
