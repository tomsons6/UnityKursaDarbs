using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour {

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    public float wallRunHeight = 2;
    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.1f;
    float speedSmothVelocity;
    float currentSpeed;
    float velocityY;

    Vector3 BodyRay = new Vector3(0, 1.6f, 0);
    RaycastHit hit;
    Ray down;
    Animator anim;
    Transform cameraT;
    CharacterController controller;
    pb_Object objectheight;
	// Use this for initialization
	void Start () {
        
        anim = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();
	}
	
	// Update is called once per frame
	void Update () {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;
        bool running = Input.GetKey(KeyCode.LeftShift);
        Move(inputDir, running);
        falling();
        wallRun();
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        anim.SetFloat("Speed", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpInPlace();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SprintJump();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RunJump();
        }
        Slide();
    }
    #region Move
    void Move(Vector2 inputDir, bool running)
    {
        if (inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, turnSmoothTime);
        }

        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmothVelocity, speedSmoothTime);

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;
        if (controller.isGrounded)
        {
            velocityY = 0;
        }


    }
    #endregion
    #region Fall
    void falling()
    {
        down = new Ray(transform.position, -Vector3.up);
        //Debug.Log(controller.velocity.y);
        if(Physics.Raycast(down,out hit))
        {
            if(hit.distance > 5f)
            anim.SetBool("IsFalling", true);

        }
        if(controller.isGrounded)
        {
            anim.SetBool("IsFalling", false);
        }
            


    }
    #endregion
    #region Jump
    void JumpInPlace()
    {
        if (controller.isGrounded && currentSpeed == 0f)
        {
            anim.SetTrigger("IsJumping");
        }
        
    }
    void RunJump()
    {
        if (controller.isGrounded && currentSpeed > 1.7f && currentSpeed < 2.1f )
        {
            anim.SetTrigger("RunJump");
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }
    void SprintJump()
    {
        if (controller.isGrounded && currentSpeed > 2.7f)
        {
            anim.SetTrigger("SprintJump");
            float jumpVelocity = Mathf.Sqrt(-3 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }
    #endregion
    #region WallRun and Slide
    //Sito wallRun vajag handlot citadi, sobrid sitas ir miskaste, fix if have time....
    void wallRun() {

        int WallRunMask = LayerMask.GetMask("WallRun");
        if (Physics.Raycast(transform.position, -transform.right, 1, WallRunMask) && Input.GetKey(KeyCode.Space))
        {
            anim.SetBool("IsFalling", false);
            anim.ResetTrigger("RunJump");
            anim.ResetTrigger("SprintJump");
            anim.SetBool("Mirror", false);
            anim.SetTrigger("WallRun");
            float wallRunVelocity = Mathf.Sqrt(-1 * gravity * wallRunHeight);
            velocityY = wallRunVelocity;
        }
        else
        {
            anim.ResetTrigger("WallRun");
        }
        if (Physics.Raycast(transform.position,transform.right,1, WallRunMask) && Input.GetKey(KeyCode.Space))          
        {
            anim.SetBool("IsFalling", false);
            anim.ResetTrigger("RunJump");
            anim.ResetTrigger("SprintJump");
            anim.SetBool("Mirror", true);
            anim.SetTrigger("WallRun");         
            float wallRunVelocity = Mathf.Sqrt(-1 * gravity * wallRunHeight);
            velocityY = wallRunVelocity;
        }

    }
    void Slide()
    {
        int SlideMask = LayerMask.GetMask("Slide");
        RaycastHit hit;
        if (Physics.Raycast(transform.position + BodyRay, transform.forward, out hit, 4, SlideMask) && currentSpeed > 2.5f && controller.isGrounded)
        {
            anim.SetTrigger("IsSliding");
            Physics.IgnoreCollision(controller, hit.collider);
        }
        else
        {
            anim.ResetTrigger("IsSliding");
            Physics.IgnoreCollision(controller, hit.collider, false);
        }       
    }
    #endregion
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Lose")
        {
            SceneManager.LoadScene(2);
        }
        if(other.gameObject.tag == "WinArea")
        {
            SceneManager.LoadScene(3);
        }
    }
}
