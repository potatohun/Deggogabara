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
        isMoving = true; // 이동 시작
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

    public void SwapScnene()
    {
        SceneManager.LoadScene(1);
    }
}
