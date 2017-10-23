using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private Vector2 inputAxis;
    private PlayerBehaviour player;

    private CameraBehaviour cameraBehaviour;
    private Vector2 mouseAxis;
    public float sensitivity;

    private MouseCursor mouseCursor;
    public Gun gun;
    
	void Start ()
    {
        player = GetComponent<PlayerBehaviour>();
        cameraBehaviour = GetComponentInChildren<CameraBehaviour>();
        mouseCursor = GetComponent<MouseCursor>();
	}
	
	void Update ()
    {
        inputAxis.x = Input.GetAxis("Horizontal");
        player.SetHorizontalAxis(inputAxis.x);
        inputAxis.y = Input.GetAxis("Vertical");
        player.SetVerticalAxis(inputAxis.y);

        if(Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }

        if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            player.Walk();
        }

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            player.Run();
        }

        //INPUT CAMERA
        mouseAxis.x = Input.GetAxis("Mouse X") * sensitivity;
        mouseAxis.y = Input.GetAxis("Mouse Y") * sensitivity;

        cameraBehaviour.SetRotationX(mouseAxis.y);
        cameraBehaviour.SetRotationY(mouseAxis.x);

        //INPUT CURSOR
        if (Input.GetButtonDown("Cancel"))
        {
            mouseCursor.Show();
        }

        if (Input.GetMouseButtonDown(0))
            mouseCursor.Hide();

        //SHOT
        if (Input.GetButtonDown("Fire1")) gun.TryShot();

        
        if (gun.auto)
        {
            if (Input.GetButton("Fire1")) gun.TryShot();
        }
        else
        {
            if (Input.GetButtonDown("Fire1")) gun.TryShot();
        }

        //TEST DAMAGE
        if (Input.GetKeyDown(KeyCode.L)) player.SetDamage(5);
    }
}
