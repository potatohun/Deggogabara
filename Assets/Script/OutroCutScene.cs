using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class OutroCutScene : MonoBehaviour
{
    public Image image; // Image ������Ʈ ����
    public float fadeDuration = 1.0f; // fade-in �� fade-out ���� �ð�

    public PlayableDirector[] PlayableDirector;
    void Start()
    {
        FadeOut();

        PlayableDirector[1].Play();
        Invoke("LoadNextScene", 10f);

        /*if (GameManager.instance.) // �� 3��
        {
            PlayableDirector[0].Play();
        }
        else // �� 2�� ����
        {
            PlayableDirector[1].Play();
        }*/
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
            yield return null; // ���� �����ӱ��� ���
        }

        image.fillAmount = endFill; // ���� fill ���� ����
    }

}