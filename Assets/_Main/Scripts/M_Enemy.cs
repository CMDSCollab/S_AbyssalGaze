using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class M_Enemy : MonoBehaviour
{
    public float spawnTime;
    private float timer;
    public GameObject pre_SeaSnake;
    public GameObject pre_Bullet;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (M_Machine.instance.isOnGround) timer += Time.deltaTime;
        else timer = 0;

        if (timer>spawnTime)
        {
            MonsterGeneration();
            timer = 0;
        }
    }

    void MonsterGeneration()
    {
        int genNum = Random.Range(1, 3);
        for (int i = 0; i < genNum; i++)
        {
            Vector3 spawnPos = new Vector3(Random.Range(-7, 7), M_Machine.instance.transform.position.y, Random.Range(-4, 4));
            Transform snake = Instantiate(pre_SeaSnake, spawnPos, Quaternion.Euler(90,0,0)).transform;
            Sequence s = DOTween.Sequence();
            s.AppendInterval(0.5f);
            s.AppendCallback(() => ShootBullet(snake.GetChild(0)));
            s.AppendCallback(() => Destroy(snake.gameObject, 1f));
      
        }
        void ShootBullet(Transform pos)
        {
            Transform bullet = Instantiate(pre_Bullet, pos.position, Quaternion.identity).transform;
            Vector3 direction = (M_Machine.instance.transform.position - bullet.transform.position).normalized;
            bullet.GetComponent<Rigidbody>().AddForce(direction * 10, ForceMode.Impulse);
        }
    }
}
