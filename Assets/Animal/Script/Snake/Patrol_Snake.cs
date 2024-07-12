using System.Collections;
using UnityEngine;

public class Patrol_Snake : Patrol
{
    public Swallow_Snake SN;
    public bool go_takeCapy = false;

    protected override void Start()
    {
        base.Start();
        SN = GetComponent<Swallow_Snake>();
    }
    override protected void Update()
    {
        base.Update();
        Patrol();
    }



    void Patrol()
    {
        if (!detectCapy)
        {
            go_takeCapy = false;
            transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, moveSpot) < 0.1f)
            {
                moveX = Random.Range(minX, maxX);
                //moveY = Random.Range(minY, maxY);
                moveSpot = initPos + new Vector3(moveX, 0);

            }
        }
        else if (detectCapy)
        {
            go_takeCapy = true;
            SN.enabled = true;

        }
    }

}
