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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        GameObject.FindGameObjectWithTag("persistentData").GetComponent<Persistent>().quantWood += slice.woodCount;
        SceneManager.LoadScene("Play");
    }
}
