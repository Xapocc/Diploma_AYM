using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menuObject;
    [SerializeField] private Player1Movement player1Movement;
    [SerializeField] private Player1MouseLook player1MouseLookX;
    [SerializeField] private Player1MouseLook player1MouseLookY;
    [SerializeField] private Player1MouseLook player1MouseLookXY;
    [SerializeField] private PlayerInteraction playerInteraction;

    [SerializeField] private GameObject settingsMenuButtons;

    [SerializeField] private GameObject qualityDropdown;
    [SerializeField] private GameObject resolutionDropdown;
    [SerializeField] private GameObject fullscreenToggle;
    [SerializeField] private GameObject volumeSlider;

    private int resolutionDefValue = 0;
    private int qualityDefValue = 0;
    private int volumeDefValue = 1;
    private bool fullscreenDefValue = true;

    public bool isOpened = false;

    // Start is called before the first frame update
    void Start()
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

        fullscreenToggle.GetComponent<Toggle>().isOn = Screen.fullScreen;

        volumeSlider.GetComponent<Slider>().value = AudioListener.volume;
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
    public void ToggleSettingsMenu()
    {
        settingsMenuButtons.SetActive(!settingsMenuButtons.activeSelf);
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

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !GameProgression.GetIsInMinigame())
        {
            ShowHideMenu();
        }
    }

    public void ShowHideMenu()
    {
        isOpened = !isOpened;
        if (isOpened)
            Cursor.lockState = CursorLockMode.None;
        player1Movement.enabled = !isOpened;
        player1MouseLookX.enabled = !isOpened;
        player1MouseLookY.enabled = !isOpened;
        player1MouseLookXY.enabled = !isOpened;
        playerInteraction.enabled = !isOpened;
        menuObject.SetActive(isOpened);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void GoToMain()
    {
        SceneManager.LoadScene("MenuScene");
    }
}
