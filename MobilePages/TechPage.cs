using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TechPage : MonoBehaviour
{
    [Header("Techs")]
    [SerializeField] private Transform[] techs1 = new Transform[3];
    [SerializeField] private Transform[] techs2 = new Transform[3];
    [SerializeField] private Transform[] techs3 = new Transform[3];
    [SerializeField] private Transform[] techs4 = new Transform[3];
    [SerializeField] private Transform[] techs5 = new Transform[3];

    public int page = 0;

    public void Start()
    {
        GetComponent<Button>().onClick.AddListener(ChangePage);
    }

    void ChangePage()
    {
        if (page == 1)
        {
            techs1[0].localPosition = new Vector3(0, -82, techs1[0].localPosition.z);
            techs1[1].localPosition = new Vector3(0, -560, techs1[1].localPosition.z);
            techs1[2].localPosition = new Vector3(880.89f, -82, techs1[2].localPosition.z);

            for (int i = 0; i < techs2.Length; i++)
            {
                techs2[i].localPosition = new Vector3(-10000, techs2[i].localPosition.y, techs2[i].localPosition.z);
            }
            for (int i = 0; i < techs3.Length; i++)
            {
                techs3[i].localPosition = new Vector3(-10000, techs3[i].localPosition.y, techs3[i].localPosition.z);
            }
            for (int i = 0; i < techs4.Length; i++)
            {
                techs4[i].localPosition = new Vector3(-10000, techs4[i].localPosition.y, techs4[i].localPosition.z);
            }
            for (int i = 0; i < techs5.Length; i++)
            {
                techs5[i].localPosition = new Vector3(-10000, techs5[i].localPosition.y, techs5[i].localPosition.z);
            }
        }

        if (page == 2)
        {
            techs2[0].localPosition = new Vector3(0, -82, techs2[0].localPosition.z);
            techs2[1].localPosition = new Vector3(0, -560, techs2[1].localPosition.z);
            techs2[2].localPosition = new Vector3(880.89f, -82, techs2[2].localPosition.z);

            for (int i = 0; i < techs1.Length; i++)
            {
                techs1[i].localPosition = new Vector3(-10000, techs1[i].localPosition.y, techs1[i].localPosition.z);
            }
            for (int i = 0; i < techs3.Length; i++)
            {
                techs3[i].localPosition = new Vector3(-10000, techs3[i].localPosition.y, techs3[i].localPosition.z);
            }
            for (int i = 0; i < techs4.Length; i++)
            {
                techs4[i].localPosition = new Vector3(-10000, techs4[i].localPosition.y, techs4[i].localPosition.z);
            }
            for (int i = 0; i < techs5.Length; i++)
            {
                techs5[i].localPosition = new Vector3(-10000, techs5[i].localPosition.y, techs5[i].localPosition.z);
            }
        }

        if (page == 3)
        {
            techs3[0].localPosition = new Vector3(0, -82, techs3[0].localPosition.z);
            techs3[1].localPosition = new Vector3(0, -560, techs3[1].localPosition.z);
            techs3[2].localPosition = new Vector3(880.89f, -82, techs3[2].localPosition.z);

            for (int i = 0; i < techs1.Length; i++)
            {
                techs1[i].localPosition = new Vector3(-10000, techs1[i].localPosition.y, techs1[i].localPosition.z);
            }
            for (int i = 0; i < techs2.Length; i++)
            {
                techs2[i].localPosition = new Vector3(-10000, techs2[i].localPosition.y, techs2[i].localPosition.z);
            }
            for (int i = 0; i < techs4.Length; i++)
            {
                techs4[i].localPosition = new Vector3(-10000, techs4[i].localPosition.y, techs4[i].localPosition.z);
            }
            for (int i = 0; i < techs5.Length; i++)
            {
                techs5[i].localPosition = new Vector3(-10000, techs5[i].localPosition.y, techs5[i].localPosition.z);
            }
        }

        if (page == 4)
        {
            techs4[0].localPosition = new Vector3(0, -82, techs4[0].localPosition.z);
            techs4[1].localPosition = new Vector3(0, -560, techs4[1].localPosition.z);
            techs4[2].localPosition = new Vector3(880.89f, -82, techs4[2].localPosition.z);

            for (int i = 0; i < techs1.Length; i++)
            {
                techs1[i].localPosition = new Vector3(-10000, techs1[i].localPosition.y, techs1[i].localPosition.z);
            }
            for (int i = 0; i < techs2.Length; i++)
            {
                techs2[i].localPosition = new Vector3(-10000, techs2[i].localPosition.y, techs2[i].localPosition.z);
            }
            for (int i = 0; i < techs3.Length; i++)
            {
                techs3[i].localPosition = new Vector3(-10000, techs3[i].localPosition.y, techs3[i].localPosition.z);
            }
            for (int i = 0; i < techs5.Length; i++)
            {
                techs5[i].localPosition = new Vector3(-10000, techs5[i].localPosition.y, techs5[i].localPosition.z);
            }
        }

        if (page == 5)
        {
            techs5[0].localPosition = new Vector3(0, -82, techs5[0].localPosition.z);
            techs5[1].localPosition = new Vector3(0, -560, techs5[1].localPosition.z);
            techs5[2].localPosition = new Vector3(880.89f, -82, techs5[2].localPosition.z);

            for (int i = 0; i < techs1.Length; i++)
            {
                techs1[i].localPosition = new Vector3(-10000, techs1[i].localPosition.y, techs1[i].localPosition.z);
            }
            for (int i = 0; i < techs2.Length; i++)
            {
                techs2[i].localPosition = new Vector3(-10000, techs2[i].localPosition.y, techs2[i].localPosition.z);
            }
            for (int i = 0; i < techs3.Length; i++)
            {
                techs3[i].localPosition = new Vector3(-10000, techs3[i].localPosition.y, techs3[i].localPosition.z);
            }
            for (int i = 0; i < techs4.Length; i++)
            {
                techs4[i].localPosition = new Vector3(-10000, techs4[i].localPosition.y, techs4[i].localPosition.z);
            }
        }
    }
}
