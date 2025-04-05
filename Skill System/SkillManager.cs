using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillManager : MonoBehaviour
{
    public int skillpoints;

    public Text pointsText;

    public Text skillPointsInfoText;

    public void OnPointsChange()
    {
        pointsText.text = "Points:" + skillpoints.ToString();
        skillPointsInfoText.text = "Points:" + skillpoints.ToString();
    }

    void Update()
    {
        pointsText.text = "Points:" + skillpoints.ToString();
        skillPointsInfoText.text = "Points:" + skillpoints.ToString();
    }
}
