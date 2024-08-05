using UnityEngine;

public class Patrol_Ele : Patrol
{
    private TakeCapy_Ele TC;
    private bool go_takeCapy = false;


    public Vector2 moveSpot1;
    public Vector2 moveSpot2;
    public Vector2 toCapy_Pos, takeCapy_Pos, toCapy_Pos1, takeCapy_Pos1, toCapy_Pos2, takeCapy_Pos2;
    public bool isWaterEle = false;


    public GameObject target;

    protected override void Start()
    {
        base.Start();
        moveSpot = moveSpot1;
        TC = GetComponent<TakeCapy_Ele>();

        toCapy_Pos1 = moveSpot1;
        takeCapy_Pos1 = moveSpot2;

        toCapy_Pos2 = moveSpot2;
        takeCapy_Pos2 = moveSpot1;

        toCapy_Pos = toCapy_Pos1;
        takeCapy_Pos = takeCapy_Pos1;



        target = GameObject.Find("Capybara_captain(Clone)");


    }
    override protected void Update()
    {
        base.Update();

        if (target != null)
        {
            if (Vector2.Distance(target.transform.position, toCapy_Pos1) < 30f)
            {
                toCapy_Pos = toCapy_Pos1;
                takeCapy_Pos = takeCapy_Pos1;
            }
            else if (Vector2.Distance(target.transform.position, toCapy_Pos2) < 30f)
            {
                toCapy_Pos = toCapy_Pos2;
                takeCapy_Pos = takeCapy_Pos2;
            }
        }
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
            transform.position = Vector2.MoveTowards(transform.position, moveSpot, speed);

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
