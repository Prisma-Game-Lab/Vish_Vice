using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityLightsController : MonoBehaviour
{
    [SerializeField] int lightsOnHour;
    [SerializeField] int lightsOnMinutes;
    [SerializeField] int lightsOffHour;
    [SerializeField] int lightsOffMinutes;

    DayCycle dayCycle;
    bool lightsTurned;

    public List<Light> poles;

    // Start is called before the first frame update
    void Start()
    {
        dayCycle = GetComponent<DayCycle>();
        lightsTurned = false;
        if ((dayCycle.hour <= 23 && dayCycle.hour >= 18) || (dayCycle.hour >= 0 && dayCycle.hour <= 5))
            StartLightsOn();
    }

    // Update is called once per frame
    void Update()
    {
        if (CheckLightsHour() && !lightsTurned)
        {
            CheckLightsMinutes();
        }
    }

    void StartLightsOn()
    {
        foreach (Light light in poles)
        {
            light.gameObject.SetActive(true);
        }
    }

    bool CheckLightsHour()
    {
        if (dayCycle.hour == lightsOnHour || dayCycle.hour == lightsOffHour)
            return true;
        if (lightsTurned)
            lightsTurned = false;
        return false;
    }

    void CheckLightsMinutes()
    {
        if (dayCycle.minutes == lightsOnMinutes)
        {
            StartCoroutine(LightsOn());
            lightsTurned = true;
        }
        else if (dayCycle.hour == lightsOffMinutes)
        {
            StartCoroutine(LightsOff());
            lightsTurned = true;
        }
    }

    IEnumerator LightsOn()
    {
        foreach (Light light in poles)
        {
            light.gameObject.SetActive(true);
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator LightsOff()
    {
        foreach (Light light in poles)
        {
            light.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.2f);
        }
    }
}
