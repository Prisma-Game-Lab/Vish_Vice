using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour
{
    public int quantWood;
    public int quantMetal;
    public int quantConcrete;
    public int currentDay;
    public List<Quest> activeQuestsUI;
    public List<string> activeQuests;
    public List<string> completedQuests;

    public Dictionary<string, bool> firstContactNPCs;

    [HideInInspector] public float currentTime = 2;
    [HideInInspector] public float fullDayLength = 2;

    public static Persistent current;

    private void Awake()
    {
        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("persistentData");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        if (current == null)
            current = this;
        firstContactNPCs = new Dictionary<string, bool>();
    }

}
