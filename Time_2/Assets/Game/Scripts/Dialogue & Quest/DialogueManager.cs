using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{

    public GameObject Joystick;

    private Queue<string> _sentences;
    private UIMaster uiMaster;
    private QuestManager questManager;
    private bool hasQuest;


    void Start()
    {
        uiMaster = GetComponent<UIMaster>();
        questManager = GetComponent<QuestManager>();
        _sentences = new Queue<string>();
    }

    public void StartDialog(Dialogue dialogue)
    {
        print(dialogue);
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
        if (_sentences.Count == 0 && !hasQuest)
        {
            EndDialogue();
            return false;
        }

        if (_sentences.Count != 0)
        {
            string sentence = _sentences.Dequeue();
            uiMaster._displayText.text = sentence;
        }

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
        print("QuestAnswer");
        if (accept)
        {
            print("aceita");
            //comeca quest
            questManager.addQuest();
        }
        EndDialogue();
    }

}
