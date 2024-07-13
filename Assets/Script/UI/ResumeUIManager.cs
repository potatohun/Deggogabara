using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Pause
{
    Resume = 0,
    Retry = 1,
    Option = 2,
    ExitToMap = 3,
}
public class ResumeUIManager : MonoBehaviour
{

    public Image fadeUI;
     
    public float movingDuration;

    public Pause pauseState;

    public Color baseColor;
    public Color pointColor;

    public List<TextMeshProUGUI> textBtn;

    private void Awake()
    {
        for (int i = 0; i < this.transform.childCount; i++)
        {
            textBtn.Add(GetComponentsInChildren<TextMeshProUGUI>()[i]);
        }
    }

    private void Start()
    {
        textBtn[0].color = pointColor;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
            switch (pauseState)
            {
                case Pause.Resume:
                    pauseState = Pause.Retry;
                    return;
                case Pause.Retry:
                    pauseState = Pause.Option;
                    return;
                case Pause.Option:
                    pauseState = Pause.ExitToMap;
                    return;
                case Pause.ExitToMap:
                    pauseState = Pause.Resume;
                    return;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
            switch (pauseState)
            {
                case Pause.Resume:
                    pauseState = Pause.ExitToMap;
                    return;
                case Pause.Retry:
                    pauseState = Pause.Resume;
                    return;
                case Pause.Option:
                    pauseState = Pause.Retry;
                    return;
                case Pause.ExitToMap:
                    pauseState = Pause.Option;
                    return;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);
            switch (pauseState)
            {
                case Pause.Resume:
                    FindAnyObjectByType<Finish>().gameState = GameState.Game;
                    return;
                case Pause.Retry:
                    SceneManager.LoadScene(3);
                    return;
                case Pause.Option:
                     
                    return;
                case Pause.ExitToMap:
                    SceneManager.LoadScene(2);
                    return;
            }
        }


        switch (pauseState)
        {
            case Pause.Resume:
                textColorReset(textBtn[0]);
                return;
            case Pause.Retry:
                textColorReset(textBtn[1]);
                return;
            case Pause.Option:
                textColorReset(textBtn[2]);
                return;
            case Pause.ExitToMap:
                textColorReset(textBtn[3]);
                return;
        }


    }

    public void SwapScene()
    {
        {
            SceneManager.LoadScene(2);
        }
    }

    public void textColorReset(TextMeshProUGUI changeText)
    {
        foreach (var t in textBtn)
        {
            if (changeText == t)
                changeText.color = pointColor;
            else
                t.color = baseColor;
        }

    }
    IEnumerator MoveUI(RectTransform element, Vector2 target, float duration)
    {
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
    }
    IEnumerator FadeUI(Image element, float target, float duration)
    {
        float initialFloat = 0;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.fillAmount += Mathf.Lerp(initialFloat, target, elapsedTime / duration);
            yield return null;
        }

        // 최종 위치에 정확히 도달하도록 보정
        element.fillAmount = target;
    }

}
