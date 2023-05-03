using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class M_Major : Singleton<M_Major>
{
    public SO_Repository repository;

    private void Start()
    {
        M_MineralPanel.Instance.InitializeMineralPanel();
        M_Mineral.Instance.GenerateCirclePivots();
        M_Depth.Instance.GenerateIntinialLevel();
    }
}
