using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessagePanel : BasePanel
{
    private CanvasGroup TempCanvasGroup;
    public  Text messageText;
    
    void Awake()
    {
        TempCanvasGroup = GetComponent<CanvasGroup>();
        messageText = GetComponentInChildren<Text>();
    }
    
    public override void OnEnter()
    {
        if (TempCanvasGroup==null)
        {
            TempCanvasGroup = GetComponent<CanvasGroup>();
        }
        TempCanvasGroup.blocksRaycasts = true;
        TempCanvasGroup.interactable = true;
        TempCanvasGroup.alpha = 1;
    }

    public override void OnExit()
    {
        TempCanvasGroup.alpha = 0;
        CanvasManger.Instance.PanelOnClose();
    }

    public override void OnPause()
    {
        
        TempCanvasGroup.blocksRaycasts = false;
        TempCanvasGroup.interactable = false;
    }

    public override void GiveMessage(string messageStr)
    {
        messageText.text = messageStr;
        Invoke("OnExit",0.8f);
    }
}
