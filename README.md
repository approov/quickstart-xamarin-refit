# Approov Quickstart: Xamarin Refit

This quickstart is written specifically for mobile iOS and Android apps that are written in C# for making the API calls that you wish to protect with Approov. The sample code shown in this guide makes use of [Refit](https://github.com/reactiveui/refit), an automatic type-safe REST library. If this is not your situation then check if there is a more relevant quickstart guide available.

This page provides all the steps for integrating Approov into your app. Additionally, a step-by-step tutorial guide using our [Shapes App Example](https://github.com/approov/quickstart-xamarin-refit/blob/master/SHAPES-EXAMPLE.md) is also available.

To follow this guide you should have received an onboarding email for a trial or paid Approov account.

Note that the minimum OS requirement for iOS is 10 and for Android the minimum SDK version is 21 (Android 5.0). You cannot use Approov in apps that need to support OS versions older than this.

We will use the latest versions of the `nuget.org` packages, `ApproovSDK-3.1.2`, `ApproovHttpClient-3.1.0` and `ApproovRefit` version `6.0.94`. 

## ADDING THE APPROOV SDK ENABLED REFIT PACKAGE

 The Approov SDK needs a slightly modified `Refit` package using a specific implementation, and it is  available as a NuGet package in the default repository `nuget.org`. Note that it is not possible to use `Refit` and the `ApproovRefit` packages in the same project so you will need to uninstall the `Refit` package and replace it with the modified `ApproovRefit` one. 

![Add Refit Package](readme-images/add-refit-package.jpg)

## ADDING THE APPROOV SDK AND THE CUSTOM HTTP CLIENT

The Approov SDK makes use of a custom `HttpClient` implementation, `ApproovHttpClient` and it is available as a NuGet package in the default repository `nuget.org` alongside the actual SDK which is  `ApproovSDK`. 

![Add ApproovSDK Package](readme-images/add-nuget-packages.png)

## USING THE APPROOV REFIT PACKAGE

The `ApproovRefit` package makes use of a modified `HttpClient` class, `ApproovService` which mimics most of the original functionality.  Before using the `ApproovService` class, you need to initialize it with a configuration string. This will have been provided in your Approov onboarding email (it will be something like `#123456#K/XPlLtfcwnWkzv99Wj5VmAxo4CrU267J1KlQyoz8Qo=`). After initializing the `ApproovService` class, you can obtain an `ApproovHttpClient` and perform network requests:

```C#           
ApproovService.Initialize("<enter-your-config-string-here>");
ApproovHttpClient httpClient = ApproovService.CreateHttpClient(); 
try
{
    apiClient = RestService.For<IApiInterface>(httpClient);
}
catch (Exception ex)
......
```

The `ApproovHttpClient` implementation and its subclasses mimic the original `HttpClient` behaviour but with an additional call to the Approov servers. If you would like to use a custom `HttpMessageHandler` instead of a default one generated by `ApproovHttpClient` you can configure it and pass it as parameter like so:

```C#
ApproovHttpClient httpClient = ApproovService.CreateHttpClient(HttpMessageHandler handler)
```

Errors will generate an `ApproovException` which is a subclass of `Exception` and includes a descriptive error message and boolean value suggesting whether if the last operation should be attempted again. The `ApproovException` class is further subclassed by `InitializationFailureException` indicating an initialization error, `ConfigurationFailureException` in the case of the Approov SDK being incorrectly configured, `PinningErrorException` if a pinning mismatch occurs, a networking exception is represented by `NetworkingErrorException`. A `PermanentException` indicates an unrecoverable error and a `RejectionException` indicates the current device has had a rejected message sent by the Approov server; in this case, further information is provided in the `ARC` and `RejectionReasons` fields, as long as it has been [enabled](https://approov.io/docs/latest/approov-cli-tool-reference/#policy-command).

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

## NEXT STEPS
To actually protect your APIs and/or secrets there are some further steps. Approov provides two different options for protection:

* [API PROTECTION](https://github.com/approov/quickstart-xamarin-refit/blob/master/API-PROTECTION.md): You should use this if you control the backend API(s) being protected and are able to modify them to ensure that a valid Approov token is being passed by the app. An [Approov Token](https://approov.io/docs/latest/approov-usage-documentation/#approov-tokens) is short lived crytographically signed JWT proving the authenticity of the call.

* [SECRETS PROTECTION](https://github.com/approov/quickstart-xamarin-refit/blob/master/SECRETS-PROTECTION.md): This allows app secrets, including API keys for 3rd party services, to be protected so that they no longer need to be included in the released app code. These secrets are only made available to valid apps at runtime.

Note that it is possible to use both approaches side-by-side in the same app.

See [REFERENCE](https://github.com/approov/quickstart-xamarin-refit/blob/master/REFERENCE.md) for a complete list of all of the `ApproovService` methods.
