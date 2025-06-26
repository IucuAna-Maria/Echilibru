using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
using Newtonsoft.Json.Linq;

public class SpecificWeather : MonoBehaviour
{
    [Header("Weather Effects")]
    public GameObject rainParticles;
    public GameObject snowParticles;

    private string weatherApiKey = "ece1484bde0242320b3b4e4fe7cf9a03";
    private string targetCity = "Andorra la Vella";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(GetWeatherForCity(targetCity));
    }

    IEnumerator GetWeatherForCity(string city)
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

        Debug.Log($"Weather in {city}: {condition}");
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
