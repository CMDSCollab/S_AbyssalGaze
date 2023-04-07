using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MMS_Base
{
    public abstract void EnterState(SM_MiningMachine sm);

    public abstract void UpdateState(SM_MiningMachine sm);

    public abstract void ExitState(SM_MiningMachine sm);

}
