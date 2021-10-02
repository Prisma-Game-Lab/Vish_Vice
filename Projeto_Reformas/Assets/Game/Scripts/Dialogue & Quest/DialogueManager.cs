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
    private bool questInProgress;
    private bool firstContact = false;


    void Start()
    {
        uiMaster = GetComponent<UIMaster>();
        questManager = GetComponent<QuestManager>();
        _sentences = new Queue<string>();
    }

    public void StartDialog(Dialogue dialogue)
    {
        firstContact = true;
        //print(dialogue);
        foreach (string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        hasQuest = dialogue.isQuest;
        questInProgress = dialogue.questInProgress;
        ResetDialoguePanel();
        DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        if (_sentences.Count == 0 && !hasQuest && !questInProgress)
        {
            EndDialogue();
            return false;
        }

        if (_sentences.Count != 0)
        {
            string sentence = _sentences.Dequeue();
            StartCoroutine(TextWritingEffect(sentence));
            firstContact = false;
        }

        if (hasQuest && _sentences.Count == 0)
        {
            uiMaster.acceptQuestButton.SetActive(true);
            uiMaster.declineQuestButton.SetActive(true);
            uiMaster._touchToContinue.SetActive(false);
            return false;
        }

        if (questInProgress && _sentences.Count == 0)
        {
            uiMaster.completeQuestButton.SetActive(true);
            uiMaster.outQuestButton.SetActive(true);
            uiMaster._touchToContinue.SetActive(false);
            return false;
        }
        return true;
    }


    IEnumerator TextWritingEffect(string sentence)
    {
        float firstContactDelay = 0.3f;
        float lettersRate = 0.01f;
        if (firstContact)
            yield return new WaitForSeconds(firstContactDelay);
        for (int i = 0; i <= sentence.Length; i++)
        {
            uiMaster._displayText.text = sentence.Substring(0, i);
            yield return new WaitForSeconds(lettersRate);
        }
    }

    public void EndDialogue()
    {
        uiMaster.interactButton.SetActive(true);
        uiMaster.dialogPanel.SetActive(false);
        Joystick.SetActive(true);
    }

    private void ResetDialoguePanel()
    {
        uiMaster.acceptQuestButton.SetActive(false);
        uiMaster.declineQuestButton.SetActive(false);
        uiMaster.completeQuestButton.SetActive(false);
        uiMaster.outQuestButton.SetActive(false);
        uiMaster._touchToContinue.SetActive(true);
        uiMaster._displayText.text = "";
    }

    public void QuestAnswer(bool accept)
    {
        if (accept)
        {
            print("aceita");
            //comeca quest
            questManager.addQuest();
        }
        EndDialogue();
    }

    public void CompleteQuestAnswer ()
    {
        bool completed = questManager.CompleteQuest();
        print("Quest completa? " + completed.ToString());
        EndDialogue();
    }

}
