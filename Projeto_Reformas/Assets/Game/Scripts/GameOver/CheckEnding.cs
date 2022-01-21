using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckEnding : MonoBehaviour
{
    public GameObject goodEnding;
    public GameObject badEnding;

    [Header("Min Charisma For Good Ending")]
    public int minCharisma;
   
    // Start is called before the first frame update
    void Start()
    {
        if (Persistent.current.quantCharisma >= minCharisma)
        {
            goodEnding.SetActive(true);
            badEnding.SetActive(false);
        } else
        {
            goodEnding.SetActive(false);
            badEnding.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ReturnTitleScreen()
    {
        Persistent.current.DeleteSave();
        Persistent.current.ResetPersistent();
        SceneManager.LoadScene("MainMenu");
    }
}
