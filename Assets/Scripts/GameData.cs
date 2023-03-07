using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData 
{
    public int clicks;

    public NPC_Character npcCharacter;

    public GameData()
    {
        clicks = 0;
    }
}
