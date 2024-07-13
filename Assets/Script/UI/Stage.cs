using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Stage : MonoBehaviour
{

    public Sprite offStar;
    public Sprite onStar;

    public Image starOne;
    public Image starTwo;
    public Image starThree;

    TextMeshProUGUI stageText;
    public int stageNum = 0;
    public MapData map;

    private void Start()
    {
        
        stageText = GetComponentInChildren<TextMeshProUGUI>();
        stageText.text = $"{stageNum}";

        switch(PlayerPrefs.GetInt($"{map.chapter}{map.stage}"))
        {
            case 0:
                starOne.sprite = offStar;
                starTwo.sprite = offStar;
                starThree.sprite = offStar;
                break;
            case 1:
                starOne.sprite = onStar;
                starTwo.sprite = offStar;
                starThree.sprite = offStar;
                break;
                
            case 2:
                starOne.sprite = onStar;
                starTwo.sprite = onStar;
                starThree.sprite = offStar;
                break;
            case 3:
                starOne.sprite = onStar;
                starTwo.sprite = onStar;
                starThree.sprite = onStar;
                break;
        }
    }
    
}
