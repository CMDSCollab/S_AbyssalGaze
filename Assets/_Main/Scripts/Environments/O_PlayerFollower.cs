using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class O_PlayerFollower : MonoBehaviour
{
    private Vector3 initialPos;
    public Transform targetPlayer;
    private Vector3 offset;

    void Start()
    {
        offset = transform.position - targetPlayer.position;
        offset = new Vector3(0, offset.y, 0);
    }

    public void AlignPos()
    {
        transform.position = new Vector3(0, targetPlayer.position.y, 0) + offset;
    }
}
