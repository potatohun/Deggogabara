using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Animator ani;
    void Start()
    {
        ani = GetComponent<Animator>();   
    }

 
}
