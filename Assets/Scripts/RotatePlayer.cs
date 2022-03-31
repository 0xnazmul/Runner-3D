using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatePlayer : MonoBehaviour
{
    public float rotationPlayer = 0;
    PlayerController playerController;
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other) 
    {
        if(other.gameObject.tag =="Player")
        {
            //other.transform.Rotate(Vector3.up * rotationPlayer);
            playerController = other.GetComponent<PlayerController>();
            playerController.CallCoroutine(rotationPlayer);
            // playerController.switchCam = true;
            // playerController.SwitchCamera();

    
        }
    }

}
