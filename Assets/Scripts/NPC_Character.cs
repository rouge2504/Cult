using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC_Character
{
    public string name;
    public float timeToAddSoul;
    public string namePrefab;
    public string description;

    public NPC_Character(string name, string namePrefab, float timeToAddSoul, string description)
    {
        this.name = name;
        this.namePrefab = namePrefab;
        this.timeToAddSoul = timeToAddSoul;
        this.description = description;
    }
}
