using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : Manager<Settings>
{
    [Header("- SETTINGS -")]
    [SerializeField] private Image soundImage;
    [SerializeField] private Image vibrateImage;
    [SerializeField] private Sprite[] soundSprites;
    [SerializeField] private Sprite[] vibrateSprites;
    private void Start()
    {
        GetData();
    }
    private void GetData()
    {
        // int i = PlayerPrefs.GetInt("sound");
        //   soundImage.sprite = soundSprites[i];
        int a = PlayerPrefs.GetInt("vibrate");
        vibrateImage.sprite = vibrateSprites[a];

    }

    public void SoundOnOff()
    {
        int i = PlayerPrefs.GetInt("sound");
        i = i == 0 ? 1 : 0;
        PlayerPrefs.SetInt("sound", i);
        GetData();
    }
    public void VibrateOnOff()
    {
        int i = PlayerPrefs.GetInt("vibrate");
        i = i == 0 ? 1 : 0;
        PlayerPrefs.SetInt("vibrate", i);
        GetData();
    }

    public bool Vibrate()
    {
        return PlayerPrefs.GetInt("vibrate") == 0;

    }

}