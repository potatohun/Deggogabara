using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JilFuckSand : MonoBehaviour
{
    public float sinkingSpeed; // ������ �ӵ�
    public bool isInQuicksand = false; // ��������?

/*    public void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
            collision.transform.position += Vector3.down * sinkingSpeed * Time.deltaTime;

    }*/

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            collision.transform.position += Vector3.down * sinkingSpeed * Time.deltaTime;
    }
    /*    void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isInQuicksand = true;
            }
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                isInQuicksand = false;
            }
        }*/


}
