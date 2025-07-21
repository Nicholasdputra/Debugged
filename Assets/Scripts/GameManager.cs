using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int state;
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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        state = 0; // Start in Preparation Phase
    }

    void Update()
    {
        // Handle state transitions or updates here
        switch (state)
        {
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
                // In-Game Phase logic
                if (!CheckIfActive(inGameView))
                {
                    preparationView.SetActive(false);
                    inGameView.SetActive(true);
                    pauseView.SetActive(false);
                    gameOverView.SetActive(false);
                    winView.SetActive(false);
                }
                break;
            case 2:
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
