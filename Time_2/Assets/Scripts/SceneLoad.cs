using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public void LoadPlay()
    {
        SceneManager.LoadScene("Play");
    }

    public void LoadOptions() { }
    public void LoadCredits() { }
}
