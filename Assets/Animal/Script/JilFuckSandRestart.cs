using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JilFuckSandRestart : MonoBehaviour
{
    Vector3 restartPos;

    private void Start()
    {
        restartPos = GameObject.Find("RestartPos").transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            SceneManager.LoadScene(3);
        else if (collision.gameObject.CompareTag("Friends"))
        {
            collision.gameObject.GetComponent<Capybara_friend>().Missing();
            collision.transform.position = restartPos;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Friends"))
        {
            collision.gameObject.GetComponent<Capybara_friend>().Missing();
            collision.transform.position = restartPos;
        }
}


}
