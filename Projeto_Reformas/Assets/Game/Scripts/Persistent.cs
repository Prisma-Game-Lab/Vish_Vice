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
    public List<string> activeQuestsUI;
    public List<string> activeQuests;
    public List<string> completedQuests;
    public List<Quest> allQuests;//lista de todas as quests que foram aceitas em algum momento
    public List<string> lostQuests;
    public List<string> neglectedQuests;
    public List<string> allQuestNames;//lista de todas as quests que apareceram no jogo ate o momento
    public List<string> allNpcs;//lista de todos os npcs

    public List<bool> objectState;

    public Dictionary<string, bool> firstContactNPCs;

    [HideInInspector] public float currentTime = 2;
    [HideInInspector] public float fullDayLength = 2;
    [HideInInspector] public Vector3 playerPosition;
    public static Persistent current;

    public int currentMetalGameLevel;
    public int earnedMetalQtd = 0;

    [HideInInspector] public float playerStartX = 0.77f;
    [HideInInspector] public float playerStartY = 0.589f;
    [HideInInspector] public float playerStartZ = 11.11f;

    [HideInInspector] public bool fadeOn;

    private void Awake()
    {
        
        GameObject[] objs = GameObject.FindGameObjectsWithTag("persistentData");
        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
        if (current == null)
        {
            current = this;
            firstContactNPCs = new Dictionary<string, bool>();
            LoadGame();
        }
            
        //firstContactNPCs = new Dictionary<string, bool>();
    }

    public void SaveGame()
    {
        int i = 0;
        PlayerPrefs.SetInt("quantWood", current.quantWood);
        PlayerPrefs.SetInt("quantMetal", current.quantMetal);
        PlayerPrefs.SetInt("quantConcrete", current.quantConcrete);
        PlayerPrefs.SetInt("quantManPower", current.quantManpower);
        PlayerPrefs.SetInt("quantCharisma", current.quantCharisma);
        PlayerPrefs.SetInt("usedManPower", current.usedManpower);
        PlayerPrefs.SetInt("currentDay", current.currentDay);

        //Saving all the lists
        foreach (string name in activeQuestsUI)//activeQuestsUI
            PlayerPrefs.SetString("ACUI_" + name, "activeQuestsUI");
        foreach (string name in activeQuests)//activeQuests
            PlayerPrefs.SetString("ACTI_" + name, "activeQuests");
        foreach (string name in completedQuests)//completedQuests
            PlayerPrefs.SetString("COMP_" + name, "completedQuests");
        foreach (string name in lostQuests)//lostQuests
            PlayerPrefs.SetString("LOST_" + name, "lostQuests");
        foreach (string name in neglectedQuests)//neglectedQuests
            PlayerPrefs.SetString("NEGL_" + name, "neglectedQuests");
        foreach (Quest quest in allQuests)//allQuests
            PlayerPrefs.SetString("ALLQ_" + quest.questName, "allQuests");

        foreach (string name in allQuestNames) {
            PlayerPrefs.SetString("quest" + i.ToString(), name);
            i++;
        }

        i = 0;
        foreach (string name in allNpcs)
        {
            PlayerPrefs.SetString("npc" + i.ToString(), name);
            i++;
        }

        i = 0;
        foreach (bool state in objectState)
        {
            if(state)
                PlayerPrefs.SetInt("objectState" + i.ToString(), 1);
            else
                PlayerPrefs.SetInt("objectState" + i.ToString(), 0);
            i++;
        }

        //Saving player position
        PlayerPrefs.SetFloat("PlayerPositionX", playerPosition.x);
        PlayerPrefs.SetFloat("PlayerPositionY", playerPosition.y);
        PlayerPrefs.SetFloat("PlayerPositionZ", playerPosition.z);

        //Saving FirstContactNpcs Dictionary
        foreach(KeyValuePair<string, bool> item in firstContactNPCs)
        {
            if (item.Value)
                PlayerPrefs.SetInt("FCN_" + item.Key, 1);
            else
                PlayerPrefs.SetInt("FCN_" + item.Key, 0);
        }

        PlayerPrefs.SetInt("SavedGame", 1);
    }

    public void LoadGame()
    {
        if (PlayerPrefs.GetInt("SavedGame") != 1)
        {
            return;
        } 

        int i = 0;
        quantWood = PlayerPrefs.GetInt("quantWood");
        quantMetal = PlayerPrefs.GetInt("quantMetal");
        quantConcrete = PlayerPrefs.GetInt("quantConcrete");
        quantManpower = PlayerPrefs.GetInt("quantManPower");
        quantCharisma = PlayerPrefs.GetInt("quantCharisma");
        usedManpower = PlayerPrefs.GetInt("usedManPower");
        currentDay = PlayerPrefs.GetInt("currentDay");

        //Restoring data lists
        while(PlayerPrefs.HasKey("quest" + i.ToString()))
        {
            allQuestNames.Add(PlayerPrefs.GetString("quest" + i.ToString()));
            PlayerPrefs.DeleteKey("quest" + i.ToString());
            i++;
        }

        i = 0;
        while (PlayerPrefs.HasKey("npc" + i.ToString()))
        {
            allNpcs.Add(PlayerPrefs.GetString("npc" + i.ToString()));
            PlayerPrefs.DeleteKey("npc" + i.ToString());
            i++;
        }

        i = 0;
        while (PlayerPrefs.HasKey("objectState" + i.ToString()))
        {
            if (PlayerPrefs.GetInt("objectState" + i.ToString()) == 1)
                objectState.Add(true);
            else
                objectState.Add(false);
            PlayerPrefs.DeleteKey("objectState" + i.ToString());
            i++;
        }

        foreach (string name in allQuestNames)//falta alterar os campos de cada quest
        {
            if(PlayerPrefs.HasKey("ACUI_" + name))
            {
                activeQuestsUI.Add(PlayerPrefs.GetString("ACUI_" + name));
                PlayerPrefs.DeleteKey("ACUI_" + name);
            }

            if (PlayerPrefs.HasKey("ACTI_" + name))
            {
                activeQuests.Add(PlayerPrefs.GetString("ACTI_" + name));
                PlayerPrefs.DeleteKey("ACTI_" + name);
            }

            if (PlayerPrefs.HasKey("COMP_" + name))
            {
                completedQuests.Add(PlayerPrefs.GetString("COMP_" + name));
                PlayerPrefs.DeleteKey("COMP_" + name);
            }

            if (PlayerPrefs.HasKey("LOST_" + name))
            {
                lostQuests.Add(PlayerPrefs.GetString("LOST_" + name));
                PlayerPrefs.DeleteKey("LOST_" + name);
            }

            if (PlayerPrefs.HasKey("NEGL_" + name))
            {
                neglectedQuests.Add(PlayerPrefs.GetString("NEGL_" + name));
                PlayerPrefs.DeleteKey("NEGL_" + name);
            }

        }

        foreach (string name in allNpcs)
        {
            if (PlayerPrefs.HasKey("FCN_" + name))
            {
                if(PlayerPrefs.GetInt("FCN_" + name) == 1)
                    firstContactNPCs.Add(name, true);
                else
                    firstContactNPCs.Add(name, false);
                PlayerPrefs.DeleteKey("FCN_" + name);
            }
        }

        playerStartX = PlayerPrefs.GetInt("PlayerPositionX");
        playerStartY = PlayerPrefs.GetInt("PlayerPositionY");
        playerStartZ = PlayerPrefs.GetInt("PlayerPositionZ");

    }

    public void DeleteSave()
    {
        int i = 0;

        //Deleting data lists
        while (PlayerPrefs.HasKey("quest" + i.ToString()))
        {
            PlayerPrefs.DeleteKey("quest" + i.ToString());
            i++;
        }

        i = 0;
        while (PlayerPrefs.HasKey("npc" + i.ToString()))
        {
            PlayerPrefs.DeleteKey("npc" + i.ToString());
            i++;
        }

        i = 0;
        while (PlayerPrefs.HasKey("objectState" + i.ToString()))
        {
            PlayerPrefs.DeleteKey("objectState" + i.ToString());
            i++;
        }

        foreach (string name in allQuestNames)
        {
            PlayerPrefs.DeleteKey("ACUI_" + name);
            PlayerPrefs.DeleteKey("ACTI_" + name);
            PlayerPrefs.DeleteKey("COMP_" + name);
            PlayerPrefs.DeleteKey("LOST_" + name);
            PlayerPrefs.DeleteKey("NEGL_" + name);
            PlayerPrefs.DeleteKey("ALLQ_" + name);
        }

        foreach (string name in allNpcs)
        {
            PlayerPrefs.DeleteKey("FCN_" + name);
        }

        //Delete player position
        PlayerPrefs.DeleteKey("PlayerPositionX");
        PlayerPrefs.DeleteKey("PlayerPositionY");
        PlayerPrefs.DeleteKey("PlayerPositionZ");

        PlayerPrefs.SetInt("SavedGame", 0);
    }

}
