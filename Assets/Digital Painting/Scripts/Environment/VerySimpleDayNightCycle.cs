using System;
using UnityEngine;

public class VerySimpleDayNightCycle : MonoBehaviour {
    [Header("Time of Day")]
    [Tooltip("Current time of day, in seconds.")]
    [Range(0, 86400)]
    public float currentTimeOfDay = 5 * 60 * 60; // (5 AM)
    [Tooltip("The speed at which a game day passes.")]
    public int secondsPerDay = 96;

    [Header("Sun settings")]
    [Tooltip("How bright should the sun be at mid day?")]
    public float maxIntensity = 1.1f;

    [Header("Overrides")]
    [Tooltip("The directional light that acts as the sun. If blank a light with the name `Sun` will be used.")]
    public Light sun;    

    private void Start()
    {
        if (sun == null)
        {
            InitializeSun();
        }
    }

    private void InitializeSun()
    {
        GameObject go = GameObject.Find("Sun");
        if (go == null)
        {
            go = new GameObject("Sun");
            sun = go.AddComponent<Light>();
            sun.type = LightType.Directional;
            sun.shadows = LightShadows.Soft;
        }
        else
        {
            sun = go.GetComponent<Light>();
        }
    }

    private void Update()
    {
        UpdateTime();
        UpdateSunPosition();
        UpdateSunIntensity();
    }

    public void UpdateTime()
    {
        currentTimeOfDay += Time.deltaTime * (86400 / secondsPerDay);
        if (currentTimeOfDay > 86400)
        {
            currentTimeOfDay = 0;
        }
    }

    private void UpdateSunIntensity()
    {
        float intensity;
        if (currentTimeOfDay < 43200)
        {
            intensity = 1 - (43200 - currentTimeOfDay) / 43200;
        }
        else
        {
            intensity = 1 - ((43200 - currentTimeOfDay) / 43200 * -1);
        }
        sun.intensity = intensity * maxIntensity;
    }

    private void UpdateSunPosition()
    {
        sun.transform.rotation = Quaternion.Euler(new Vector3((currentTimeOfDay - 21600) / 864000 * 360, 0, 0));
    }
}
