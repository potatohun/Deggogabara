
using UnityEngine;

public class TraceAndSwallow : MonoBehaviour
{
    private Animal animal;
    private Patrol patrol;

    [SerializeField]
    private Vector2 swallowDestination;

    [SerializeField]
    private float speed;

    [SerializeField]
    private LayerMask playerLayer;

    [SerializeField]
    private float FindRange = 4f;

    [SerializeField]
    private Collider2D target;


    private bool isCatch = false;




    private void Start()
    {
        animal = GetComponent<Animal>();
        patrol = GetComponent<Patrol>();
    }

    private void Update()
    {
        if (!isCatch)
            target = Physics2D.OverlapCircle(transform.position, FindRange, playerLayer); // ��ġ���������Ŭ


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
        if (isCatch == false && target != null) // ��ġ���̾ƴϸ�,ī�ǹٶ� ��ġ�ݰ濡 ��������
        {
            patrol.enabled = false; // ������ũ��Ʈ ��
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, speed * Time.deltaTime);

            patrol.SpriteFlip(target.transform.position);
        }
        else if (target == null)
            patrol.enabled = true;

    }


    void Catch()
    {
        if (isCatch) // ��Ҵ�
        {
            if (target.gameObject.CompareTag("Player"))
                FriendManager.friendManager.CaptainTaken();
            else if (target.gameObject.CompareTag("Friends"))
                target.gameObject.GetComponent<Capybara_friend>().Missing();


            target.GetComponent<SpriteRenderer>().sortingOrder = -1;
            target.transform.position = transform.GetChild(0).GetComponent<Transform>().position;
            transform.position = Vector2.MoveTowards(transform.position, swallowDestination, speed * Time.deltaTime);
            patrol.SpriteFlip(swallowDestination);

            FriendManager.friendManager.SetCanRotate(false);
        }
    }

    void ArriveDestination()
    {

        if (Vector2.Distance(transform.position, swallowDestination) < 0.1f) // swallowDestination�� �ٿԴ�   
        {
            isCatch = false;
            target.GetComponent<SpriteRenderer>().sortingOrder = 0;
            target.GetComponent<Rigidbody2D>().isKinematic = false;
            FriendManager.friendManager.SetCanRotate(true);

            animal.ani.SetBool("Catch", false);
            this.enabled = false;
            Invoke("ReturnToFly", 2f);

        }

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision == target)
        {
            animal.ani.SetBool("Catch", true);


            if (transform.localScale.x > 0)
                target.GetComponent<SpriteRenderer>().flipX = false;
            else
                target.GetComponent<SpriteRenderer>().flipX = true;


            target.GetComponent<Rigidbody2D>().isKinematic = true;

            isCatch = true;
        }
    }

    void ReturnToFly() // Invoke��, patrol�� �ٷ� �ѹ����� 
    {
        patrol.enabled = true;
    }
}