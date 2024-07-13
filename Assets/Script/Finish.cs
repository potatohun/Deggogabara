using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    Game,
    Pause,
    Finish
}

public class Finish : MonoBehaviour
{

    public GameState gameState;

    int currentStar = 0;
    bool isMoving = false;
    bool isSwap = false;

    Image fadeUI;
    Image swapUI;
    RectTransform resultGroup;
    RectTransform pauseGroup;


    bool isFinish = false;

    private void OnTriggerExit2D(Collider2D collision)
    {
        isFinish = true;

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("��");
            ClearChapter();
        }
    }

    Timer timer;

    private void Awake()
    {
        currentStar = 0;
        gameState = GameState.Game;
        timer = GameObject.FindAnyObjectByType<Timer>();
        fadeUI = GameObject.Find("FinishFade").GetComponent<Image>();
        resultGroup = GameObject.Find("ResultUI").GetComponent<RectTransform>();
        swapUI = GameObject.Find("swapFade").GetComponent<Image>();
        pauseGroup = GameObject.Find("resumePanel").GetComponent<RectTransform>();
    }

    private void Start()
    {
        resultGroup.gameObject.SetActive(false);
        pauseGroup.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            switch(gameState)
            {
                case GameState.Game:
                    StartCoroutine(FadeUI(fadeUI, 0.9f, 0.5f));
                    pauseGroup.gameObject.SetActive(true);
                    timer.isStop = true;
                    gameState = GameState.Pause;
                    return;

                case GameState.Pause:
                    StartCoroutine(FadeUI(fadeUI, 0, 0.5f));
                    timer.isStop = false;
                    pauseGroup.gameObject.SetActive(false);
                    gameState = GameState.Game;
                    return;
                case GameState.Finish:
                    return;
            }


            Time.timeScale = 0;
        }

        if (Input.anyKeyDown && isFinish && !isMoving && isSwap)
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);

            //��ȯ ȿ��
            StartCoroutine(fillUI(swapUI, 1, 0.5f));
            Invoke("SwapScnene", 0.5f);
        }
    }

    void SwapScnene()
    {
        if (currentStar >= PlayerPrefs.GetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}"))
        {
            PlayerPrefs.SetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}", currentStar);
        }

        Debug.Log($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}: {PlayerPrefs.GetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}")}");
        SceneManager.LoadScene(2);
    }


    void ClearChapter()
    {
        Debug.Log("Ŭ���� é��!"); // �� 1��
        Debug.Log("ī�ǹٶ� �� : " + FriendManager.friendManager.TotalCapybaraFriendCount()); // ������ �ִ� �� ī�ǹٶ� �� // �� 2��
        Debug.Log("�÷��� Ÿ�� : "); // ���ӸŴ������� ������ // �� 3��

        // Destroy(FriendManager.friendManager.gameObject); // �ش� ���� �Ŵ��� ����
        currentStar++;

        if (FriendManager.friendManager.TotalCapybaraFriendCount() == GameManager.instance.map.TotalCapybaraSpawnCount)
            currentStar++;
        if (GameManager.instance.map.targetTime > timer.elapsedTime)
            currentStar++;

        if (currentStar > PlayerPrefs.GetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}"))
            PlayerPrefs.SetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}", currentStar);

        StartCoroutine(FadeUI(fadeUI, 0.9f, 0.5f));
        Invoke("ShowResult", 0.5f);
        Invoke("IsSwap", 1.5f);
    }
    public void ShowResult()
    {
        timer.isStop = true;
        resultGroup.GetComponent<ResultUIManager>().starSetting(currentStar);
        resultGroup.gameObject.SetActive(true);
        resultGroup.GetComponent<ResultUIManager>().resultText.text = timer.time; 
    }
    public void IsSwap()
    {
        isSwap = true;  
    }

    IEnumerator FadeUI(Image element, float target, float duration)
    {
        isMoving = true;
        float initialAlpha = element.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.color = new Color(0, 0, 0, Mathf.Lerp(initialAlpha, target, elapsedTime / duration));
            yield return null;
        }

        //  ����
        element.color = new Color(0, 0, 0, target);
        isMoving = false;
    }

    IEnumerator fillUI(Image element, float target, float duration)
    {
        float fill = element.fillAmount;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.fillAmount = Mathf.Lerp(fill, target, elapsedTime / duration);
            yield return null;
        }

        //  ����
        element.fillAmount = target;
    }


}
