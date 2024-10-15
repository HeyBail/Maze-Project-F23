using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject targetText;
    public Slider heathBarSlider;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void refreshTargetText() 
    {
        targetText.GetComponent<TextMeshProUGUI>().text = "Targets: " + GameObject.FindGameObjectsWithTag("Target").Length + "/3";
        //check how many targets are left / of total
    }

    public void refreshPlayerHealthText(float health) 
    {
        heathBarSlider.value = health;
        //check how much health the player has 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
