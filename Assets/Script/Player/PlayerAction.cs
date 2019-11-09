
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlayerAction : MonoBehaviour
{
    public GameObject MainRote;
    public GameObject Point;
    public GameObject MainPanel;
    public CanvasGroup TempGroup;
    
    private void Start()
    {
       TempGroup = MainPanel.GetComponent<CanvasGroup>();
       
    }

     public void OnClickMove()
    {

         StartCoroutine(wait());
    }

    IEnumerator wait()
    {
        TempGroup.blocksRaycasts = false;
        MainRote.SetActive(true);
        Point.transform.eulerAngles=new Vector3(0,0,0);
        int rannum = Random.Range(1, 7);
        int Ang = 60 * rannum+720-30;
        print(rannum);
        while( Ang >0) 
        {
            Point.transform.Rotate(0,0,-10);
            Ang -= 10;
            yield return new WaitForEndOfFrame();
        }
        yield return  new  WaitForSeconds(0.4f);
        MainRote.SetActive(false);
        yield return  new  WaitForSeconds(0.4f);
        PlayerManger.Instance.moveNum = rannum;
        PlayerManger.Instance.tempPlayer.IsRun = true;
        
        TempGroup.blocksRaycasts = true;
        
    }        
   
    
    
}
