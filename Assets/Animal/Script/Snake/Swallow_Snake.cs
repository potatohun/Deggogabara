using UnityEngine;

public class Swallow_Snake : MonoBehaviour
{
    //   Swim하다가.. 발견하면 Spot으로이동.. 3초간 trigger반응없으면 다시 돌아가

    [SerializeField]
    private float speed;

    private bool letsGo = false;

    [SerializeField]
    private GameObject target;

    private Patrol_Snake patrol;
    void Start()
    {
        patrol = GetComponent<Patrol_Snake>();
    }


    void Update()
    {

        if (patrol.GetGoTakeCapy())
        {

            patrol.SetGoTakeCapy(false);
            patrol.enabled = false;

        }

        /*   if (Vector2.Distance(transform.position, new Vector3(110.09f, -4.3f, 0)) > 0.2f && !letsGo)
               ToCapy();*/


        if (letsGo)
            Invoke("TakeCapy", 1f);

        if (Vector2.Distance(transform.position, new Vector3(110.5f, -4.3f, 0)) < 0.1f)
        {
            letsGo = false;
            target.gameObject.SetActive(true);
            Invoke("ReturnToPatrol", 3f);
        }

    }


    /*   void ToCapy()
       {       
           transform.position = Vector2.MoveTowards(transform.position, new Vector3(110.09f, -4.3f, 0), speed * Time.deltaTime);
           patrol.SpriteFlip(new Vector3(-48.37f, -19.5f, 0));
       }
   */
    void TakeCapy()
    {
        patrol.enabled = false;
        transform.position = Vector2.MoveTowards(transform.position, new Vector3(110.5f, -4.3f, 0), speed * Time.deltaTime);
        target.transform.position = transform.position;
        patrol.SpriteFlip(new Vector3(110.5f, -4.3f, 0));
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

    /*  private void OnCollisionExit2D(Collision2D collision)
      {
          if (collision.gameObject.CompareTag("Player"))
              Invoke("ReturnToSwim", 2f);

      }*/

    void ReturnToPatrol()
    {
        patrol.enabled = true;
        this.enabled = false;
        letsGo = false;
    }
}
