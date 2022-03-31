using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float lastFingerPos;
    private float MoveMentX;
    public float MoveXpos => MoveMentX;
    private float swipeSpeed = 0.5f;
    private float maxSwipeSpeed = 1.0f;
    private Animator anim;
    private float playerSpeed = 3;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        playerMoveMent();
        SwipeController();
        SwipeMovement();
        
        
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            Restart();
        }
        
        
    }
    void playerMoveMent()
    {
        float moveMent = Input.GetAxis("Horizontal");

        transform.Translate(Vector3.forward * playerSpeed * Time.deltaTime);

    }
    public IEnumerator RotateMe(Vector3 byAngles) 
     {    
         var fromAngle = transform.rotation;
         var toAngle = Quaternion.Euler(transform.eulerAngles + byAngles);
         for(var t = 0f; t < 1.1f; t += Time.deltaTime) 
         {
             transform.rotation = Quaternion.Slerp(fromAngle, toAngle, t);
             yield return null;
         }
     }
     public void CallCoroutine(float angel)
     {
        StartCoroutine(RotateMe(Vector3.up * angel));
     }

     void SwipeController()

    {
        if (Input.GetMouseButtonDown(0))
        {
            lastFingerPos = Input.mousePosition.x;
            
        }
        else if (Input.GetMouseButton(0))
        {
            MoveMentX = Input.mousePosition.x - lastFingerPos;
            lastFingerPos = Input.mousePosition.x;
            
        }
        else if (Input.GetMouseButtonUp(0))
        {
            MoveMentX = 0f;
        }
    }
   
    void SwipeMovement()
    {
        float swerveAmount = Time.deltaTime * swipeSpeed * MoveXpos *2;
        swerveAmount = Mathf.Clamp(swerveAmount, -maxSwipeSpeed, maxSwipeSpeed);
        
        transform.localPosition = new Vector3(transform.localPosition.x + swerveAmount, transform.localPosition.y, transform.localPosition.z);

    }
    

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex,LoadSceneMode.Single);
    }

    







        


}
