using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HubScript : MonoBehaviour
{
    [Header("Charge Settings")]
    public float currentCharge;
    public float maxCharge;
    // public Slider chargeBar;
    public TextMeshProUGUI chargeText;

    [Header("Load Settings")]
    public int currentLoad;
    public int maxLoad;
    public TextMeshProUGUI loadText;


    // Start is called before the first frame update
    void Start()
    {
        // chargeBar = GetComponent<Slider>();
    }

    // Update is called once per frame
    void Update()
    {
        SetUI();
    }

    void SetUI()
    {
        // chargeBar.value = currentCharge / maxCharge;
        chargeText.text = currentCharge + "/" + maxCharge;
        loadText.text = currentLoad + "/" + maxLoad;
    }
}