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
public enum MinigameType
{
    Wood,
    Concrete,
    Metal
}
public class QuestManager : MonoBehaviour
{
    private UIMaster uiMaster;
    [HideInInspector] public NPCInteraction newQuestNPC;

    private GameObject[] questsNpcs;
    private Persistent persistenData;

    private void Start()
    {
        questsNpcs = GameObject.FindGameObjectsWithTag("NPC");
        uiMaster = GetComponent<UIMaster>();
        persistenData = Persistent.current;

        if (persistenData.activeQuests != null && persistenData.activeQuests.Count > 0)
        {
            foreach (Quest quest in persistenData.activeQuestsUI)
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
            persistenData.activeQuests.Add(newQuestNPC.dayQuest.questName);
            persistenData.activeQuestsUI.Add(newQuestNPC.dayQuest);
            CreateQuestUI(newQuestNPC.dayQuest);
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
        persistenData.activeQuests.Remove(newQuestNPC.dayQuest.questName);
        foreach (Quest quest in persistenData.activeQuestsUI)
        {
            if (quest.questName == newQuestNPC.dayQuest.questName)
            {
                persistenData.activeQuestsUI.Remove(quest);
                break;
            }
        }
        newQuestNPC.dayQuest.completed = true;
        persistenData.completedQuests.Add(newQuestNPC.dayQuest.questName);

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
        uiMaster.woodText.text = "Wood: " + persistenData.quantWood.ToString();
        foreach (Transform questUI in uiMaster.allQuestsPanel.transform)
        {
            if (questUI.GetComponentInChildren<TextMeshProUGUI>().text == newQuestNPC.dayQuest.questName)
            {
                Destroy(questUI.gameObject);
            }
        }
    }

    public void ClearQuestsPanel()
    {
        foreach (Transform questUI in uiMaster.allQuestsPanel.transform)
        {
            Destroy(questUI.gameObject);
        }
    }

}