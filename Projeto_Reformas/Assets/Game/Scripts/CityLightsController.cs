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

    public List<GameObject> poles;
    private List<Light> polesLights;
    private List<MeshRenderer> polesMaterials;

    public Material lightOnMaterial;
    public Material lightOffMaterial;

    // Start is called before the first frame update
    void Start()
    {
        dayCycle = GetComponent<DayCycle>();
        lightsTurned = false;
        polesMaterials = new List<MeshRenderer>();
        polesLights = new List<Light>();
        for (int i = 0; i < poles.Count; i++)
        {
            if (poles[i] != null)
            {
                polesMaterials.Add(poles[i].gameObject.transform.GetChild(2).GetComponent<MeshRenderer>());
                polesLights.Add(poles[i].gameObject.transform.GetChild(3).GetComponent<Light>());
            }
        }
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
            for (int i = 0; i < polesLights.Count; i++)
            {
                if (polesLights[i] != null && polesMaterials[i]!= null)
                {
                    polesLights[i].gameObject.SetActive(true);
                    polesMaterials[i].material = lightOnMaterial;
                }
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
        print(polesLights.Count);
        for (int i = 0; i < poles.Count; i++)
        {
            if (polesLights[i] != null && polesMaterials[i] != null)
            {
                polesLights[i].gameObject.SetActive(true);
                polesMaterials[i].material = lightOnMaterial;
                yield return new WaitForSeconds(0.15f);
            }
        }
    }

    IEnumerator LightsOff()
    {
        for (int i = 0; i < poles.Count; i++)
        {
            if (polesLights[i] != null && polesMaterials[i] != null)
            {
                polesLights[i].gameObject.SetActive(false);
                polesMaterials[i].material = lightOffMaterial;
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}
