using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UIElements;
using static UnityEngine.Rendering.DebugUI;

public class FriendManager : MonoBehaviour
{
    public static FriendManager friendManager;

    [SerializeField]
    [Header("朝杷郊虞 庁姥級 持失 呪")]
    private int friend_Count;

    [SerializeField]
    [Header("朝杷郊虞 庁姥級 繊覗 渠傾戚")]
    private float jumpDelay;

    [SerializeField]
    [Header("朝杷郊虞 庁姥級 神覗実")]
    private float friendsOffset = 3;

    [SerializeField]
    [Header("企舌 朝杷郊虞")]
    GameObject captain;

    [SerializeField]
    [Header("噺穿 亜管")]
    private bool canRotate;

    [SerializeField]
    [Header("繊覗 亜管")]
    private bool canJump;

    [SerializeField]
    [Header("企舌 朝杷郊虞 袴軒 是帖")]
    private GameObject headofCaptain;

    [SerializeField]
    [Header("袴軒 是 雌呪")]
    private float headConstant = 2;

    [SerializeField]
    [Header("朝杷郊虞 庁姥級 何軒 Queue")]
    Queue<Capybara_friend> friends_on_tail = new Queue<Capybara_friend>();

    [SerializeField]
    [Header("朝杷郊虞 庁姥級 袴軒 Queue")]
    Queue<Capybara_friend> friends_on_head = new Queue<Capybara_friend>();

    [Header("朝杷郊虞 庁姥 覗軒噸")]
    public GameObject friend_prefab;

    public bool CanRotate()
    {
        return canRotate;
    }

    public void SetCanRotate(bool input)
    {
        canRotate = input;
    }
    public bool CanJump()
    {
        return canJump;
    }

    private void Awake()
    {
        friendManager = this;

        canRotate = true; // 庁姥亜 馬蟹 戚雌 赤聖 井酔 Rotate 災亜!
        canJump = true; // 袴軒是拭 庁姥亜 蒸聖 井酔 Jump 亜管
    }

    private void Start()
    {
        // 銚鴇戚 郊虞左澗 号狽 亜閃神奄
        for (int i = 0; i < friend_Count; i++)
        {
            GameObject friend = Instantiate(friend_prefab, this.transform);

            // 持失吉 朝杷郊虞 Enqueue
            //friends_on_tail.Enqueue(friend.GetComponent<Capybara_friend>());
            EnqueueToTail(friend.GetComponent<Capybara_friend>());

            // 採乞 竺舛 貢 是帖 段奄鉢
            friend.transform.parent = captain.transform;
            //friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);

            // 持失吉 朝杷郊虞 庁姥級 段奄鉢(授辞, 暗軒 去)
            friend.GetComponent<Capybara_friend>().Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset); // (授辞, 暗軒)
            friend.gameObject.name = "朝杷郊虞 " + (friends_on_tail.Count) + "腰属";

            StateCheck();
        }
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow)) // 竣奄
        {
            if (!captain.GetComponent<Capybara_Move>().GetFloorStuck()) // 袴軒拭 混戚 赤生檎 袴軒 是稽 公竣惟 敗!
            {
                EnqueueToHead();
            }
        }

        if (Input.GetKeyDown(KeyCode.DownArrow)) // 鎧軒奄
        {
            Debug.Log("鎧軒奄");
            DequeueFromHead();
        }

        if (Input.GetKeyDown(KeyCode.F)) // 鎧軒奄
        {
            JoinFriend();
        }

        /*Debug.Log("袴軒 :" + friends_on_head.Count);
        Debug.Log("何軒 :" + friends_on_tail.Count);*/
    }
    private void EnqueueToHead() // 朝杷郊虞 庁姥級聖 企舌 袴軒 是稽 竣奄
    {
        if (friends_on_tail.Count > 0 && !captain.GetComponent<Capybara_Move>().GetJuming()) // 臣険 朝杷郊虞亜 糎仙馬檎
        {
            if ((friends_on_tail.Peek().transform.localPosition.y < -1)||(friends_on_tail.Peek().transform.localPosition.y > 1))
            {
                Debug.Log("株戚亜 含虞辞 竣聖 呪 蒸製");
                return;
            }
            Capybara_friend friend = friends_on_tail.Dequeue(); // 何軒 Queue拭辞 Dequeue
            friends_on_head.Enqueue(friend); // 袴軒 Queue拭 Enqueue

            friend.transform.parent = headofCaptain.transform; // 企舌 朝杷郊虞税 袴軒研 採乞稽 昼厭
            friend.transform.position = headofCaptain.transform.position + new Vector3(0, (friends_on_head.Count) * headConstant, 0); // 叔薦 是帖 戚疑

            friend.PushOnHead();

            foreach (Capybara_friend otherFriends in friends_on_tail) // 何軒拭 害精 陥献 庁姥級 蒋生稽 雁奄奄
            {
                otherFriends.RePosition(); // 是帖 仙竺舛
            }
        }
        StateCheck();
    }

    public void DequeueFromHead() // 朝杷郊虞 庁姥級聖 企舌 袴軒拭辞 鎧軒奄
    {
        if (!captain.GetComponent<Capybara_Move>().GetJuming())
        {
            if (friends_on_head.Count > 0) // 鎧険 朝杷郊虞亜 糎仙馬檎
            {
                Capybara_friend friend = friends_on_head.Dequeue();
                friends_on_tail.Enqueue(friend);
                friend.PopFromHead();

                friend.transform.parent = captain.transform; // 企舌 朝杷郊虞研 採乞稽 昼厭

                if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
                    // 図楕聖 左壱 赤聖 凶
                    friend.transform.position = captain.transform.position + new Vector3((friends_on_tail.Count) * friendsOffset, 0, 0);
                else
                    // 神献楕聖 左壱 赤聖 凶
                    friend.transform.position = captain.transform.position + new Vector3(-(friends_on_tail.Count) * friendsOffset, 0, 0);



                // 是帖 仙竺舛
                friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);
            }
            else if(friends_on_tail.Count > 0) // 何軒拭 切研 朝杷郊虞亜 赤生檎
            {
                //何軒切牽奄
                CutTheTail();
            }
        }
        StateCheck();
    }

    private void CutTheTail()//何軒 切牽奄
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_tail.Count;
        // 背雁 朝杷郊虞 何軒 Queue拭辞 薦暗
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_tail.Dequeue();
            if (i == cnt1 - 1) // 何軒拭 赤澗 橿汐 Missing 坦軒
                f.Missing();
            else
                tmp.Enqueue(f);
        }

        friends_on_tail.Clear();

        int cnt2 = tmp.Count;
        // 何軒 泥 仙 竺舛 貢 庁姥級 授辞 是帖 仙舛税;
        for (int i = 0; i < cnt2; i++)
        {
            Capybara_friend f = tmp.Dequeue();
            friends_on_tail.Enqueue(f);
        }

        tmp.Clear();

        StateCheck();
    }

    public void MissingFromHead(Capybara_friend capybara)
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_head.Count;
        // 背雁 朝杷郊虞 何軒 Queue拭辞 薦暗
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_head.Dequeue();

            if (i == cnt1 - 1)
            {
                f.Missing();
            }
            else
            {
                friends_on_head.Enqueue(f);
            }
        }

        StateCheck();
    }

    public void AllFriendJump()
    {
        StartCoroutine(AllFriendJumpCoroutine());
    }
    private IEnumerator AllFriendJumpCoroutine()
    {
        foreach (Capybara_friend friend in friends_on_tail)
        {
            yield return new WaitForSeconds(jumpDelay);
            if(!friend.GetMissing())
                friend.Jump();
        }
    }

    public void DequeueFromTail(Capybara_friend capybara) // 何軒拭辞 込嬢貝 朝杷郊虞 坦軒
    {
        Queue<Capybara_friend> tmp = new Queue<Capybara_friend>();

        int cnt1 = friends_on_tail.Count;
        // 背雁 朝杷郊虞 何軒 Queue拭辞 薦暗
        for (int i = 0; i < cnt1; i++)
        {
            Capybara_friend f = friends_on_tail.Dequeue();
            if (capybara != f)
                tmp.Enqueue(f);
        }
       
        friends_on_tail.Clear();

        int cnt2 = tmp.Count;
        // 何軒 泥 仙 竺舛 貢 庁姥級 授辞 是帖 仙舛税;
        for (int i = 0; i < cnt2; i++)
        {
            Capybara_friend f = tmp.Dequeue();
            friends_on_tail.Enqueue(f);
            f.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);
        }

        tmp.Clear();

        StateCheck();
    }

    public void EnqueueToTail(Capybara_friend capybara_friend)
    {
        friends_on_tail.Enqueue(capybara_friend);
        capybara_friend.PopFromHead();

        if (captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
            // 図楕聖 左壱 赤聖 凶
            capybara_friend.transform.position = captain.transform.position + new Vector3(((friends_on_tail.Count) * friendsOffset), 0, 0);
        else
            // 神献楕聖 左壱 赤聖 凶
            capybara_friend.transform.position = captain.transform.position + new Vector3(-((friends_on_tail.Count) * friendsOffset), 0, 0);

        Debug.Log(capybara_friend.transform.localScale);

        if(captain.GetComponent<Capybara_Move>().GetFront() == Vector2.left)
        {
            capybara_friend.GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            capybara_friend.GetComponent<SpriteRenderer>().flipX = false;
        }
        //capybara_friend.transform.localScale = new Vector3(Mathf.Abs(capybara_friend.transform.localScale.x), capybara_friend.transform.localScale.y, capybara_friend.transform.localScale.z); // 什追析 段奄鉢
        capybara_friend.Initialize((friends_on_tail.Count), (friends_on_tail.Count) * friendsOffset);

        StateCheck();
    }

    public void CaptainTaken() // 企舌 朝杷郊虞亜 説毘
    {
        Debug.Log("せせせせせせせせせせ");
        foreach (Capybara_friend friend in friends_on_head) // 袴軒拭 糎仙馬揮 朝杷郊虞級 鎧軒奄
        {
            DequeueFromHead();
        }

        foreach (Capybara_friend friend in friends_on_tail) // 何軒拭 糎仙馬揮 朝杷郊虞級 鎧軒奄
        {
            DequeueFromTail(friend);
        }
    }

    void StateCheck()
    {
        if (friends_on_tail.Count > 0) canRotate = false; // 噺穿 薦嬢
        else canRotate = true;

        if (friends_on_head.Count > 0) canJump = false; // 繊覗 薦嬢
        else canJump = true;
    }

    public int HeadQueueCount()
    {
        return friends_on_head.Count;
    }
    public float HeadConstant()
    {
        return headConstant;
    }
    public float FriendOffset()
    {
        return friendsOffset;
    }

    public void FriendsMoveTrue()
    {
        foreach(Capybara_friend friend in friends_on_tail)
        {
            friend.GetComponent<Animator>().SetBool("isMove", true);
        }
    }
    public void FriendsMoveFalse()
    {
        foreach (Capybara_friend friend in friends_on_tail)
        {
            friend.GetComponent<Animator>().SetBool("isMove", false);
        }
    }

    public void JoinFriend()
    {
        float leastDistance = 10000f;
        Capybara_friend target = null;
        // 亜舌 亜猿錘 庁姥級 杯嫌獣徹奄
        Capybara_friend[] friend = transform.GetComponentsInChildren<Capybara_friend>();
        for (int i = 0; i < friend.Length; i++)
        {
            
            if(leastDistance > friend[i].DistanceToCaptain()) // 置社左陥 希 拙生檎
            {
                if (friend[i].canJoin)
                {
                    target = friend[i];
                }
            }
        }

        if (target != null)
            target.JoinToGroup();
    }

    public void HeadFlip(bool LR)
    {
        foreach(Capybara_friend friend in friends_on_head)
        {
            friend.GetComponent<SpriteRenderer>().flipX = LR;
        }
    }

    public int TotalCapybaraFriendCount()
    {
        return friends_on_head.Count + friends_on_tail.Count;
    }
}
