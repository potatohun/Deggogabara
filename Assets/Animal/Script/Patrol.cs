using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField]
    protected float speed;

    protected float moveX;

    [SerializeField]
    protected float minX, maxX; 

    protected Vector3 initPos;
    protected Vector2 moveSpot;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private float FindRange;

    [SerializeField]
    protected bool detectCapy = false;

    protected Vector3 leftScale, rightScale;

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


    public Vector3 GetScale(bool input)
    {
        if (input == true)
            return rightScale;    
        else
            return leftScale;
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