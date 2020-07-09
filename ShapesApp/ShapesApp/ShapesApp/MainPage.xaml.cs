using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Net.Http;
using Newtonsoft.Json;




namespace ShapesApp
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        private Dictionary<string, string> allImages = new Dictionary<string, string>();
        private ImageSource currentStatusImage;
        public ImageSource CurrentStatusImage
        {
            get 
            { 

                if (currentStatusImage == null) return ImageSource.FromResource(allImages["approov"], typeof(MainPage).GetTypeInfo().Assembly);
                return currentStatusImage; 
            }
            set
            {
                if (value == null) ImageSource.FromResource(allImages["approov"], typeof(MainPage).GetTypeInfo().Assembly);
                else currentStatusImage = value;
                OnPropertyChanged(nameof(CurrentStatusImage));
            }
        }

        private string statusLabelProperty;
        public string StatusLabelProperty
        {
            get { return statusLabelProperty; }
            set
            {
                statusLabelProperty = value;
                OnPropertyChanged(nameof(StatusLabelProperty));
            }
        }


        public MainPage()
        {
            InitializeComponent();
            // Load all images
            LoadAllImages();
            BindingContext = this;
            
            StatusLabelProperty = "Say Hello or Get Shape?";
        }

        public void SetStatusImageFromString(string status)
        {
            CurrentStatusImage = ImageSource.FromResource(allImages[status], typeof(MainPage).GetTypeInfo().Assembly);
        }

        protected void LoadAllImages() {
            allImages.Add("approov", "ShapesApp.Images.approov.png");
            allImages.Add("circle", "ShapesApp.Images.circle.png");
            allImages.Add("confused", "ShapesApp.Images.confused.png");
            allImages.Add("hello", "ShapesApp.Images.hello.png");
            allImages.Add("rectangle", "ShapesApp.Images.rectangle.png");
            allImages.Add("square", "ShapesApp.Images.square.png");
            allImages.Add("triangle", "ShapesApp.Images.triangle.png");
        }

        protected void HelloEvent(object sender, EventArgs args) 
        {
            try
            {
                Dictionary<string, string> responseData = DependencyService.Get<IGetShape>().GetHello();
                if (!responseData.ContainsKey("text"))
                {
                    SetStatusImageFromString("confused");
                    StatusLabelProperty = responseData["response"];
                }
                else
                {
                    SetStatusImageFromString("hello");
                    StatusLabelProperty = responseData["text"];
                }
            }
            catch (Exception e) 
            {
                Console.WriteLine("Exception during Hello: " + e.Message);
                SetStatusImageFromString("confused");
                StatusLabelProperty = "Exception during Hello";
            }
        }

        protected void ShapeEvent(object sender, EventArgs args) 
        {
            try
            {
                Dictionary<string, string> responseData = DependencyService.Get<IGetShape>().GetShape();
                if (!responseData.ContainsKey("shape"))
                {
                    SetStatusImageFromString("confused");

                }
                else
                {
                    SetStatusImageFromString(responseData["shape"].ToLower());
                }
                StatusLabelProperty = responseData["response"];

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception during Get Shape: " + e.Message);
                SetStatusImageFromString("confused");
                StatusLabelProperty = "Exception during Get Shape";
            }
        }

    }

    [ContentProperty(nameof(Source))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string Source { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (Source == null)
            {
                return null;
            }

            // Do your translation lookup here, using whatever method you require
            var imageSource = ImageSource.FromResource(Source, typeof(ImageResourceExtension).GetTypeInfo().Assembly);

            return imageSource;
        }
    }
}
