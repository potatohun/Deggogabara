using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Finish : MonoBehaviour
{
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            ClearChapter();
        }
    }

    void ClearChapter()
    {
        Debug.Log("Ŭ���� é��!"); // �� 1��
        Debug.Log("ī�ǹٶ� �� : " + FriendManager.friendManager.TotalCapybaraFriendCount()); // ������ �ִ� �� ī�ǹٶ� �� // �� 2��
        Debug.Log("�÷��� Ÿ�� : "); // ���ӸŴ������� ������ // �� 3��
    }
}
