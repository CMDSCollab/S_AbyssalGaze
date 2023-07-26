using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using UnityEngine.UI;

public class OE_BossMouth : MonoBehaviour
{
    private TentaclePoke[] tentacles;
    public float attackCoolDown;
    private float attackTimer;
    private bool isAttacking = false;

    public float maxHealth;
    protected float currentHealth;
    private bool isDead = false;
   public  MMF_Player mmf_shakeCam;
    public GameObject allBlackHide;

    void Start()
    {
        currentHealth = maxHealth;
        tentacles = new TentaclePoke[16];
        for (int i = 0; i < 16; i++)
        {
            tentacles[i] = transform.GetChild(i).GetComponent<TentaclePoke>();
        }

        ResetAttackTimer();
    }

    void Update()
    {
        attackTimer -= Time.deltaTime;
        if (attackTimer < 0 && !isAttacking)
        {
            if (Random.Range(0, 10) > 5) Poke_MeanTime(); 
            else StartCoroutine(Poke_Interval());
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            GameObject hitExplosion = Instantiate(FindObjectOfType<M_Firearm>().fx_ExplosionSmall, collision.transform.position, Quaternion.identity);
            Destroy(hitExplosion, 2f);
            currentHealth -= collision.gameObject.GetComponentInParent<O_Bullet>().damage;
            Destroy(collision.transform.parent.gameObject);
            if (currentHealth <= 0 && !isDead) StartCoroutine(BossMouthDefeated());
        }
    }

    IEnumerator BossMouthDefeated()
    {
        isDead = true;
        M_Audio.PlayOneShotAudio("Boss Death");
        attackTimer = 20000;
        int[] tenIndexes = new int[tentacles.Length];
        for (int i = 0; i < tenIndexes.Length; i++) tenIndexes[i] = i;
        ChangeTentaclesState(tenIndexes, BossTantacleState.Sway);
        foreach (TentaclePoke tentacle in tentacles)
        {
            tentacle.smoothSpeed = 0.01f;
            tentacle.trailSpeed = 3000;
            tentacle.wiggleSpeed = 10;
            tentacle.wiggleMagnitude = 80;
        }
        yield return new WaitForSeconds(2f);
        mmf_shakeCam.PlayFeedbacks();
        yield return new WaitForSeconds(2f);
        foreach (TentaclePoke tentacle in tentacles)
        {
            tentacle.targetDistance = 0.3f;
            tentacle.smoothSpeed = 0.02f;
            tentacle.trailSpeed = 1000;
            tentacle.wiggleSpeed = 5;
            tentacle.wiggleMagnitude = 60;
        }
        ChangeTentaclesState(tenIndexes, BossTantacleState.PokeCam);
        yield return new WaitForSeconds(0.3f);
        GameObject.Find("Canvas").transform.Find("All Black").GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.2f);
        FindObjectOfType<M_BossFight>().GenerateBossSide(transform.position);
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }

    public void DamagedByLaser()
    {
        currentHealth -= Time.deltaTime * 10;
        if (currentHealth <= 0 && !isDead) StartCoroutine(BossMouthDefeated());
    }

    private void ResetAttackTimer()
    {
        attackTimer = Random.Range(attackCoolDown - attackCoolDown / 2, attackCoolDown + attackCoolDown / 2);
    }

    IEnumerator Poke_Interval()
    {
        isAttacking = true;
        float[] distances = GetAllTentacleDistance();
        int pokeNum = Random.Range(3, 6);
        int[] pokeIndexes = GetMinFloatIndexesInArray(distances, pokeNum);

        ChangeTentaclesState(pokeIndexes, BossTantacleState.Shrink);
        yield return new WaitForSeconds(1f);
        foreach (int tentacle in pokeIndexes)
        {
            ChangeTentaclesState(new int[1] { tentacle }, BossTantacleState.Poke);
            M_Audio.PlayOneShotAudio("Tentacle Attack");
            yield return new WaitForSeconds(0.5f);
        }
        yield return new WaitForSeconds(0.5f);
        ChangeTentaclesState(pokeIndexes, BossTantacleState.Sway);
        ResetAttackTimer();
        isAttacking = false;
    }

    private void Poke_MeanTime()
    {
        M_Audio.PlayOneShotAudio("Tentacle Attack");
        isAttacking = true;
        float[] distances = GetAllTentacleDistance();
        int pokeNum = Random.Range(3, 6);
        int[] pokeIndexes = GetMinFloatIndexesInArray(distances, pokeNum);

        Sequence s = DOTween.Sequence();
        s.AppendCallback(()=> ChangeTentaclesState(pokeIndexes, BossTantacleState.Shrink));
        s.AppendInterval(1);
        s.AppendCallback(() => ChangeTentaclesState(pokeIndexes, BossTantacleState.Poke));
        s.AppendInterval(1);
        s.AppendCallback(() => ChangeTentaclesState(pokeIndexes, BossTantacleState.Sway));
        s.AppendCallback(() => ResetAttackTimer());
        s.AppendCallback(() => isAttacking = false);
   
    }

    float[] GetAllTentacleDistance()
    {
        float[] distances = new float[tentacles.Length];
        for (int i = 0; i < distances.Length; i++)
        {
            Vector3 tenPos = new Vector3(tentacles[i].transform.position.x, 0, tentacles[i].transform.position.z);
            Vector3 macPos = new Vector3(M_Machine.Instance.transform.position.x, 0, M_Machine.Instance.transform.position.z);
            distances[i] = Vector3.Distance(tenPos, macPos);
        }
        return distances;
    }

    void ChangeTentaclesState(int[] Indexes, BossTantacleState targetState)
    {
        foreach (int index in Indexes) tentacles[index].state = targetState;
    }

    int[] GetMinFloatIndexesInArray(float[] array,int count)
    {
        int[] indexes = new int[count];
        float maxValue = Max(array);
        for (int i = 0; i < indexes.Length; i++)
        {
            float minValue = Min(array);
            //Debug.Log(minValue);
            for (int j = 0; j < array.Length; j++)
            {
                if (array[j] == minValue)
                {
                    indexes[i] = j;
                    array[j] = maxValue;
                }
            }
        }
        return indexes;

        float Min(float[] array)
        {
            float value = 0;
            bool hasValue = false;
            foreach (float x in array)
            {
                if (hasValue)
                {
                    if (x < value) value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            }
            return value;
        }

        float Max(float[] array)
        {
            float value = 0;
            bool hasValue = false;
            foreach (float x in array)
            {
                if (hasValue)
                {
                    if (x > value) value = x;
                }
                else
                {
                    value = x;
                    hasValue = true;
                }
            }
            return value;
        }
    }
}
