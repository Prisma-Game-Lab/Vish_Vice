using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    private Queue<string> _sentences;
    public GameObject DialogPanel;
    public GameObject Joystick;

    void Start()
    {
        _sentences = new Queue<string>();
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
        DialogPanel.transform.GetChild(0).GetComponent<Text>().text = sentence;
        return true;
    }

    public void EndDialogue()
    {
        DialogPanel.SetActive(false);
        Joystick.SetActive(true);

    }

}
