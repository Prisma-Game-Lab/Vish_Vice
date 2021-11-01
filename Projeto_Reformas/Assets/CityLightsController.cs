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
    public List<MeshRenderer> polesMaterials;

    public Material lightOnMaterial;
    public Material lightOffMaterial;

    // Start is called before the first frame update
    void Start()
    {
        dayCycle = GetComponent<DayCycle>();
        lightsTurned = false;
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
        if (dayCycle.hour >= lightsOnHour || dayCycle.hour <= lightsOffHour)
        {
            if (dayCycle.hour == lightsOnHour && dayCycle.minutes <= lightsOnMinutes)
                return;
            else if (dayCycle.hour == lightsOffHour && dayCycle.minutes >= lightsOffMinutes)
                return;
            for (int i = 0; i < poles.Count; i++)
            {
                poles[i].gameObject.SetActive(true);
                polesMaterials[i].material = lightOnMaterial;
            }
        }
    }

    bool CheckLightsHour()
    {
        if (dayCycle.hour == lightsOnHour || dayCycle.hour == lightsOffHour)
            return true;
        if (lightsTurned)
        {
            lightsTurned = false;
        }
        return false;
    }

    void CheckLightsMinutes()
    {

        if (dayCycle.hour == lightsOnHour && dayCycle.minutes == lightsOnMinutes)
        {
            StartCoroutine(LightsOn());
            lightsTurned = true;
        }
        else if (dayCycle.hour == lightsOffHour && dayCycle.minutes == lightsOffMinutes)
        {
            StartCoroutine(LightsOff());
            lightsTurned = true;
        }
    }

    IEnumerator LightsOn()
    {
        for (int i = 0; i < poles.Count; i++)
        {
            poles[i].gameObject.SetActive(true);
            polesMaterials[i].material = lightOnMaterial;
            yield return new WaitForSeconds(0.2f);
        }
    }

    IEnumerator LightsOff()
    {
        for (int i = 0; i < poles.Count; i++)
        {
            poles[i].gameObject.SetActive(false);
            polesMaterials[i].material = lightOffMaterial;
            yield return new WaitForSeconds(0.2f);
        }
    }
}
