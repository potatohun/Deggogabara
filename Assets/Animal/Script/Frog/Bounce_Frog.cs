using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    private Animal animal;
    private CapsuleCollider2D bounceCol;
    private Rigidbody2D rb;

    [SerializeField]
    private float bounceDistance;
    [SerializeField]
    private float bounceHeight;

    void Start()
    {
        animal = GetComponent<Animal>();
        bounceCol = GetComponentInChildren<CapsuleCollider2D>();
        rb = GetComponent<Rigidbody2D>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") || collision.gameObject.CompareTag("Friends"))
        {
            SpriteRenderer CapySR = collision.gameObject.GetComponent<SpriteRenderer>();

            Vector3 normalVec = new Vector3(0, -1, 0);//collision.contacts[0].normal;

            // Vector3 collidePoint = collision.contacts[0].point;
            Vector2 incomingVec; //= collidePoint - collision.transform.position;

            if (CapySR.flipX == false)
                incomingVec = new Vector2(1, -bounceHeight);
            else
                incomingVec = new Vector2(-1, -bounceHeight);
           
            Debug.Log("입사각:" + incomingVec + "법선벡터:" + normalVec);
            Vector2 bounceVec = Vector2.Reflect(incomingVec.normalized ,normalVec);
            // Debug.Log(bounceVec);
            animal.audioSource.Play();
            collision.rigidbody.AddForce(bounceVec * bounceDistance, ForceMode2D.Impulse);
        }
    }
}
