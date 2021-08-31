# Approov Quickstart: Xamarin Refit

This quickstart is written specifically for mobile iOS and Android apps that are written in C# for making the API calls that you wish to protect with Approov. The sample code shown in this guide makes use of [Refit](https://github.com/reactiveui/refit), an automatic type-safe REST library. If this is not your situation then check if there is a more relevant quickstart guide available.

This quickstart provides the basic steps for integrating Approov into your app. A more detailed step-by-step guide using a [Shapes App Example](https://github.com/approov/quickstart-xamarin-refit/blob/master/SHAPES-EXAMPLE.md) is also available.

To follow this guide you should have received an onboarding email for a trial or paid Approov account.

## ADDING THE APPROOV SDK ENABLED REFIT PACKAGE

The ApproovSDK makes use of a custom `HttpClient` implementation, `ApproovHttpClient`. It needs a slightly modified `Refit` package using that specific implementation, and it is available as a NuGet package in the default repository `nuget.org`. Note that it is not possible to use `Refit` and the `ApproovRefit` packages in the same project so you will need to uninstall the `Refit` package and replace it with the modified `ApproovRefit` one. 

![Add ApproovSDK Package](readme-images/add-approovsdk-package.png)

You will also need to install the custom `ApproovHttpClient` implementation, wich consists of the platform independent package `ApproovHttpClient` and its corresponding iOS/Android implementations `ApproovHttpClient-Platform-Specific`.

![Add HttpClient Package](readme-images/add-http-client-package.png)

## ADDING THE APPROOV SDK

The Approov SDK is also available as a NuGet package in the repository `nuget.org`, the packages name being `ApproovSDK`.

![Add ApproovSDK Package](readme-images/add-approov-sdk-package.png)

## USING THE APPROOV SDK ENABLED REFIT PACKAGE

The `ApproovRefit` package makes use of a modified `HttpClient` class, `ApproovHttpClient` which mimics most the original functionality and is subclassed by the platform specific `IosApproovHttpClient` and `AndroidApproovHttpClient`. The only requirement is instantiating the platform specific client with an additional configuration string, specific to your account, obtained by the `approov` command line utility (it will be something like #123456#K/XPlLtfcwnWkzv99Wj5VmAxo4CrU267J1KlQyoz8Qo=). Instantiating the platform specific clients can be done like so:

```C#
string approovSDKConfig = "#123456#K/XPlLtfcwnWkzv99Wj5VmAxo4CrU267J1KlQyoz8Qo=";            
httpClient = new IosApproovHttpClient(approovSDKConfig)
{
    BaseAddress = new Uri("https://shapes.approov.io")
};
try
            {
                apiClient = RestService.For<IApiInterface>(httpClient);
            }
catch (Exception ex)
......
```

or for Android:

```C#
string approovSDKConfig = "#123456#K/XPlLtfcwnWkzv99Wj5VmAxo4CrU267J1KlQyoz8Qo=";            
httpClient = new AndroidApproovHttpClient(approovSDKConfig)
{
    BaseAddress = new Uri("https://shapes.approov.io")
};
try
            {
                apiClient = RestService.For<IApiInterface>(httpClient);
            }
catch (Exception ex)
......
```

The above sample code assumes the following definition for `IApiInterface`:

```C#
public interface IApiInterface
    {
        [Get("/v1/hello")]
        Task<Dictionary<string, string>> GetHello();
        [Get("/v2/shapes")]
        Task<Dictionary<string, string>> GetShape();
    }
```

## CHECKING IT WORKS
Initially you won't have set which API domains to protect, so the interceptor will not add anything. It will have called Approov though and made contact with the Approov cloud service. You will see logging from Approov saying `UNKNOWN_URL`.

Your Approov onboarding email should contain a link allowing you to access [Live Metrics Graphs](https://approov.io/docs/latest/approov-usage-documentation/#metrics-graphs). After you've run your app with Approov integration you should be able to see the results in the live metrics within a minute or so. At this stage you could even release your app to get details of your app population and the attributes of the devices they are running upon.

However, to actually protect your APIs there are some further steps you can learn about in [Next Steps](https://github.com/approov/quickstart-xamarin-refit/blob/master/NEXT-STEPS.md).