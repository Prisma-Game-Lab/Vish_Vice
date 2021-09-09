using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public GameObject DialogPanel;
    public GameObject Joystick;
    private Queue<string> _sentences;
    private Text _displayText;
    [HideInInspector] public Text _displayName;

    void Start()
    {
        _sentences = new Queue<string>();
        _displayText = DialogPanel.transform.GetChild(0).GetComponent<Text>();
        _displayName = DialogPanel.transform.GetChild(1).GetComponent<Text>();
    }

    public void StartDialog(Dialogue dialogue)
    {
        foreach(string sentence in dialogue.sentences)
        {
            _sentences.Enqueue(sentence);
        }
        DisplayNextSentence();
    }

    public bool DisplayNextSentence()
    {
        if(_sentences.Count == 0)
        {
            EndDialogue();
            return false;
        }

        string sentence = _sentences.Dequeue();
        _displayText.text = sentence;
        return true;
    }

    public void EndDialogue()
    {
        DialogPanel.SetActive(false);
        Joystick.SetActive(true);
    }

}
