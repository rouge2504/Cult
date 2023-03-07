using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public static NPCManager instance = null;

    public NPC_Character[] npcs;

    public List<GameObject> actualNPCs;

    private void Awake()
    {
        
        instance = this;
        actualNPCs = new List<GameObject>();
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
                new NPC_Character("Zombo", "Bapho", 0.5f, "Hola"),
                new NPC_Character("Zimbo", "Baphy", 0.5f, "Hola"),
                
            };


            SaveSystem.Instance.WriteNPC(npcs, GameConstants.CHARACTER_DATA);
            
        }
    }

    public void Add(GameObject npcTemp)
    {
        actualNPCs.Add(npcTemp);
    }

}
