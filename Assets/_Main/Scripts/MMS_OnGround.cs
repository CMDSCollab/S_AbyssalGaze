using UnityEngine;

public class MMS_OnGround : MMS_Base
{
    public override void EnterState(SM_MiningMachine stateManager)
    {
        
    }

    public override void ExitState(SM_MiningMachine stateManager)
    {
       
    }

    public override void UpdateState(SM_MiningMachine stateManager)
    {
        float horiAxis = Input.GetAxis("Horizontal");
        float verAxis = Input.GetAxis("Vertical");
        //Vector3 direction = new Vector3(horiAxis, 0, verAxis).normalized;
        Vector3 direction = new Vector3(horiAxis, verAxis, 0).normalized;
        if (direction != Vector3.zero)
        {
            stateManager.rb.velocity = direction * stateManager.moveSpeed;
        }
    }
}
