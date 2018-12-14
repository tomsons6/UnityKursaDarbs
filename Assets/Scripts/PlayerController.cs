using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        wallRun();
        falling();
        Slide();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jump();
        }
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        anim.SetFloat("Speed", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
        
    }

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
    void jump()
    {
        if (controller.isGrounded)
        {
            anim.SetTrigger("IsJumping");
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
        
    }
    void wallRun() {  
        int WallRunMask = LayerMask.GetMask("WallRun");
        Renderer rend;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,transform.forward,out hit,1, WallRunMask))          
        {
            rend =  hit.collider.GetComponent<Renderer>();
            Vector3 augsts = rend.bounds.center;
            Debug.Log(augsts);
            Debug.Log(hit.collider.bounds.size.ToString());

            Debug.Log("Fignja");
            anim.SetTrigger("WallRun");
            float wallRunVelocity = Mathf.Sqrt(-1 * gravity * wallRunHeight);
            velocityY = wallRunVelocity;
        }
        else
        {
            anim.ResetTrigger("WallRun");
        }
    }
    void Slide()
    {
        int SlideMask = LayerMask.GetMask("Slide");
        RaycastHit hit;

        if(Physics.Raycast(transform.position+BodyRay,transform.forward,out hit, 3, SlideMask) && currentSpeed > 0.7f)
        {
            anim.SetTrigger("IsSliding");
            controller.height = 1;
        }
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
        {

        }
        else
        {
            anim.ResetTrigger("IsSliding");
            controller.height = 2;
        }
    }
}
