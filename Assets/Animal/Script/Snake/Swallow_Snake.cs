using UnityEngine;

public class Swallow_Snake : MonoBehaviour
{
    //   Swim하다가.. 발견하면 Spot으로이동.. 3초간 trigger반응없으면 다시 돌아가

    private Animal animal;
    private Patrol_Snake patrol;

    [SerializeField]
    private bool letsGo = false;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float takeCapy_PosX;

    [SerializeField]
    private GameObject target;

    private Vector2 takeCapy_Pos;

    void Start()
    {
        patrol = GetComponent<Patrol_Snake>();
        animal = GetComponent<Animal>();

        takeCapy_Pos = new Vector2(takeCapy_PosX, transform.position.y);
    }


    void FixedUpdate()
    {

        if (patrol.GetGoTakeCapy())
        {
          
            animal.ani.SetBool("StartSwallow", true);
            patrol.SetGoTakeCapy(false);
            patrol.enabled = false;

        }


        if (letsGo)
            Invoke("TakeCapy", 1f);

        if (Vector2.Distance(transform.position, takeCapy_Pos) < 0.1f && letsGo)
        {
            
            letsGo = false;
            target.GetComponent<SpriteRenderer>().enabled = true;
            target.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = true;
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.None;
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeRotation;
            Invoke("ReturnToPatrol",3f);
        }

    }


 
    void TakeCapy()
    {
       
        patrol.enabled = false;
        transform.position = Vector2.MoveTowards(transform.position, takeCapy_Pos, speed);
        target.transform.position = transform.position;
        patrol.SpriteFlip(takeCapy_Pos);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            animal.ani.SetTrigger("Swallow");
            target = collision.gameObject;
            target.GetComponent<SpriteRenderer>().enabled = false;
            target.transform.GetChild(2).GetComponent<SpriteRenderer>().enabled = false;
            target.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
            letsGo = true;
        }
    }



    void ReturnToPatrol()
    {
        patrol.enabled = true;
        this.enabled = false;
        letsGo = false;
        animal.ani.SetBool("StartSwallow", false);
    }
}
