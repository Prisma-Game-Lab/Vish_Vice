using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Quest
{
    [Serializable]
    public class Item
    {
        public ItemType type;
        public int quantity;
    }
    [HideInInspector] public string npcName;
    public string questName;
    public int questDay;
    public int charismaGain;
    public int charismaLost;
    public QuestType questType;
    [HideInInspector] public bool inProgress;
    [HideInInspector] public bool completed;
    [HideInInspector] public bool lost;
    [HideInInspector] public Dialogue startDialogue;
    [HideInInspector] public Dialogue inProgressDialogue;
    [HideInInspector] public Dialogue completedDialogue;
    public GameObject questDialogues;
    public List<Item> wantedItens;
    public GameObject activateObject;
    public GameObject desactivateObject;
    public string questDescription;
}
