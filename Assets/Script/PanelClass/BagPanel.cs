using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class BagPanel : BasePanel
{
    
    #region 单例模式
    private static BagPanel _instance;
    public static BagPanel Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance =  GameObject.Find("BagPanel").GetComponent<BagPanel>();
            }
            return _instance;
        }
    }
    #endregion
    
     protected Slot[] slotList;

    public float targetAlpha = 0;

    private float smoothing = 4;

    private CanvasGroup canvasGroup;

	// Use this for initialization
	public void Start () {
        slotList = GetComponentsInChildren<Slot>();
        canvasGroup = GetComponent<CanvasGroup>();
	}


    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha, smoothing * Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < .01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    public bool StoreCard(int id)
    {
        Card card = CardManger.Instance.GetCardById(id);
        return StoreCard(card);
    }
    public bool StoreCard(Card card)
    {
        if (card == null)
        {
            Debug.LogWarning("要存储的物品的id不存在");
            return false;
        }
        if (card.capacity == 1)
        {
            Slot slot = FindEmptySlot();
            if (slot == null)
            {
                Debug.LogWarning("没有空的物品槽");
                return false;
            }
            else
            {
                slot.StoreCard(card);//把物品存储到这个空的物品槽里面
            }
        }
        else
        {
            Slot slot = FindSameIdSlot(card);
            if (slot != null)
            {
                slot.StoreCard(card);
            }
            else
            {
                Slot emptySlot = FindEmptySlot();
                if (emptySlot != null)
                {
                    emptySlot.StoreCard(card);
                }
                else
                {
                    Debug.LogWarning("没有空的物品槽");
                    return false;
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 这个方法用来找到一个空的物品槽
    /// </summary>
    /// <returns></returns>
    private Slot FindEmptySlot()
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return null;
    }

    private Slot FindSameIdSlot(Card item)
    {
        foreach (Slot slot in slotList)
        {
            if (slot.transform.childCount >= 1 && slot.GetCardId() == item.id &&slot.IsFull()==false )
            {
                return slot;
            }
        }
        return null;
    }


//    public void DisplaySwitch()
//    {
//        if (targetAlpha == 0)
//        {
//            Show();
//        }
//        else
//        {
//            Hide();
//        }
//    }

    #region save and load
//    public void SaveInventory()
//    {
//       
//        StringBuilder sb = new StringBuilder();
//        foreach (Slot slot in slotList)
//        {
//            if (slot.transform.childCount > 0)
//            {
//                CardUI CardUI = slot.transform.GetChild(0).GetComponent<CardUI>();
//                sb.Append(CardUI.card.id + ","+CardUI.amount+"-");
//            }
//            else
//            {
//                sb.Append("0-");
//            }
//        }
//        PlayerPrefs.SetString(this.gameObject.name, sb.ToString());
//    }
//    public void LoadInventory()
//    {
//        if (PlayerPrefs.HasKey(this.gameObject.name) == false) return;
//        string str = PlayerPrefs.GetString(this.gameObject.name);
//        //print(str);
//        string[] itemArray = str.Split('-');
//        for (int i = 0; i < itemArray.Length-1; i++)
//        {
//            string itemStr = itemArray[i];
//            if (itemStr != "0")
//            {
//                //print(itemStr);
//                string[] temp = itemStr.Split(',');
//                int id = int.Parse(temp[0]);
//                Card item = CardManger.Instance.GetCardById(id);
//                int amount = int.Parse(temp[1]);
//                for (int j = 0; j < amount; j++)
//                {
//                    slotList[i].StoreCard(item);
//                }
//            }
//        }
//    }
    #endregion

    public override void OnEnter()
    {
        if (canvasGroup==null)
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        targetAlpha = 1;
    }

    public override void OnExit()
    {
        targetAlpha = 0;
        CanvasManger.Instance.PanelOnClose();
    }

    public override void OnPause()
    {
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
    }
    
//    public void Show()
//    {
//        canvasGroup.blocksRaycasts = true;
//        targetAlpha = 1;
//    }
//    public void Hide()
//    {
//        canvasGroup.blocksRaycasts = false;
//        targetAlpha = 0;
//    }

    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.interactable = true;

    }
}
