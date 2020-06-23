using System;
using System.Collections.Generic;
using UIKit;
using Refit;

using System.Net.Http;

namespace ShapesApp.iOS
{
    public partial class ViewController : UIViewController
    {
        // Response images
        static Dictionary<string, UIImage> allImages = new Dictionary<string, UIImage>();
        // Response status
        static readonly string[] messageResponse = { "hello", "confused", "circle", "rectangle", "square", "triangle" };

        // Refit API interface
        private IApiInterface apiClient;

        /* Comment out the line to use Approov SDK */
        private HttpClient httpClient;
        /* Uncomment the line to use Approov SDK */
        //private IosApproovHttpClient httpClient;

        public ViewController(IntPtr handle) : base(handle)
        {
            /* Comment out the line to use Approov SDK */
            httpClient = new HttpClient
            /* Comment out the line to use Approov SDK */
            //httpClient = new IosApproovHttpClient
            {
                BaseAddress = new Uri("https://shapes.approov.io")
            };
            try
            {
                apiClient = RestService.For<IApiInterface>(httpClient);
            }
            catch (Exception ex) {
                Console.WriteLine("Exception during RestService: " + ex.Message);
            }
            
            // Load all the status images
            foreach (string message in messageResponse)
            {
                UIImage messageImage = UIImage.FromBundle(message + ".png");
                if (messageImage == null)
                {
                    throw new Exception("ApproovShapes failed loading image " + message + ".png from main bundle");
                }
                allImages.Add(message, messageImage);
            }
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Perform any additional setup after loading the view, typically from a nib.
            UIImage approovImage = UIImage.FromBundle("approov.png");
            if (approovImage != null) {
                logoImageView.Image = approovImage;
            }
            
        }

        public override void DidReceiveMemoryWarning()
        {
            base.DidReceiveMemoryWarning();
            // Release any cached data, images, etc that aren't in use.		
        }

        partial void HelloButton_TouchUpInside(UIButton sender)
        {
            getHello();
        }

        partial void ShapeButton_TouchUpInside(UIButton sender)
        {
            getShape();
            
        }

        /*  "Hello" button click network request
         *
         */
        async void getHello() {
            
            try
            {
                var message = await apiClient.GetHello();
                if (message.ContainsKey("text"))
                {
                    logoImageView.Image = allImages["hello"];
                }
                else
                {
                    logoImageView.Image = allImages["confused"];
                }
                
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in getHello(): " + e.Message);
                logoImageView.Image = allImages["confused"];
            }
            
        }

        /*  "Get Shape" button click network request
         *
         */
        async void getShape() {
            try
            {
                var message = await apiClient.GetShape();
                if (message.ContainsKey("shape"))
                {
                    string shapeToDisplay = message["shape"].ToLower();
                    foreach(string messageString in messageResponse)
                    {
                        if (messageString.Equals(shapeToDisplay))
                        {
                            logoImageView.Image = allImages[shapeToDisplay];
                            return;
                        }
                    }

                    logoImageView.Image = allImages["confused"];
                }
                else
                {
                    logoImageView.Image = allImages["confused"];
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception in getShape():" + e.Message);
                logoImageView.Image = allImages["confused"];
            }
        }
    }
}
