using UnityEngine;

public class Swallow_Snake : MonoBehaviour
{
    //   Swim하다가.. 발견하면 Spot으로이동.. 3초간 trigger반응없으면 다시 돌아가

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
  
        takeCapy_Pos = new Vector2(takeCapy_PosX, transform.position.y);
    }


    void Update()
    {

        if (patrol.GetGoTakeCapy())
        {

            patrol.SetGoTakeCapy(false);
            patrol.enabled = false;

        }


        if (letsGo)
            Invoke("TakeCapy", 1f);

        if (Vector2.Distance(transform.position, takeCapy_Pos) < 0.1f)
        {
            letsGo = false;
            target.gameObject.SetActive(true);
            Invoke("ReturnToPatrol", 3f);
        }

    }


 
    void TakeCapy()
    {
        patrol.enabled = false;
        transform.position = Vector2.MoveTowards(transform.position, takeCapy_Pos, speed * Time.deltaTime);
        target.transform.position = transform.position;
        patrol.SpriteFlip(takeCapy_Pos);
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            target = collision.gameObject;
            target.SetActive(false);
            letsGo = true;
        }
    }



    void ReturnToPatrol()
    {
        patrol.enabled = true;
        this.enabled = false;
        letsGo = false;
    }
}
