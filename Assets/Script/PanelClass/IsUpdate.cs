using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class IsUpdate : BasePanel
{
    private CanvasGroup TempCanvasGroup;
    // Start is called before the first frame update

    public Button[] buttons;
    public Text text;
    public ArcBaseInfo tempArc;
    
    void Start()
    {
        TempCanvasGroup = GetComponent<CanvasGroup>();
        buttons = GetComponentsInChildren<Button>();
    }
    
    public override void OnEnter()
    {
        if (TempCanvasGroup==null)
        {
            TempCanvasGroup = GetComponent<CanvasGroup>();
        }
        tempArc = ArcManger.Instacnce.postInfo[PlayerManger.Instance.tempPlayer.playerPost].GetChild(1).GetComponent<ArcBaseInfo>();//需要修改
        TempCanvasGroup.interactable = true;
        TempCanvasGroup.blocksRaycasts = true;
        TempCanvasGroup.alpha = 1;
        gameObject.SetActive(true);
        
            text = transform.GetChild(1).GetComponent<Text>();
            text.text = "升级将花费" + tempArc.updateCost;
    }

    public override void OnExit()
    {
        CanvasManger.Instance.PanelOnClose();
        TempCanvasGroup.alpha = 0;
        
        PlayerManger.Instance.DelayTurnToNext();
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

    public void UpLevel()
    {
        if (tempArc.UpdateLevel())
        {
            OnExit();
        }
        else
        {
            CanvasManger.Instance.PanelOnEnter(UIPanelType.Message,"金钱不足或建筑已满级");
        }
        
    }
    
}
