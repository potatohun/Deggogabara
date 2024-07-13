using UnityEngine;

public class TakeCapy_Ele : MonoBehaviour
{
    //   Swim하다가.. 발견하면 Spot으로이동.. 3초간 trigger반응없으면 다시 돌아가
    private Patrol_Ele patrol;

    [SerializeField]
    private float speed;

    [SerializeField]
    private bool letsGo = false;

    [SerializeField]
    private float toCapy_PosX;
    [SerializeField]
    private float takeCapy_PosX;

    [SerializeField]
    private Vector2 toCapy_Pos, takeCapy_Pos;
  
    void Start()
    {
        patrol = GetComponent<Patrol_Ele>();

        toCapy_Pos = new Vector2(toCapy_PosX, transform.position.y);
        takeCapy_Pos = new Vector2(takeCapy_PosX, transform.position.y);

    }


    void Update()
    {

        if (patrol.GetGoTakeCapy())
        {
            patrol.SetGoTakeCapy(false);
            patrol.enabled = false;
        }

        if (Vector2.Distance(transform.position, toCapy_Pos) > 0.2f && !letsGo)
            ToCapy();




        if (Vector2.Distance(transform.position, takeCapy_Pos) < 0.2f)
            transform.localScale = patrol.GetScale(false);




        if (letsGo && Vector2.Distance(transform.position, takeCapy_Pos) > 0.1f)
            Invoke("TakeCapy", 3f);




    }


    void ToCapy()
    {
        transform.position = transform.TransformDirection(Vector2.MoveTowards(transform.position, toCapy_Pos, speed * Time.deltaTime));
        transform.position = transform.TransformDirection(transform.position);
        patrol.SpriteFlip(toCapy_Pos);
    }

    void TakeCapy()
    {
        transform.position = Vector2.MoveTowards(transform.position, takeCapy_Pos, speed * Time.deltaTime);
        patrol.SpriteFlip(takeCapy_Pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
            letsGo = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
            Invoke("ReturnToSwim", 2f);

    }

    void ReturnToSwim()
    {
        patrol.enabled = true;
        this.enabled = false;

    }
}
