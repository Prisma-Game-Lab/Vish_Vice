using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeInteraction : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject DialogueManagerObject;
    public GameObject DialogPanel;
    [HideInInspector]public bool interacting;
    private GameObject _npc;
    private DialogueManager _dialogueManager;

    private void Start()
    {
        _dialogueManager = DialogueManagerObject.GetComponent<DialogueManager>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && interacting)
        {
            if(interacting)
                interacting = _dialogueManager.DisplayNextSentence();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            gameManager.GetComponent<UIMaster>().InteractButton.SetActive(true);
            _npc = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            gameManager.GetComponent<UIMaster>().InteractButton.SetActive(false);
            _npc = null;
        }
    }

    public void GreetNpc()
    {
        interacting = true;
        GetComponent<PlayerInput>().variableJoystick.gameObject.SetActive(false);
        _dialogueManager.StartDialog(_npc.GetComponent<NPCInteraction>().Greet());
        DialogPanel.SetActive(true);
        gameManager.GetComponent<UIMaster>().InteractButton.SetActive(false);

    }


}
