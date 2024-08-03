using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JilFuckSandRestart : MonoBehaviour
{
    Vector3 restartPos;

    private void Start()
    {
        restartPos = GameObject.Find("RestartPos").transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Capybara"))
            collision.gameObject.transform.position = restartPos;
    }
}
