using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TaskButton : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI taskName;
    public Notebook notebook;
    [HideInInspector]public string task;
    void Start()
    {
        taskName = transform.GetChild(0).GetComponent<TextMeshProUGUI>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallNotebook()
    {
        notebook.ExpandQuestInfo(task);
    }
}
