using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ResultUIManager : MonoBehaviour
{
    public Image starOne;
    public Image starTwo;
    public Image starThree;

    public Sprite offStar;
    public Sprite onStar;

    public TextMeshProUGUI resultText;

    public void starSetting(int i)
    {
        switch (i)
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
