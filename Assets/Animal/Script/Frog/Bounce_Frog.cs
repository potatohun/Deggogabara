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

            Vector2 normalVec = new Vector2(0, -1);  // 평면이기에 collision.contacts[0].normal을 안써도 될것같음          
            Vector2 incomingVec; 

            if (CapySR.flipX == false)
                incomingVec = new Vector2(1, -bounceHeight);
            else
                incomingVec = new Vector2(-1, -bounceHeight);

            Vector2 bounceVec = Vector2.Reflect(incomingVec.normalized ,normalVec);

            animal.audioSource.Play();
            collision.rigidbody.AddForce(bounceVec * bounceDistance, ForceMode2D.Impulse);
        }
    }
}
