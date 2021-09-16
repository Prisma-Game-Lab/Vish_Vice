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
    public GameObject InteractButton;
    public GameObject Player;
    public GameObject DialogPanel;
    private bool intActive = false;
    private bool _paused = false;

    [HideInInspector] public Text _displayName;
    [HideInInspector] public Text _displayText;
    [HideInInspector] public GameObject _touchToContinue;
    [HideInInspector] public GameObject acceptQuestButton;
    [HideInInspector] public GameObject declineQuestButton;

    private void Start()
    {
        _displayText = DialogPanel.transform.GetChild(0).GetComponent<Text>();
        _displayName = DialogPanel.transform.GetChild(1).GetComponent<Text>();
        _touchToContinue = DialogPanel.transform.GetChild(2).gameObject;
        acceptQuestButton = DialogPanel.transform.GetChild(3).gameObject;
        declineQuestButton = DialogPanel.transform.GetChild(4).gameObject;
        InteractButton.SetActive(false);
    }

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
    
    public void InteractRender()
    {
        intActive = !intActive;
        InteractButton.SetActive(intActive);
    }

    public void InteractButtonAction()
    {
        Player.GetComponent<MakeInteraction>().GreetNpc();
    }
}
