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
        //mdoText.text = Persistent.current.quantManpower.ToString();
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
        RegenerateButtons();
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
        Language language = LanguageManager.instance.activeLanguage;
        taskExpansionPanel.SetActive(true);
        Quest quest = questManager.FindQuestInfo(taskName);
        TextMeshProUGUI taskStatus = taskExpansionPanel.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
        
        if (quest.completed)
            taskStatus.text = GetQuestDeadline(quest.questDay, quest.duration, 1);
        else if (quest.lost)
            taskStatus.text = GetQuestDeadline(quest.questDay, quest.duration, 2);
        else if (quest.inProgress)
            taskStatus.text = GetQuestDeadline(quest.questDay, quest.duration, 0);
        foreach (Quest.Item item in quest.wantedItens)
        {
            GameObject questResourceItem = Instantiate(questResource, taskExpansionPanel.transform.GetChild(1));
            questResourceItem.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = item.quantity.ToString();
            questResourceItem.GetComponent<Image>().sprite = questManager.GetItemSprite(item);
            resources.Add(questResourceItem);
        }
        Transform description = taskExpansionPanel.transform.GetChild(2).GetChild(0);
        if(language == Language.Portuguese)
            description.GetComponent<TextMeshProUGUI>().text = quest.Description;
        else
            description.GetComponent<TextMeshProUGUI>().text = quest.DescriptionEnglish;
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
        Language language = LanguageManager.instance.activeLanguage;

        while (i < limit)
        {
            foreach(string name in Persistent.current.activeQuests)
            {
                Quest quest = questManager.FindQuestInfo(name);
                tasks.transform.GetChild(i).GetComponent<TaskButton>().task = name;
                if (language == Language.Portuguese)
                    tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                else
                    tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.questNameEnglish;
                i++;
                //Debug.Log(name + i.ToString());
                tasks.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = GetQuestDeadline(quest.questDay, quest.duration, 0);
                //Debug.Log(name + i.ToString());
                i++;
            }
            foreach (string name in Persistent.current.completedQuests)
            {
                Quest quest = questManager.FindQuestInfo(name);
                tasks.transform.GetChild(i).GetComponent<TaskButton>().task = name;
                if (language == Language.Portuguese)
                    tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                else
                    tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.questNameEnglish;
                i++;
                tasks.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = GetQuestDeadline(quest.questDay, quest.duration, 1);
                //Debug.Log(name + i.ToString());
                i++;
            }
            foreach (string name in Persistent.current.lostQuests)
            {
                Quest quest = questManager.FindQuestInfo(name);
                tasks.transform.GetChild(i).GetComponent<TaskButton>().task = name;
                if (language == Language.Portuguese)
                    tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = name;
                else
                    tasks.transform.GetChild(i).GetChild(0).GetComponent<TextMeshProUGUI>().text = quest.questNameEnglish;
                i++;
                tasks.transform.GetChild(i).GetComponent<TextMeshProUGUI>().text = GetQuestDeadline(quest.questDay, quest.duration, 2);
                //Debug.Log(name + i.ToString());
                i++;
            }
        }

    }

    public void RegenerateButtons()
    {
        Debug.Log("allQuestsActivated count: " + (Persistent.current.allQuestsActivated.Count).ToString());
        Debug.Log("allQuestsActivated*2 count: "+ (Persistent.current.allQuestsActivated.Count * 2).ToString());
        while(tasks.transform.childCount < (Persistent.current.allQuestsActivated.Count*2 + 2))
        {
            Debug.Log("tasks.transform.childCount: " + tasks.transform.childCount.ToString());
            GameObject button = Instantiate(questButton, tasks.transform);
            GameObject text = Instantiate(questLimit, tasks.transform);
            button.SetActive(true);
            text.SetActive(true);
        }
    }

    public string GetQuestDeadline(int questDay, int questDuration, int questStatus)
    {
        Language language = LanguageManager.instance.activeLanguage;
        switch (questStatus)
        {
            case 0://active
                if (questDuration < 7)
                {
                    if(language == Language.Portuguese)
                        return "Vence 23:59 do dia " + (questDay + questDuration - 1).ToString();
                    else
                        return "Ends at 23:59 of day " + (questDay + questDuration - 1).ToString();
                }
                else
                {
                    if (language == Language.Portuguese)
                        return "Prazo indeterminado";
                    else
                        return "Deadline not established";
                }
            case 1://completed
                if (language == Language.Portuguese)
                    return "Tarefa concluída!";
                else
                    return "Accomplished task!";
            default://lost
                if (language == Language.Portuguese)
                    return "Perdeu o prazo!";
                else
                    return "Failed task!";
        }
    }
    
}
