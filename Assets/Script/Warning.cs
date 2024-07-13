using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public TMP_Text text;
    public float fadeDuration = 2.0f; // 페이드 아웃 지속 시간

    private Color originalColor;

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        originalColor = text.color; // 원래 색상 저장
    }
    public void PresentWarning(string str)
    {
        text.text = str;
        text.color = originalColor; // 텍스트를 원래 색상으로 설정하여 보이게 함
        StartCoroutine(FadeOut());
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            text.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null; // 다음 프레임까지 대기
        }

        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // 완전히 투명하게 설정
    }
}
