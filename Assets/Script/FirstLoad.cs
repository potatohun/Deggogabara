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
}
