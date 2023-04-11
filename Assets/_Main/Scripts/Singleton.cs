using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T:MonoBehaviour
{
    private static T instance;

    //private T () { }

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                //GameObject go = new GameObject();
                //go.name = typeof(T).ToString();
                instance = FindObjectOfType<T>();
            }
            return instance;
        }
    }
}
