using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card
{
    // Start is called before the first frame update
    public int id { set; get; }
    public string name { set; get; }
    public CardType type { set; get; }
    public CardQuility quality { set; get; }
    public string description { set; get; }
    public int capacity { set; get; }
    public int power { set; get; }

    public string sprite;

    public Card()
    {
        id = -1;
    }

    public Card(int id, string name, CardType type, CardQuility quality, string description, int capacity, int power,string sprite)
    {
        this.id = id;
        this.name = name;
        this.type = type;
        this.quality = quality;
        this.description = description;
        this.capacity = capacity;
        this.power = power;
        this.sprite = sprite;
    }

    public virtual string GetToolTipText()//获得物品信息
    {
        string color = "";
        switch (quality)
        {
            case CardQuility.Common:
                color = "white";
                break;
            case CardQuility.Uncommon:
                color = "lime";
                break;
            case CardQuility.Rare:
                color = "navy";
                break;
            case CardQuility.Epic:
                color = "magenta";
                break;
            case CardQuility.Legendary:
                color = "orange";
                break;
            case CardQuility.Artifact:
                color = "red";
                break;
        }

        string text = string.Format("<color={3}>{0}</color>\n<size=30><color=green>使用体力消耗：{1} </color></size>\n<color=yellow><size=30>{2}</size></color>", name, power, description, color);
        return text;
    }
}


public enum CardType//卡牌类型
{
    ToMySelf,
    ToEnemy,
    ToPost,
    ToArc
}

public enum CardQuility//卡片品质
{
    Common   ,
    Uncommon  ,
    Rare       ,
    Epic      ,
    Legendary ,
    Artifact   
}
