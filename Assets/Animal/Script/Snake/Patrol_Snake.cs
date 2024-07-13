using System.Collections;
using UnityEngine;

public class Patrol_Snake : Patrol
{
    private Swallow_Snake SN;

    [SerializeField]
    private bool go_takeCapy = false;

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


    public bool GetGoTakeCapy()
    { return go_takeCapy; } 

    public void SetGoTakeCapy(bool input)
    { go_takeCapy = input; }

    void Patrol()
    {
        if (!detectCapy)
        {
            go_takeCapy = false;
            transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, moveSpot) < 0.1f)
            {
                moveX = Random.Range(minX, maxX);            
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
