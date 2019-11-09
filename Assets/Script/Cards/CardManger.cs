using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManger : MonoBehaviour
{

    #region 单例模式
    
    private static CardManger _instance;//单例模式

    public static CardManger Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.Find("CardManger").GetComponent<CardManger>();
            }

            return _instance;
        }
    }

    #endregion
    
    private List<Card> cardList;
    #region ToolTip
    private ToolTip toolTip;

    private bool isToolTipShow = false;

    private Vector2 toolTipPosionOffset = new Vector2(150, -50);
    #endregion

    private Canvas canvas;

    #region PickedItem
    private bool isPickedCard = false;
    public bool IsPickedCard
    {
        get
        {
            return isPickedCard;
        }
    }

    private CardUI pickedCard;//鼠标选中的物体

    public CardUI PickedCard
    {
        get
        {
            return pickedCard;
        }
    }
    #endregion
    
    void Start()
    {
        toolTip = FindObjectOfType<ToolTip>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        pickedCard = GameObject.Find("PickedCard").GetComponent<CardUI>();
        pickedCard.Hide();
    }

     void Awake()  
    {
        ParseCardJson();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPickedCard)
        {
            //如果我们捡起了物品，我们就要让物品跟随鼠标
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            pickedCard.SetPostion(position);

        }else if (isToolTipShow)
        {
            
            print("Show");
            //控制提示面板跟随鼠标
             Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, Input.mousePosition, null, out position);
            toolTip.SetPosition(position+toolTipPosionOffset);
        }

        //物品丢弃的处理
        if (isPickedCard && Input.GetMouseButtonDown(0) && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject(-1)==false)
        {
            isPickedCard = false;
            PickedCard.Hide();
        }
    }

    private void ParseCardJson()
    {
        TextAsset cardText = Resources.Load<TextAsset>("CardInfo") ;
        string cardJson = cardText.text;
        JSONObject jsonObject=new JSONObject(cardJson);
        cardList=new List<Card>();
        
        foreach (var VARIABLE in jsonObject.list)
        {
            int id = (int)VARIABLE["id"].n;
            string name = VARIABLE["name"].str;
            
            CardType type = (CardType)Enum.Parse(typeof(CardType),VARIABLE["type"].str)  ;
            CardQuility quality = (CardQuility) Enum.Parse(typeof(CardQuility), VARIABLE["quality"].str);
            string description = VARIABLE["description"].str;
            int capacity = (int) VARIABLE["capacity"].n;
            int power = (int) VARIABLE["power"].n;
            string sprite = VARIABLE["sprite"].str;
            Card tempCard=new Card(id,name,type,quality,description,capacity,power,sprite);
            cardList.Add(tempCard);
        }
        
    }

    public Card GetCardById(int id)
    {
        foreach (var card in cardList)
        {
            if (card.id == id) return card;
        }

        return null;
    }

    public void ShowToolTip(string content)
    {
        if (this.isPickedCard) return;
        isToolTipShow = true;
        toolTip.Show(content);
    }

    public void HideToolTip()
    {
       isToolTipShow = false;
        toolTip.Hide();
    }

    public void PickUpCard(Card card, int amount)
    {
        PickedCard.SetCard(card,amount);
        isPickedCard = true;
        
        PickedCard.Show();
        toolTip.Hide(); 
    }
    
    
    /// </summary>
    public void RemoveCard(int amount=1)
    {
       PickedCard.ReduceAmount(amount);

       if (PickedCard.amount <= 0)
       {
           isPickedCard = false;
           PickedCard.Hide();
       }
    }
    
    public void SaveInventory()//保存场景
    {
       
    }

    public void LoadInventory()//读取场景
    {
       
    }
}
