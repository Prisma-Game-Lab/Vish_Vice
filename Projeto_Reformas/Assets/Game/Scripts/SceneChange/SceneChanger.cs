using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public void WoodMinigame()
    {
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene("WoodGame");
    }

    public void MetalMinigame()
    {
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene("MetalGame");
    }

    public void ConcreteMinigame()
    {
        AudioManager.instance.Play("ButtonClick");
        SceneManager.LoadScene("ConcreteGame");
    }
}
