using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AIinfo : PlayerInfo
{
    // Start is called before the first frame update
    public static bool  IsRunAi=false;//掩盖基类的IsRun
    public Button clickRote;


     void Start()
     {
         clickRote = GameObject.FindWithTag("ClickRote").GetComponent<Button>();
     }

    // Update is called once per frame
    void Update()
    {

        if (IsRunAi)
        {
            clickRote.onClick.Invoke();
            IsRunAi = false;
        }

        if (PlayerManger.Instance.tempPlayer.name == name&& IsRun)//PlayerManger中可行动的玩家姓名是不是自己
        {
            MoveNext();//执行行动
        }
    }
    
    
}
