using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,IPointerExitHandler,IPointerEnterHandler,IPointerDownHandler
{ 
    public GameObject cardPrefab;

    public void StoreCard(Card card)
    {
        if (transform.childCount == 0)
        {
            GameObject itemGameObject = Instantiate(cardPrefab) as GameObject;
            itemGameObject.transform.SetParent(this.transform);
            itemGameObject.transform.localScale = Vector3.one;
            itemGameObject.transform.localPosition = Vector3.zero;
            itemGameObject.GetComponent<CardUI>().SetCard(card);
        }
        else
        {
            transform.GetChild(0).GetComponent<CardUI>().AddAmount();
        }
    }

    public CardType GetItemType()
    {
        return transform.GetChild(0).GetComponent<CardUI>().card.type;
    }
    
    public int GetCardId()
    {
        return transform.GetChild(0).GetComponent<CardUI>().card.id;
    }

    public bool IsFull()
    {
        CardUI tempUI = transform.GetChild(0).GetComponent<CardUI>();

        return tempUI.amount >= tempUI.card.capacity;
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        print("out");
        if(transform.childCount>0)
            CardManger.Instance.HideToolTip();//隐藏物品信息
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        print("in");
        if (transform.childCount > 0)
        {
            string toolTipText = transform.GetChild(0).GetComponent<CardUI>().card.GetToolTipText();
            CardManger.Instance.ShowToolTip(toolTipText);//显示物品信息
        }
        
    }

   

    public void OnPointerDown(PointerEventData eventData)
    {
//       if (eventData.button == PointerEventData.InputButton.Right)
//        {
//            if (CardManger.Instance.IsPickedItem==false&& transform.childCount > 0)
//            {
//               CardUI currentCardUI = transform.GetChild(0).GetComponent<CardUI>();
//                if (currentCardUI.card is Equipment || currentItemUI.Item is Weapon)
//                {
//                    currentCardUI.ReduceAmount(1);
//                    
//                    Item currentItem = currentItemUI.Item;
//                    if (currentCardUI.Amount <= 0)
//                    {
//                        
//                        DestroyImmediate(transform.GetChild(0).gameObject);
//                        InventoryManager.Instance.HideToolTip();
//                    }
//                    CharacterPanel.Instance.PutOn(currentItem);   
//                }
//            }
//        }

        if (eventData.button != PointerEventData.InputButton.Left) return;
        // 自身是空 1,IsPickedItem ==true  pickedItem放在这个位置
                            // 按下ctrl      放置当前鼠标上的物品的一个
                            // 没有按下ctrl   放置当前鼠标上的物品的所有
                 //2,IsPickedItem==false  不做任何处理
        // 自身不是空 
                 //1,IsPickedItem==true
                        //自身的id==pickedItem.id  
                                    // 按下ctrl      放置当前鼠标上的物品的一个
                                    // 没有按下ctrl   放置当前鼠标上的物品的所有
                                                    //可以完全放下
                                                    //只能放下其中一部分
                        //自身的id!=pickedItem.id   pickedItem跟当前物品交换          
                 //2,IsPickedItem==false
                        //ctrl按下 取得当前物品槽中物品的一半
                        //ctrl没有按下 把当前物品槽里面的物品放到鼠标上
        if (transform.childCount > 0)
        {
            CardUI currentCard = transform.GetChild(0).GetComponent<CardUI>();

            if (CardManger.Instance.IsPickedCard == false)//当前没有选中任何物品( 当前手上没有任何物品)当前鼠标上没有任何物品
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    int amountPicked = (currentCard.amount + 1) / 2;
                    CardManger.Instance.PickUpCard(currentCard.card, amountPicked);
                    int amountRemained = currentCard.amount - amountPicked;
                    if (amountRemained <= 0)
                    {
                        Destroy(currentCard.gameObject);//销毁当前物品7
                    }
                    else
                    {
                        currentCard.SetAmount(amountRemained);
                    }
                }
                else
                {
                    CardManger.Instance.PickUpCard(currentCard.card,currentCard.amount);
                    Destroy(currentCard.gameObject);//销毁当前物品
                }
            }else
            {
                //1,IsPickedItem==true
                    //自身的id==pickedItem.id  
                        // 按下ctrl      放置当前鼠标上的物品的一个
                        // 没有按下ctrl   放置当前鼠标上的物品的所有
                            //可以完全放下
                            //只能放下其中一部分
                    //自身的id!=pickedItem.id   pickedItem跟当前物品交换          
                if (currentCard.card.id == CardManger.Instance.PickedCard.card.id)
                {
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        if (currentCard.card.capacity > currentCard.amount)//当前物品槽还有容量
                        {
                            currentCard.AddAmount();
                            CardManger.Instance.RemoveCard();
                        }
                        else
                        {
                            return;
                        }
                    }
                    else
                    {
                        if (currentCard.card.capacity > currentCard.amount)
                        {
                            int amountRemain = currentCard.card.capacity - currentCard.amount;//当前物品槽剩余的空间
                            if (amountRemain >= CardManger.Instance.PickedCard.amount)
                            {
                                currentCard.SetAmount(currentCard.amount + CardManger.Instance.PickedCard.amount);
                                CardManger.Instance.RemoveCard(CardManger.Instance.PickedCard.amount);
                            }
                            else
                            {
                                currentCard.SetAmount(currentCard.amount + amountRemain);
                                CardManger.Instance.RemoveCard(amountRemain);
                            }
                        }
                        else
                        {
                            return;
                        }
                    }
                }
                else
                {
                    Card item = currentCard.card;
                    int amount = currentCard.amount;
                    currentCard.SetCard(CardManger.Instance.PickedCard.card, CardManger.Instance.PickedCard.amount);
                    CardManger.Instance.PickedCard.SetCard(item, amount);
                }

            }
        }
        else
        {
            // 自身是空  
                        //1,IsPickedItem ==true  pickedItem放在这个位置
                            // 按下ctrl      放置当前鼠标上的物品的一个
                            // 没有按下ctrl   放置当前鼠标上的物品所有数量
                        //2,IsPickedItem==false  不做任何处理
            if (CardManger.Instance.IsPickedCard == true)
            {
                if (Input.GetKey(KeyCode.LeftControl))
                {
                    this.StoreCard(CardManger.Instance.PickedCard.card);
                    CardManger.Instance.RemoveCard();
                }
                else
                {
                    for (int i = 0; i < CardManger.Instance.PickedCard.amount; i++)
                    {
                        this.StoreCard(CardManger.Instance.PickedCard.card);
                    }
                    CardManger.Instance.RemoveCard(CardManger.Instance.PickedCard.amount);
                }
            }
            else
            {
                return;
            }

        }
    }
}
