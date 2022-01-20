using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Language
{
    Portuguese,
    English
}
public class LanguageManager : MonoBehaviour
{
    private static string _languagePrefs = "LanguagePrefs";
    public Language activeLanguage;
    public static LanguageManager instance;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);


        if (PlayerPrefs.GetInt("FirstGame") != 0)
        {
            activeLanguage = GetLanguage();

        }
        else
        {
            activeLanguage = Language.Portuguese;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLanguage(Language language)
    {
        int languageValue = -1;
        switch (language)
        {
            case Language.Portuguese:
                activeLanguage = Language.Portuguese;
                languageValue = 0;
                break;
            case Language.English:
                activeLanguage = Language.English;
                languageValue = 1;
                break;
        }
        if(languageValue != -1)
            PlayerPrefs.SetInt(_languagePrefs, languageValue);
    }

    public Language GetLanguage()
    {
        int language =  PlayerPrefs.GetInt("LanguagePrefs");
        switch (language)
        {
            case 0:
                return Language.Portuguese;
            default:
                return Language.English;
        }
    }

}
