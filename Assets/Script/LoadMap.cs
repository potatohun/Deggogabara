using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    public GameObject player;
    private void Awake()
    {
        
    }
    private void Start()
    {
        Instantiate(player, GameManager.instance.map.initPlayerPosition, Quaternion.identity);
        Instantiate(GameManager.instance.map.mapPrefab, new Vector3(0,0,0),Quaternion.identity);
    }
}

