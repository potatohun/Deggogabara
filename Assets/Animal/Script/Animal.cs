using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Animal : MonoBehaviour
{
    public Animator ani;
    public AudioSource audioSource;
    void Start()
    {
        ani = gameObject.GetComponent<Animator>();   
        audioSource = gameObject.GetComponent<AudioSource>();
    }

 
}
