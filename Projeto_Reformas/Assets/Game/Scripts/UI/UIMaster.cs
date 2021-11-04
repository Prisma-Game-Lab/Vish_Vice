using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class UIMaster : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject settingsUI;
    public GameObject interactButton;
    public GameObject player;
    public GameObject dialogPanel;
    public GameObject allQuestsPanel;
    public GameObject questUI;
    public GameObject questItemUI;
    public Sprite woodQuestIcon;
    public Sprite metalQuestIcon;
    public Sprite concreteQuestIcon;
    private bool intActive = false;
    private bool _paused = false;

    [HideInInspector] public Text _displayName;
    [HideInInspector] public Text _displayText;
    [HideInInspector] public GameObject _touchToContinue;
    [HideInInspector] public GameObject acceptQuestButton;
    [HideInInspector] public GameObject declineQuestButton;
    [HideInInspector] public GameObject completeQuestButton;
    [HideInInspector] public GameObject outQuestButton;
    [HideInInspector] public GameObject WoodMinigameButton;
    [HideInInspector] public GameObject ConcreteMinigameButton;
    [HideInInspector] public GameObject MetalMinigameButton;


    private void Start()
    {
        Persistent persistentData = GameObject.FindGameObjectWithTag("persistentData").GetComponent<Persistent>();
        Transform dialoguePanelChild = dialogPanel.transform.GetChild(0);
        _displayText = dialoguePanelChild.transform.GetChild(0).GetComponent<Text>();
        _displayName = dialoguePanelChild.transform.GetChild(1).GetComponent<Text>();
        _touchToContinue = dialoguePanelChild.transform.GetChild(2).gameObject;
        acceptQuestButton = dialoguePanelChild.transform.GetChild(3).gameObject;
        declineQuestButton = dialoguePanelChild.transform.GetChild(4).gameObject;
        completeQuestButton = dialoguePanelChild.transform.GetChild(5).gameObject;
        outQuestButton = dialoguePanelChild.transform.GetChild(6).gameObject;
        WoodMinigameButton=dialoguePanelChild.transform.GetChild(7).gameObject;
        ConcreteMinigameButton= dialoguePanelChild.transform.GetChild(8).gameObject;
        MetalMinigameButton = dialoguePanelChild.transform.GetChild(9).gameObject;
        interactButton.SetActive(false);
    }

    public void Pause(){
        AudioManager.instance.Play("ButtonClick");
        if (_paused)
        {
            pauseMenuUI.SetActive(false);
            _paused = false;
            Time.timeScale = 1f;
            return;
        }
        else
        {
            pauseMenuUI.SetActive(true);
            _paused = true;
            Time.timeScale = 0f;
        }
    }

    public void BackToGame()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        _paused = false;
    }

    public void GoToSettings()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseMenuUI.SetActive(false);
        settingsUI.SetActive(true);
    }

    public void BackToPauseMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        pauseMenuUI.SetActive(true);
        settingsUI.SetActive(false);
    }
    public void GoToMainMenu()
    {
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1f;
    }
    
    public void InteractRender()
    {
        AudioManager.instance.Play("ButtonClick");
        intActive = !intActive;
        interactButton.SetActive(intActive);
    }

    public void InteractButtonAction()
    {
        AudioManager.instance.Play("ButtonClick");
        player.GetComponent<MakeInteraction>().GreetNpc();
    }
}
