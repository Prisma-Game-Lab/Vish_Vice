using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    Wood,
    Stone,
    Iron,
    Supplies
}
public class QuestManager : MonoBehaviour
{
    private UIMaster uiMaster;
    [HideInInspector] public NPCInteraction newQuestNPC;
    public List<Quest> activeQuests;

    private GameObject[] questsNpcs;


    private void Start()
    {
        questsNpcs = GameObject.FindGameObjectsWithTag("NPC");
        uiMaster = GetComponent<UIMaster>();
    }

    public void addQuest()
    {
        if (newQuestNPC.dayQuest != null)
        {
            print("Quest adicionada!");
            Quest quest = newQuestNPC.dayQuest;
            activeQuests.Add(quest);
            newQuestNPC.dayQuest.inProgress = true;
        }
    }

    public void CheckDayQuests()
    {
        foreach  (GameObject npc in questsNpcs)
        {
            npc.GetComponent<NPCInteraction>().CheckDayQuest();
        }
    }

}
