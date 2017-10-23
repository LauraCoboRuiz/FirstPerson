using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private CharacterController controller;
    public Vector3 moveDirection;

    private Vector2 axis;
    private Vector3 desiredDirection;

    public float speed;
    public float jumpSpeed;
    public float forceToGround = Physics.gravity.y;
    public float gravityMagnitud;
    [Header("Stats")]
    public int life = 200;
    public bool dead = false;
    public bool jump;
    public bool isGrounded;
    [Header("UI")]
    public LifeBarUI lifeBar;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        lifeBar.Init(life);
    }

    void Update()
    {
        if (!controller.isGrounded) isGrounded = false;

        if (isGrounded & !jump)
        {
            moveDirection.y = forceToGround;
        }
        else
        {
            moveDirection.y += forceToGround * gravityMagnitud * Time.deltaTime;
        }

        desiredDirection = transform.forward * axis.y + transform.right * axis.x;

        moveDirection.x = desiredDirection.x * speed;
        moveDirection.z = desiredDirection.z * speed;

        controller.Move(moveDirection * Time.deltaTime);
    }

    public void SetHorizontalAxis(float x)
    {
        axis.x = x;
    }

    public void SetVerticalAxis(float y)
    {
        axis.y = y;
    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            jump = true;
            moveDirection.y = jumpSpeed;
        }
    }

    public void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            if (controller.isGrounded)
            {
                isGrounded = true;
                jump = false;
            }
        }
    }

    public void Run()
    {
        speed = speed * 2;
    }

    public void Walk()
    {
        speed = speed / 2;
    }

    public void SetDamage(int damage)
    {
        life -= damage;
        if(life <= 0)
        {
            dead = true;
            life = 0;

            //SOUND
            //ANIMATION
            //FEEDBACK
            //GAMEOVER??

        }

        else
        {
            //SOUND
            //ANIMATION
        }

        lifeBar.UpdateBar(life);
    }
}
