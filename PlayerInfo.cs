using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using NT_Utils.Conversion;

public class PlayerInfo : MonoBehaviour
{
    public Text lvlText;
    public Text xpText;
    public Battle battle;
    public Slider xpBar;
    void Update()
    {
        if (battle.isSetup)
        {
            lvlText.text = "Lvl." + battle.player.lvl.ToString();
            xpText.text = NumberConversion.AbbreviateNumber(battle.player.xp, NumberConversion.StartAbbrevation.M) + "/" + NumberConversion.AbbreviateNumber(battle.player.xpCap, NumberConversion.StartAbbrevation.M);

            float dif = (battle.player.xpCap - battle.player.xp);
            float prer = dif / battle.player.xpCap;
            float r = 1 - prer;
            xpBar.value = r;
        }
    }
}
