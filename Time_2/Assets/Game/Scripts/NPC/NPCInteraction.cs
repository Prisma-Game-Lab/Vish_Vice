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

    public GameObject greetingOptions;
    private List<Dialogue> GreetingOptionsDialogue = new List<Dialogue>();
    public List<Quest> quests = new List<Quest>();

    private Persistent persistenData;

    [HideInInspector] public Quest dayQuest;

    private void Start()
    {

        //Adiciona dialogos na lista de dialogos desse npc
        foreach (Transform child in greetingOptions.transform)
        {
            GreetingOptionsDialogue.Add(child.GetComponent<Dialogue>());
        }

        persistenData = Persistent.current;

        CheckDayQuest();

        //npcStatus = quando o sistema de relacionamentos for implementado, pegar o valor dessa variavel que esta guardado na memoria.
    }

    //Acha a quest do dia atual e referencia os dialogos
    public void CheckDayQuest()
    {
        int currentDay = persistenData.currentDay;
        dayQuest = null;
        if (quests.Count > 0)
        {
            foreach (Quest quest in quests)
            {
                if (quest.questDay == currentDay)
                {
                    dayQuest = quest;
                    foreach (Quest persistentQuest in persistenData.activeQuests)
                    {
                        if (quest.questName == persistentQuest.questName)
                        {
                            print("atribuiu");
                            dayQuest.inProgress = persistentQuest.inProgress;
                            dayQuest.completed = persistentQuest.completed;
                            break;
                        }
                    }
                    FillQuestDialogues(quest);

                    print(dayQuest.completed);
                    print(dayQuest.inProgress);
                    if (dayQuest.completed)
                    {
                        print("Completa e nulifica");
                        dayQuest = null;
                    }
                    break;
                }
            }
        }
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
        if (dayQuest != null && dayQuest.questName == null)
        {
            dayQuest = null;
            Debug.LogWarning("Quest sem nome definido!");
        }
        if (dayQuest != null)
        {
            print("dialogo quest");
            return questDialogue();
        }
        else
        {
            print("dialogo normal");
            return normalDialogue();
        }
    }

    private Dialogue questDialogue()
    {
        if (dayQuest.inProgress)
            return dayQuest.inProgressDialogue;
        else if (dayQuest.completed)
            return dayQuest.completedDialogue;
        else
            return dayQuest.startDialogue;
    }

    private Dialogue normalDialogue()
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
