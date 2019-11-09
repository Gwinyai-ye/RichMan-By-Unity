using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcHotel : ArcBaseInfo
{
    ArcHotel()
    {
        getMoney = 800;
        tempLevel = 1;
        createCost = 500;
        updateCost = 400;
        fillLevel = 3;
    }

    public override void AffectEnemy(PlayerInfo player,ArcBaseInfo arc=null)
    {

        player.ConsumeMoney(getMoney, true);
       // MainPanel.Instance.IntailMainPanelMessage(player);
        CanvasManger.Instance.PanelOnEnter(UIPanelType.Message,"被收取了"+getMoney+"元");
        if (arc!=null)
        {
            for (int i = 0; i < PlayerManger.Instance.playerNumber; i++)
            {
                if (PlayerManger.Instance.playerInfo[i].playername == arc.belong)
                {
                    PlayerManger.Instance.playerInfo[i].GetMoney(getMoney);
                    break;
                }
            }
        }
        PlayerManger.Instance.DelayTurnToNext();//等待已设定秒数，轮到下一个角色行动
    }
    public override bool UpdateLevel()
    {
        if (PlayerManger.Instance.tempPlayer.ConsumeMoney(updateCost) && tempLevel + 1 <= fillLevel)
        {
            updateCost += 200;
            getMoney += 200;
            tempLevel++;
            return true;
        }
        return false;
    }
    

}
