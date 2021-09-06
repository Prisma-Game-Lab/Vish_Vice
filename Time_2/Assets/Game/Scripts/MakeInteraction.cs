using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeInteraction : MonoBehaviour
{
    public GameObject gameManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            gameManager.GetComponent<UIMaster>().InteractRender();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            gameManager.GetComponent<UIMaster>().InteractRender();
        }
    }
}
