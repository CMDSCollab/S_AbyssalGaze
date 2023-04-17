using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Delegate : Singleton<M_Delegate>
{
    void Start()
    {
        M_Machine.Instance.MachineOnDive += M_Depth.Instance.GetCurrentDepth;
        M_Machine.Instance.MachineOnDive += M_Enemy.Instance.EnemyGenerationStop;

        M_Machine.Instance.MachineOnGround += M_Enemy.Instance.EnemyGenerationProcess;

        M_Machine.Instance.MachineOnHit += M_MachineValue.Instance.OxygenDecrease;
        M_Machine.Instance.MineComplete += M_MachineValue.Instance.OxygenIncrease;
    }
}
