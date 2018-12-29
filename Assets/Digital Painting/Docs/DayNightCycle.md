# Day Night Cycle

It is intended that the Digital Painting will use other assets to deliver a day night cycle. The idea is that users select an implementation that delivers the right atmosphere for their scene. However, it's obviously not possible to provide automatic support for other assets. Someone needs to integrate the solution first and even before that we need a way to abstract the settings needed by different assets. So, at the current time we provide a simple Day Night Solution and plan to add support for more in the future. Patches welcome.

## Supported Day/Night Assets

At present The Digital Painting supports the following assets available:

  * Very Simple Day Night Cycle (Built In - See `Environment/VerySimpleDayNightCycle` script)

## Adding A Day Night Cycle to Your Digital Painting


### Very Simple Day Night Cycle (Built In)

This is about as simple as a day night cycle can get - and it's pretty ugly too. The Sun rises and sets at exactly the same time at exactly the simple place. We will gladly accept pull requests to improve this, but in reality we expect most users to use a specialist Asset.

  * Create an empty object called `Managers`
  * Drop the `Environment/DayNightCycle` script onto the `Managers` object
  * Add a directional light (if you don't have one already) and rename it to Sun.

## Possible Future Supported Assets

Patches are welcome to add support for any Day Night Cycle solution. Here are a few that might be interesting.

  * Spyblood Games [Simple Day And Night Cycle System](https://assetstore.unity.com/packages/templates/tutorials/simple-day-and-night-cycle-system-66647) - Free from the Unity Asset Store
  * Flat Tutorials [Day and Night Cycle with Scattering and Moving Clouds](https://assetstore.unity.com/packages/tools/particles-effects/day-and-night-cycle-with-scattering-and-moving-clouds-27024) - Free on the Unity Asset Store
