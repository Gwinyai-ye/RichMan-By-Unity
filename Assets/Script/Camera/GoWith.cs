using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoWith : MonoBehaviour
{

       public Transform [] playerTranform;
    
        public float distanceAway=1.5f;
 
        public float distanceUp=30.81f;

        public float distanceRight = 30.81f;
 
        public float smooth=6.18f; // how smooth the camera movement is
    
        private Vector3 m_TargetPosition; // the position the camera is trying to be in)

 //       Transform follow; //the position of Player
        
     //   private Transform Temp;
        
        
 
        void Start()
        {


           

        }
        
        void LateUpdate ()
 
        {

            if (PlayerManger.Instance.tempPlayerTranform)
            {
                m_TargetPosition =PlayerManger.Instance.tempPlayerTranform.position + Vector3.up * distanceUp -PlayerManger.Instance.tempPlayerTranform.forward * distanceAway +Vector3.right*distanceRight;
 
            
 
// making a smooth transition between it's current position and the position it wants to be in
 
                transform.position = Vector3.Lerp(transform.position, m_TargetPosition, Time.deltaTime * smooth);
            
// make sure the camera is looking the right way!
       
                transform.LookAt(PlayerManger.Instance.tempPlayerTranform);
            }
// setting the target position to be the correct offset from the
 
           
 
        }
        
}
