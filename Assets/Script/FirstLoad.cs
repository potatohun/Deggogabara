using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstLoad : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void ResetPreferbs()
    {
        for(int i = 0; i < 9; i++) 
        {
            for(int j = 0; j < 9 ; j++) 
            {
                PlayerPrefs.SetInt($"{i}{j}", 0);
            }

        }
    }
    private void Start()
    {
        for (int i = 0; i < 9; i++)
        {
            for (int j = 0; j < 9; j++)
            {
                PlayerPrefs.SetInt($"{i}{j}", 0);
                Debug.Log($"{i}챕터 {j} 스테이지 별데이터 지정: " + PlayerPrefs.GetInt($"{i}{j}"));
            }

        }
    }
}
