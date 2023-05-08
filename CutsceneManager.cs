using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private Transform cutscenes;
    [SerializeField] private GameObject[] notCutsceneObjs = new GameObject[10];

    [SerializeField] private AutoSave saveSystem;

    [SerializeField] private Button choice1;
    [SerializeField] private Button choice2;

    [SerializeField] private GameObject dialogue;
    [SerializeField] private GameObject consequence1;
    [SerializeField] private GameObject consequence2;

    private bool[] shownCutscenes = new bool[1];

    public static bool isShowing;

    public void Awake()
    {
        choice1.onClick.AddListener(delegate { Choose(1); });
        choice2.onClick.AddListener(delegate { Choose(2); });
    }

    public void Choose(int choice)
    {
        dialogue.SetActive(false);

        if (choice == 1)
        {
            consequence1.SetActive(true);
            StartCoroutine(AfterCutsceneEffect());
        }
        else if (choice == 2)
        {
            consequence2.SetActive(true);
            StartCoroutine(AfterCutsceneEffect());
        }
    }

    public IEnumerator AfterCutsceneEffect()
    {
        yield return new WaitForSeconds(5);
        isShowing = false;
        foreach (GameObject obj in notCutsceneObjs)
        {
            obj.SetActive(true);
        }
        cutscenes.gameObject.SetActive(false);
    }

    public void ShowCutscene(int id)
    {
        cutscenes.GetChild(id - 1).gameObject.SetActive(true);
        
        foreach (GameObject obj in notCutsceneObjs)
        {
            obj.SetActive(false);
        }
        isShowing = true;
        saveSystem.SaveWithout();
    }

    public void Save()
    {
        SaveSystem.SaveCutscenes(shownCutscenes);
    }

    public void Load()
    {
        CutsceneData data = SaveSystem.LoadCutscenes();

        if (data != null)
        {
            shownCutscenes = data.shownCutscenes;
            isShowing = data.isShowing;

            if (isShowing)
            {
                ShowCutscene(1);
                foreach (GameObject obj in notCutsceneObjs)
                {
                    obj.SetActive(false);
                }
            }
            else
            {
                cutscenes.gameObject.SetActive(false);
            }
        }
        else
        {
            cutscenes.gameObject.SetActive(false);
        }

    }
}
