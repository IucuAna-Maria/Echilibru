using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class LocationBasedWeather : MonoBehaviour
{
    [Header("Weather Effects")]
    public GameObject rainParticles;
    public GameObject snowParticles;

    private string weatherApiKey = "ece1484bde0242320b3b4e4fe7cf9a03";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetUserLocation());
        
        // Update weather every 2 hours
        //InvokeRepeating(nameof(RefreshWeather), 7200f, 7200f);
    }

    void RefreshWeather()
    {
        StartCoroutine(GetUserLocation());
    }

    IEnumerator GetUserLocation()
    {
        UnityWebRequest request = UnityWebRequest.Get("https://ipapi.co/json");
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Location couldn't be obtained: " + request.error);
            yield break;
        }

        JObject locationData = JObject.Parse(request.downloadHandler.text);
        string city = locationData["city"]?.ToString();

        if (string.IsNullOrEmpty(city))
        {
            Debug.LogWarning("City couldn't be identified.");
            yield break;
        }

        Debug.Log("City detected: " + city);
        StartCoroutine(GetWeather(city));
    }

    IEnumerator GetWeather(string city)
    {
        string url = $"https://api.openweathermap.org/data/2.5/weather?q={city}&appid={weatherApiKey}";
        UnityWebRequest request = UnityWebRequest.Get(url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogWarning("Couldn't get weather: " + request.error);
            yield break;
        }

        JObject weatherData = JObject.Parse(request.downloadHandler.text);
        string condition = weatherData["weather"]?[0]?["main"]?.ToString();

        Debug.Log("Weather detected: " + condition);
        UpdateWeather(condition);
    }

    void UpdateWeather(string condition)
    {
        bool rain = condition == "Rain";
        bool snow = condition == "Snow";

        if (rainParticles != null) rainParticles.SetActive(rain);
        if (snowParticles != null) snowParticles.SetActive(snow);

        if (!rain && !snow)
        {
            Debug.Log("Clear weather.");
        }
    }
}
