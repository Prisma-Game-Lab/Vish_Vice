using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public enum ItemType
{
    Wood,
    Concrete,
    Metal,
    Manpower
}
public enum MinigameType
{
    Wood,
    Concrete,
    Metal
}
public enum QuestType
{
    Exclamatory,
    Pondering,
    Complaint   
}
public class QuestManager : MonoBehaviour
{
    private UIMaster uiMaster;
    private Notebook notebook;
    [HideInInspector] public NPCInteraction newQuestNPC;

    private GameObject[] questsNpcs;
    private Persistent persistenData;

    private void Start()
    {
        questsNpcs = GameObject.FindGameObjectsWithTag("NPC");
        uiMaster = GetComponent<UIMaster>();
        notebook = GetComponent<Notebook>();
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
            //persistenData.activeQuests.Add(newQuestNPC.dayQuest.questName);
            persistenData.activeQuests.Insert(0, newQuestNPC.dayQuest.questName);//as ~quests novas sao sempre inseridas no comeco da lista, pois sao mais recentes
            persistenData.activeQuestsUI.Add(newQuestNPC.dayQuest);
            persistenData.allQuests.Add(newQuestNPC.dayQuest);
            CreateQuestUI(newQuestNPC.dayQuest);

        }
    }

    public void CheckDayQuests()
    {
        foreach (GameObject npc in questsNpcs)
        {
            npc.GetComponent<NPCInteraction>().CheckDayQuest();
            npc.GetComponent<NPCInteraction>().UpdateQuestPopUp();
        }
    }

    private void CreateQuestUI(Quest quest)
    {
        /*GameObject questUI = Instantiate(uiMaster.questUI, uiMaster.allQuestsPanel.transform);
        questUI.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;

        Transform allQuestItens = questUI.gameObject.transform.GetChild(1).transform;

        foreach (Quest.Item item in quest.wantedItens)
        {
            GameObject questItem = Instantiate(uiMaster.questItemUI, allQuestItens.transform);
            questItem.GetComponentInChildren<Image>().sprite = GetItemSprite(item);
            questItem.GetComponentInChildren<TextMeshProUGUI>().text = item.quantity.ToString();
        }*/
        Debug.Log("Cria botao");
        GameObject questButton = Instantiate(notebook.questButton, notebook.tasks.transform);
        GameObject questText = Instantiate(notebook.questLimit, notebook.tasks.transform);
        //questButton.GetComponentInChildren<TextMeshProUGUI>().text = quest.questName;
        questButton.SetActive(true);
        //questText.GetComponent<TextMeshProUGUI>().text = "Até 00h do dia " + quest.questDay.ToString();
        questText.SetActive(true);
    }

    public Sprite GetItemSprite(Quest.Item item)
    {
        switch (item.type)
        {
            case ItemType.Wood:
                return uiMaster.woodQuestIcon;
            case ItemType.Concrete:
                return uiMaster.concreteQuestIcon;
            case ItemType.Metal:
                return uiMaster.metalQuestIcon;
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
        //UpdateUI();
        persistenData.activeQuests.Remove(newQuestNPC.dayQuest.questName);
        foreach (Quest quest in persistenData.activeQuestsUI)
        {
            if (quest.questName == newQuestNPC.dayQuest.questName)
            {
                quest.activateObject.SetActive(true);
                Persistent.current.objectState[quest.activateObject.transform.GetSiblingIndex()] = true;
                quest.desactivateObject.SetActive(false);
                Persistent.current.objectState[quest.desactivateObject.transform.GetSiblingIndex()] = false;
                persistenData.activeQuestsUI.Remove(quest);
                break;
            }
        }
        newQuestNPC.dayQuest.completed = true;
        persistenData.completedQuests.Add(newQuestNPC.dayQuest.questName);

        if(persistenData.quantManpower < 4)
        persistenData.quantManpower += 1;

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
            case ItemType.Manpower:
                return item.quantity <= persistenData.quantManpower;
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
                case ItemType.Manpower:
                    persistenData.quantManpower -= item.quantity;
                    persistenData.usedManpower += item.quantity;
                    break;
            }
        }
    }

    private void UpdateUI()
    {
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

    public Quest FindQuestInfo(string questName)
    {
        foreach (Quest quest in persistenData.allQuests)
        {
            if (quest.questName == questName)
            {
                return quest;
            }
        }

        return null;
    }

}