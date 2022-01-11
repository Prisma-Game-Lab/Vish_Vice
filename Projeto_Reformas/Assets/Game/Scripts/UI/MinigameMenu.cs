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
    public MixConcrete mix;

    public Sprite woodQuestIcon;
    public Sprite metalQuestIcon;
    public Sprite concreteQuestIcon;

    [HideInInspector] public bool paused = false;


    public void MinigamePause()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseButton.SetActive(false);
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void MinigameUnpause()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseButton.SetActive(true);
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        paused = false;
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
        if (SceneManager.GetActiveScene().name == "WoodGame")
            Persistent.current.quantWood += slice.woodCount;
        else if (SceneManager.GetActiveScene().name == "MetalGame")
        {
            Debug.Log("Sai do game do metal");
            Debug.Log(Persistent.current.earnedMetalQtd);
            Persistent.current.quantMetal += Persistent.current.earnedMetalQtd;
            Persistent.current.earnedMetalQtd = 0;
        }
        else if (SceneManager.GetActiveScene().name == "ConcreteGame")
        {
            Persistent.current.quantConcrete += mix.totalConcrete;
        }
            
        SceneManager.LoadScene("Play");
    }
    public void MinigameRestart()
    {
        AudioManager.instance.Play("ButtonClick");
        Time.timeScale = 1f;
        if (SceneManager.GetActiveScene().name == "WoodGame")
        {
            Persistent.current.quantWood += slice.woodCount;
            SceneManager.LoadScene("WoodGame");
        }   
        else if (SceneManager.GetActiveScene().name == "MetalGame")
        {
            Persistent.current.quantMetal += Persistent.current.earnedMetalQtd;
            Persistent.current.earnedMetalQtd = 0;
            SceneManager.LoadScene("MetalGame");
        }
        else if (SceneManager.GetActiveScene().name == "ConcreteGame")
        {
            Persistent.current.quantConcrete += mix.totalConcrete;
            SceneManager.LoadScene("ConcreteGame");
        }
        paused = false;


    }
}
