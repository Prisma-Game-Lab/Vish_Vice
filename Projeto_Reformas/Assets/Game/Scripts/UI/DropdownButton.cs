using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DropdownButton : MonoBehaviour
{
    private Dropdown dropdown;
    public string[] options;
    // Start is called before the first frame update
    void Start()
    {
        dropdown = GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
        ChangeLanguageOptions();
    }

    public void ChangeLanguage()
    {
        switch (dropdown.value)
        {
            case 1:
                LanguageManager.instance.activeLanguage = Language.English;
                dropdown.captionText.text = "ENGLISH";
                break;
            default:
                LanguageManager.instance.activeLanguage = Language.Portuguese;
                dropdown.captionText.text = "PORTUGUÊS";
                break;
        }
        
    }

    public void ChangeLanguageOptions()
    {
        switch (LanguageManager.instance.activeLanguage)
        {
            case Language.Portuguese:
                dropdown.options[0].text = options[0];
                dropdown.options[1].text = options[1];
                break;
            case Language.English:
                dropdown.options[0].text = options[2];
                dropdown.options[1].text = options[3];
                break;
        }
    }
}
