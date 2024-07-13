using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BlinkText : MonoBehaviour
{
    public float blinkTime;
    TextMeshProUGUI blinkText;

    public Color baseColor =  new Color(204, 204, 204,1);
    public Color pointColor = Color.white;

    public void Awake()
    {
        blinkText = GetComponent<TextMeshProUGUI>();
    }

    public void Start()
    {
        StartCoroutine(Blink());
    }

    IEnumerator Blink()
    {
        while (true)
        {
            blinkText.color = baseColor;
            yield return new WaitForSeconds(blinkTime);
            blinkText.color = pointColor;
            yield return new WaitForSeconds(blinkTime);
        }

    }
}
