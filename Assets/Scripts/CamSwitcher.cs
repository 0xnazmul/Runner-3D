using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitcher : MonoBehaviour
{
    public Camera[] secCam;
    public bool switchCam = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    // public void SwitchCamera()
    // {
    //     if(secCam[0].enabled)
    //     {
    //         secCam[0].enabled = false;
    //         secCam[1].enabled = true;


    //     }
    //     else
    //     {
    //         secCam[1].enabled = false;
    //         secCam[0].enabled = true;
    //     }
    //     switchCam = false;
    // }

    public void SwitchCamera2()
    {
        if (secCam[0].enabled)
        {
            secCam[0].enabled = false;
            secCam[2].enabled = true;
        }
}

}