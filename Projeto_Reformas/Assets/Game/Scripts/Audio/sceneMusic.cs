using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sceneMusic : MonoBehaviour
{
    public string musicName;
    // Start is called before the first frame update
    void Start()
    {
        AudioManager.instance.Play(musicName);
    }
    private void OnDestroy()
    {
        if(AudioManager.instance!=null)
            AudioManager.instance.StopAllMusicSounds();
    }
}
