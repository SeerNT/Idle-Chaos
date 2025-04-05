using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainPage : MonoBehaviour
{
    [Header("Trainings")]
    [SerializeField] private Transform[] trains1 = new Transform[4];
    [SerializeField] private Transform[] trains2 = new Transform[4];
    [SerializeField] private Transform[] trains3 = new Transform[4];

    public int page = 0;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangePage);
    }

    void ChangePage()
    {
        if (page == 1)
        {
            trains1[0].localPosition = new Vector3(0, trains1[0].localPosition.y, trains1[0].localPosition.z);
            trains1[1].localPosition = new Vector3(0, trains1[1].localPosition.y, trains1[1].localPosition.z);
            trains1[2].localPosition = new Vector3(806.5f, trains1[2].localPosition.y, trains1[2].localPosition.z);
            trains1[3].localPosition = new Vector3(806.5f, trains1[3].localPosition.y, trains1[3].localPosition.z);

            for (int i = 0; i < trains2.Length; i++)
            {
                trains2[i].localPosition = new Vector3(-10000, trains2[i].localPosition.y, trains2[i].localPosition.z);
            }
            for (int i = 0; i < trains3.Length; i++)
            {
                trains3[i].localPosition = new Vector3(-10000, trains3[i].localPosition.y, trains3[i].localPosition.z);
            }
        }

        if (page == 2)
        {
            trains2[0].localPosition = new Vector3(0, trains2[0].localPosition.y, trains2[0].localPosition.z);
            trains2[1].localPosition = new Vector3(0, trains2[1].localPosition.y, trains2[1].localPosition.z);
            trains2[2].localPosition = new Vector3(806.5f, trains2[2].localPosition.y, trains2[2].localPosition.z);
            trains2[3].localPosition = new Vector3(806.5f, trains2[3].localPosition.y, trains2[3].localPosition.z);

            for (int i = 0; i < trains1.Length; i++)
            {
                trains1[i].localPosition = new Vector3(-10000, trains1[i].localPosition.y, trains1[i].localPosition.z);
            }
            for (int i = 0; i < trains3.Length; i++)
            {
                trains3[i].localPosition = new Vector3(-10000, trains3[i].localPosition.y, trains3[i].localPosition.z);
            }
        }

        if (page == 3)
        {
            trains3[0].localPosition = new Vector3(0, trains3[0].localPosition.y, trains3[0].localPosition.z);
            trains3[1].localPosition = new Vector3(0, trains3[1].localPosition.y, trains3[1].localPosition.z);
            trains3[2].localPosition = new Vector3(806.5f, trains3[2].localPosition.y, trains3[2].localPosition.z);
            trains3[3].localPosition = new Vector3(806.5f, trains3[3].localPosition.y, trains3[3].localPosition.z);

            for (int i = 0; i < trains1.Length; i++)
            {
                trains1[i].localPosition = new Vector3(-10000, trains1[i].localPosition.y, trains1[i].localPosition.z);
            }
            for (int i = 0; i < trains2.Length; i++)
            {
                trains2[i].localPosition = new Vector3(-10000, trains2[i].localPosition.y, trains2[i].localPosition.z);
            }
        }
    }
}
