using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcRestaurant : ArcBaseInfo
{
    ArcRestaurant()
    {
        getPower = 10;
        tempLevel = 1;
        createCost = 500;
        updateCost = 400;
        fillLevel = 3;
    }

    public override void Myself(PlayerInfo player,ArcBaseInfo arc=null)
    {
        player.power += 10;
        CanvasManger.Instance.PanelOnEnter(UIPanelType.Message,"体力恢复了"+getPower);
        if (!arc)
        {
            PlayerManger.Instance.DelayTurnToNext();
        }else if (arc.tempLevel >= arc.fillLevel)
        {
            StartCoroutine(ArcManger.Instacnce.DelayIsUpdate());
        }
    }

    public override bool UpdateLevel()
    {
        if (PlayerManger.Instance.tempPlayer.ConsumeMoney(updateCost) && tempLevel + 1 <= fillLevel)
        {
            updateCost += 200;
            getPower += 5;
            tempLevel++;
            return true;
        }
        return false;
    }
    
}
