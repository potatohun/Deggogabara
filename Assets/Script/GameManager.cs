using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        var obj = FindObjectsOfType<GameManager>();
        if (obj.Length == 1)
        {
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public GameObject map;

    public void SelectMap()
    {
        ChapterManager ChapterManager  = GameObject.Find("ChapterManager").GetComponent<ChapterManager>();
        GameManager.instance.map = ChapterManager.stageList[ChapterManager.selectedStage].GetComponent<Stage>().map;
    }
}
