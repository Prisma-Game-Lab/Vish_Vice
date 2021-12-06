using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistent : MonoBehaviour
{
    public int quantWood;
    public int quantMetal;
    public int quantConcrete;
    public int quantManpower;
    public int quantCharisma;
    [HideInInspector]public int usedManpower;
    public int currentDay;
    public List<Quest> activeQuestsUI;
    public List<string> activeQuests;
    public List<string> completedQuests;
    public List<Quest> allQuests;
    public List<string> lostQuests;
    public List<string> neglectedQuests;

    public List<bool> objectState;

    public Dictionary<string, bool> firstContactNPCs;

    [HideInInspector] public float currentTime = 2;
    [HideInInspector] public float fullDayLength = 2;
    [HideInInspector] public Vector3 playerPosition;
    public static Persistent current;

    public int currentMetalGameLevel;
    public int earnedMetalQtd = 0;
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
