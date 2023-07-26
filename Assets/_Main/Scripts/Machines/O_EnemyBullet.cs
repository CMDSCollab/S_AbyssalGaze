using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_EnemyBullet : MonoBehaviour
{
    public int damageAmount;
    private float lifeSpan = 5;

    private void Start()
    {
        if (gameObject.name!="Poke Detection") Destroy(gameObject, lifeSpan);
    }
}
