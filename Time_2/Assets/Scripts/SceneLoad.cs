using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject SettingsUI;
    public GameObject CreditsUI;

    public void LoadPlay()
    {
        SceneManager.LoadScene("Play");

    }

    public void LoadSettings() {
        MainMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }
    public void QuitSettings()
    {
        SettingsUI.SetActive(false);
        MainMenuUI.SetActive(true);   
    }

    public void LoadCredits() {
        MainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    public void QuitCredits()
    {
        CreditsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }


    
}
