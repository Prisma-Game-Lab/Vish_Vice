using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoundControl : MonoBehaviour
{
    private static string _masterSoundPrefs = "MasterSoundPrefs";
    private static string _soundEffectsPrefs = "SoundEffectsPrefs";
    private static string _backgroundPrefs = "BackgroundPrefs";
    private float _soundValue;
    private Slider _slider;


    void Start()
    {
        _slider = this.GetComponent<Slider>();
        SetSoundPrefs();
    }

    public void SetSoundPrefs()
    {
        if (PlayerPrefs.GetInt("FirstGame") != 0)
        {
            if (_slider.tag == "MasterSlider")
                _soundValue = PlayerPrefs.GetFloat(_masterSoundPrefs);
            else if (_slider.tag == "BackgroundSlider")
                _soundValue = PlayerPrefs.GetFloat(_backgroundPrefs);
            else
                _soundValue = PlayerPrefs.GetFloat(_soundEffectsPrefs);
        }
        _slider.value = _soundValue;
    }

    public void SetSliderValue()
    {
        _soundValue = _slider.value;
        if (_slider.tag == "MasterSlider")
            PlayerPrefs.SetFloat(_masterSoundPrefs, _soundValue);
        else if (_slider.tag == "BackgroundSlider")
            PlayerPrefs.SetFloat(_backgroundPrefs, _soundValue);
        else
            PlayerPrefs.SetFloat(_soundEffectsPrefs, _soundValue);
        AudioManager.instance.UpdateSoundVolumes();
    }
}
