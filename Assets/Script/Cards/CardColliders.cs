using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardColliders : MonoBehaviour
{
    // Start is called before the first frame update

    public CardType type;
    public SpriteRenderer cardImage;
    public BoxCollider boxTrigger;

    private void Awake()
    {
        boxTrigger = GetComponent<BoxCollider>();
        cardImage = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(boxTrigger.isTrigger==false)return;
        boxTrigger.isTrigger = false;
        cardImage.color=new Color(255,255,255,0);
        BagPanel.Instance.StoreCard(1);
    }
}
