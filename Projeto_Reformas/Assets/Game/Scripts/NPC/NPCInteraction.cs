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
    public Button questPopUpPrefab;
    public Canvas WorldCanvas;

    public QuestManager questManager;
    public GameObject greetingOptions;
    private List<Dialogue> GreetingOptionsDialogue = new List<Dialogue>();
    public List<Quest> quests = new List<Quest>();

    private Persistent persistenData;
    private Button questPopUp = null;

    public bool isMinigameNPC;

    [HideInInspector] public Quest dayQuest;

    public Sprite exclamationSprite;
    public Sprite interrogationSprite;
    public Sprite asteriskSprite;
    public Sprite interactionSprite;


    private void Start()
    {

        //Adiciona dialogos na lista de dialogos desse npc
        foreach (Transform child in greetingOptions.transform)
        {
            GreetingOptionsDialogue.Add(child.GetComponent<Dialogue>());
        }

        persistenData = Persistent.current;
        if (persistenData.firstContactNPCs == null || !persistenData.firstContactNPCs.ContainsKey(npcName))
        {
            persistenData.firstContactNPCs.Add(npcName, true);
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
            print("procura");
            if (quest.questDay == currentDay)
            {
                dayQuest = quest;

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
                quest.lost = true;
                persistenData.lostQuests.Add(quest.questName);
                quest.inProgress = false;
                persistenData.activeQuests.Remove(quest.questName);

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
        questPopUp.transform.rotation = transform.rotation;
    }
    public void SetInteractionButtonOn()
    {
        questPopUp.interactable = true;
    }

    public void SetInteractionButtonOff()
    {
        questPopUp.interactable = false;
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

    public void UpdateQuestPopUp()
    {
        if (dayQuest == null)
            questPopUp.GetComponent<Image>().sprite = interactionSprite;
        else
        {
            questPopUp.GetComponent<Image>().sprite = GetQuestPopUpSprite(dayQuest.questType);
            Debug.Log("MudouSprite");
        }
    }
}
