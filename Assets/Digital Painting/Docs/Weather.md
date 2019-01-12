# Weather in The Digital Painting

As with other parts of The Digital Painting we try to make the weather system pluggable, but also like other parts this plugability has to grow with the needs of the developers working on the code. In other words, if we don't yet support the weather feature or asset you need we welcome your patches.

## Adding a Weather Cycle to a Digital Painting

  * Create a Weather System file using `Assets -> Create -> Wizards Code -> Weather ... `
    * There will be entries in this folder for each of the supported assets you have added [plugins](../plugins/README.md) for, if you have not added any plugins only the supplied Dummy Weather System will be available, this simply logs weather to the console
  * Add the `WeatherManager` component to your `managers` game object
  * Drag the Weather System file created above to the `Implementation` field of the WeatherManager component

## Developing a Weather Implementation

In its simplest form integration doesn't require any significant work. If you simply want your Weather Asset to run independent of the control of your Digital Painting you can simply add it to your application as normal and it should work. However, if you want your Digital Painting to be in control of the weather then you will need to do some work. Below we describe the steps for what is typically required, in this case we are adding support for [Digital Ruby's Weather Maker](https://assetstore.unity.com/packages/tools/particles-effects/weather-maker-sky-weather-water-volumetric-light-60955) which does much more than simply add a weather system, but here we are only concerning ourselves with the weather components.

  * Create a folder for your integration at `Digital Paintings/Plugins/Weather_WeatherMaker/`
  * Create a folder for your test scene at `Digital Paintings/Plugins/Weather_WeatherMaker/Scenes/DevTest`
  * Make a copy of the `Digital Paintings/Scenes/DevTest/Dev Scene` in your `Digital Paintings/Plugins/DayNightCycle_WeatherMaker/Scenes/DevTest` folder
  * Open this scene 
  * Create a folder for your code at `Digital Paintings/Plugins/Weather_WeatherMaker/Scripts`
  * Create a script in that folder called `WeatherMakerWeatherSystem` which inherits from `wizardscode.environment.AbstractWeatherSystem`
  * Implement the methods in this script (this is the hard part that will vary from asset to asset, use the RainMaker weather plugin as an example, a free solution from the same publisher as Weather Maker, you can also look at the Dummy weather system that comes with The Digital Painting asset)
    * Have a line such as `[CreateAssetMenu(fileName = "WeatherMakerWeatherSystem", menuName = "Wizards Code/Weather/Weather Maker")]` before your class definition to make your implementation appear in the menus.
  * Configure the `Start` and `Update` methods as necessary

That's it from an implementation perspective. Of course you will need to test things. That's covered in the next section.
