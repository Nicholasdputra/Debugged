using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HubScript : MonoBehaviour
{
    public float currentCharge;
    public float maxCharge;

    public Slider chargeBar;

    // Start is called before the first frame update
    void Start()
    {
        chargeBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}