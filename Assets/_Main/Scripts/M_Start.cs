using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_Start : MonoBehaviour
{
    private void Start()
    {
        string[] world1BgAudio = new string[1] { "Cover" };
        M_Audio.PlayLoopAudio(world1BgAudio);
    }

    public void OnClick_Start()
    {
        M_Audio.PlayOneShotAudio("Button Click");
        SceneManager.LoadScene(1);
    }

    public void OnClick_Exit()
    {
        M_Audio.PlayOneShotAudio("Button Click");
        Application.Quit();
    }
}
