using UnityEngine;
using UnityEngine.UI;

public class IsCreate : BasePanel
{
    private CanvasGroup TempCanvasGroup;
    private Toggle[] toggles;
    public Button[] buttons;
    public Text text;
    public Text[] ArcDescription;
    void Awake()
    {
        TempCanvasGroup = GetComponent<CanvasGroup>();
        toggles = GetComponentsInChildren<Toggle>();
        buttons = GetComponentsInChildren<Button>();
        text = transform.GetChild(2).GetComponent<Text>();
        
    }

    private void Update()
    {
        IsCreatButtonVaild();
    }

    public override void OnEnter()
    {
        if (TempCanvasGroup==null)
        {
            TempCanvasGroup = GetComponent<CanvasGroup>();
        }

        TempCanvasGroup.interactable = true;
        TempCanvasGroup.blocksRaycasts = true;
        TempCanvasGroup.alpha = 1;
        gameObject.SetActive(true);
    }

    public override void OnExit()
    {
        CanvasManger.Instance.PanelOnClose();
        TempCanvasGroup.alpha = 0;
        PlayerManger.Instance.DelayTurnToNext();//轮到下一个角色行动
        
    }

    public override void OnPause()
    {
        TempCanvasGroup.interactable = false;
        TempCanvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        TempCanvasGroup.interactable = true;
        TempCanvasGroup.blocksRaycasts = true;
    }
    
    
    public void CreatArc()//生成建筑
    {
        int id;
        for (id = 0; id < toggles.Length; id++)
        {
            if (toggles[id].isOn)
            {
                toggles[id].isOn = false;
                break;
            }
        }

        ArcType tempType = (ArcType) id;
        ArcBaseInfo tempArcBaseInfo=null;
        ArcManger.Instacnce.ArcMessage.TryGetValue(tempType, out tempArcBaseInfo);
        GameObject child=null;
        if (PlayerManger.Instance.tempPlayer.ConsumeMoney(tempArcBaseInfo.createCost))
        {
            child=Instantiate(Resources.Load(tempArcBaseInfo.arcPath)) as  GameObject;
            
            Transform parent = ArcManger.Instacnce.postInfo[PlayerManger.Instance.tempPlayer.playerPost];//找到当前位置的tranform
            child.transform.SetParent(parent);//设置其父母
            child.transform.position = parent.GetChild(0).position + new Vector3(0, 0.4f, 0);//设置位置在Panel上
            PlayerManger.Instance.tempPlayer.PersonArc.Add(child.GetComponent<ArcBaseInfo>());//将建立的建筑保存到角色的所有建筑信息中
            child.GetComponent<ArcBaseInfo>().belong = PlayerManger.Instance.tempPlayer.playername;
            Image tempImage = child.transform.GetChild(2).transform.GetChild(0).transform.GetChild(0).GetComponent<Image>();//获取图标
            tempImage.sprite = PlayerManger.Instance.tempPlayer.headIcon;

            PlayerManger.Instance.tempPlayer.arcNum++;//建筑数量+1
            OnExit();//关闭建筑界面UI，两秒后让下一个角色行动
        }
        else
        {
            CanvasManger.Instance.PanelOnEnter(UIPanelType.Message,"金钱不足");
        //    MessagePanel.Instance.GiveMessage("金钱不足");//显示要给于玩家的提示
        }
    }

     public void IsCreatButtonVaild()//控制按键的可交互性
    {
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                buttons[1].interactable=true;
                text.text ="";
                ArcDescription[i].enabled = true;
            }
            else
            {
                text.text = "@请选择要建立的建筑";//命名不太好看到时候改
                ArcDescription[i].enabled = false;
            }
            
        }
        
        for (int i = 0; i < toggles.Length; i++)
        {
            if (toggles[i].isOn)
            {
                buttons[1].interactable=true;
                text.text = null;
                return;
            }
            buttons[1].interactable=false;
            text.text = "@请选择要建立的建筑";
        }
    }
     
     
     
}
