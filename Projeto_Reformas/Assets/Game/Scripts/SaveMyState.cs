using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveMyState : MonoBehaviour
{
    private void OnEnable()
    {
        //UpdateState(true);
    }
    private void OnDisable()
    {
        //UpdateState(false);
    }

    private void UpdateState(bool state)
    {
        Persistent.current.objectState[gameObject.transform.GetSiblingIndex()] = state;
    }
}
