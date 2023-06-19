using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Abyssal Gaze/Skill")]
public class SO_Skill : ScriptableObject
{
    public string skillName;
    public Sprite skillIcon;
    [TextArea(2, 2)]
    public string skillDes;
    public SkillInfo SkillInfo;
    public UpgradeRequire[] upgradeRequires;
}

public enum SkillType
{
    DmgUp25,
    AtkSpeedUp30,
    Unlock,
    PelletUp2,
    ChargeSpdUp20,
    MoveSpdUp20,
    WeaponRotateSpd20,
    VisibilityArc25,
    RepairCostDecrease30,
    MiningSpd15,
    DmgRangedDecrease10,
    DmgMeleeDecrease10,
    StunDurationDecrease25,
    DmgFromAllAtkDecrease15,
    ShellChanceUp10,
}

public enum TargetType
{
    Rifle,
    Minigun,
    Shotgun,
    Laser,
    Machine,
    Shell,
}

[System.Serializable]
public class SkillInfo
{
    public SkillType skillType;
    public TargetType targetType;
}

[System.Serializable]
public class UpgradeRequire
{
    public MineralType mineralType;
    public int number;
}