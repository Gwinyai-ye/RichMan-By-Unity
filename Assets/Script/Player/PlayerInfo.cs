using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo: MonoBehaviour
{
   public int money;//角色拥有的金钱数量
   public int arcNum;//角色拥有的建筑数量
   public int cardNum;//角色拥有的卡牌数量
   public int power;
   public PlayerName playername;
   public int playerPost;//角色位置
   public  bool IsRun;//是否允许行动
   public float speed = 3.0f;//移动的速度
   public Sprite headIcon;
   public List<ArcBaseInfo> PersonArc;//存储角色拥有的建筑
  // public Color color;
   private Animator ami;//保存动画组件

   private void Awake()
   {
      ami = GetComponent<Animator>();
   }


   private void Update()
   {
      if (PlayerManger.Instance.tempPlayer.name == name&& IsRun)//PlayerManger中可行动的玩家姓名是不是自己
      {
         MoveNext();//执行行动
      }
   }


   public virtual void ReleaseSkill()
   {
       
   }

   public bool ConsumeMoney(int money,bool isForce=false)//消费
   {
      if (isForce)
      {
         this.money -= money;
         MainPanel.Instance.IntailMainPanelMessage(this);

         if (this.money < 0)
         {
            GameOver();
         }
         
         return false;
      }
      
      if (this.money - money > 0)
      {
            this.money -= money;
            MainPanel.Instance.IntailMainPanelMessage(this);
            return true;
      }
      return false;
         
   }

   public bool ConsumePower(int power)//消耗体力
   {
      if (this.power - power > 0)
      {
         this.power -= power;
         MainPanel.Instance.IntailMainPanelMessage(this);
         return true;
      }
      return false;
   }

   public void GetMoney(int money)
   {
      this.money += money;
   }

   public void MoveNext()
   {
      if (IsRun)
      {
         
         //在地图上方走格子
         int nextPost = (playerPost + 1) % ArcManger.Instacnce.postNum;
         ami.SetFloat("Speed",1.0f);
           
         Vector3 tar=ArcManger.Instacnce.postInfo[nextPost].position-transform.position;
         tar.y += 1;
         transform.position += tar.normalized * speed * Time.deltaTime;
            
         transform.LookAt(ArcManger.Instacnce.postInfo[ (nextPost + 1) % ArcManger.Instacnce.postNum].position);
         if (Vector3.Distance(ArcManger.Instacnce.postInfo[nextPost].position, transform.position) < 2.0f)
         {
            playerPost = nextPost;
            PlayerManger.Instance.moveNum--;

         }

         //判断是否走动完毕，若一走动完毕，检查玩家在当前位置的状态，生成建筑相关UI与玩家交互
         if (PlayerManger.Instance.moveNum == 0)
         {
            IsRun = false;
            PlayerManger.Instance.IsToNext = true;
            ami.SetFloat("Speed",0.0f);
            
            //玩家与Ai公共处理部分
            
              if(playername==PlayerManger.Instance.userName)//当前角色姓名是否等于玩家姓名
               ArcManger.Instacnce.DealUserPost(this);//处理玩家角色停下的位置（产生相应的后果）
              else
              {
               ArcManger.Instacnce.DealAiPost(this);//处理AI角色停下的位置（产生相应的后果）
              }
         }
      }
   }

   public void GameOver()
   {
      PlayerManger.Instance.playerObject[PlayerManger.Instance.trunPost].SetActive(false);
      PlayerManger.Instance.playerInfo.RemoveAt(PlayerManger.Instance.trunPost);
      PlayerManger.Instance.playerObject.RemoveAt(PlayerManger.Instance.trunPost);
      PlayerManger.Instance.playerNumber--;
      PlayerManger.Instance.trunPost--;
     //  CanvasManger.Instance.PanelOnEnter(UIPanelType.Message,this.name+"破产了");
   }

   public void GameWin()
   {
      
   }
   
   
}
