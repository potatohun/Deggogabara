using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkTime;
    TextMeshProUGUI blinkText;
    public string tempText;

    public void Awake()
    {
        blinkText = GetComponent<TextMeshProUGUI>();
        tempText = blinkText.text;
    }

    public void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkText.text = tempText;
            yield return new WaitForSeconds(blinkTime);
            blinkText.text = "";
            yield return new WaitForSeconds(blinkTime);
        }

    }
}
