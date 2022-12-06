using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameplayManager : MonoBehaviour
{
    public static GameplayManager instance;
    private int clicks;

    public int Clicks
    {
        get
        {
            return clicks;
        }

        set
        {
            clicks = value;
            clicksText.text = clicks.ToString() + " Souls";
        }
    }
    
    [Header("UI")]
    public TextMeshProUGUI clicksText;

    private GameData gameData = new GameData();
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
        
        InitGame();
    }

    void InitGame()
    {
        gameData = (GameData)SaveSystem.Instance.Read(GameConstants.GAME_DATA, gameData.GetType());
        Clicks = gameData.clicks;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateClicks()
    {
        Clicks++;
        gameData.clicks = Clicks;
        SaveSystem.Instance.Write(gameData, GameConstants.GAME_DATA);
        PoolManager.instance.skullPool.Next();
    }
}
