using UnityEngine;

public class MMS_Falling : MMS_Base
{
    public override void EnterState(SM_MiningMachine sm)
    {
    
    }

    public override void ExitState(SM_MiningMachine sm)
    {
        throw new System.NotImplementedException();
    }

    public override void UpdateState(SM_MiningMachine sm)
    {
        RaycastHit hitInfo;
        //Debug.DrawRay(sm.rb.transform.position, sm.rb.transform.forward, Color.cyan);
        if (Physics.Raycast(sm.transform.position,Vector3.forward, out hitInfo))
        {
            sm.line.SetPosition(0, sm.transform.position);
            sm.line.SetPosition(1, hitInfo.point);
            Debug.Log(hitInfo.collider.gameObject.name);
            //Debug.DrawLine(sm.rb.position, hitInfo.point);
            //Debug.DrawRay(sm.rb.transform.position, sm.rb.transform.forward,Color.cyan);
        }
        //if (Physics.Raycast(stateManager.rb.position, stateManager.rb.position, out hitInfo))
        //{
        //    Debug.Log(hitInfo.collider.gameObject.name);
        //}
        float horiAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horiAxis, 0, verAxis).normalized;
        Vector3 direction = new Vector3(horiAxis, verAxis, 0).normalized;
        if (direction != Vector3.zero)
        {
            sm.rb.velocity = direction * sm.moveSpeed;
        }
        else
        {
            sm.rb.velocity = Vector3.zero;
        }
    }
}
