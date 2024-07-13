using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RisedWater_Whale : MonoBehaviour
{

    private BoxCollider2D waterCol;

    private void Start()
    {
        waterCol = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
