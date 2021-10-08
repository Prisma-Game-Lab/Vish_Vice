using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatesManager : MonoBehaviour
{
    private Persistent persistentData;
    private void Start()
    {
        persistentData = Persistent.current;
        if (persistentData.objectState.Count <= gameObject.transform.childCount)
        {
            foreach (Transform child in gameObject.transform)
            {
                persistentData.objectState.Add(child.gameObject.activeInHierarchy);
            }
        }
        else
        {
            int count = 0;
            foreach (Transform child in gameObject.transform)
            {
                child.gameObject.SetActive(persistentData.objectState[count]);
                count++;
            }
        }
    }
}
