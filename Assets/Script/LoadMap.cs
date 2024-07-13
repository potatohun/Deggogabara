using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadMap : MonoBehaviour
{
    private void Start()
    {
        Instantiate(GameManager.instance.map, new Vector3(0,0,0),Quaternion.identity);
    }
}

