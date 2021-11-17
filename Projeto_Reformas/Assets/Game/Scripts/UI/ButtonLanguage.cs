using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonLanguage : MonoBehaviour
{
    //[TextArea]
    public string textPT;
    public string textEN;
    private Text text;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            switch (LanguageManager.instance.activeLanguage)
            {
                case Language.Portuguese:
                    text.text = textPT;
                    break;
                default:
                    text.text = textEN;
                    break;
            }
        }
    }
}
