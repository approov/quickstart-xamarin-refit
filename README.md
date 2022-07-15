# Approov Quickstart: Xamarin Refit

This quickstart is written specifically for mobile iOS and Android apps that are written in C# for making the API calls that you wish to protect with Approov. The sample code shown in this guide makes use of [Refit](https://github.com/reactiveui/refit), an automatic type-safe REST library. If this is not your situation then check if there is a more relevant quickstart guide available.

This quickstart provides the basic steps for integrating Approov into your app. A more detailed step-by-step guide using a [Shapes App Example](https://github.com/approov/quickstart-xamarin-refit/blob/master/SHAPES-EXAMPLE.md) is also available.

To follow this guide you should have received an onboarding email for a trial or paid Approov account.

Note that the minimum OS requirement for iOS is 10 and for Android the minimum SDK version is 21 (Android 5.0). You cannot use Approov in apps that need to support OS versions older than this.

## ADDING THE APPROOV SDK ENABLED REFIT PACKAGE

The ApproovSDK makes use of a custom `HttpClient` implementation, `ApproovHttpClient`. It needs a slightly modified `Refit` package using that specific implementation, and it is available as a NuGet package in the default repository `nuget.org`. Note that it is not possible to use `Refit` and the `ApproovRefit` packages in the same project so you will need to uninstall the `Refit` package and replace it with the modified `ApproovRefit` one. 

![Add ApproovSDK Package](readme-images/add-approovsdk-package.png)

You will also need to install the custom `ApproovHttpClient` implementation, wich consists of the platform independent package `ApproovHttpClient` and its corresponding iOS/Android implementations `ApproovHttpClient-Platform-Specific`.

![Add HttpClient Package](readme-images/add-http-client-package.png)

## ADDING THE APPROOV SDK

The Approov SDK is also available as a NuGet package in the repository `nuget.org`, the packages name being `ApproovSDK`.

![Add ApproovSDK Package](readme-images/add-approovsdk-package.png)

## USING THE APPROOV SDK ENABLED REFIT PACKAGE

The `ApproovRefit` package makes use of a modified `HttpClient` class, `ApproovHttpClient` which mimics most of the original functionality and is subclassed by the platform specific `IosApproovHttpClient` and `AndroidApproovHttpClient`. A Factory Method Patern is used to instantiate the platform specific clients with the additional requirement of a configuration string, specific to your account and required by the Approov SDK before being used. This will have been provided in your Approov onboarding email (it will be something like `#123456#K/XPlLtfcwnWkzv99Wj5VmAxo4CrU267J1KlQyoz8Qo=`).

```C#           
var factory = new ApproovHttpClientFactory();
httpClient = factory.GetApproovHttpClient("<enter-your-config-string-here>")
try
{
    apiClient = RestService.For<IApiInterface>(httpClient);
}
catch (Exception ex)
......
```

The `ApproovHttpClient` implementation and its subclasses mimic the original `HttpClient` behaviour but with an additional call to the Approov servers. To explore additional options on how to configure and use `ApproovHttpClient` classes go to [Next Steps](https://github.com/approov/quickstart-xamarin-httpclient/blob/master/NEXT-STEPS.md). If you would like to use a custom `HttpMessageHandler` instead of a default one generated by `HttpClient` you can configure it and pass it as parameter like so:

```C#
httpClient = factory.GetApproovHttpClient(customHandlerParam, "<enter-your-config-string-here>")            
```

## ANDROID MANIFEST CHANGES
The following app permissions need to be available in the manifest to use Approov:

```xml
<uses-permission android:name="android.permission.ACCESS_NETWORK_STATE" />
<uses-permission android:name="android.permission.INTERNET" />
```

Please [read this](https://approov.io/docs/latest/approov-usage-documentation/#targetting-android-11-and-above) section of the reference documentation if targetting Android 11 (API level 30) or above.

## CHECKING IT WORKS
Initially you won't have set which API domains to protect, so the interceptor will not add anything. It will have called Approov though and made contact with the Approov cloud service. You will see logging from Approov saying `UNKNOWN_URL`.

Your Approov onboarding email should contain a link allowing you to access [Live Metrics Graphs](https://approov.io/docs/latest/approov-usage-documentation/#metrics-graphs). After you've run your app with Approov integration you should be able to see the results in the live metrics within a minute or so. At this stage you could even release your app to get details of your app population and the attributes of the devices they are running upon.

However, to actually protect your APIs there are some further steps you can learn about in [Next Steps](https://github.com/approov/quickstart-xamarin-refit/blob/master/NEXT-STEPS.md).
