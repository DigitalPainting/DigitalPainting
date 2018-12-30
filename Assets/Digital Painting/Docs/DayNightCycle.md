# Day Night Cycle

It is intended that the Digital Painting will use other assets to deliver a day night cycle. The idea is that users select an implementation that delivers the right atmosphere for their scene. However, it's obviously not possible to provide automatic support for other assets. Someone needs to integrate the solution first and even before that we need a way to abstract the settings needed by different assets. So, at the current time we provide a simple Day Night Solution and plan to add support for more in the future. Patches welcome.

## Supported Day/Night Assets

At present The Digital Painting supports the following assets available:

  * Simple Day Night Cycle (Built In - See `Environment/SimpleDayNightCycle` script)

## Adding A Day Night Cycle to Your Digital Painting

  * Create a configuration file using `Assets -> Create -> Wizards Code ... `
    * Note that there are predefined configuration files in `Prefabs/Data`
  * Add the `DayNightCycle` component to your `managers` game object
  * Add a Configuration file to the `DayNightCycle` component

## Adding Support for a new Day Night Cycle Asset

Use `Scenes/DevTest/DayNightCycle` to test your scene in a game environment.

The interface between The Digital Painting and your chosen Day Night Cycle asset is provided by a 
 scriptable object that inherits from `AbstractDayNightCycle`. To add support for a new Day Night Cycle asset you will need to create an instance of this Scriptable Object. Once created you can provide this scriptable object to the configuration field of the `DayNightCycle` component as described above.

### Possible Future Supported Assets

Patches are welcome to add support for any Day Night Cycle solution. Here are a few that might be interesting.

  * Spyblood Games [Simple Day And Night Cycle System](https://assetstore.unity.com/packages/templates/tutorials/simple-day-and-night-cycle-system-66647) - Free from the Unity Asset Store
  * Flat Tutorials [Day and Night Cycle with Scattering and Moving Clouds](https://assetstore.unity.com/packages/tools/particles-effects/day-and-night-cycle-with-scattering-and-moving-clouds-27024) - Free on the Unity Asset Store
