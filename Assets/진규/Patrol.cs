
using System.Collections;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float moveX; //, moveY;
    public float minX, maxX; //, minY, maxY;

    public Vector3 initPos;
    public Vector2 moveSpot;

    public LayerMask playerLayer;
    public float FindRange = 4f;

    public bool detectCapy = false;

  
    virtual protected void Start()
    {
        initPos = transform.position;
        moveSpot = initPos;
  
    }

    virtual protected void Update()
    {
        detectCapy = Physics2D.OverlapCircle(transform.position, FindRange, playerLayer);

        SpriteFlip(moveSpot);  
  
    }


    void OnDrawGizmos() 
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, FindRange);
    }

    public void SpriteFlip(Vector3 pos)
    {
     
        if (transform.position.x - pos.x < 0) // 스프라이트 flip
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);

    }



}