using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;

public class CapybaraLife : MonoBehaviour
{
    public Sprite onLife;
    public Sprite offLife;

    public GameObject capybaraLife;

    private void Start()
    {
        for (int i = 0; i < GameManager.instance.map.TotalCapybaraSpawnCount; i++)
            Instantiate(capybaraLife, transform);
    }
    private void Update()
    {
        for(int i = 0; i <GameManager.instance.map.TotalCapybaraSpawnCount;  i++)
        {
            if (i < FriendManager.friendManager.TotalCapybaraFriendCount())
                this.transform.GetComponentsInChildren<Image>()[i].sprite = onLife;
            else
                this.transform.GetComponentsInChildren<Image>()[i].sprite = offLife;
        }

    }
}
