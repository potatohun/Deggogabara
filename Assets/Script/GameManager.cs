using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    

    public AudioSource uiAudioMaster;
    public AudioSource bgmAudioMaster;

    [Header("È¿°úÀ½")]
    public AudioClip btnSwap;
    public AudioClip btnSelect;

    public MapData map;

    private void OnEnable()
    {
    }

    public void SelectMap()
    {
        ChapterManager ChapterManager  = GameObject.Find("ChapterManager").GetComponent<ChapterManager>();
        map = ChapterManager.stageList[ChapterManager.selectedStage].GetComponent<Stage>().map;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.F5))
        {
            SceneManager.LoadScene("Title");
        }
    }
}
