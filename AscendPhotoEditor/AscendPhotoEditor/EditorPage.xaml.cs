using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;

namespace AscendPhotoEditor
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EditorPage : ContentPage
    {
        private string imagePath { get; set;}
        public EditorPage(string imagePath)
        {
            InitializeComponent();

            selectedImage.Source = imagePath;
        }
    }
}