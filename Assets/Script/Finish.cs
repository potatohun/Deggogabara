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
        Debug.Log("클리어 챕터!"); // 별 1개
        Debug.Log("카피바라 수 : " + FriendManager.friendManager.TotalCapybaraFriendCount()); // 데리고 있는 총 카피바라 수 // 별 2개
        Debug.Log("플레이 타임 : "); // 게임매니저에서 들고오기 // 별 3개
    }
}
