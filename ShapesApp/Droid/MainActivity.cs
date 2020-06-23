using System;
using System.Collections.Generic;
using System.Net.Http;
using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Support.Design.Widget;
using Android.Support.V7.App;
using Android.Views;
using Android.Widget;
using Refit;

namespace ShapesApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {

        // Response images
        static Dictionary<string, int> allImages = new Dictionary<string, int>();
        // Response status
        static readonly string[] messageResponse = { "hello", "confused", "circle", "rectangle", "square", "triangle" };

        /* Without Approov */
        private HttpClient httpClient;
        /* With Approov */
        //private AndroidApproovHttpClient httpClient;

        // Refit API interface
        private static IApiInterface apiClient;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            Android.Support.V7.Widget.Toolbar toolbar = FindViewById<Android.Support.V7.Widget.Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            /* Without Approov */
            httpClient = new HttpClient
            /* With Approov */
            // httpClient = new AndroidApproovHttpClient
            {
                BaseAddress = new Uri("https://shapes.approov.io")
            };
            apiClient = RestService.For<IApiInterface>(httpClient);

            // Load images from resources folder
            LoadImages();
            // background image
            int resImage = Resources.GetIdentifier("approov", "drawable", PackageName);
            ImageView img = FindViewById<ImageView>(Resource.Id.imageView1);
            img.SetImageResource(resImage);
            // Events
            FloatingActionButton fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.Click += FabOnClick;
            FloatingActionButton helloFab = FindViewById<FloatingActionButton>(Resource.Id.helloButton);
            helloFab.Click += HelloOnClick;
        }

        private void LoadImages() {
            // Load all the status images
            foreach (string message in messageResponse)
            {
                int messageImage = Resources.GetIdentifier(message, "drawable", PackageName);
                allImages.Add(message, messageImage);
            }
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            int id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }
        // Get shape event handler
        private void FabOnClick(object sender, EventArgs eventArgs)
        {
            getShape();
        }

        // Hello event handler
        private void HelloOnClick(object sender, EventArgs eventArgs) {
            getHello();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        /*  "Hello" button click network request
         *
         */
        async void getHello()
        {
            ImageView img = FindViewById<ImageView>(Resource.Id.imageView1);
            
            try
            {
                var message = await apiClient.GetHello();
                if (message.ContainsKey("text"))
                {
                    img.SetImageResource(allImages["hello"]);
                }
                else
                {
                    img.SetImageResource(allImages["confused"]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                img.SetImageResource(allImages["confused"]);
            }

        }

        /*  "Get Shape" button click network request
         *
         */
        async void getShape()
        {
            ImageView img = FindViewById<ImageView>(Resource.Id.imageView1);
            try
            {
                var message = await apiClient.GetShape();
                if (message.ContainsKey("shape"))
                {
                    string shapeToDisplay = message["shape"].ToLower();
                    foreach (string messageString in messageResponse)
                    {
                        if (messageString.Equals(shapeToDisplay))
                        {
                            img.SetImageResource(allImages[shapeToDisplay]);
                            return;
                        }
                    }

                    img.SetImageResource(allImages["confused"]);
                }
                else
                {
                    img.SetImageResource(allImages["confused"]);
                }

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception " + e.Message);
                img.SetImageResource(allImages["confused"]);
            }
        }
    }
}
