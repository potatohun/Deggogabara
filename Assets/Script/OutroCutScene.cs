using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;
using TMPro;

public class OutroCutScene : MonoBehaviour
{
    public Image image; // Image ������Ʈ ����
    public float fadeDuration = 1.0f; // fade-in �� fade-out ���� �ð�
    public TMP_Text text;

    public PlayableDirector[] PlayableDirector;
    void Start()
    {
        Invoke("OpenSkipText", 3f);
        FadeOut();

        //PlayableDirector[1].Play();
        //Invoke("LoadNextScene", 5f);

        /*if (GameManager.instance.) // �� 3��
        {
            PlayableDirector[0].Play();
        }
        else // �� 2�� ����
        {
            PlayableDirector[1].Play();
        }*/
    }

    private void Update()
    {
        if (Input.anyKeyDown && text.gameObject.activeSelf)
        {
            LoadNextScene();
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
            yield return null; // ���� �����ӱ��� ���
        }

        image.fillAmount = endFill; // ���� fill ���� ����
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
            if (text.alpha <= 0)
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
