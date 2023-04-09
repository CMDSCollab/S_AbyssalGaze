using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Mineral : MonoBehaviour
{
    public float posShrinkage;
    public int spawnNum;
    public GameObject pre_Mineral;
    public static M_Mineral instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
