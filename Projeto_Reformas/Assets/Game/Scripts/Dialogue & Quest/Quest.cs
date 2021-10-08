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
    [HideInInspector] public bool inProgress;
    [HideInInspector] public bool completed;
    [HideInInspector] public Dialogue startDialogue;
    [HideInInspector] public Dialogue inProgressDialogue;
    [HideInInspector] public Dialogue completedDialogue;
    public GameObject questDialogues;
    public List<Item> wantedItens;
    public GameObject activate;
    public GameObject desactivate;
}
