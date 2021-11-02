using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Notebook : MonoBehaviour
{
    public TextMeshProUGUI woodText;
    public TextMeshProUGUI metalText;
    public TextMeshProUGUI concreteText;
    public TextMeshProUGUI mdoText;
    public GameObject notebookUI;
    // Start is called before the first frame update
    void Start()
    {

    }   

    // Update is called once per frame
    void Update()
    {
        woodText.text = Persistent.current.quantWood.ToString();
        metalText.text = Persistent.current.quantMetal.ToString();
        concreteText.text = Persistent.current.quantConcrete.ToString();
    }

    public void CloseNotebook()
    {
        notebookUI.SetActive(false);
        Time.timeScale = 1f;
    }

    public void OpenNotebook()
    {
        notebookUI.SetActive(true);
        Time.timeScale = 0f;
    }
}
