using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum RelationshipStatus{
    low,
    neutral,
    high
}
public class NPCInteraction : MonoBehaviour
{
    public string npcName;
    public RelationshipStatus npcStatus;
    public GameObject questPopUpPrefab;
    public Canvas WorldCanvas;

    public QuestManager questManager;
    public GameObject greetingOptions;
    private List<Dialogue> GreetingOptionsDialogue = new List<Dialogue>();
    public List<Quest> quests = new List<Quest>();

    private Persistent persistenData;
    [HideInInspector]public GameObject questPopUp = null;
    private Quest activeQuest = null;

    public bool isMinigameNPC;
    public bool isObject;

    [HideInInspector] public Quest dayQuest;

    public Sprite exclamationSprite;
    public Sprite interrogationSprite;
    public Sprite asteriskSprite;
    public Sprite interactionSprite;


    private void Start()
    {

        //Adiciona dialogos na lista de dialogos desse npc
        persistenData = Persistent.current;
        if (!isObject)
        {
            foreach (Transform child in greetingOptions.transform)
            {
                GreetingOptionsDialogue.Add(child.GetComponent<Dialogue>());
            }

            
            if (persistenData.firstContactNPCs == null || !persistenData.firstContactNPCs.ContainsKey(npcName))
            {
                persistenData.firstContactNPCs.Add(npcName, true);
            }
        }
        CheckDayQuest();
        RefreshActiveQuestList();
        if(questPopUp == null)
            questPopUp = Instantiate(questPopUpPrefab, WorldCanvas.transform);
        questPopUp.gameObject.SetActive(true);
        UpdateQuestPopUp();
        SetQuestPopUpPosition();
        //npcStatus = quando o sistema de relacionamentos for implementado, pegar o valor dessa variavel que esta guardado na memoria.
    }


    //Acha a quest do dia atual e referencia os dialogos
    public void CheckDayQuest()
    {
        int currentDay = persistenData.currentDay;
        dayQuest = null;

        if (quests.Count <= 0)
            return;

        foreach (Quest quest in quests)
        {
            //print("procura");
            if (quest.questDay == currentDay)
            {
                dayQuest = quest;
                activeQuest = quest;
                if(questPopUp != null)
                    questPopUp.gameObject.SetActive(true);

                if (persistenData.completedQuests.Contains(quest.questName))
                {
                    print("completa");
                    dayQuest.completed = true;
                }
                else if (persistenData.activeQuests.Contains(quest.questName))
                {
                    print("progresso");
                    dayQuest.inProgress = true;
                }
                FillQuestDialogues(dayQuest);
                break;
            }
            else if (quest.questDay < currentDay && persistenData.activeQuests.Contains(quest.questName))
            {
                if(quest.questDay + quest.duration -1 < currentDay)
                {
                    quest.lost = true;
                    persistenData.lostQuests.Add(quest.questName);
                    persistenData.neglectedQuests.Add(quest.questName);
                    quest.inProgress = false;
                    persistenData.activeQuests.Remove(quest.questName);
                    persistenData.quantCharisma -= quest.charismaLost;
                    activeQuest = null;
                }
                
            } else if (quest.questDay < currentDay && !persistenData.neglectedQuests.Contains(quest.questName))
            {
                if (quest.questDay + quest.duration - 1 < currentDay)
                {
                    persistenData.neglectedQuests.Add(quest.questName);
                    persistenData.quantCharisma -= quest.charismaLost;
                    quest.inProgress = false;
                    quest.lost = false;
                    activeQuest = null;
                }
                
            }
        }
    }
    public void RefreshActiveQuestList()
    {
        if (dayQuest != null && persistenData.activeQuests.Contains(dayQuest.questName))
        {
            for (int i = 0; i < persistenData.activeQuestsUI.Count; i++)
            {
                Quest quest = persistenData.activeQuestsUI[i];
                if (quest.questName == dayQuest.questName)
                {
                    int index = persistenData.activeQuestsUI.IndexOf(quest);
                    persistenData.activeQuestsUI[index] = dayQuest;
                }
            }
        }
    }

    private bool CheckQuestCompleted(Quest quest)
    {
        if (persistenData.completedQuests.Contains(quest.questName))
        {
            print("completa");
            dayQuest = quest;
            dayQuest.completed = true;
            FillQuestDialogues(dayQuest);
            return true;
        }
        return false;
    }

    private void FillQuestDialogues(Quest quest)
    {
        dayQuest.startDialogue = dayQuest.questDialogues.transform.GetChild(0).GetComponent<Dialogue>();
        dayQuest.inProgressDialogue = dayQuest.questDialogues.transform.GetChild(1).GetComponent<Dialogue>();
        dayQuest.completedDialogue = dayQuest.questDialogues.transform.GetChild(2).GetComponent<Dialogue>();
    }

    //Define qual diálogo o npc usará
    public Dialogue Greet()
    {
        //print(dayQuest);
        if (dayQuest != null && (dayQuest.questName == null || dayQuest.questName == ""))
        {
            dayQuest = null;
            Debug.LogWarning("Quest sem nome definido!");
        }
        if (isMinigameNPC)
        {
            //print("MinigameDialogue");
            return minigameDialogue();
        }

        else if (dayQuest != null)
        {
            return questDialogue();
        }

        else
        {
            //print("normal");
            return moodDialogue();
        }
    }
    private Dialogue minigameDialogue()
    {
        bool firstContact;
        persistenData.firstContactNPCs.TryGetValue(npcName, out firstContact);
        if (firstContact)
        {
            persistenData.firstContactNPCs.Remove(npcName);
            persistenData.firstContactNPCs.Add(npcName, false);
            return GreetingOptionsDialogue[0];
        }
        else
            return GreetingOptionsDialogue[1];
    }

    private Dialogue questDialogue()
    {
        if (dayQuest.completed)
            return dayQuest.completedDialogue;
        else if (dayQuest.inProgress)
            return dayQuest.inProgressDialogue;
        else
            return dayQuest.startDialogue;
    }

    private Dialogue moodDialogue()
    {
        switch (npcStatus)
        {
            case RelationshipStatus.low:
                return GreetingOptionsDialogue[0];
            case RelationshipStatus.neutral:
                return GreetingOptionsDialogue[1];
            default:
                return GreetingOptionsDialogue[2];
        }
    }
    private void SetQuestPopUpPosition()
    {
        questPopUp.transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z + 1f);
        questPopUp.transform.Rotate(new Vector3(1f, 0f, 0f), 45);
    }
    public void SetInteractionButtonOn()
    {
        questPopUp.transform.GetChild(0).GetComponent<Button>().interactable = true;
        questPopUp.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void SetInteractionButtonOff()
    {
        questPopUp.transform.GetChild(0).GetComponent<Button>().interactable = false;
        questPopUp.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public void EnableInteractionButton()
    {
        questPopUp.transform.GetChild(0).gameObject.SetActive(true);
        questPopUp.GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
    }

    public void DisableInteractionButton()
    {
        questPopUp.transform.GetChild(0).gameObject.SetActive(false);
        questPopUp.GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.5f);
    }

    public Sprite GetQuestPopUpSprite(QuestType questType)
    {
        switch (questType)
        {
            case QuestType.Exclamatory:
                return exclamationSprite;
            case QuestType.Pondering:
                return interrogationSprite;
            default:
                return asteriskSprite;
        }
    }


    public void ChangeInteractionSprite()
    {
        questPopUp.GetComponent<Image>().sprite = interactionSprite;
    }
    public void UpdateQuestPopUp()
    {
        if (dayQuest == null && activeQuest == null)
        {
            if (isObject)
                questPopUp.SetActive(false);
            else
                questPopUp.GetComponent<Image>().sprite = interactionSprite;
        }
        else if(dayQuest == null && activeQuest != null)
        {
            questPopUp.GetComponent<Image>().sprite = GetQuestPopUpSprite(activeQuest.questType);
            Debug.Log("MudouSprite");
        }
        else
        {
            questPopUp.GetComponent<Image>().sprite = GetQuestPopUpSprite(dayQuest.questType);
            Debug.Log("MudouSprite");
        }
    }
}
