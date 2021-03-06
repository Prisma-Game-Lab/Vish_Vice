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
    private QuestManager questManager;

    private void Start()
    {
        questManager = gameManager.GetComponent<QuestManager>();
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
            other.gameObject.GetComponent<NPCInteraction>().SetInteractionButtonOn();
            //_uiMaster.interactButton.SetActive(true);
            _npc = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("NPC"))
        {
            other.gameObject.GetComponent<NPCInteraction>().SetInteractionButtonOff();
            //_uiMaster.interactButton.SetActive(false);
            _npc = null;
        }
    }

    public void GreetNpc()
    {
        interacting = true;
        _playerInput.variableJoystick.gameObject.SetActive(false);

        DialogPanel.SetActive(true);
        DialogPanel.GetComponent<Animator>().Play("DialogueUp");
        //_uiMaster.interactButton.SetActive(false);

        NPCInteraction npcInteraction = _npc.GetComponent<NPCInteraction>();
        questManager.newQuestNPC = npcInteraction;
        npcInteraction.DisableInteractionButton();
        _dialogueManager.StartDialog(npcInteraction.Greet());
        if(LanguageManager.instance.activeLanguage == Language.Portuguese)
        {
            _uiMaster._displayName.text = npcInteraction.npcName != "" ? npcInteraction.npcName : "sem nome";
        }
        else
            _uiMaster._displayName.text = npcInteraction.npcNameEnglish != "" ? npcInteraction.npcNameEnglish : "no name";


    }

    public GameObject ReturnNpc()
    {
        return _npc;
    }


}
