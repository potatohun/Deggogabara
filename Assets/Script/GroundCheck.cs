using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    public bool isForGround;
    public bool isGround = false;
    public Collider2D collisionObj;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isForGround)
        {
            if (collision.CompareTag("Ground") || collision.CompareTag("Friends") || collision.CompareTag("Player"))
            {
                collisionObj = collision;
                isGround = true;
            }
        }
        else
        {
            if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
            {
                collisionObj = collision;
                isGround = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isForGround)
        {
            if (collision.CompareTag("Ground") || collision.CompareTag("Friends") || collision.CompareTag("Player"))
            {
                if (collisionObj == collision)
                {
                    collisionObj = null;
                    isGround = false;
                }

            }
        }
        else
        {
            if (collision.CompareTag("Ground") || collision.CompareTag("Player"))
            {
                if (collisionObj == collision)
                {
                    collisionObj = null;
                    isGround = false;
                }

            }
        }
    }

    public bool IsGround()
    {
        return isGround;
    }
}
