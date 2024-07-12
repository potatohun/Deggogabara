using UnityEngine;

public class Patrol : MonoBehaviour
{
    public float speed;
    public float moveX; //, moveY;
    public float minX, maxX; //, minY, maxY;

    public Vector3 initPos;
    public Vector2 moveSpot;

    public LayerMask playerLayer;
    public float FindRange;
 

    public bool detectCapy = false;

    public Vector3 leftScale, rightScale;

    virtual protected void Start()
    {
        initPos = transform.position;
        moveSpot = initPos;

        leftScale = transform.localScale; leftScale.x = -leftScale.x;
        rightScale = transform.localScale;
    }

    virtual protected void Update()
    {
        detectCapy = Physics2D.OverlapCircle(transform.position, FindRange ,playerLayer);
    
        SpriteFlip(moveSpot);

    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,FindRange);
    }

    public void SpriteFlip(Vector3 pos)
    {


        if (transform.position.x - pos.x < 0) // 스프라이트 flip
            transform.localScale = rightScale;
        else
            transform.localScale = leftScale;

    }



}