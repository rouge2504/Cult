using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private int clicks;
    
    [Header("UI")]
    public TextMeshProUGUI clicksText;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateClicks()
    {
        clicks++;
        clicksText.text = clicks.ToString() + " Souls";
    }
}
