using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject Joystick;

    private Queue<string> _sentences;
    private UIMaster uiMaster;
    private bool hasQuest;


    void Start()
    {
        uiMaster = GetComponent<UIMaster>();
        _sentences = new Queue<string>();

    }

    public void StartDialog(Dialogue dialogue)
    {
        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        hasQuest = dialogue.isQuest;
        ResetDialoguePanel();
        DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        if (_sentences.Count == 0)
        {
            EndDialogue();
            return false;
        }

        string sentence = _sentences.Dequeue();
        uiMaster._displayText.text = sentence;

        if (hasQuest && _sentences.Count == 0)
        {
            uiMaster.acceptQuestButton.SetActive(true);
            uiMaster.declineQuestButton.SetActive(true);
            uiMaster._touchToContinue.SetActive(false);
            return false;
        }
        return true;
    }

    public void EndDialogue()
    {
        uiMaster.InteractButton.SetActive(true);
        uiMaster.DialogPanel.SetActive(false);
        Joystick.SetActive(true);
    }

    private void ResetDialoguePanel()
    {
        uiMaster.acceptQuestButton.SetActive(false);
        uiMaster.declineQuestButton.SetActive(false);
        uiMaster._touchToContinue.SetActive(true);
    }

    public void QuestAnswer(bool accept)
    {
        if (accept)
        {
            //comeca quest
            //falta implementar um sistema de quests
        }
        else
        {
            EndDialogue();
        }
    }

}
