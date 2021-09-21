using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ItemType
{
    Wood,
    Stone,
    Metal,
    Supplies
}
public class QuestManager : MonoBehaviour
{
    private UIMaster uiMaster;
    [HideInInspector] public NPCInteraction newQuestNPC;
    public List<Quest> activeQuests;

    private GameObject[] questsNpcs;
    private Persistent persistenData;

    private void Start()
    {
        questsNpcs = GameObject.FindGameObjectsWithTag("NPC");
        uiMaster = GetComponent<UIMaster>();
        persistenData = Persistent.current;
        if(persistenData.activeQuests!=null && persistenData.activeQuests.Count > 0)
        {
            activeQuests = persistenData.activeQuests;
        }
        if (activeQuests != null && activeQuests.Count > 0)
        {
            foreach (Quest quest in activeQuests)
            {
                CreateQuestUI(quest);
            }
        }
    }

    public void addQuest()
    {
        if (newQuestNPC.dayQuest != null)
        {
            print("Quest adicionada!");
            newQuestNPC.dayQuest.inProgress = true;
            Quest quest = newQuestNPC.dayQuest;
            activeQuests.Add(quest);
            CreateQuestUI(quest);

        }
    }

    public void CheckDayQuests()
    {
        foreach (GameObject npc in questsNpcs)
        {
            npc.GetComponent<NPCInteraction>().CheckDayQuest();
        }
    }

    private void CreateQuestUI(Quest quest)
    {
        GameObject questUI = Instantiate(uiMaster.questUI, uiMaster.allQuestsPanel.transform);
        questUI.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;
        Transform allQuestItens = questUI.gameObject.transform.GetChild(1).transform;
        foreach (Quest.Item item in quest.wantedItens)
        {
            GameObject questItem = Instantiate(uiMaster.questItemUI, allQuestItens.transform);
            questItem.GetComponentInChildren<Image>().sprite = GetItemSprite(item);
            questItem.GetComponentInChildren<TextMeshProUGUI>().text = item.quantity.ToString();
        }
    }

    private Sprite GetItemSprite(Quest.Item item)
    {
        switch (item.type)
        {
            case ItemType.Wood:
                return uiMaster.woodQuestIcon;
            case ItemType.Stone:
                return uiMaster.woodQuestIcon;
            case ItemType.Metal:
                return uiMaster.woodQuestIcon;
            case ItemType.Supplies:
                return uiMaster.woodQuestIcon;
            default:
                return uiMaster.woodQuestIcon;
        }
    }

    private void OnDestroy()
    {
        persistenData.activeQuests = activeQuests;
    }

}
