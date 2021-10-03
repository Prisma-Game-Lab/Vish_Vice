using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Dialogue : MonoBehaviour
{
    [TextArea(3, 10)]
    public string[] sentences;
    public MinigameType minigameType;
    public bool hasMinigame;
    public bool isQuest;
    public bool questInProgress;
}
