using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript Instance;
    public int state;
    // Define game states
    // -1 = Not In Level
    // 0 = Preparation Phase
    // 1 = In-Game Phase
    // 2 = Round Win State
    // 3 = Round Lose State
    // 4 = Paused State

    public GameObject preparationView;
    public GameObject pauseView;
    public GameObject gameOverView;
    public GameObject winView;
    public GameObject inGameView;

    public int currentLevel = 0;
    public EnemyCompositionDataSO enemyCompositionDataSO;
    public SpawnManagerScript spawnManager;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        state = -1; 
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().name.Contains("Level"))
        {
            // if (preparationView == null)
            // {
            //     preparationView = GameObject.Find("PreparationView");
            // }
            // if (pauseView == null)
            // {
            //     pauseView = GameObject.Find("PauseView");
            // }
            // if (gameOverView == null)
            // {
            //     gameOverView = GameObject.Find("GameOverView");
            // }
            // if (winView == null)
            // {
            //     winView = GameObject.Find("WinView");
            // }
            if (inGameView == null)
            {
                inGameView = GameObject.Find("InGameView");
            }
        }

        // Handle state transitions or updates here
        switch (state)
        {
            case -1:
                // Not In Level logic

                break;

            case 0:
                // Preparation Phase logic
                if (!CheckIfActive(preparationView))
                {
                    preparationView.SetActive(true);
                    inGameView.SetActive(false);
                    pauseView.SetActive(false);
                    gameOverView.SetActive(false);
                    winView.SetActive(false);
                }
                break;

            case 1:
                if (inGameView == null)
                {
                    inGameView = GameObject.Find("InGameView");
                    Debug.Log("InGameView found: " + inGameView.name);
                }
                // else
                // {
                //     Debug.Log("InGameView already exists: " + inGameView.name);
                // }

                if(spawnManager.enemiesLeft <= 0 && spawnManager.spawnCompleted)
                {
                    // All enemies defeated, change to win state
                    state = 2; // Change to win state
                }
                break;

            case 2:
                Debug.Log("State: " + state + " - Round Win State");
                // Round Win State logic
                if (!CheckIfActive(winView))
                {
                    preparationView.SetActive(false);
                    inGameView.SetActive(false);
                    pauseView.SetActive(false);
                    gameOverView.SetActive(false);
                    winView.SetActive(true);
                }
                break;

            case 3:
                // Round Lose State logic
                if (!CheckIfActive(gameOverView))
                {
                    preparationView.SetActive(false);
                    inGameView.SetActive(false);
                    pauseView.SetActive(false);
                    gameOverView.SetActive(true);
                    winView.SetActive(false);
                }
                break;

            case 4:
                // Paused State logic
                if (!CheckIfActive(pauseView))
                {
                    preparationView.SetActive(false);
                    inGameView.SetActive(false);
                    pauseView.SetActive(true);
                    gameOverView.SetActive(false);
                    winView.SetActive(false);
                }
                break;
                
        }
    }

    bool CheckIfActive(GameObject obj)
    {
        if (obj.activeInHierarchy)
        {
            // Debug.Log(obj.name + " is active.");
            return true;
        }
        else
        {
            // Debug.Log(obj.name + " is not active.");
            return false;
        }
    }
}
