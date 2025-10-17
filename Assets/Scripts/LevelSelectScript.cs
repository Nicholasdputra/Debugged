using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelSelectScript : MonoBehaviour
{
    public GameObject backButton;
    public GameObject[] levelButtons;

    // Start is called before the first frame update
    void Start()
    {
        levelButtons = GameObject.FindGameObjectsWithTag("LevelButton");
        foreach (GameObject button in levelButtons)
        {
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            button.GetComponent<Button>().onClick.AddListener
            (
                () => GoToLevel(ExtractLevelNumber(button.name))
            );
        }

        backButton.GetComponent<Button>().onClick.RemoveAllListeners();
        backButton.GetComponent<Button>().onClick.AddListener(BackToMainMenu);
    }

    void GoToLevel(int levelIndex)
    {
        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.buttonClickSFXClip);
        // Load the selected level scene
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.LoadScene("Level" + levelIndex);
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Level"))
        {
            GameManagerScript gameManager = GameObject.Find("GameManager").GetComponent<GameManagerScript>();
            gameManager.state = 1; // Change state to In-Game Phase
        }
    }

    int ExtractLevelNumber(string buttonName)
    {
        // Extract number from "Level0", "Level1", etc.
        string numberPart = buttonName.Replace("Level", "");
        return int.Parse(numberPart);
    }

    void BackToMainMenu()
    {
        AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.buttonClickSFXClip);
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }
}