using System.Collections;
using UnityEngine;

public class Patrol_Ele : Patrol
{
    public TakeCapy_Ele TC;
    public bool go_takeCapy = false;

    protected override void Start()
    {
        base.Start();
        TC = GetComponent<TakeCapy_Ele>();
    }
    override protected void Update()
    {
        base.Update();
        Swim();
    }


 
    void Swim()
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
            TC.enabled = true;
            
        }
    }

}
