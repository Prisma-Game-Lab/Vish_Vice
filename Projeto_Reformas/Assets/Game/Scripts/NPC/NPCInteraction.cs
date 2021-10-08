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

    public QuestManager questManager;
    public GameObject greetingOptions;
    private List<Dialogue> GreetingOptionsDialogue = new List<Dialogue>();
    public List<Quest> quests = new List<Quest>();

    private Persistent persistenData;

    public bool isMinigameNPC;

    [HideInInspector] public Quest dayQuest;

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

}
