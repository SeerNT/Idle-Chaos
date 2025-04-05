using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StatUpgPage : MonoBehaviour
{
    [Header("Upgs")]
    [SerializeField] private Transform[] upgs1 = new Transform[3];
    [SerializeField] private Transform[] upgs2 = new Transform[3];
    [SerializeField] private Transform[] upgs3 = new Transform[1];

    public int page = 0;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangePage);
    }

    void ChangePage()
    {
        if (page == 1)
        {
            upgs1[0].localPosition = new Vector3(-568.4f, upgs1[0].localPosition.y, upgs1[0].localPosition.z);
            upgs1[1].localPosition = new Vector3(-568.4f, upgs1[1].localPosition.y, upgs1[1].localPosition.z);
            upgs1[2].localPosition = new Vector3(-568.4f, upgs1[2].localPosition.y, upgs1[2].localPosition.z);

            for (int i = 0; i < upgs2.Length; i++)
            {
                upgs2[i].localPosition = new Vector3(-10000, upgs2[i].localPosition.y, upgs2[i].localPosition.z);
            }
            for (int i = 0; i < upgs3.Length; i++)
            {
                upgs3[i].localPosition = new Vector3(-10000, upgs3[i].localPosition.y, upgs3[i].localPosition.z);
            }
        }

        if (page == 2)
        {
            upgs2[0].localPosition = new Vector3(-568.4f, upgs2[0].localPosition.y, upgs2[0].localPosition.z);
            upgs2[1].localPosition = new Vector3(-568.4f, upgs2[1].localPosition.y, upgs2[1].localPosition.z);
            upgs2[2].localPosition = new Vector3(-568.4f, upgs2[2].localPosition.y, upgs2[2].localPosition.z);

            for (int i = 0; i < upgs1.Length; i++)
            {
                upgs1[i].localPosition = new Vector3(-10000, upgs1[i].localPosition.y, upgs1[i].localPosition.z);
            }
            for (int i = 0; i < upgs3.Length; i++)
            {
                upgs3[i].localPosition = new Vector3(-10000, upgs3[i].localPosition.y, upgs3[i].localPosition.z);
            }
        }

        if (page == 3)
        {
            upgs3[0].localPosition = new Vector3(-568.4f, upgs3[0].localPosition.y, upgs3[0].localPosition.z);

            for (int i = 0; i < upgs1.Length; i++)
            {
                upgs1[i].localPosition = new Vector3(-10000, upgs1[i].localPosition.y, upgs1[i].localPosition.z);
            }
            for (int i = 0; i < upgs2.Length; i++)
            {
                upgs2[i].localPosition = new Vector3(-10000, upgs2[i].localPosition.y, upgs2[i].localPosition.z);
            }
        }
    }
}
