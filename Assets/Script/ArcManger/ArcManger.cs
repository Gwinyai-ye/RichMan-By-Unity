using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ArcManger:MonoBehaviour
{
    public int postNum;//路径格子总数

    public Transform[] postInfo;//存储路径信息

    public CardColliders[] cardCollider;
    
    private static ArcManger _instacnce;//单例模式

    public Dictionary<ArcType, ArcBaseInfo> ArcMessage;

    public static ArcManger Instacnce
    {
        get
        {
            if (_instacnce == null)
                _instacnce = GameObject.FindWithTag("MovePost").GetComponent<ArcManger>();
            return _instacnce;
        }
    }


    private void Awake()
    {
        postNum =transform.childCount;
        postInfo=new Transform[postNum];
        for (int i = 0; i < postNum; i++)
        {
            postInfo[i] = transform.GetChild(i);
            //isExistCard[i] 
        }
        FromJsonToObject();
    }

    private void Start()
    {
        cardCollider = GameObject.FindWithTag("CardPosition").GetComponentsInChildren<CardColliders>();
        IntialCardPostion();
    }


    private void IntialCardPostion()
    {
        foreach (var t in cardCollider)
        {
            t.cardImage.color=new Color(255,255,255,0);
        }

        int res = 0;
        while (true)
        {
            int post = Random.Range(0, cardCollider.Length);
            if (cardCollider[post].cardImage.color.a-0<0.01f)
            {
                cardCollider[post].cardImage.color=new Color(255,255,255,255);
                cardCollider[post].boxTrigger.isTrigger = true;
                res++;
            }
            else
              continue;
            if (res == 7) break;
        }
    }
    
    
    
    void FromJsonToObject()
    {
        ArcMessage=new Dictionary<ArcType, ArcBaseInfo>();
        TextAsset arcText = Resources.Load<TextAsset>("ArcInfo");
        string jsonText = arcText.text;
        JSONObject arcJson=new JSONObject(jsonText);
        string typeStr;
        
        foreach (var VARIABLE in arcJson.list)
        {
            ArcBaseInfo tempArc=new ArcBaseInfo();
            typeStr = VARIABLE["ArcType"].str;
            ArcType type = (ArcType)System.Enum.Parse(typeof(ArcType), typeStr);
            tempArc.arcPath = VARIABLE["path"].str;
            tempArc.createCost =(int) VARIABLE["createCost"].n;
            tempArc.updateCost =(int) VARIABLE["updateCost"].n;
            Debug.Log(type);
            Debug.Log(tempArc.arcPath );
            ArcMessage.Add(type,tempArc);
        }
    }

    private void IsCreate()//询问玩家是否购买土地
    {
        CanvasManger.Instance.PanelOnEnter(UIPanelType.IsCreate);
    }

    private void IsUpdate()//询问玩家是否更新建筑信息
    {
        CanvasManger.Instance.PanelOnEnter(UIPanelType.IsUpdate);
    }

    public IEnumerator DelayIsUpdate()
    {
        yield return  new  WaitForSeconds(1.5f);
        IsUpdate();
    }
    

    public void DealUserPost(PlayerInfo player)//处理玩家行动结束后，当前位置对玩家产生的影响
    {
        if (postInfo[player.playerPost].childCount < 2)
        {
            IsCreate();
        }
        else
        {
            ArcBaseInfo tempArc= postInfo[player.playerPost].GetChild(1).GetComponent<ArcBaseInfo>();
            //判断是不是自己的建筑
            if (tempArc.belong == player.playername)
            {
                tempArc.Myself(player,tempArc);
            }
            else
            {
                tempArc.AffectEnemy(player,tempArc);
            }
        }
    }

    public void DealAiPost(PlayerInfo player)//处理Ai的行动
    {

        if (postInfo[player.playerPost].childCount < 2)
        {
            foreach (var VARIABLE in ArcMessage)
            {
                if (player.ConsumeMoney(VARIABLE.Value.createCost))
                {
                    GameObject child = GameObject.Instantiate(Resources.Load("Architecture/House")) as GameObject;
                    Transform parent = postInfo[player.playerPost]; //找到当前位置的tranform    
                    child.transform.SetParent(parent); //设置其父母
                    child.transform.position = parent.GetChild(0).position + new Vector3(0, 0.4f, 0); //设置位置在Panel上
                    PlayerManger.Instance.tempPlayer.PersonArc.Add(child.GetComponent<ArcBaseInfo>()); //将建立的建筑保存到角色的所有建筑信息中
                    child.GetComponent<ArcBaseInfo>().belong = PlayerManger.Instance.tempPlayer.playername;
                    Image tempImage = child.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();//获取图标
                    tempImage.sprite = PlayerManger.Instance.tempPlayer.headIcon;
                    PlayerManger.Instance.tempPlayer.arcNum++; //建筑数量+1
                    break;
                }
            }
            PlayerManger.Instance.DelayTurnToNext();
        }
        else
        {
            ArcBaseInfo tempArc= postInfo[player.playerPost].GetChild(1).GetComponent<ArcBaseInfo>();
            //判断是不是自己的建筑
            if (tempArc.belong == player.playername)
            {
                if (player.ConsumeMoney(tempArc.updateCost))
                {
                    tempArc.UpdateLevel();
                }
                tempArc.Myself(player);
            }
            else
            {
                tempArc.AffectEnemy(player,tempArc);
            }
        }
    }
}