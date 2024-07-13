using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AnyKeyDown : MonoBehaviour
{
    public RectTransform logo;
    public RectTransform text;
    
    public float movingTime;
    bool isMoving;


    private void Update()
    {
        if(Input.anyKeyDown && !isMoving)
        {
            GameManager.instance.uiAudioMaster.PlayOneShot(GameManager.instance.btnSelect);
            StartCoroutine(MoveUI(logo, new Vector2(0,685), movingTime));
            StartCoroutine(MoveUI(text, new Vector2(0,-135), movingTime));
            Invoke("SwapScnene", movingTime);
        }
    }
    IEnumerator MoveUI(RectTransform element, Vector2 target, float duration)
    {
        isMoving = true; // �̵� ����
        Vector2 initialPosition = element.anchoredPosition;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            element.anchoredPosition = Vector2.Lerp(initialPosition, target, elapsedTime / duration);
            yield return null;
        }

        // ���� ��ġ�� ��Ȯ�� �����ϵ��� ����
        element.anchoredPosition = target;
        isMoving = false; // �̵� ����
    }

    public void SwapScnene()
    {
        SceneManager.LoadScene(1);
    }
}
