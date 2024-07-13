using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private CapsuleCollider2D bounceCol;
    private Rigidbody2D rb;

    [SerializeField]
    private float bouncePower;

    void Start()
    {
        bounceCol = GetComponentInChildren<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            SpriteRenderer CapySR = collision.gameObject.GetComponent<SpriteRenderer>();

            Vector3 normalVec = new Vector3(0, -1, 0);//collision.contacts[0].normal;

            // Vector3 collidePoint = collision.contacts[0].point;
            Vector2 incomingVec; //= collidePoint - collision.transform.position;

            if (CapySR.flipX == false)
                incomingVec = new Vector2(1, -1);
            else
                incomingVec = new Vector2(-1, -1);
           
            Debug.Log("�Ի簢:" + incomingVec + "��������:" + normalVec);
            Vector2 bounceVec = Vector2.Reflect(incomingVec.normalized ,normalVec);
           // Debug.Log(bounceVec);
            collision.rigidbody.AddForce(bounceVec * bouncePower, ForceMode2D.Impulse);
        }
    }
}
