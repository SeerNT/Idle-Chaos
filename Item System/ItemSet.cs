using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(menuName = "Item/ItemSet")]
public class ItemSet : ScriptableObject
{
    public string id;
    public string itemSetName;
    public int completionStatus = 0;

    public GameItem[] itemsIncluded = new GameItem[7];
    public float bonusHP = 0;
    public float bonusArm = 0;
    public float bonusMgcArm = 0;
    public float bonusPP = 0;
    public float bonusMP = 0;
    public float bonusCrit = 0;
    public float bonusPrc = 0;
    public float bonusLuck = 0;
    public float bonusAS = 0;
    public float bonusManaReg = 0;
    public float bonusHpReg = 0;

    public Dictionary<string, float> givenBonuses = new Dictionary<string, float>();

    public void InitSet()
    {
        if (bonusHP != 0) { givenBonuses.Add("HP", bonusHP); }
        if (bonusArm != 0) { givenBonuses.Add("ARM", bonusArm); }
        if (bonusMgcArm != 0) { givenBonuses.Add("MARM", bonusMgcArm); }
        if (bonusMP != 0) { givenBonuses.Add("MP", bonusMP); }
        if (bonusPP != 0) { givenBonuses.Add("PP", bonusPP); }
        if (bonusCrit != 0) { givenBonuses.Add("CR", bonusCrit); }
        if (bonusPrc != 0) { givenBonuses.Add("PRC", bonusPrc); }
        if (bonusLuck != 0) { givenBonuses.Add("LC", bonusLuck); }
        if (bonusAS != 0) { givenBonuses.Add("AS", bonusAS); }
        if (bonusManaReg != 0) { givenBonuses.Add("MREG", bonusManaReg); }
        if (bonusHpReg != 0) { givenBonuses.Add("HPREG", bonusHpReg); }
    }
}
