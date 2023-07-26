using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MoreMountains.Feedbacks;
using UnityEngine.SceneManagement;

public class M_BossFight : MonoBehaviour
{
    public GameObject pre_NoCenterGround;
    public GameObject pre_BossMouth;
    public GameObject pre_BossSide;

    public Image allBlack;
    private bool isNotEnteredFight = true;

    private GameObject bossGround;
    private GameObject bossSide;
    public Vector3 bottomLid_targetPos;
    public MMF_Player mmf_GameEnd;
    public Transform bottomLid;

    public Light light_EN;
    private bool isLightChange = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isLightChange)
        {
            light_EN.spotAngle = Mathf.Lerp(light_EN.spotAngle, 100, 5 * Time.deltaTime);
        }
    }

    public void GenerateBossGround(Vector3 targetPos)
    {
        if (isNotEnteredFight)
        {
            bossGround =  Instantiate(pre_NoCenterGround, targetPos, Quaternion.identity);
            Instantiate(pre_BossMouth, targetPos + new Vector3(0, 0.4f, 0), Quaternion.Euler(90, 0, 0));
            isNotEnteredFight = false;
        }
    }
    public void GenerateBossSide(Vector3 targetPos)
    {
        bossSide = Instantiate(pre_BossSide, targetPos, Quaternion.Euler(0,0,0));
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => DOTween.To(() => allBlack.color, x => allBlack.color = x, new Color(0, 0, 0, 0), 1));
        s.AppendInterval(1);
        s.AppendCallback(() => allBlack.gameObject.SetActive(false));
    }

    public void DestroyBossAndGround()
    {
        if (bossGround!=null) Destroy(bossGround);
        if (bossSide != null) Destroy(bossSide);
     
    }

    public void UIBlackFadeOut()
    {
        Sequence s = DOTween.Sequence();
        s.AppendCallback(() => DOTween.To(() => allBlack.color, x => allBlack.color = x, new Color(0, 0, 0, 0), 1));
        s.AppendInterval(1);
        s.AppendCallback(() => allBlack.gameObject.SetActive(false));
    }

    public void OpenEye()
    {
        bottomLid.GetComponent<Animator>().SetBool("IsOpen", true);
        //bottomLid.moveto
        //bottomLid.DOLocalMove(bottomLid_targetPos, 1);
    }

    public void DisableCamFollow()
    {
        FindObjectOfType<M_Camera>().enabled = false;
        isLightChange = true;
    }

    public void AllGameBlack()
    {
        allBlack.gameObject.SetActive(true);
        allBlack.GetComponent<Image>().enabled = true;
        DOTween.To(() => allBlack.color, x => allBlack.color = x, Color.black, 1);
        Sequence s = DOTween.Sequence();
        s.AppendInterval(2);
        s.AppendCallback(() => SceneManager.LoadScene(0));

    }
}
