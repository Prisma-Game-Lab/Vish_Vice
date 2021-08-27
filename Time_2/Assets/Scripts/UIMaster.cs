using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class UIMaster : MonoBehaviour
{
    public GameObject PauseMenuUI;
    public GameObject SettingsUI;
    private bool _paused = false;

    public void Pause(){
        if (_paused)
        {
            PauseMenuUI.SetActive(false);
            _paused = false;
            Time.timeScale = 1f;
            return;
        }
        else
        {
            PauseMenuUI.SetActive(true);
            _paused = true;
            Time.timeScale = 0f;
        }
    }

    public void BackToGame()
    {
        PauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _paused = false;
    }

    public void GoToSettings()
    {
        PauseMenuUI.SetActive(false);
        SettingsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        PauseMenuUI.SetActive(true);
        SettingsUI.SetActive(false);
    }
    public void GoToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }

}
