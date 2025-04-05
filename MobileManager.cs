using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileManager : MonoBehaviour
{
    [Header("Trainings")]
    [SerializeField] private Transform trainings;
    [SerializeField] private GameObject mobileTrainings;
    [Header("Trainings")]
    [SerializeField] private Transform techs;
    [SerializeField] private GameObject mobileTechs;
    [Header("Trainings")]
    [SerializeField] private Transform upgs;
    [SerializeField] private GameObject mobileUpgs;

    private void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            for(int i = 1; i < 12; i++) {
                trainings.GetChild(i).gameObject.SetActive(false);
            }
            mobileTrainings.SetActive(true);

            for (int i = 1; i < 15; i++)
            {
                techs.GetChild(i).gameObject.SetActive(false);
            }
            mobileTechs.SetActive(true);

            for (int i = 2; i < 15; i++)
            {
                upgs.GetChild(i).gameObject.SetActive(false);
            }
            mobileUpgs.SetActive(true);

            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }
        else
        {
            for (int i = 1; i < 12; i++)
            {
                trainings.GetChild(i).gameObject.SetActive(true);
            }
            mobileTrainings.SetActive(false);

            for (int i = 1; i < 15; i++)
            {
                techs.GetChild(i).gameObject.SetActive(true);
            }
            mobileTechs.SetActive(false);

            for (int i = 2; i < 15; i++)
            {
                upgs.GetChild(i).gameObject.SetActive(true);
            }
            mobileUpgs.SetActive(false);
        }
    }
}
