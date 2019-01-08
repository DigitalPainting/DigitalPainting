# Weather in The Digital Painting

As with other parts of The Digital Painting we try to make the weather system pluggable, but also like other parts this pluggability has to grow with the needs of the developers working on the code. In other words, if we don't yet support the weather feature or asset you need we welcome your patches.

## Adding a Weather Cycle to a Digital Painting

  * Create a Weather System file using `Assets -> Create -> Wizards Code -> Weather ... `
    * There will be entries in this folder for each of the supported assets you have added plugins for, if you have not added any plugins only the supplied Dummy Weather System will be available, this simply logs weather to the console
  * Add the `WeatherManager` component to your `managers` game object
  * Drag the Weather System file created above to the `Implementation` field of the WeatherManager component

## Developing a Weather Implementation

The Digital Painting provides a dummy implementation of a Weather solution in `DummyWeatherImplementation`. This can be used as a basis for adding support for your favourite Weather asset.