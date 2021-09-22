using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ItemType
{
    Wood,
    Concrete,
    Metal
}
public class QuestManager : MonoBehaviour
{
    private UIMaster uiMaster;
    [HideInInspector] public NPCInteraction newQuestNPC;
    public List<Quest> activeQuests;
    public List<Quest> completedQuests;

    private GameObject[] questsNpcs;
    private Persistent persistenData;

    private void Start()
    {
        questsNpcs = GameObject.FindGameObjectsWithTag("NPC");
        uiMaster = GetComponent<UIMaster>();
        persistenData = Persistent.current;
        if (persistenData.activeQuests != null && persistenData.activeQuests.Count > 0)
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
            newQuestNPC.dayQuest.npc = newQuestNPC;
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
            case ItemType.Concrete:
                return uiMaster.woodQuestIcon;
            case ItemType.Metal:
                return uiMaster.woodQuestIcon;
            default:
                return uiMaster.woodQuestIcon;
        }
    }

    public bool CompleteQuest()
    {
        if (newQuestNPC.dayQuest == null)
            return false;

        bool hasResources;
        foreach (Quest.Item item in newQuestNPC.dayQuest.wantedItens)
        {
            hasResources = CheckItemQuantity(item);
            if (!hasResources)
                return false;
        }
        ConsumeItems(newQuestNPC.dayQuest);
        UpdateUI();
        activeQuests.Remove(newQuestNPC.dayQuest);
        newQuestNPC.dayQuest.completed = true;
        completedQuests.Add(newQuestNPC.dayQuest);
        newQuestNPC.dayQuest = null;
        
        return true;
    }

    private bool CheckItemQuantity(Quest.Item item)
    {
        switch (item.type)
        {
            case ItemType.Wood:
                return item.quantity <= persistenData.quantWood;
            case ItemType.Concrete:
                return item.quantity <= persistenData.quantConcrete;
            case ItemType.Metal:
                return item.quantity <= persistenData.quantMetal;
            default:
                return false;
        }
    }
    private void ConsumeItems(Quest quest)
    {
        foreach (Quest.Item item in quest.wantedItens)
        {
            switch (item.type)
            {
                case ItemType.Wood:
                    persistenData.quantWood -= item.quantity;
                    break;
                case ItemType.Concrete:
                    persistenData.quantConcrete -= item.quantity;
                    break;
                case ItemType.Metal:
                    persistenData.quantMetal -= item.quantity;
                    break;
            }
        }
    }

    private void UpdateUI()
    {
        uiMaster.woodText.text = "Wood: "+persistenData.quantWood.ToString();
    }

    private void OnDestroy()
    {
        persistenData.activeQuests = activeQuests;
        persistenData.completedQuests = completedQuests;
    }


}