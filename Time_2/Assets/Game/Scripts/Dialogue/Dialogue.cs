using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    [TextArea(3,10)]
    public string[] sentences;
    public bool isQuest;
}
