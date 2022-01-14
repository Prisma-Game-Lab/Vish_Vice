using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAnimEvents : MonoBehaviour
{

    public Animator menuBGAnim;


    public void DesactivateThis(){
        gameObject.SetActive(false);
    }

    public void MenuBGFadeInOut(int fadeIn)
    {
        if (fadeIn==1)
            menuBGAnim.Play("BG Fadein");
        else
            menuBGAnim.Play("BG Fadeout");
    }

    public void PauseUnpauseTime(int timeOn)
    {
        if (timeOn == 1)
            Time.timeScale = 1f;
        else
            Time.timeScale = 0f;
    }
}
