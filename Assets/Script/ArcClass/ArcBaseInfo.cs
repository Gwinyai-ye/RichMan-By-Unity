using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcBaseInfo:MonoBehaviour
{
    public PlayerName belong;//建筑所属

    public int createCost;

    public int updateCost;

    public int tempLevel;
    
    public int fillLevel;

    public int getMoney;

    public int getPower;

    public string arcPath;//所在路径

    public Transform imageTranform;

    private float targetScale=0.6f;
    
    private float smoothing = 3f;

    private void Update()
    {
        if (imageTranform.localScale.x!=targetScale)
        {

            float toScale = Mathf.Lerp(imageTranform.localScale.x, targetScale, Time.deltaTime * smoothing);
            imageTranform.localScale=new Vector3(toScale,toScale,toScale);
            if (Mathf.Abs(imageTranform.localScale.x - targetScale) < 0.01f)
            {
                targetScale = Mathf.Abs(targetScale -0.6f)<0.01f ? 0.4f : 0.6f;
            }
        }
    }

    public virtual void AffectEnemy(PlayerInfo player,ArcBaseInfo arc=null)
    {
       PlayerManger.Instance.DelayTurnToNext();
    }

    public virtual void Myself(PlayerInfo player,ArcBaseInfo arc=null)
    {
        if (!arc)
        {
            PlayerManger.Instance.DelayTurnToNext();
        }else if (arc.tempLevel < arc.fillLevel)
        {
            StartCoroutine(ArcManger.Instacnce.DelayIsUpdate());
        }
        
    }

    public virtual bool UpdateLevel()
    {
        return false;
    }
}

