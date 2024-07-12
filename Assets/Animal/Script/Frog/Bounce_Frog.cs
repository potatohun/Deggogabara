using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public CapsuleCollider2D bounceCol;
    public Rigidbody2D rb;
    public float bouncePower;
    void Start()
    {
        bounceCol = GetComponentInChildren<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector3 normalVec = new Vector3(0, -1, 0);//collision.contacts[0].normal;

            // Vector3 collidePoint = collision.contacts[0].point;
            Vector2 incomingVec; //= collidePoint - collision.transform.position;

            if (collision.transform.localScale.y < 0)
                incomingVec = new Vector2(1, -1);
            else
                incomingVec = new Vector2(-1, -1);
           
            Debug.Log("입사각:" + incomingVec + "법선벡터:" + normalVec);
            Vector2 bounceVec = Vector2.Reflect(incomingVec.normalized ,normalVec);
           // Debug.Log(bounceVec);
            collision.rigidbody.AddForce(bounceVec * bouncePower, ForceMode2D.Impulse);
        }
    }
}
