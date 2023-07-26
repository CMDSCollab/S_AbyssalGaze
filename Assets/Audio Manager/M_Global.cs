using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_Global : MonoBehaviour
{
    private static M_Global instance;
    public static M_Global Instance { get { return instance; } }

    public SO_AudioRepo repo_Audio;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else Destroy(gameObject);
    }
}
