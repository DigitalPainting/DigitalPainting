# Day Night Cycle

It is intended that the Digital Painting will use other assets to deliver a day night cycle. The idea is that users select an implementation that delivers the right atmosphere for their scene. However, it's obviously not possible to provide automatic support for other assets. Someone needs to integrate the solution first and even before that we need a way to abstract the settings needed by different assets. The first section of this document talks about which assets are supported and how to use them. The second half talks about how to add support for new Day Night assets.

### Using a Day Night Cycle in Your Digital Painting

  * Create a configuration file using `Assets -> Create -> Wizards Code/Day Night Cycle ... `
    * There will be entries in this folder for each of the supported assets you have added [plugins](../plugins/README.md) for, The Digital Painting provides a Simple Day Night solution, so you will at least have this
  * Drag the configuration file created above into the `Configuration` field of the `Day Night Cycle Manager` component
    * Note if this field is null then no DayNightCycle will be active.

## Adding Support for a new Day Night Cycle Asset

In its simplest form integration doesn't require any significant work. If you simply want your Day Night Cycle to run independent of the control of your Digital Painting you can simply add it to your application as normal and it should work. However, if you want other inhabitants of your Digital Painting to be aware of the Day Night Cycle then you will need to do some work. Below we describe the steps for what is typically required, in this case we are adding support for [Digital Ruby's Weather Maker](https://assetstore.unity.com/packages/tools/particles-effects/weather-maker-sky-weather-water-volumetric-light-60955) which does much more than simply add a day night cycle, but here we are only concerning ourselves with the Day Night Cycle components.

  * Create a folder for your integration at `DigitalPaintingsIntegrations/WeatherMaker/`
  * Create a folder for your test scene at `DigitalPaintingsIntegrations/WeatherMaker/Scenes/DevTest`
  * Make a copy of the `DigitalPaintings/Scenes/DevTest/DayNightCycle` in your `DigitalPaintingsIntegrations/WeatherMaker/Scenes/DevTest` folder
  * Open this scene
  * Create a folder for your code at `DigitalPaintingsIntegrations/WeatherMaker/Scripts`
  * Create a script in that folder called `WeatherMakerDayNightCycle` which inherits from `wizardscode.environment.AbstractDayNightCycle`
  * Implement the methods in this script (this is the hard part that will vary from asset to asset)
    * Have a line such as `[CreateAssetMenu(fileName = "WeatherMakerDayNightCycleConfig", menuName = "Wizards Code/Day Night Cycle/Weather Maker Day Night Cycle Config")]` before your class definition to make your implementation appear in the menus.

That's it from an implementation perspective. Of course you will need to test things. That's covered in the next section.


### Testing a Day Night Cycle Implementation

In the development section you created a DevTest scene. In order to use this with your new implementation you need to perform these simple steps:

  * Assets -> Create -> Wizards Code -> Digital Painting -> Day Night Cycle -> Weather Maker Day Night Cycle Config
    * Configure is as required
  * Drag this into the `Configuration` field of the `Day Night Cycle` component attached to the `Managers` object

Hitting play at this point will run the scene and, hopefully, your new Day Night asset will be managing the cycle. The UI in the dev scene will provide some insights into whether your implementation is working or not.

### Possible Future Supported Assets

Patches are welcome to add support for any Day Night Cycle solution. Here are a few that might be interesting.

  * Spyblood Games [Simple Day And Night Cycle System](https://assetstore.unity.com/packages/templates/tutorials/simple-day-and-night-cycle-system-66647) - Free from the Unity Asset Store
  * Flat Tutorials [Day and Night Cycle with Scattering and Moving Clouds](https://assetstore.unity.com/packages/tools/particles-effects/day-and-night-cycle-with-scattering-and-moving-clouds-27024) - Free on the Unity Asset Store
