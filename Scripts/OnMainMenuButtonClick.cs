using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnMainMenuButtonClick : MonoBehaviour
{
    [SerializeField] private GameObject loadingText;
    [SerializeField] private GameObject mainMenuButtons;
    [SerializeField] private GameObject settingsMenuButtons;

    [SerializeField] private GameObject qualityDropdown;
    [SerializeField] private GameObject resolutionDropdown;
    [SerializeField] private GameObject fullscreenToggle;
    [SerializeField] private GameObject volumeSlider;

    private int resolutionDefValue = 0;
    private int qualityDefValue = 0;
    private int volumeDefValue = 1;
    private bool fullscreenDefValue = true;

    private void Start()
    {
        int i = 0;
        foreach (Resolution item in Screen.resolutions)
        {
            resolutionDropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(item.ToString()));
            if (Screen.currentResolution.ToString() == item.ToString())
            {
                resolutionDropdown.GetComponent<Dropdown>().value = i;
                resolutionDefValue = i;
            }
            i++;
        }

        i = 0;
        foreach (string item in QualitySettings.names)
        {
            qualityDropdown.GetComponent<Dropdown>().options.Add(new Dropdown.OptionData(item));
            if (QualitySettings.GetQualityLevel() == i)
            {
                qualityDropdown.GetComponent<Dropdown>().value = i;
                qualityDropdown.GetComponent<Dropdown>().captionText.text = item;
                qualityDefValue = i;
            }
            i++;
        }

        ApplySettings();
    }

    public void SetDefaultsButtonHandler()
    {
        resolutionDropdown.GetComponent<Dropdown>().value = resolutionDefValue;
        qualityDropdown.GetComponent<Dropdown>().value = qualityDefValue;
        fullscreenToggle.GetComponent<Toggle>().isOn = fullscreenDefValue;
        volumeSlider.GetComponent<Slider>().value = volumeDefValue;
    }

    public void ApplySettingsButtonHandler()
    {
        ApplySettings();
        ToggleSettingsMenu();
    }

    private void ApplySettings()
    {
        Screen.SetResolution(
            Screen.resolutions[resolutionDropdown.GetComponent<Dropdown>().value].width,
            Screen.resolutions[resolutionDropdown.GetComponent<Dropdown>().value].height,
            fullscreenToggle.GetComponent<Toggle>().isOn);

        QualitySettings.SetQualityLevel(qualityDropdown.GetComponent<Dropdown>().value, true);

        AudioListener.volume = volumeSlider.GetComponent<Slider>().value;
    }

    public void ToggleSettingsMenu()
    {
        settingsMenuButtons.SetActive(!settingsMenuButtons.activeSelf);
    }

    public void Exit()
    {
        Debug.Log("Closing the application");
        Application.Quit();
    }

    public void StartGameScene()
    {
        mainMenuButtons.SetActive(false);
        loadingText.SetActive(true);
        SceneManager.LoadScene("SampleScene");
    }

    public void JoinGameScene()
    {
        mainMenuButtons.SetActive(false);
        loadingText.SetActive(true);
        SceneManager.LoadScene("Player2Scene");
    }
}
