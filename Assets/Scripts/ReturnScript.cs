using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnScript : MonoBehaviour
{
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
