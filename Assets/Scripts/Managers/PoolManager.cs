using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public static PoolManager instance;

    public SkullPool skullPool;
    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }


}
