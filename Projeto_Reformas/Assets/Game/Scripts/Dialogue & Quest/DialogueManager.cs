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
    private bool hasMinigame;
    private bool hasQuest;
    private bool questInProgress;
    private bool firstContact = false;
    private MinigameType minigameType;


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
        hasMinigame = dialogue.hasMinigame;
        minigameType = dialogue.minigameType;
        ResetDialoguePanel();
        DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        AudioManager.instance.StopAllEffectsSounds();
        if (_sentences.Count == 0 && !hasQuest && !questInProgress && !hasMinigame)
        {
            EndDialogue();
            return false;
        }

        if (_sentences.Count != 0)
        {
            int randomNumber = Random.Range(0, 2);
            if (randomNumber == 1)
                AudioManager.instance.Play("Voz1");
            else
                AudioManager.instance.Play("Voz2");

            StopAllCoroutines();
            string sentence = _sentences.Dequeue();
            StartCoroutine(TextWritingEffect(sentence));
            firstContact = false;
        }
        if(hasMinigame && _sentences.Count == 0)
        {
            TurnMinigameButtonOn();
            return false;
        }

        if (hasQuest && _sentences.Count == 0)
        {
            TurnButtonsOn(uiMaster.acceptQuestButton, uiMaster.declineQuestButton, uiMaster._touchToContinue);
            return false;
        }

        if (questInProgress && _sentences.Count == 0)
        {
            TurnButtonsOn(uiMaster.completeQuestButton, uiMaster.outQuestButton, uiMaster._touchToContinue);
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

    private void TurnMinigameButtonOn()
    {
        switch (minigameType)
        {
            case MinigameType.Wood:
                TurnButtonsOn(uiMaster.WoodMinigameButton, uiMaster.outQuestButton, uiMaster._touchToContinue);
                break;
            case MinigameType.Concrete:
                TurnButtonsOn(uiMaster.ConcreteMinigameButton, uiMaster.outQuestButton, uiMaster._touchToContinue);
                break;
            case MinigameType.Metal:
                TurnButtonsOn(uiMaster.MetalMinigameButton, uiMaster.outQuestButton, uiMaster._touchToContinue);
                break;
        }
    }

    private void TurnButtonsOn(GameObject buttonOn1=null, GameObject buttonOn2=null, GameObject buttonOff = null)
    {
        if (buttonOn1 != null)
            buttonOn1.SetActive(true);
        if (buttonOn2 != null)
            buttonOn2.SetActive(true);
        if (buttonOff != null)
            buttonOff.SetActive(false);
    }

    private void ResetDialoguePanel()
    {
        uiMaster.acceptQuestButton.SetActive(false);
        uiMaster.declineQuestButton.SetActive(false);
        uiMaster.completeQuestButton.SetActive(false);
        uiMaster.outQuestButton.SetActive(false);
        uiMaster.WoodMinigameButton.SetActive(false);
        uiMaster.ConcreteMinigameButton.SetActive(false);
        uiMaster.MetalMinigameButton.SetActive(false);
        uiMaster._touchToContinue.SetActive(true);
        uiMaster._displayText.text = "";
    }

    public void QuestAnswer(bool accept)
    {
        if (accept)
        {
            //print("aceita");
            //comeca quest
            questManager.addQuest();
        }
        EndDialogue();
    }

    public void CompleteQuestAnswer ()
    {
        bool completed = questManager.CompleteQuest();
        //print("Quest completa? " + completed.ToString());
        EndDialogue();
    }

}
