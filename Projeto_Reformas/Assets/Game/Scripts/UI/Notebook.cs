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
    public GameObject questLimit;
    public GameObject tasks;
    public ScrollRect scrollRectTasks;
    public ScrollRect scrollRectDescription;
    public GameObject taskExpansionPanel;
    public GameObject questResource;
    public GameObject scrollBarButtons;
    public Scrollbar scrollbarDescription;
    public List<GameObject> resources;
    private QuestManager questManager;
    public Animator notesAnimator;

    // Start is called before the first frame update
    void Start()
    {
        questManager = GetComponent<QuestManager>();
        //RegenerateButtons();
    }   

    // Update is called once per frame
    void Update()
    {
        woodText.text = Persistent.current.quantWood.ToString();
        metalText.text = Persistent.current.quantMetal.ToString();
        concreteText.text = Persistent.current.quantConcrete.ToString();
        mdoText.text = Persistent.current.quantManpower.ToString();
    }

    public void CloseNotebook()
    {
        //notebookUI.SetActive(false);
        //scrollBarButtons.SetActive(false);
        taskExpansionPanel.SetActive(false);
        Time.timeScale = 1f;
        notesAnimator.Play("NotesDown");
        
    }

    public void OpenNotebook()
    {
        //notebookUI.SetActive(true);
        notebookUI.SetActive(true);
        OrderTaskList();
        notesAnimator.Play("NotesUp");
        if (tasks.transform.childCount >= 12)
            scrollRectTasks.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        else
            scrollRectTasks.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
        //Time.timeScale = 0f;
    }

    public void ExpandQuestInfo(string taskName)
    {
        taskExpansionPanel.SetActive(true);
        Quest quest = questManager.FindQuestInfo(taskName);
        TextMeshProUGUI taskStatus = taskExpansionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        if (quest.completed)
            taskStatus.text = "Completa";
        else if (quest.lost)
            taskStatus.text = "Perdeu o prazo";
        else if (quest.inProgress)
            taskStatus.text = "Prazo: Até 00:00 do dia "+ (quest.questDay+1).ToString();
        foreach (Quest.Item item in quest.wantedItens)
        {
            GameObject questResourceItem = Instantiate(questResource, taskExpansionPanel.transform.GetChild(1));
            questResourceItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.quantity.ToString();
            questResourceItem.GetComponent<Image>().sprite = questManager.GetItemSprite(item);
            resources.Add(questResourceItem);
        }
        Transform description = taskExpansionPanel.transform.GetChild(2).GetChild(0);
        description.GetComponent<TextMeshProUGUI>().text = quest.questDescription;
        /*if (description.GetComponent<RectTransform>().rect.height >= 190f)
            scrollRectDescription.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.Permanent;
        else
            scrollRectDescription.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;*/
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
        int i = 2;
        int limit = tasks.transform.childCount;

        while (i < limit)
        {
            foreach(string name in Persistent.current.activeQuests)
            {
                tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                i++;
                tasks.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "Até 00h do dia "+ (questManager.FindQuestInfo(name).questDay + 1).ToString();
                Debug.Log(name + i.ToString());
                i++;
            }
            foreach (string name in Persistent.current.completedQuests)
            {
                tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                i++;
                tasks.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "Completa";
                Debug.Log(name + i.ToString());
                i++;
            }
            foreach (string name in Persistent.current.lostQuests)
            {
                tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                i++;
                tasks.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = "Perdeu o prazo";
                Debug.Log(name + i.ToString());
                i++;
            }
        }

    }

    public void RegenerateButtons()
    {
        while(tasks.transform.childCount < Persistent.current.allQuests.Count*2 + 2)
        {
            GameObject button = Instantiate(questButton, tasks.transform);
            GameObject text = Instantiate(questLimit, tasks.transform);
            button.SetActive(true);
            text.SetActive(true);
        }
    }

    
}
