using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class M_Start : MonoBehaviour
{
    public Transform[] pivots_Upper;
    public Transform[] pivots_Bottom;
    public Transform lid_Upper;
    public Transform lid_Bottom;
    public GameObject m_Light;
    public float time_Delay;
    public float time_Blink;
    public float time_Open;
    public CanvasGroup toFade;
    public float time_Fade;
    public Transform eyeBall;

    private void Start()
    {
        string[] world1BgAudio = new string[1] { "Cover" };
        M_Audio.PlayLoopAudio(world1BgAudio);
        InitializeScene();
        PlayOpenAnimation();
    }

    public void OnClick_Start()
    {
        M_Audio.PlayOneShotAudio("Button Click");
        Debug.Log("ENtered");
        StartCoroutine(StartGame());
    }

    public void OnClick_Exit()
    {
        M_Audio.PlayOneShotAudio("Button Click");
        Application.Quit();
    }

    void InitializeScene()
    {
        m_Light.SetActive(false);
        lid_Bottom.position = pivots_Bottom[0].position;
        lid_Upper.position = pivots_Upper[0].position;
    }

    void PlayOpenAnimation()
    {
        Sequence s = DOTween.Sequence();
        s.AppendInterval(time_Delay);
        s.AppendCallback(() => m_Light.SetActive(true));
        s.AppendInterval(time_Blink);
        s.AppendCallback(() => m_Light.SetActive(false));
        s.AppendInterval(time_Blink);
        s.AppendCallback(() => m_Light.SetActive(true));
        s.AppendCallback(() => LidOpenTogether(1));
    }

    void LidOpenTogether(int targetState)
    {
        lid_Bottom.DOMove(pivots_Bottom[targetState].position, time_Open);
        lid_Upper.DOMove(pivots_Upper[targetState].position, time_Open);
    }

    IEnumerator StartGame()
    {
        toFade.transform.parent.gameObject.SetActive(true);
        LidOpenTogether(2);
        Sequence s = DOTween.Sequence();
        s.Append(eyeBall.DOScale(1.2f, 0.7f));
        s.Append(eyeBall.DOScale(0.8f, 0.4f));
        yield return new WaitForSeconds(1.1f);
        DOTween.To(() => toFade.alpha, x => toFade.alpha = x, 1, time_Fade);
        yield return new WaitForSeconds(time_Fade);
        SceneManager.LoadScene(1);
        yield return new WaitForSeconds(0.2f);
        DOTween.To(() => toFade.alpha, x => toFade.alpha = x, 0, time_Fade).OnComplete(()=>toFade.transform.parent.gameObject.SetActive(false));
    }
}
