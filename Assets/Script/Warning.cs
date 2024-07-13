using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Warning : MonoBehaviour
{
    public TMP_Text text;
    public float fadeDuration = 2.0f; // ���̵� �ƿ� ���� �ð�

    private Color originalColor;

    void Start()
    {
        if (text == null)
        {
            text = GetComponent<TextMeshProUGUI>();
        }
        originalColor = text.color; // ���� ���� ����
    }
    public void PresentWarning(string str)
    {
        text.text = str;
        text.color = originalColor; // �ؽ�Ʈ�� ���� �������� �����Ͽ� ���̰� ��
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
            yield return null; // ���� �����ӱ��� ���
        }

        text.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f); // ������ �����ϰ� ����
    }
}
