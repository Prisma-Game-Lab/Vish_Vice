using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public GameObject MainMenuUI;
    public GameObject SettingsUI;
    public GameObject CreditsUI;
    public GameObject LoadingUI;
    public void LoadPlay()
    {
        AudioManager.instance.Play("ButtonClick");
        GameObject.FindGameObjectWithTag("loadingScreen").GetComponent<LoadingScreen>().StartGame();

    }

    public void LoadSettings() {
        AudioManager.instance.Play("ButtonClick");
        MainMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }
    public void QuitSettings()
    {
        AudioManager.instance.Play("ButtonClick");
        SettingsUI.SetActive(false);
        MainMenuUI.SetActive(true);   
    }

    public void LoadCredits() {
        AudioManager.instance.Play("ButtonClick");
        MainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
    }

    public void QuitCredits()
    {
        AudioManager.instance.Play("ButtonClick");
        CreditsUI.SetActive(false);
        MainMenuUI.SetActive(true);
    }


    
}
