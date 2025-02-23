using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    private Slider volumeSlider;

    private void Start()
    {
        volumeSlider = gameObject.GetComponent<Slider>();
        volumeSlider.value = PlayerPrefs.GetFloat(SoundManager.Instance.MasterVolumeKey, 0.5f);
        volumeSlider.onValueChanged.AddListener(OnVolumeChanged);
    }


    private void OnVolumeChanged(float volume)
    {
        SoundManager.Instance.SetMasterVolume(volume);
    }
}
