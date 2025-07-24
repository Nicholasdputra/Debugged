using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PassiveSoundsScript : MonoBehaviour
{
    public List<TeslaTrapScript> teslaTrapScripts;

    void OnAwake()
    {
        //Find current scene
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        if (currentSceneName.Contains("Level"))
        {
            //Find In Game View
            GameObject inGameView = GameObject.Find("InGameView");

            //Inside of in game view, find gameobjects that have 'Tesla' in their name
            if (inGameView != null)
            {
                // Get all children of InGameView that have 'Tesla' in their name
                Transform[] allChildren = inGameView.GetComponentsInChildren<Transform>();

                foreach (Transform child in allChildren)
                {
                    if (child.name.Contains("Tesla"))
                    {
                        // Add the TeslaTrapScript component to the child if it doesn't already have it
                        if (child.GetComponent<TeslaTrapScript>() != null)
                        {
                            teslaTrapScripts.Add(child.GetComponent<TeslaTrapScript>());
                        }
                        else
                        {
                            Debug.LogWarning("TeslaTrapScript not found on " + child.name);
                        }
                    }
                }
            }
        }

        StartCoroutine(CheckAnyTeslaTrapActive());
    }

    IEnumerator CheckAnyTeslaTrapActive()
    {
        while(GameManagerScript.Instance.state == 1) // Assuming state 1 means in-game phase
        {
            foreach (TeslaTrapScript teslaTrap in teslaTrapScripts)
            {
                if (teslaTrap.state == 1) // Assuming state 1 means active
                {
                    AudioManagerScript.Instance.PlaySFX(AudioManagerScript.Instance.teslaChargeSFXClip);
                    yield return new WaitForSeconds(1f); // Wait for 1 second before checking again
                    break;
                }
            }
        }
    }
}
