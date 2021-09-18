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
    
    
    [HideInInspector] public Quest dayQuest;
    
    private void Start()
    {
        //Adiciona dialogos na lista de dialogos
        foreach (Transform child in greetingOptions.transform)
        {
            GreetingOptionsDialogue.Add(child.GetComponent<Dialogue>());
        }
        dayQuest = null;
        //Acha a quest do dia atual se ja nao estiver iniciado e referencia os dialogos
        int currentDay = GameObject.FindGameObjectWithTag("persistentData").GetComponent<Persistent>().currentDay;
        if (quests.Count > 0)
        {
            foreach (Quest quest in quests)
            {
                if (quest.questDay == currentDay)
                {
                    dayQuest = quest;
                    dayQuest.startDialogue = dayQuest.questDialogues.transform.GetChild(0).GetComponent<Dialogue>();
                    dayQuest.inProgressDialogue = dayQuest.questDialogues.transform.GetChild(1).GetComponent<Dialogue>();
                    dayQuest.completedDialogue = dayQuest.questDialogues.transform.GetChild(2).GetComponent<Dialogue>();
                    break;
                }
            }
        }
        print(quests.Count);
        print(dayQuest);
        if (dayQuest != null)
            print(dayQuest.inProgress);
        //npcStatus = quando o sistema de relacionamentos for implementado, pegar o valor dessa variavel que esta guardado na memoria.
    }

    public Dialogue Greet()
    {
        if (dayQuest!=null && dayQuest.questName == null)
        {
            dayQuest = null;
            Debug.LogWarning("Quest sem nome definido!");
        }
        if (dayQuest != null)
        {
            print("dialogo quest");
            if (dayQuest.inProgress)
                return dayQuest.inProgressDialogue;
            else if (dayQuest.completed)
                return dayQuest.completedDialogue;
            else
                return dayQuest.startDialogue;
        }
        else
        {
            print("dialogo normal");
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
}
