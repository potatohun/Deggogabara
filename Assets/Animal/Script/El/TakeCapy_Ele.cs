using UnityEngine;

public class TakeCapy_Ele : MonoBehaviour
{
    //   Swim�ϴٰ�.. �߰��ϸ� Spot�����̵�.. 3�ʰ� trigger���������� �ٽ� ���ư�
    private Patrol_Ele patrol;

    [SerializeField]
    private bool letsGo = false;


    public float toCapySpeed;
    public float takeCapySpeed;



    void Start()
    {
        patrol = GetComponent<Patrol_Ele>();

    }


    void FixedUpdate()
    {

        if (patrol.GetGoTakeCapy())
        {
            patrol.SetGoTakeCapy(false);
            patrol.enabled = false;
        }



        if (Vector2.Distance(transform.position, patrol.toCapy_Pos) > 0.2f && !letsGo)
            ToCapy();




        if (Vector2.Distance(transform.position, patrol.takeCapy_Pos) < 0.2f)
            transform.localScale = patrol.GetScale(false);




        if (letsGo && Vector2.Distance(transform.position, patrol.takeCapy_Pos) > 0.1f)
            Invoke("TakeCapy", 2f);




    }


    void ToCapy()
    {

        transform.position = Vector2.MoveTowards(transform.position, patrol.toCapy_Pos, toCapySpeed);

        patrol.SpriteFlip(patrol.toCapy_Pos);
    }

    void TakeCapy()
    {


        transform.position = Vector2.MoveTowards(transform.position, patrol.takeCapy_Pos, takeCapySpeed);
        patrol.SpriteFlip(patrol.takeCapy_Pos);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.CompareTag("Player"))
            letsGo = true;

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {

            Invoke("ReturnToSwim", 1f);
        }
    }

    void ReturnToSwim()
    {
        patrol.enabled = true;
        letsGo = false;
        this.enabled = false;

    }
}
