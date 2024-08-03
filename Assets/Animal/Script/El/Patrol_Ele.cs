using System.Collections;
using UnityEngine;

public class Patrol_Ele : Patrol
{
    private TakeCapy_Ele TC;
    private bool go_takeCapy = false;

   
    public Vector2 moveSpot1;
    public Vector2 moveSpot2;

    public bool isWaterEle = false;
    protected override void Start()
    {
        base.Start();
        moveSpot = moveSpot1;
        TC = GetComponent<TakeCapy_Ele>();
    }
    override protected void Update()
    {
        base.Update();
      
    }

    private void FixedUpdate()
    {
        Swim();
    }

    public bool GetGoTakeCapy()
    { return go_takeCapy; }

    public void SetGoTakeCapy(bool input)
    { go_takeCapy = input; }

    void Swim()
    {
        if (!detectCapy)
        {
            go_takeCapy = false;    
            transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);

           if (Vector2.Distance(transform.position, moveSpot) < 0.1f && moveSpot == moveSpot1)
            {
                moveSpot = moveSpot2;
            }
           else if (Vector2.Distance(transform.position, moveSpot) < 0.1f && moveSpot == moveSpot2)
            {
                moveSpot = moveSpot1;
            }

        }
        else if (detectCapy && !isWaterEle)
        {
            go_takeCapy = true;
            TC.enabled = true;
            
        }
    }



   
}
