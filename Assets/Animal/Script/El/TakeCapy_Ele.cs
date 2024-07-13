using System.Collections;
using UnityEngine;

public class TakeCapy_Ele : MonoBehaviour
{
    //   Swim하다가.. 발견하면 Spot으로이동.. 3초간 trigger반응없으면 다시 돌아가
    public float speed;
    public bool letsGo = false;

    public Patrol_Ele patrol;
    void Start()
    {
        patrol = GetComponent<Patrol_Ele>();
    }


    void Update()
    {

        if (patrol.go_takeCapy)
        {

            patrol.go_takeCapy = false;
            patrol.enabled = false;

        }

        if (Vector2.Distance(transform.position, new Vector3(-48.37f, -19.5f, 0)) > 0.2f && !letsGo)
            ToCapy();

        if (Vector2.Distance(transform.position, new Vector3(-48.37f, -19.5f, 0)) < 0.2f)
        {
            transform.localScale = patrol.leftScale;
            StartCoroutine(Return());
        }


        if (letsGo)
        {
            StopCoroutine(Return());
            Invoke("TakeCapy", 3f);
    
        }
        
      
    }

    IEnumerator Return() // 5초안에 안타면 돌아가는건데... 흠... 업데이트에서 두다닥ㅋ
    {
        yield return new WaitForSeconds(5f);
        ReturnToSwim();
    }
    void ToCapy()
    {
        transform.position = Vector2.MoveTowards(transform.position, new Vector3(-48.37f, -19.5f, 0), speed * Time.deltaTime);
        patrol.SpriteFlip(new Vector3(-48.37f, -19.5f, 0));
    }

    void TakeCapy()
    {

        transform.position = Vector2.MoveTowards(transform.position, new Vector3(-75.45f, -19.5f, 0), speed * Time.deltaTime);
        patrol.SpriteFlip(new Vector3(-75.45f, -19.5f, 0));
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
        letsGo = false;
    }
}
