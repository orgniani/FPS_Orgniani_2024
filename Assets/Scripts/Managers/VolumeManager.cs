using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    [Header("Volume")]
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string volumeName = "musicVolume";

    [Header("Sound On/Off Icon")]
    [SerializeField] private Sprite soundOnIcon;
    [SerializeField] private Sprite soundOffIcon;
    [SerializeField] private Image soundIcon;

    private void Start()
    {
        if(!PlayerPrefs.HasKey(volumeName))
        {
            PlayerPrefs.SetFloat(volumeName, 1);
            AudioListener.volume = 1;
            volumeSlider.value = 1; 
        }

        else
        {
            Load();
        }
    }
    public void ChangeVolume()
    {
        AudioListener.volume = volumeSlider.value;
        Save();

        UpdateSoundIcon(volumeSlider.value);
    }

    private void Load()
    {
        volumeSlider.value = PlayerPrefs.GetFloat(volumeName);
    }

    private void Save()
    {
        PlayerPrefs.SetFloat(volumeName, volumeSlider.value);
    }

    private void UpdateSoundIcon(float volume)
    {
        if (volume == 0) soundIcon.sprite = soundOffIcon;
        else soundIcon.sprite = soundOnIcon;
    }
}
