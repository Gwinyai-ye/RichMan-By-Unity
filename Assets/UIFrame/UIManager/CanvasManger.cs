using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasManger : MonoBehaviour
{
    // Start is called before the first frame update
    
    private static CanvasManger _instance;

    public static CanvasManger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("Canvas").GetComponent<CanvasManger>();
            }
            return _instance;
        }
    }
    
    public Stack<BasePanel> stackUIpanel;
    private Dictionary<UIPanelType, BasePanel> messageUIpanel;

    private void Awake()
    {
       messageUIpanel=new Dictionary<UIPanelType, BasePanel>();
        for (int i = 0; i < transform.childCount; i++)
        { 
            BasePanel tempBasePanel = transform.GetChild(i).GetComponent<BasePanel>();
           UIPanelType tempType = tempBasePanel.UItype;
           messageUIpanel.Add(tempType,tempBasePanel);
        }
    }
    
    

    private void Start()
    {
       PanelOnEnter(UIPanelType.Main);
    }

    // Update is called once per frame
    public void PanelOnEnter(UIPanelType type,string messageStr=null)
    {
        if (stackUIpanel == null)
        {
            stackUIpanel = new Stack<BasePanel>();
        }

        if (stackUIpanel.Count > 0&& type!=UIPanelType.Bag)//如果是打开背包，则不禁用主界面
        {
            stackUIpanel.Peek().OnPause();
        }

        BasePanel tempBasePanel;
        messageUIpanel.TryGetValue(type, out tempBasePanel);
        stackUIpanel.Push(tempBasePanel);
        tempBasePanel.OnEnter();
        
        if (type==UIPanelType.Message)
        {
            tempBasePanel.GiveMessage(messageStr);    
        }
    }

    public void PanelOnClose()
    {
        print(stackUIpanel.Count);
        
        if (stackUIpanel == null)
        {
            stackUIpanel=new Stack<BasePanel>();
        }

        if (stackUIpanel.Count>0)
        {
            stackUIpanel.Pop().OnPause();
        }
        
        print(stackUIpanel.Count);

        if (stackUIpanel.Count > 0)
        {
           stackUIpanel.Peek().OnResume();
        }
    }

    public void PanelOnEnterByString(string typeString)
    {
        UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), typeString);
        PanelOnEnter(type);
    }
    
    public void PanelOnCloseByString()
    {
    //    UIPanelType type = (UIPanelType)System.Enum.Parse(typeof(UIPanelType), typeString);
    }
}
