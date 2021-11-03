using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notebook : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI metalText;
    public TextMeshProUGUI concreteText;
    public TextMeshProUGUI mdoText;
    public GameObject notebookUI;
    public GameObject questButton;
    public GameObject tasks;
    public ScrollRect scrollRectTasks;
    public GameObject taskExpansionPanel;
    public GameObject questResource;
    public GameObject scrollBarButtons;
    public List<GameObject> resources;
    private QuestManager questManager;
    // Start is called before the first frame update
    void Start()
    {
        questManager = GetComponent<QuestManager>();
    }   

    // Update is called once per frame
    void Update()
    {
        woodText.text = Persistent.current.quantWood.ToString();
        metalText.text = Persistent.current.quantMetal.ToString();
        concreteText.text = Persistent.current.quantConcrete.ToString();
    }

    public void CloseNotebook()
    {
        notebookUI.SetActive(false);
        scrollBarButtons.SetActive(false);
        taskExpansionPanel.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenNotebook()
    {
        notebookUI.SetActive(true);
        OrderTaskList();
        if (tasks.transform.childCount >= 6)
            scrollRectTasks.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        else
            scrollRectTasks.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        Time.timeScale = 0f;
    }

    public void ExpandQuestInfo(string taskName)
    {
        taskExpansionPanel.SetActive(true);
        Quest quest = questManager.FindQuestInfo(taskName);
        //taskExpansionPanel.SetActive(true);
        if (quest.completed)
            taskExpansionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Completa";
        else if (quest.lost)
            taskExpansionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Perdeu o prazo";
        else if (quest.inProgress)
            taskExpansionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Prazo: Até 00:00 do dia "+ quest.questDay.ToString();
        foreach (Quest.Item item in quest.wantedItens)
        {
            GameObject questResourceItem = Instantiate(questResource, taskExpansionPanel.transform.GetChild(1));
            questResourceItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.quantity.ToString();
            questResourceItem.GetComponent<Image>().sprite = questManager.GetItemSprite(item);
            resources.Add(questResourceItem);
        }
        taskExpansionPanel.transform.GetChild(2).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.questDescription;
    }

    public void ClearResources()
    {
        while (resources.Count != 0 )
        {
            GameObject res = resources[0];
            resources.RemoveAt(0);
            Destroy(res);
        }
    }

    public void OrderTaskList()//coloca botoes de quests na ordem correta de preferencia
    {
        int i = 1;
        int limit = tasks.transform.childCount;

        while (i < limit)
        {
            foreach(string name in Persistent.current.activeQuests)
            {
                tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                Debug.Log(name + i.ToString());
                i++;
            }
            foreach (string name in Persistent.current.completedQuests)
            {
                tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                Debug.Log(name + i.ToString());
                i++;
            }
            foreach (string name in Persistent.current.lostQuests)
            {
                tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                Debug.Log(name + i.ToString());
                i++;
            }
        }
    }


    
}
