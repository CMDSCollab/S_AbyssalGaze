using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Repository",menuName ="Abyssal Gaze/Repository")]
public class SO_Repository : ScriptableObject
{
    public MineralInfo[] minerals;
}

[System.Serializable]
public class MineralInfo
{
    public Sprite image;
    public MineralType type;
    public int minDepth;
    public int maxDepth;
    public int emergePossibility;
    public Color lightColor;
    public int difficulty;
}

public enum MineralType { M1,M2,M3,M4,M5,M6,M7,M8,M9,M10}