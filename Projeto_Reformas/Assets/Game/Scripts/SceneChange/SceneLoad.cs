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
    public GameObject SaveUI;

    public void LoadPlay()
    {
        AudioManager.instance.Play("ButtonClick");
        GameObject.FindGameObjectWithTag("loadingScreen").GetComponent<LoadingScreen>().StartGame();

    }

    public void LoadSettings() {
        AudioManager.instance.Play("ButtonClick");
        SettingsUI.SetActive(true);
        SettingsUI.GetComponent<Animator>().Play("MainMenu Settings Open");
    }
    public void QuitSettings()
    {
        AudioManager.instance.Play("ButtonClick");
        SettingsUI.GetComponent<Animator>().Play("MainMenu Settings Close");
    }

    public void LoadCredits() {
        AudioManager.instance.Play("ButtonClick");
        MainMenuUI.SetActive(false);
        CreditsUI.SetActive(true);
        CreditsUI.GetComponent<Animator>().Play("MainMenu Settings Open");
    }

    public void QuitCredits()
    {
        AudioManager.instance.Play("ButtonClick");
        CreditsUI.GetComponent<Animator>().Play("MainMenu Settings Close");
        MainMenuUI.SetActive(true);
    }

    public void YesSave()
    {
        AudioManager.instance.Play("ButtonClick");
        Persistent.current.DeleteSave();
        Persistent.current.ResetPersistent();
        SaveUI.GetComponent<Animator>().Play("MainMenu Settings Close");
    }

    public void NoSave()
    {
        AudioManager.instance.Play("ButtonClick");
        SaveUI.GetComponent<Animator>().Play("MainMenu Settings Close");
    }

    public void OpenSave()
    {
        AudioManager.instance.Play("ButtonClick");
        SaveUI.SetActive(true);
        SaveUI.GetComponent<Animator>().Play("MainMenu Settings Open");
    }

}
