using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SM_MiningMachine : MonoBehaviour
{
    MMS_Base state_Current;
    MMS_Falling state_Falling = new MMS_Falling();
    MMS_OnGround state_OnGround = new MMS_OnGround();

    public Rigidbody2D rb;
    public float moveSpeed;
    public LayerMask layer_Ground;
    public LineRenderer line; 

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        line = GetComponent<LineRenderer>();

        state_Current = state_Falling;

        state_Current.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        state_Current.UpdateState(this);
    }

    private void OnDrawGizmos()
    {
     
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Ground"))
    //    {
    //        state_Current = state_OnGround;
    //        state_Current.EnterState(this);
    //        Debug.Log("dsadasasd");
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Ground"))
    //    {
    //        state_Current = state_OnGround;
    //        state_Current.EnterState(this);
    //    }
    //}
}
