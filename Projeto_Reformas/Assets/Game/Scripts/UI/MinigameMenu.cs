using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MinigameMenu : MonoBehaviour
{
    public GameObject pauseButton;
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public Slice slice;



    public void MinigamePause()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
    }

    public void MinigameUnpause()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void MinigameSettings()
    {
        AudioManager.instance.Play("ButtonClick");
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void MinigameCloseSettings()
    {
        AudioManager.instance.Play("ButtonClick");
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void MinigameEnd()
    {
        AudioManager.instance.Play("ButtonClick");
        Time.timeScale = 1f;
        Persistent.current.quantWood += slice.woodCount;
        //Persistent.current.startTime
        SceneManager.LoadScene("Play");
    }
    public void MinigameRestart()
    {
        AudioManager.instance.Play("ButtonClick");
        Time.timeScale = 1f;
        Persistent.current.quantWood += slice.woodCount;
        SceneManager.LoadScene("WoodGame");
    }
}
