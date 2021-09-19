using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

public class DayCycle : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float time;
    public float fullDayLenght;
    private float timeRate;
    public Vector3 noon;

    [Header("Sun")]
    public Light sun;
    public Gradient sunColor;
    public AnimationCurve sunIntensity;

    [Header("Moon")]
    public Light moon;
    public Gradient moonColor;
    public AnimationCurve moonIntensity;

    [Header("Lighting effects")]
    public AnimationCurve lightingIntensityMultiplier;
    public AnimationCurve reflectionsIntensityMultiplier;

    public TextMeshProUGUI time_txt;
    public TextMeshProUGUI day_txt;

    Persistent persistentData;
    [SerializeField] UnityEvent OnChangeDay;

    private void Start()
    {
        persistentData = Persistent.current;
        time = (persistentData.currentTime != 2) ? persistentData.currentTime : time;
        fullDayLenght = (persistentData.fullDayLength != 2) ? persistentData.fullDayLength : fullDayLenght;
        timeRate = 1.0f / fullDayLenght;
        day_txt.text = "Dia: " + persistentData.currentDay;
    }

    private void Update()
    {
        checkDay();
        calculateTime();
        lightController();
    }

    //Faz a passagem de dia
    void checkDay()
    {
        if (time <= 0)
        {
            persistentData.currentDay += 1;
            day_txt.text = "Dia: " + persistentData.currentDay;
            if (TryGetComponent(out QuestManager questManager))
                questManager.CheckDayQuests();
        }
    }

    void calculateTime()
    {
        //incrementa tempo
        time += timeRate * Time.deltaTime;
        if (time >= 1.0f)
            time = 0f;
        int hour = Mathf.FloorToInt(time * 24);
        int minutes = ((int)(((time * 24) % 1) * 6))*10;
        time_txt.text = hour.ToString("00") + ":" + minutes.ToString("00");
    }

    void lightController()
    {
        //rotação da luz
        sun.transform.eulerAngles = (time - 0.3f) * noon * 4.0f;
        moon.transform.eulerAngles = (time - 0.75f) * noon * 4.0f;

        //intensidade da luz
        sun.intensity = sunIntensity.Evaluate(time);
        moon.intensity = moonIntensity.Evaluate(time);

        //mudança de cor
        sun.color = sunColor.Evaluate(time);
        moon.color = moonColor.Evaluate(time);

        //ativa/desativa sol
        if (sun.intensity <= 0 && sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(false);
        else if (sun.intensity > 0 && !sun.gameObject.activeInHierarchy)
            sun.gameObject.SetActive(true);

        //ativa/desativa lua
        if (moon.intensity <= 0 && moon.gameObject.activeInHierarchy)
            moon.gameObject.SetActive(false);
        else if (moon.intensity > 0 && !moon.gameObject.activeInHierarchy)
            moon.gameObject.SetActive(true);

        //intensidade do reflexo da luz
        RenderSettings.ambientIntensity = lightingIntensityMultiplier.Evaluate(time);
        RenderSettings.reflectionIntensity = reflectionsIntensityMultiplier.Evaluate(time);
    }
    private void OnDestroy()
    {
        persistentData.currentTime = time;
        persistentData.fullDayLength = fullDayLenght;
    }
}
