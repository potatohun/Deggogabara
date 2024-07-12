
using UnityEngine;

public class TraceAndSwallow : MonoBehaviour
{
    public Patrol patrol;

    public float speed;

    public LayerMask playerLayer;
    public float FindRange = 4f;

    public Collider2D target;
    public bool isCatch = false;

    public GameObject swallowDestination;


    private void Start()
    {
        patrol = GetComponent<Patrol>();
    }
    private void Update()
    {
        target = Physics2D.OverlapCircle(transform.position, FindRange, playerLayer); // 납치용오버랩서클


        Trace();
        Catch();
        ArriveDestination();
         

    }
    void OnDrawGizmos() 
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, FindRange);
    }


    void Trace()
    {
        if (isCatch == false && target != null) // 납치중이아니며,카피바라가 납치반경에 들어왔을때
        {
            patrol.enabled = false; // 순찰스크립트 꺼
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            patrol.SpriteFlip(target.transform.position);
        }
        else if (target == null)
            patrol.enabled = true;

    }


    void Catch()
    {
        if (isCatch) // 잡았다
        {

            transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder--;
            transform.GetChild(1).GetComponent<Transform>().position = transform.GetChild(0).GetComponent<Transform>().position;
            transform.position = Vector2.MoveTowards(transform.position, swallowDestination.transform.position + new Vector3(0, 3f, 0), speed * Time.deltaTime);

            patrol.SpriteFlip(swallowDestination.transform.position);
        }
    }

    void ArriveDestination()
    {

        if (Vector2.Distance(transform.position, swallowDestination.transform.position + new Vector3(0, 3f, 0)) < 0.1f) // swallowDestination에 다왔다   
        {
            isCatch = false;
            transform.GetChild(1).GetComponent<SpriteRenderer>().sortingOrder++;
            target.transform.parent = null;
            target.GetComponent<Rigidbody2D>().gravityScale = 20;

            this.enabled = false;
            patrol.enabled = true;
        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.GetComponent<Rigidbody2D>().gravityScale = 0.1f;
            isCatch = true;
            collision.transform.SetParent(transform);
        }
    }
}