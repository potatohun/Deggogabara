using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapFade : MonoBehaviour
{
    public float fadeDuration = 1.0f; // fade-in 및 fade-out 지속 시간
    private Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
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
}
