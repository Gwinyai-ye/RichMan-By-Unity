using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CardUI : MonoBehaviour
{

    public Card card{ get; private set; }//卡片属性信息
    public int amount{ get; private set; }//卡片数量
    private Text amountText;//显示到UI上的数量与amount关联
    private Image cardImage;//卡片的样子（图片）

    public Text AmountText
    {
        get
        {
            if (amountText == null)
            {
                amountText = GetComponentInChildren<Text>();
            }
            return amountText;
        }
    }//get，set

    public Image CardImage
    {
        get
        {
            if (cardImage == null)
            {
                cardImage = GetComponent<Image>();
            }

            return cardImage;
        }
    }//get，set
    
    private float targetScale = 1f;//卡片的当前大小

    private Vector3 animationScale = new Vector3(1.4f, 1.4f, 1.4f);//被选中预定大小

    private float smoothing = 4;//大小改变的平滑程度
    
    // Update is called once per frame
    void Update()
    {
       if (transform.localScale.x!=targetScale)
        {
            float toScale = Mathf.Lerp(transform.localScale.x, targetScale, Time.deltaTime * smoothing);
            transform.localScale=new Vector3(toScale,toScale,toScale);
            if (Mathf.Abs(transform.localPosition.x-targetScale)<.02f )
            {
                transform.localScale=new Vector3(targetScale,targetScale,targetScale);
            }
        }
    }

    public void SetCard(Card card, int amount = 1)
    {
        transform.localScale = animationScale;
        this.card = card;
        this.amount = amount;
        CardImage.sprite = Resources.Load<Sprite>(card.sprite);

        if (card.capacity > 1)
        {
            AmountText.text = this.amount.ToString();
        }
        else AmountText.text = "";
    }

    public void AddAmount(int amount=1)
    {
        transform.localScale = animationScale;
        this.amount += amount;
        if (card.capacity > 1)
        {
            AmountText.text = this.amount.ToString();
        }
        else AmountText.text = "";
    }
    
    public void SetAmount(int amount)
    {
        transform.localScale = animationScale;
        this.amount = amount;
        //update ui 
        if (card.capacity > 1)
            AmountText.text = amount.ToString();
        else
            AmountText.text = "";
    }//更改同时，两者的初始化
    
    public void ReduceAmount(int amount=1)
    {
        transform.localScale = animationScale;
        this.amount -= amount;
        if (card.capacity > 1)
        {
            AmountText.text = this.amount.ToString();
        }
        else AmountText.text = "";
    }

    public void Exchange(CardUI cardUI)
    {
        Card cardTemp = cardUI.card;
        int amountTemp = cardUI.amount;
        cardUI.SetCard(this.card,this.amount);
        this.SetCard(cardUI.card,cardUI.amount);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetPostion(Vector3 postion)
    {
        transform.localPosition = postion;
    }
}
