using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroCutscene : MonoBehaviour
{
    public Image image; // Image 컴포넌트 참조
    public TMP_Text text;
    public float fadeDuration = 1.0f; // fade-in 및 fade-out 지속 시간

    void Start()
    {
        FadeOut();
        Invoke("OpenSkipText", 3f);
        Invoke("FadeIn", 40f);
    }

    private void Update()
    {
        if(Input.anyKeyDown && text.gameObject.activeSelf)
        {
            FadeIn();
        }


    }

    void LoadNextScene()
    {
        SceneManager.LoadScene("Chapter");
    }

    public void FadeIn()
    {
        StartCoroutine(Fade(0f, 1f));
        Invoke("LoadNextScene", fadeDuration);
    }

    public void FadeOut()
    {
        StartCoroutine(Fade(1f, 0f));
    }

    private IEnumerator Fade(float startFill, float endFill)
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float fillAmount = Mathf.Lerp(startFill, endFill, elapsedTime / fadeDuration);
            image.fillAmount = fillAmount;
            yield return null; // 다음 프레임까지 대기
        }

        image.fillAmount = endFill; // 최종 fill 값을 설정
    }

    void OpenSkipText()
    {
        text.gameObject.SetActive(true);
        StartCoroutine(BlinkText()); 
    }
    IEnumerator BlinkText()
    {
        while (true)
        {
            if(text.alpha <= 0)
            {
                text.alpha = 255;
            }
            else
            {
                text.alpha = 0;
            }
            yield return new WaitForSeconds(1f);
        }
    }
}
