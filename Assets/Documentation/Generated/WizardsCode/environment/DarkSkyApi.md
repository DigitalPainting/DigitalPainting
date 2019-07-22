# wizardscode.environment.DarkSkyApi

## Api Key (String)

DarkSky API key, see https://darksky.net/dev


## Forecast Update Frequency (Single)

How frequently, in seconds, we should update the Dark Sky data

Default Value     : 300


## Latitude (Single)

Latitude of the place from which we want to retrieve the weather.

Default Value     : 0


## Longitude (Single)

Longitude of the place from which we want to retrieve the weather.

Default Value     : 0


## Time Offset (Int32)

How far into the future (in units identified by timeOffsetUnits) should our painting display

Default Value     : 30


## Time Offset Units (TimeOffsetType)

Units that the time offset (below) uses

Default Value     : Minutes


## Delegate Weather System (AbstractWeatherSystem)

The Weather System to delegate rendering of the weather to. If left blank then the weather will simply be logged to the console.


## Skybox Material (Material)

Skybox material

