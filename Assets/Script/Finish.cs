using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    int currentStar = 0;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ClearChapter();
        }
    }

    Timer timer;

    private void Start()
    {
        timer = GameObject.FindAnyObjectByType<Timer>();
    }

    void ClearChapter()
    {
        Debug.Log("Ŭ���� é��!"); // �� 1��
        Debug.Log("ī�ǹٶ� �� : " + FriendManager.friendManager.TotalCapybaraFriendCount()); // ������ �ִ� �� ī�ǹٶ� �� // �� 2��
        Debug.Log("�÷��� Ÿ�� : "); // ���ӸŴ������� ������ // �� 3��

        // Destroy(FriendManager.friendManager.gameObject); // �ش� ���� �Ŵ��� ����

        if (FriendManager.friendManager.TotalCapybaraFriendCount() == GameManager.instance.map.TotalCapybaraSpawnCount)
            currentStar++;
        if (GameManager.instance.map.targetTime > timer.elapsedTime)
            currentStar++;

        if (currentStar > PlayerPrefs.GetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}"))
            PlayerPrefs.SetInt($"{GameManager.instance.map.chapter}{GameManager.instance.map.stage}", currentStar);




    }
    

}
