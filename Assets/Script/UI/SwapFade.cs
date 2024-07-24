using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapFade : MonoBehaviour
{
    public float fadeDuration = 1.0f; // fade-in �� fade-out ���� �ð�
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
            yield return null; // ���� �����ӱ��� ���
        }

        image.fillAmount = endFill; // ���� fill ���� ����
    }
}
