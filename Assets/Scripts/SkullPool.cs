using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullPool : MonoBehaviour
{
    
    
    [SerializeField]
    private List<GameObject> childs;

    private int it = 0;

    private int Succesive
    {
        get
        {
            return it;
        }

        set
        {
            
            it = value;
            if (it > childs.Count - 1)
            {
                it = 0;
            }
        }
    }

    private void Start()
    {
        for(int i = 0; i < this.transform.childCount; i++)
        {
            childs.Add(this.transform.GetChild(i).gameObject);
        }

        ResetAll();
    }

    public void ResetAll()
    {
        foreach(GameObject child in childs)
        {
            child.SetActive(false);
        }
    }

    public void Next()
    {

        bool find = false;
        int loops = 0;
        while (!find)
        {
            if (!childs[Succesive].activeSelf)
            {
                childs[Succesive].SetActive(true);
                find = true;
            }

            if (Succesive == childs.Count - 1)
            {
                loops++;
            }

            if (loops > 2)
            {
                find = true;
                Debug.LogError("Not enough objects");
            }
            Succesive++;
        }

    }
}
