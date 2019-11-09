using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PlayerManger : MonoBehaviour
{
    public List<GameObject> playerObject;
    public int playerNumber;//角色数量
    public PlayerName tempTrun;//当前行动的角色姓名
    public int trunPost;//当前行动角色在playerInfo中的下标
//    public PlayerInfo[] playerInfo;
    public List<PlayerInfo>playerInfo;
    public PlayerInfo tempPlayer;//存储当前行动玩家的信息 
    public Transform tempPlayerTranform;
    public PlayerName userName;//记录用户玩家信息
    public int moveNum;//允许当前行动角色移动的步数
    public bool IsToNext;//是否轮到下一个角色行动

    private static  PlayerManger _instance;

    public static  PlayerManger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("PlayerManger").GetComponent<PlayerManger>();
            }

            return _instance;
        }
    }


    private void Awake()
    {
        playerObject = GameObject.FindGameObjectsWithTag("Player").ToList();
        //playerInfo=new PlayerInfo[playerObject.Length];
        playerInfo=new List<PlayerInfo>();
        playerNumber = playerObject.Count;
        
        for (int i = 0; i < playerNumber; i++)
        {
            playerInfo.Add(playerObject[i].transform.GetComponent<PlayerInfo>()); 
        }

        for (int i = 0; i < playerNumber; i++)
        {
            tempPlayer = playerInfo[i];
            tempPlayer.money = 5000;
            tempPlayer.power = 100;
            tempPlayer.arcNum = 0;
            tempPlayer.cardNum = 0;
            tempPlayer.IsRun = false;
            tempPlayer.playerPost = Random.Range(0, ArcManger.Instacnce.postNum);
            if (ArcManger.Instacnce.postInfo[tempPlayer.playerPost]!=null)
            {
                tempPlayer.transform.position = ArcManger.Instacnce.postInfo[tempPlayer.playerPost].position+new Vector3(0,1f,0);//调整角色初始位置
                tempPlayer.transform.LookAt( ArcManger.Instacnce.postInfo[(tempPlayer.playerPost + 1) % ArcManger.Instacnce.postNum].position);
            }
        }
        
        tempPlayer = playerInfo[trunPost].GetComponent<PlayerInfo>();//tempPlater存储当前正在行动的玩家
        tempPlayerTranform = playerObject[trunPost].transform;
    }

    private void Start()
    {
        //初始化管理类相关标志
        tempTrun = PlayerName.XiaoHua;
        trunPost = 0;
        IsToNext = false;
        moveNum = 0;
        
        MainPanel.Instance.IntailMainPanelMessage(tempPlayer);
    }//初始化每个玩家的信息


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            BagPanel.Instance.StoreCard(1);
        }
        
    }

    public void DelayTurnToNext()//1.5秒之后轮到下一玩家行动
    {

        StartCoroutine(TurnToNext());
    }


    IEnumerator TurnToNext()
    {
        yield return  new  WaitForSeconds(1.5f);
        CanvasManger.Instance.PanelOnEnter(UIPanelType.Message,"轮到下一个玩家行动。。。。。");
        yield return  new  WaitForSeconds(1);
        while (true)
        {
            trunPost = (trunPost + 1) % playerNumber;
            tempPlayer = playerInfo[trunPost];//获得下一个玩家的信息
            tempPlayerTranform = playerObject[trunPost].transform;//获得下一个玩家的Tranform组件
            if(playerNumber==1) tempPlayer.GameWin();//获得胜利
            MainPanel.Instance.IntailMainPanelMessage(tempPlayer);
//            if (tempPlayer.money < 0)
//            {
//                tempPlayer.GameOver();//这个玩家游戏结束了
//                playerObject[trunPost].SetActive(false);
//                playerInfo.RemoveAt(trunPost);
//                playerObject.RemoveAt(trunPost);
//                playerNumber--;
//                trunPost--;
//            }
            break;
        }
        
        if (tempPlayer.playername != userName)//如果不是用户玩家，则开启Ai行动模式
        {
            MainPanel.Instance.OnPause();
            AIinfo.IsRunAi = true;
        }else MainPanel.Instance.OnResume();
        IsToNext = false;
    }

}
