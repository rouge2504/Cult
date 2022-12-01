using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPC_Character
{
    public string name;
    public float timeToAddSoul;
    public string description;

    public NPC_Character(string name, float timeToAddSoul, string description)
    {
        this.name = name;
        this.timeToAddSoul = timeToAddSoul;
        this.description = description;
    }
}
