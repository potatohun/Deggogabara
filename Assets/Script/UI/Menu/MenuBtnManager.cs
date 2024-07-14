using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum Menu
{
    Play = 0,
    Option = 1,
    Exit = 2    
}
public class MenuBtnManager : MonoBehaviour
{
    bool isMoving = false;

    public Image fadeUI;

    public Animator logoAnimator;
    public RectTransform textGroup;
    public float movingDuration;

    public Menu menuState;

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
        logoAnimator.Play("Bounce");
        StartCoroutine(MoveUI(textGroup, new Vector2(0, 40), movingDuration));
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) && !isMoving)
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
            switch (menuState)
            {
                case Menu.Play:
                    menuState = Menu.Option;
                    return; 
                case Menu.Option:
                    menuState = Menu.Exit;
                    return;
                case Menu.Exit:
                    menuState = Menu.Play;
                    return;
            }
        }
        if( Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) && !isMoving)
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSwap);
            switch (menuState)
            {
                case Menu.Play:
                    menuState = Menu.Exit;
                    return;
                case Menu.Option:
                    menuState = Menu.Play;
                    return;
                case Menu.Exit:
                    menuState = Menu.Option;
                    return;
            }
        }
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return) && !isMoving)
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);
            switch (menuState)
            {
                case Menu.Play:
                    StartCoroutine(FadeUI(fadeUI, 1f, 0.3f));
                    Invoke("SwapScene", 0.3f);
                    return;
                case Menu.Option:
                    
                    return;
                case Menu.Exit:
                    Application.Quit();
                    return;
            }
        }


        switch (menuState) 
        {
            case Menu.Play:
                textColorReset(textBtn[0]);
                return;
            case Menu.Option:
                textColorReset(textBtn[1]);
                return; 
            case Menu.Exit:
                textColorReset(textBtn[2]);
                return;
        }

        
    }

    public void SwapScene()
    {
        {
            SceneManager.LoadScene(5);
        }
    }

    public void textColorReset(TextMeshProUGUI changeText)
    {
        foreach(var t in textBtn)
        {
            if(changeText == t)
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
        isMoving = false; // 이동 종료
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
