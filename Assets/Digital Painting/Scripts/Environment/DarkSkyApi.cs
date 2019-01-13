using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using UnityEngine;

namespace wizardscode.environment
{
    /// <summary>
    /// Retrieves the weather (current or forecasted) from DarkSky and feeds it into a Digital Paintings weather system implementation.
    /// </summary>
    [CreateAssetMenu(fileName = "DarkSkyWeatherSystem", menuName = "Wizards Code/Weather/Dark Sky Weather System")]
    public class DarkSkyApi : AbstractWeatherSystem
    {
        [Header("Dark Sky Settings (powered by Dark Sky - see https://darksky.net/poweredby/")]
        [Tooltip("DarkSky API key, see https://darksky.net/dev")]
        public string apiKey;
        [Tooltip("How frequently, in seconds, we should update the Dark Sky data")]
        public float forecastUpdateFrequency = 300;
        [Tooltip("Latitude of the place from which we want to retrieve the weather.")]
        public float latitude;
        [Tooltip("Longitude of the place from which we want to retrieve the weather.")]
        public float longitude;

        [Header("Data Retrieval Settings")]
        [Tooltip("How far into the future (in units identified by timeOffsetUnits) should our painting display")]
        public int timeOffset = 30;
        [Tooltip("Units that the time offset (below) uses")]
        public TimeOffsetType timeOffsetUnits = TimeOffsetType.Minutes;

        [Header("Weather System")]
        [Tooltip("The Weather System to delegate rendering of the weather to. If left blank then the weather will simply be logged to the console.")]
        public AbstractWeatherSystem delegateWeatherSystem;

        string apiRequestURL;
        string apiRequestTemplate = "https://api.darksky.net/forecast/{0}/{1},{2}?exclude=alerts,flags";
        private float timeToNextUpdate = 0;
        private object lastWeather;

        public enum TimeOffsetType { Current, Minutes, Hours, Days }


        /// <summary>
        /// Force an immediate update of the weather, regardless of the how long is left until the next update.
        /// </summary>
        public void UpdateNow()
        {
            timeToNextUpdate = 0;
        }

        internal void UpdateWeather(WeatherDataPoint weather)
        {
            WeatherProfile newProfile;
            switch (weather.icon)
            {
                case "clear-day":
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Clear, WeatherProfile.CloudTypeEnum.Clear);
                    break;
                case "clear-night":
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Clear, WeatherProfile.CloudTypeEnum.Clear);
                    break;
                case "rain":
                    WeatherProfile.CloudTypeEnum cloud;
                    if (weather.precipIntensity <= 2.5)
                    {
                        cloud = WeatherProfile.CloudTypeEnum.Light;
                    }
                    else if (weather.precipIntensity <= 7.6)
                    {
                        // TODO: we need medium clouds for moderate rain
                        cloud = WeatherProfile.CloudTypeEnum.Light;
                    }
                    else if (weather.precipIntensity <= 50)
                    {
                        cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    }
                    else
                    {
                        // TODO: we need storm clouds for violent rain
                        cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    }

                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Rain, cloud);
                    break;
                case "snow":
                    cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Snow, cloud); ;
                    break;
                case "sleet":
                    cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Sleet, cloud); ;
                    break;
                case "cloudy":
                    // TODO: don't always assume cloudy days need heavy cloud
                    cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Clear, cloud);
                    break;
                case "partly-cloudy-day":
                    // TODO: don't always assume partly cloudy days need light cloud
                    cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Clear, cloud);
                    break;
                case "partly-cloudy-night":
                    // TODO: don't always assume partly cloudy days need light cloud
                    cloud = WeatherProfile.CloudTypeEnum.Heavy;
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Clear, cloud);
                    break;
                /** FIXME: add remaining weather profiles
                case "wind":
                    newProfile = windBaseProfile;
                    break;
                case "fog":
                    newProfile = fogBaseProfile;
                    break;
                */
                default:
                    Debug.LogError("Don't have a base weather profile for Icon named " + weather.icon);
                    newProfile = new WeatherProfile(WeatherProfile.PrecipitationTypeEnum.Clear, WeatherProfile.CloudTypeEnum.Clear);
                    break;
            }
            
            newProfile.PrecipitationIntensity = weather.precipIntensity;
            // TODO: Use precipitation probability to vary the likelihood of rainfall
            // TODO: Use the precipitation error to vary the intensity of the rainfall
            newProfile.cloudIntensity = weather.cloudCover * 100;

            lastWeather = CurrentProfile;
            CurrentProfile = newProfile;

            if (delegateWeatherSystem != null)
            {
                delegateWeatherSystem.CurrentProfile = CurrentProfile;
            }
            Debug.Log("Weather report: " + CurrentProfile.ToString());
        }

        internal override void Initialize()
        {
            timeToNextUpdate = 0;

            if (delegateWeatherSystem != null)
            {
                delegateWeatherSystem.Initialize();
            }
        }

        internal override void Start()
        {
            apiRequestURL = string.Format(apiRequestTemplate, apiKey, latitude, longitude);

            if (delegateWeatherSystem != null)
            {
                delegateWeatherSystem.Start();
            }
        }

        internal override void Update()
        {
            Weather weather = JsonUtility.FromJson<Weather>(Get(apiRequestURL));
            SetWeather(weather);
            timeToNextUpdate = forecastUpdateFrequency;

            if (delegateWeatherSystem != null)
            {
                delegateWeatherSystem.Update();
            }
        }

        void SetWeather(Weather weather)
        {
            WeatherDataPoint data;
            switch (timeOffsetUnits)
            {
                case TimeOffsetType.Current:
                    //Debug.Log("Setting weather to data point Current");
                    data = weather.currently;
                    UpdateWeather(data);
                    break;
                case TimeOffsetType.Minutes:
                    // Sometimes the API fails to return the forecast data, if this happens use the current data
                    if (weather.minutely.data != null)
                    {
                        //Debug.Log("Setting weather to data point minute " + timeOffset);
                        data = weather.minutely.data[timeOffset];
                        data.summary = weather.minutely.summary;
                        data.icon = weather.minutely.icon;
                    }
                    else
                    {
                        //Debug.Log("Setting weather to data point Current because minutely forecast is unavailable");
                        data = weather.currently;
                    }

                    UpdateWeather(data);
                    break;
                case TimeOffsetType.Hours:
                    // Sometimes the API fails to return the forecast data, if this happens use the current data
                    if (weather.hourly != null)
                    {
                        //Debug.Log("Setting weather to data point hour " + timeOffset);
                        data = weather.hourly.data[timeOffset];
                    }
                    else
                    {
                        //Debug.Log("Setting weather to data point Current because hourly forecast is unavailable");
                        data = weather.currently;
                    }

                    UpdateWeather(data);
                    break;
                case TimeOffsetType.Days:
                    // Sometimes the API fails to return the forecast data, if this happens use the current data
                    if (weather.daily != null)
                    {
                        //Debug.Log("Setting weather to data point day " + timeOffset);
                        data = weather.daily.data[timeOffset];
                    }
                    else
                    {
                        //Debug.Log("Setting weather to data point Current because daily forecast us unavailable");
                        data = weather.currently;
                    }

                    UpdateWeather(data);
                    break;
                default:
                    Debug.LogError("Unknown time offset units, using Current.");
                    break;
            }
        }

        private string Get(string url)
        {
            Debug.Log("Getting DarkSky data with " + url);
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            try
            {
                WebResponse response = request.GetResponse();
                using (Stream responseStream = response.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(responseStream, System.Text.Encoding.UTF8);
                    return reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                Debug.LogError("Caught an error talking to DarkSky API: " + ex.Message);
                throw;
            }
        }
    }

    [Serializable]
    public class Weather
    {
        public double latitude;
        public double longitude;
        public string timezone;
        public int offset;

        public WeatherDataPoint currently;
        public WeatherDataBlock minutely;
        public WeatherDataBlock hourly;
        public WeatherDataBlock daily;
    }

    [Serializable]
    public class WeatherDataPoint
    {
        public int time;
        public string summary;
        public string icon;
        public float nearestStormDistance;
        public float nearestStormBearing;
        public float precipIntensity;
        public float precipIntensityError;
        public string precipType;
        public float precipProbability;
        public float temperature;
        public float apparentTemperature;
        public float dewPoint;
        public float humidity;
        public float pressure;
        public float windSpeed;
        public float windGust;
        public float windBearing;
        public float cloudCover;
        public float uvIndex;
        public float visibility;
        public float ozone;
    }

    [Serializable]
    public class WeatherDataBlock
    {
        public string icon;
        public string summary;
        public List<WeatherDataPoint> data;
    }
}
