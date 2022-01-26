using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public void WoodMinigame()
    {
        AudioManager.instance.Play("ButtonClick");
        Persistent.current.DeleteSave();
        Persistent.current.SaveGame();
        SceneManager.LoadScene("WoodGame");
    }

    public void MetalMinigame()
    {
        AudioManager.instance.Play("ButtonClick");
        Persistent.current.DeleteSave();
        Persistent.current.SaveGame();
        SceneManager.LoadScene("MetalGame");
    }

    public void ConcreteMinigame()
    {
        AudioManager.instance.Play("ButtonClick");
        Persistent.current.DeleteSave();
        Persistent.current.SaveGame();
        SceneManager.LoadScene("ConcreteGame");
    }
}
