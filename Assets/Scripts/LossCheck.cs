using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LossCheck : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            Debug.Log("Enemy has reached the loss check point.");
            GameManagerScript.Instance.state = 3; // Set to Round Lose State
        }
    }
}
