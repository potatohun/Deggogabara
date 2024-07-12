using UnityEngine;

public class Patrol_Pell : Patrol
{
    public TraceAndSwallow TAS;
    override protected void Start()
    {
        base.Start();
        TAS = GetComponent<TraceAndSwallow>();
    }


    override protected void Update()
    {

        base.Update();

        Fly();
        if (transform.position == initPos && TAS != null)
            TAS.enabled = true;
    }

    void Fly()
    {
        if (detectCapy) // 카피바라발견 허세부릴려고 날아다니기시작
        {
            transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);


            if (Vector2.Distance(transform.position, moveSpot) < 0.1f)
            {
                moveX = Random.Range(minX, maxX);
                //moveY = Random.Range(minY, maxY);
                moveSpot = initPos + new Vector3(moveX, 0);

            }
        }
        else if (!detectCapy) // 카피바라가 시야에서 사라짐.. 
        {
            moveSpot = initPos;
            transform.position = Vector2.MoveTowards(transform.position, initPos, speed * Time.deltaTime); // 원래자리로
        }

    }
}
