using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    public TextMeshProUGUI timeText;
    public float elapsedTime;

    private void Start()
    {
        timeText = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        elapsedTime += Time.deltaTime;

        int minutes = Mathf.FloorToInt(elapsedTime / 60F);
        int seconds = Mathf.FloorToInt(elapsedTime % 60F);

        
        timeText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
