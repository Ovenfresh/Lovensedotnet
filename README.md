# Lovensedotnet - A Standard API Quick Start Guide
*A simple client to send API calls along the Lovense Standard API*
*And to receive the Callback*

So, you want to start developing something for your toys, or someone else's, in .NET C# and need a place to start. For you, this will be a little tutorial w/ boilerplate to follow along with to get started!

## Quick overview of how the application works

This application uses 2 components to handle the communication to and from the Lovense servers, using their [_Standard API_](https://developer.lovense.com/#standard-api).

* WebAPI to 
  1. Handle the Callback
  2. Provide basic operations to the toy(s) for testing purposes
* An HttpClient to handle the POST-requests to the Lovense servers
  * To get the necessary QR-code
  * To POST commands for vibrations, patterns & presets

While this solution can function as boilerplate to build off of, some understanding of how RESTful API's is definitely recommended. Having the Standard API documentation to follow along with is also recommended.
To pass along the data with the request, I use Data Tranfer Objects (DTO's), which is essentialy a class designed specifically to fit in these requests. You can look at this as if it were some kind of naming scheme. They designate objects as meant for use in an Http Request.

At the end of this guide, you should be able to
1. Navigate through the solution
2. Understand how the requests are made & handled.
3. Able to connect to a toy with Lovense Standard API
4. Send commands to one, or all, of your toys.
    * Function 
    * Preset
    * Pattern

## Setting up the application

First things first, my IDE is Visual Studio 2019, and I will be using an extension called [_conveyor_](https://conveyor.cloud/), which will allow you to 'open up' the localhost client to the internet so you can capture the callback coming from the Lovense servers. You will need an account for this.
You will also need to request developer access on the Lovense site itself, so you can get you developer token. For this you'll need a lovense account.

Look for the Extension on the extension manager in Visual Studio (Extensions > Manage Extensions > search for conveyor) and install it.

The Appsettings.json file needs to be filled in with your developer token & username.

## Application flow

To use the various commands, the application follows a specific flow to reach that point.

1. The HttpClient sends a POST-request to the lovense server using 'LovenseClient.GetQR()', this is an async task of which the result will be a string
    * This is done using the AuthDTO, of which the important elements are the DevToken & DevID
2. If this request is successful, the client receives a link to a QR code that needs to be scanned by either the Lovense Connect, or the Lovense Remote application on your user's smarphone
3. The User scans the QR code in the aforementioned Apps
4. After the scan, the Lovense servers send an HttpPost request to your Callback URL and deliver JSON that will be parsed into an object of the Callback class
    * The Callback is captured by the LovenseController, using the HttpPost Callback method, and through the URL supplied by Conveyor
    * This Callback will be stored in the LovenseClient
5. Then, using a specific Toy ID, or none if you want to target all active toys, you are able to send commands to the toy(s) through the HttpClient
    * The Toys are delivered as a dictionary<string,Toy>, but to prevent having to keep track of the toy ID's in order to send them commands, I access the toys by their Index in the dictionary.

## The DTO's

So, in order to pass on the data along with the Http Requests, I use DTO's. These are classes of which i've annotated the properties so that these allign with how the key-value pairs are structured in the JSON that the Lovense API expects.
If the properties have the same name as the keys in the JSON, these annotations are not necessary, however, if you'd like the properties to have different names in your program, then you have to use the  annotation '[JsonProperty("nameofproperty")]'.

### AuthDTO

The AuthDTO is used in the first POST request to get the url. The LovenseClient fills in the DevID & Token which are filled in in _Startup.cs_ where the client is constructed as a singleton to be used throughout the application.
If you don't want to use _appsettings.json_ you can hardcode your credentials, but that never recommended (best practices, folks, keep your unique keys private).

### QRDTO

This is the DTO used to receive the URL for the QR, and is parsed from the response received in LovenseClient.GetQR().

### CommandDTO

Arguably the most important DTO. This is the object that will be sent along the POST requests whenever you try and make a toy do something.

* Token: This is your developer token. Gets filled in by the LovenseClient in the DoCMD(CommandDTO) method
* UserID: This is the ID of the user of _your_ application, not the ID of the developer. This also gets filled in during the DoCMD method and gets pulled from the Callback
* Command: This determines the type of request, which each need different parameters
    * Function: This will have you use the 'Action'-property
    * Preset: This will have you use the 'Name'-Property
    * Pattern: This will have you use the 'Pattern'- and 'Strength'-Properties
 * Duration: This determines how long your toys will be doing the thing you just told them to do, and is REQUIRED for every request
 * TargetToyID: Pretty self explanatory, leaving this empty will target all available toys of the User specified with UserID
 * ApiVersion: this is 1 by default, and can be left as is 

#### Command-specific Properties
##### Function
* Action: tells your toy what to do, along with the intensity. Noted as "function:intensity", for example "vibrate:8"
    * Vibrate, with an intensity range of 0 - 20
    * Rotate, range of 0 - 20
    * Pump, range of 0 - 3
* LoopLength: Determines how long the action will be performed, before being interrupted by the LoopInterval (optional)
* LoopInterval: Determines how long the action will be interrupted after the LoopLength.

Do note that the action will cycle between Length & Interval for the Duration of the action.
For example: if Duration is 4 (sec), and Length and Interval each 1 (sec), you'll have two cycles that are 2 seconds long, being Length (action on) + Interval (action off).
If an ongoing cycle extends beyond the duration, it will simply be cut off.
#### Preset
Calls a preset present on the device
* Name: The name of  the preset
    * Pulse
    * Wave
    * Fireworks
    * Earthquake
    * I don't know if it's possible to access custom patterns, either way, i've not gotten it to work
#### Pattern
* Structure: this is the structure of the pattern and is passed through as a string "V:1;F:x;S:y#"
    * 'V:1': The API version, leave as is
    * 'F:x': The Functions used in the pattern. Use the first letter of the function(s) you want to use in the pattern.
    -> Ex. 'F:v' to vibrate, 'F:rp' to use both rotation & pump.
    * 'S:y#': The time it takes to cycle through each step of the pattern, in ms
    -> Ex. 'S:1000#' would be a cycle of 1sec. The pattern will progress through the pattern each second.
* Pattern: formatted as "x;y;z;...", this defines the pattern. It will iterate through each intensity step by the cycle/interval defined in the pattern.
    -> Ex. "5;10;15;20": after the time set in 'S:y#', the pattern will move on to the next step.
    
 Now to put these together.
 Say, Duration is 4s, Pattern:"V:1;F:v;S:1000#", and strength:"5;10;15;20", the following will happen with 'v' being vibration.
 
 |Time       |1s      |2s      |3s      |4s      |5s      |6s      |...|
 |-----------|--------|--------|--------|--------|--------|--------|---|
 |Action     |v       |v       |v       |v       |/       |/       |...|
 |Strength   |5       |10      |15      |20      |/       |/       |...|
 
 The pattern will be repeated for the Duration, and if the Pattern exceeds the duration, it will simply cut off.
 
 ## Setting the Callback URL
 
 Once you've loaded the solution, head to Tools > Conveyor & select it. You'll see a window that 'll look something like this
 
![Capture](https://user-images.githubusercontent.com/29046191/174491583-a4b3c6db-8a1d-49a9-81d8-7a9569199cff.JPG)

You won't have the Internet URL yet, to do that, you'll have to sign up for/in with an account. Then, once you launch your application, you can click Access Over Internet & it'll generate a url. This url will redirect to your homepage, which in this case would be the swagger index, where you can find an overview of all the API calls that can be made to the _application_ through webAPI.
You'll need to navigate to this url, try out either the Get or Post Callback request, and from there, you can take the request URL, and enter that as your Callback URL on the developer dashboard.




