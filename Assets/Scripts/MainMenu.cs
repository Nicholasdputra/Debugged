using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    [SerializeField] Button playButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    public GameObject mainMenuButtons;
    public GameObject settingsPanel;

    void Start()
    {
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(Play);
        playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Play";

        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(Settings);
        settingsButton.GetComponentInChildren<TextMeshProUGUI>().text = "Settings";

        quitButton.onClick.RemoveAllListeners();
        quitButton.onClick.AddListener(Quit);
        quitButton.GetComponentInChildren<TextMeshProUGUI>().text = "Quit";
    }

    void OnEnable()
    {
        if(audioManager == null)
            audioManager = GameObject.Find("AudioManager").GetComponent<AudioManagerScript>();
        Time.timeScale = 1f; // Pause the game at the start   
        settingsPanel.GetComponent<SettingsScript>().LoadSettings();
        settingsPanel.SetActive(false);
        mainMenuButtons.SetActive(true);
        audioManager.PlayBGM(audioManager.mainMenuBGMClip);
    }

    void Play()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Time.timeScale = 1f; // Ensure the game runs at normal speed
        SceneManager.LoadScene("LevelSelect");
    }

    void Settings()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        // Show the settings panel
        settingsPanel.SetActive(true);
    }

    void Quit()
    {
        audioManager.PlaySFX(audioManager.buttonClickSFXClip);
        Application.Quit();
    }
}
