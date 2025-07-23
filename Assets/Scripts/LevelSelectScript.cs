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
        // Load the selected level scene
        GameManagerScript.Instance.currentLevel = levelIndex;
        SceneManager.sceneLoaded += OnLevelLoaded;
        SceneManager.LoadScene("Level" + levelIndex);
    }

    void OnLevelLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name.Contains("Level"))
        {
            GameManagerScript gameManager = GameManagerScript.Instance;

            // Set Up InGameView
            gameManager.inGameView = GameObject.Find("InGameView");
            if (gameManager.inGameView == null)
            {
                Debug.LogWarning("InGameView not found in the scene.");
            }
            else
            {
                // Debug.Log("InGameView found: " + gameManager.inGameView.name);
            }

            // //Set Up PreparationView 
            // gameManager.preparationView = GameObject.Find("PreparationView");
            // if (gameManager.preparationView == null)
            // {
            //     Debug.LogWarning("PreparationView not found in the scene.");
            // }
            // else
            // {
            //     Debug.Log("PreparationView found: " + gameManager.preparationView.name);
            // }

            // // Set Up PauseView
            // gameManager.pauseView = GameObject.Find("PauseView");
            // if (gameManager.pauseView == null)
            // {
            //     Debug.LogWarning("PauseView not found in the scene.");
            // }
            // else
            // {
            //     Debug.Log("PauseView found: " + gameManager.pauseView.name);
            // }

            // // Set Up GameOverView
            // gameManager.gameOverView = GameObject.Find("GameOverView");
            // if (gameManager.gameOverView == null)
            // {
            //     Debug.LogWarning("GameOverView not found in the scene.");
            // }
            // else
            // {
            //     Debug.Log("GameOverView found: " + gameManager.gameOverView.name);
            // }

            // // Set Up WinView
            // gameManager.winView = GameObject.Find("WinView");   
            // if (gameManager.winView == null)
            // {
            //     Debug.LogWarning("WinView not found in the scene.");
            // }
            // else
            // {
            //     Debug.Log("WinView found: " + gameManager.winView.name);
            // }

            // Set Up SpawnManager
            SpawnManagerScript spawnManager = FindObjectOfType<SpawnManagerScript>();
            
            if (spawnManager != null)
            {
                // Set enemy data BEFORE changing state
                spawnManager.enemiesToSpawn = gameManager.enemyCompositionDataSO.GetEnemyComposition(gameManager.currentLevel);
            }

            gameManager.spawnManager = spawnManager;
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
        // Load the main menu scene
        SceneManager.LoadScene("MainMenu");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
