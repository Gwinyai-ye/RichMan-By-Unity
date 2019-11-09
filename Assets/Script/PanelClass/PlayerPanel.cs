using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerPanel :BasePanel
{
    private CanvasGroup TempCanvasGroup;
    // Start is called before the first frame update

//    private static PlayerPanel _instance;
    public Text[] money;
    public Slider []power;
    

//    public static PlayerPanel Instance
//    {
//        get
//        {
//            if (_instance == null)
//                _instance = GameObject.Find("PlayerPanel").GetComponent<PlayerPanel>();
//            return _instance;
//        }
//    }
    
    private void Start()
    {
        TempCanvasGroup = GetComponent<CanvasGroup>();
    }

    public override void OnEnter()
    {
        if (TempCanvasGroup==null)
        {
            TempCanvasGroup = GetComponent<CanvasGroup>();
        }

        for (int i = 0; i < PlayerManger.Instance.playerNumber; i++)
        {
            money[i].text = PlayerManger.Instance.playerInfo[i].money.ToString();
            power[i].value =PlayerManger.Instance.playerInfo[i].power *1.0f / 100;
        }

        TempCanvasGroup.interactable = true;
        TempCanvasGroup.blocksRaycasts = true;
        TempCanvasGroup.alpha = 1;
    }

    public override void OnExit()
    {
        TempCanvasGroup.alpha = 0;
        CanvasManger.Instance.PanelOnClose();
    }

    public override void OnPause()
    {
        TempCanvasGroup.interactable = false;
        TempCanvasGroup.blocksRaycasts = false;
    }
    
}
