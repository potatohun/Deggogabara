using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum UI
{
    Chapter,
    Stage
}

public class ChapterManager : MonoBehaviour
{
    Image chapterBackground;

    RectTransform chapterRect;
    RectTransform stageRect;

    TextMeshProUGUI chapterText;

    public Vector2 targetPosition; // 목표 위치
    public float duration = 0.3f; // 이동 시간

    private bool isMoving = false; // 현재 이동 중인지 확인하기 위한 변수
    private bool isSizing = false; // 현재 조정 중인지 확인하기 위한 변수

    public Chapter chapterState;
    public UI uiState;

    public List<RectTransform> chapterList;
    public List<GameObject> stageList;

    [SerializeField]
    public int lastChapter = 0;
    public int selectedStage = 0;

    public Chapter stage;

    public GameObject stagePrefab;

    [Header("챕터 사이즈")]
    public Vector2 focusSize = new Vector2(600,600);
    public Vector2 defaultSize = new Vector2(700,700);

    private void Awake()
    {
        chapterBackground = GameObject.Find("ChapterBackground").GetComponent<Image>();
        chapterRect = transform.Find("ChapterGroup").GetComponent<RectTransform>();
        stageRect = transform.Find("StageGroup").GetComponent<RectTransform>();
        chapterText = GameObject.Find("ChapterName").GetComponent<TextMeshProUGUI>();

        for (int i = 0; i < 3; i++)
        {
            chapterList.Add(transform.Find("ChapterGroup").GetComponentsInChildren<RectTransform>()[i + 1]);
        }
        DefaultSize();
    }   

    private void Update()
    {
        chapterText.text = chapterList[lastChapter].GetComponent<Chapter>().chapterName;
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            switch(uiState)
            {
                case UI.Chapter:
                    {
                        if (lastChapter < 2)
                        {
                            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
                            lastChapter++;
                            targetPosition = new Vector2(-960 * lastChapter, -250);
                            StartCoroutine(MoveUI(chapterRect, targetPosition, duration));
                            
                            SwapChapter();


                        }
                        return;
                    }
                case UI.Stage: 
                    {
                        if (selectedStage < stage.stageData.Count - 1)
                        {
                            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
                            selectedStage++;
                            SwapStage();
                        }
                        
                        return;
                    }   
            }
            
        } 
        if(Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            switch (uiState)
            {
                case UI.Chapter:
                    {
                        if (lastChapter > 0)
                        {
                            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
                            lastChapter--;
                            targetPosition = new Vector2(-960 * lastChapter, -250);
                            StartCoroutine(MoveUI(chapterRect, targetPosition, duration));

                            SwapChapter();
                        }
                        return;
                    }
                case UI.Stage:
                    {
                        if (selectedStage > 0)
                        {
                            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
                            selectedStage--;
                            SwapStage();
                        }
                        
                        return;
                    }
            }    
        }

        if(Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);
            switch (uiState)
            {
                case UI.Chapter:
                    {
                        selectedStage = 0;
                        foreach (var stage in stageList)
                        {
                            Destroy(stage);
                        }
                        stageList.Clear();

                        stage = chapterList[lastChapter].GetComponent<Chapter>();
                        
                        for(int i = 0; i < stage.stageData.Count; i++)
                        {
                            GameObject stg = Instantiate(stagePrefab, stageRect.transform);
                            stg.GetComponent<Stage>().stageNum = i;
                            stg.GetComponent<Stage>().map = stage.stageData[i];
                            stageList.Add(stg); 
                        }
                        SwapStage();


                        targetPosition = new Vector2(0, 35);
                        StartCoroutine(MoveUI(stageRect, targetPosition, duration));
                        StartCoroutine(ResizeUI(chapterList[lastChapter], defaultSize, duration));
                        foreach (var chapter in chapterList)
                        {
                            if (chapter != chapterList[lastChapter])
                            {
                                StartCoroutine(FadeUI(chapter.GetComponent<Image>(), 0, duration));
                            }
                        }
                        ColorReset();
                        uiState = UI.Stage;
                        return;
                    }
                case UI.Stage:
                    {
                        GameManager.instance.SelectMap();
                        SceneManager.LoadScene(3);
                        return;
                    }
            }
        }
        if(Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            switch(uiState) 
            {
                case UI.Stage:
                    {
                        targetPosition = new Vector2(0, -140);
                        StartCoroutine(MoveUI(stageRect, targetPosition, duration));
                        SwapChapter();
                        uiState = UI.Chapter;
                        return;
                    }
            }
        }

    }
    private void Start()
    {
        Transform original = this.transform;
        
    }

    void SwapChapter()
    {
        foreach (var chapter in chapterList)
        {
            if (chapter == chapterList[lastChapter])
            {
                StartCoroutine(ResizeUI(chapter, focusSize, duration));
                StartCoroutine(FadeUI(chapter.GetComponent<Image>(), 1f, duration));
                chapterBackground.sprite = chapter.GetComponent<Chapter>().backgroundImg;
            }
            else
            {
                StartCoroutine(ResizeUI(chapter, defaultSize, duration));
                StartCoroutine(FadeUI(chapter.GetComponent<Image>(), 0.8f, duration));
            }
        }
         
    }
    void SwapStage()
    {
        foreach (var stage in stageList)
        {
            if (stage == stageList[selectedStage])
            {
                StartCoroutine(ResizeUI(stage.GetComponent<RectTransform>(), new Vector2(150,150), duration));
                
            }
            else
            {
                StartCoroutine(ResizeUI(stage.GetComponent<RectTransform>(), new Vector2(125, 125), duration));
            }
        }

    }

    void DefaultSize()
    {
        foreach(var chapter in chapterList)
        {
            if (chapterList[lastChapter] == chapter)
                chapter.sizeDelta = focusSize;  
            else
                chapter.sizeDelta = defaultSize;
        }
    }

    public void StarSet()
    {
        foreach(var a in stageList)
        {
            Stage t = a.GetComponent<Stage>();
            int star = PlayerPrefs.GetInt($"{t.map.chapter}{t.map.stage}");

            switch (star)
            {
                case 0:
                    t.starOne.sprite = t.offStar;
                    t.starTwo.sprite = t.offStar;
                    t.starThree.sprite = t.offStar;
                    break;
                case 1:
                    t.starOne.sprite = t.onStar;
                    t.starTwo.sprite = t.offStar;
                    t.starThree.sprite = t.offStar;
                    break;

                case 2:
                    t.starOne.sprite = t.onStar;
                    t.starTwo.sprite = t.onStar;
                    t.starThree.sprite = t.offStar;
                    break;
                case 3:
                    t.starOne.sprite = t.onStar;
                    t.starTwo.sprite = t.onStar;
                    t.starThree.sprite = t.onStar;
                    break;
            }
        }
    }

    public void ColorReset()
    {
        foreach (var t in stageList)
        {
            if (stageList[selectedStage] == t)
                t.GetComponentInChildren<TextMeshProUGUI>().color = new Color(255, 255, 255, 1f);
            else
                t.GetComponentInChildren<TextMeshProUGUI>().color = new Color(191, 191, 191, 1f);
        }
    }

    IEnumerator MoveUI(RectTransform element, Vector2 target, float duration)
    {
        isMoving = true; // 이동 시작
        Vector2 initialPosition = element.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsedTime / duration);
            yield return null;
        }

        // 최종 위치에 정확히 도달하도록 보정
        element.anchoredPosition = target;
        isMoving = false; // 이동 종료
    }

    IEnumerator ResizeUI(RectTransform element, Vector2 target, float duration)
    {
        isSizing = true; // 이동 시작
        Vector2 initialSize = element.sizeDelta;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.sizeDelta = Vector2.Lerp(initialSize, target, elapsedTime / duration);
            yield return null;
        }

        //  보정
        element.sizeDelta = target;
        isSizing = false; // 이동 종료
    }

    IEnumerator FadeUI(Image element, float target, float duration)
    {
        isSizing = true; // 이동 시작
        float initialAlpha = element.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.color = new Color(255, 255, 255, Mathf.Lerp(initialAlpha, target, elapsedTime / duration));
            yield return null;
        }

        //  보정
        element.color = new Color(255, 255, 255, target);
        isSizing = false; // 이동 종료
    }
}

