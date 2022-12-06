using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullParticleObject : MonoBehaviour
{
    public ParticleSystem particleSystem;

    public void OnEnable()
    {
        StartCoroutine(Disappear());
    }

    public IEnumerator Disappear()
    {
        yield return new WaitForSeconds(particleSystem.duration);
        this.gameObject.SetActive(false);
    } 
}
