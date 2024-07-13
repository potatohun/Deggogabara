using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Stage : MonoBehaviour
{
    TextMeshProUGUI stageText;
    public int stageNum = 0;
    public GameObject map;

    private void Start()
    {
        
        stageText = GetComponentInChildren<TextMeshProUGUI>();
        stageText.text = $"{stageNum}";
    }
}
