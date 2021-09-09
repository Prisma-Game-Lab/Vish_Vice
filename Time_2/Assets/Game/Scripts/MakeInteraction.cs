using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MakeInteraction : MonoBehaviour
{
    public GameObject gameManager;
    public GameObject DialogPanel;
    [HideInInspector]public bool interacting;
    private GameObject _npc;
    private DialogueManager _dialogueManager;
    private UIMaster _uiMaster;
    private PlayerInput _playerInput;

    private void Start()
    {
        _dialogueManager = gameManager.GetComponent<DialogueManager>();
        _uiMaster = gameManager.GetComponent<UIMaster>();
        _playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && interacting)
        {
             interacting = _dialogueManager.DisplayNextSentence();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("NPC")) {
            _uiMaster.InteractButton.SetActive(true);
            _npc = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            _uiMaster.InteractButton.SetActive(false);
            _npc = null;
        }
    }

    public void GreetNpc()
    {
        interacting = true;
        _playerInput.variableJoystick.gameObject.SetActive(false);

        NPCInteraction npcInteraction = _npc.GetComponent<NPCInteraction>();
        _dialogueManager.StartDialog(npcInteraction.Greet());
        _dialogueManager._displayName.text = npcInteraction.npcName != "" ? npcInteraction.npcName : "sem nome";

        DialogPanel.SetActive(true);
        _uiMaster.InteractButton.SetActive(false);

    }


}
