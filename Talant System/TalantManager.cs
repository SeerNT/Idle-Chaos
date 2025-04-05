using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TalantManager : MonoBehaviour
{
    public static float automationSpeed = 1;
    public static float herbAutomationSpeed = 1;

    public static bool bloodUnlocked;

    public static bool isAutoMoment = false;

    public int talantPoints;

    public Text pointsText;

    public Text talantPointsInfoText;

    public void OnPointsChange()
    {
        pointsText.text = "Points:" + talantPoints.ToString();
        talantPointsInfoText.text = "Points:" + talantPoints.ToString();
    }
}
