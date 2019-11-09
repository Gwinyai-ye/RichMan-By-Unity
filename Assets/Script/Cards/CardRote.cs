using UnityEngine;

public class CardRote : MonoBehaviour
{
    private Transform[] childCard;

    public float speed = 3f;
    // Start is called before the first frame update
    private void Awake()
    {
        childCard=new Transform[transform.childCount];
        for (int i = 0; i < childCard.Length; i++)
        {
            childCard[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (var VARIABLE in childCard)
        {
            VARIABLE.Rotate(Vector3.up*speed);
        }
    }
}