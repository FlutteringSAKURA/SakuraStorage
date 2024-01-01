using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DressingTableManager : MonoBehaviour
{
    public Transform dressingTables;
    
    List<GameObject> stuffs = new List<GameObject>();
    int currentStuffNum = 0;

    private void Awake() {
       // dressingTables = transform.Find("dressingTables");
    }
    private void Start()
    {
            

        //% 집어넣기
        ChoiceStuff(dressingTables, stuffs);
        //% 선택하기
        ShowStuff(stuffs, currentStuffNum);

    }
    private void ChoiceStuff(Transform dressingTable, List<GameObject> stuffsList)
    {
        foreach (Transform stuff in dressingTable)
        {
            stuff.gameObject.SetActive(false);
            stuffsList.Add(stuff.gameObject);
        }
    }

    private void ShowStuff(List<GameObject> stuffBox, int stuffNum)
    {
        for (int i = 0; i < stuffBox.Count; i++)
        {
            stuffBox[i].SetActive(false);
        }
        stuffBox[stuffNum].SetActive(true);
    }


    public void ChangeStuff()
    {
        currentStuffNum++;
        if (currentStuffNum > stuffs.Count - 1)
        {
            currentStuffNum = 0;
        }
        ShowStuff(stuffs, currentStuffNum);
        // print("화장대 갯수: " + stuffs.Count);
    }

    public void SaveCurrentTableStuffs()
    {
        PlayerPrefs.SetInt("stuffs",currentStuffNum);
        SceneManager.LoadScene("dressupSakuraTEST02");
    }


}
