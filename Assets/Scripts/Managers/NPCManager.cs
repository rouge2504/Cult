using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance = null;

    public NPC_Character[] npcs;

    private void Awake()
    {
        
        instance = this;
        Init();
    }

    void Start()
    {
    }

    void Init()
    {
        if (SaveSystem.Instance.Exists(GameConstants.CHARACTER_DATA))
        {
            npcs = SaveSystem.Instance.ReadNPC(GameConstants.CHARACTER_DATA);

        }
        else
        {
            npcs = new NPC_Character[]
            {
                new NPC_Character("Zombo", 0.5f, "Hola"),
                new NPC_Character("Zimbo", 0.5f, "Hola"),
                
            };


            SaveSystem.Instance.WriteNPC(npcs, GameConstants.CHARACTER_DATA);
            
        }
    }

}
