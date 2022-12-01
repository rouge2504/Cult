using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance = null;

    private NPC_Character[] npcs;

    void Start()
    {
        instance = this;
        Init();
    }

    void Init()
    {
        if (SaveSystem.Instance.Exists(GameConstants.CHARACTER_DATA))
        {
            //npcs = (NPC_Character)SaveSystem.Instance.Read(npcs.GetType(), GameConstants.CHARACTER_DATA);
        }
        else
        {
            npcs = new NPC_Character[]
            {
                new NPC_Character("Zombo", 0.5f, "Hola"),
                new NPC_Character("Zimbo", 0.5f, "Hola"),
                
            };

            SaveSystem.Instance.Write(npcs, GameConstants.CHARACTER_DATA);
            
        }
    }

}
