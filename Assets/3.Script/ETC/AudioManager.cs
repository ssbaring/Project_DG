using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance = null;

    private void Awake()
    {
        #region Singleton
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        #endregion
    }

    [Header("Mixer")]
    public AudioMixer masterMixer;
    public AudioMixer BGMMixer;
    public AudioMixer SFXMixer;

    [Header("Audio Slider")]
    public Slider masterSlider;
    public Slider BGMSlider;
    public Slider SFXSlider;

    [Header("Text")]
    public TextMeshProUGUI MasterValue;
    public TextMeshProUGUI BGMValue;
    public TextMeshProUGUI SFXValue;

    public void MasterAudioControl()
    {
        int sound = Mathf.RoundToInt(masterSlider.value);
        int absValue = Mathf.RoundToInt(SFXSlider.maxValue - SFXSlider.minValue);
        int MasterText;
        masterSlider.value = sound;
        if (sound == -40.0f)
        {
            masterMixer.SetFloat("Master", -80);
            MasterText = 0;
        }
        else masterMixer.SetFloat("Master", sound);

        if (sound == -80.0f) MasterText = 0;
        else MasterText = Mathf.RoundToInt((absValue + sound) * 2.5f);

        MasterValue.text = string.Format("{0}", MasterText);
    }

    public void BGMAudioControl()
    {
        int sound = Mathf.RoundToInt(BGMSlider.value);
        int absValue = Mathf.RoundToInt(SFXSlider.maxValue - SFXSlider.minValue);
        int BGMText;
        BGMSlider.value = sound;
        if (sound == -40.0f)
        {
            BGMMixer.SetFloat("BGM", -80);
            BGMText = 0;
        }
        else BGMMixer.SetFloat("BGM", sound);

        if (sound == -80.0f) BGMText = 0;
        else BGMText = Mathf.RoundToInt((absValue + sound) * 2.5f);

        BGMValue.text = string.Format("{0}", BGMText);
    }

    public void SFXAudioControl()
    {
        int sound = Mathf.RoundToInt(SFXSlider.value);
        int absValue = Mathf.RoundToInt(SFXSlider.maxValue - SFXSlider.minValue);
        int SFXText;
        SFXSlider.value = sound;
        if (sound == -40.0f)
        {
            SFXMixer.SetFloat("SFX", -80);
            SFXText = 0;
        }
        else SFXMixer.SetFloat("SFX", sound);

        if (sound == -80.0f) SFXText = 0;
        else SFXText = Mathf.RoundToInt((absValue + sound) * 2.5f);

        SFXValue.text = string.Format("{0}", SFXText);
    }

    public void MuteAll(bool isMute)
    {
        masterMixer.SetFloat("Master", isMute ? -80 : masterSlider.value);
    }

    public void MuteBGM(bool isMute)
    {
        BGMMixer.SetFloat("BGM", isMute ? -80 : BGMSlider.value);
    }

    public void MuteSFX(bool isMute)
    {
        SFXMixer.SetFloat("SFX", isMute ? -80 : SFXSlider.value);
    }

}
