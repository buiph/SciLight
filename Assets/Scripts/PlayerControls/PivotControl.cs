using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;

public class PivotControl : MonoBehaviour
{
    private Camera cam;

    PlayerInputAction inputAction;

    Vector2 mouseInput;
    Vector2 stickInput;

    void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.PlayerControls.LookAtMouse.performed += ctx => mouseInput = ctx.ReadValue<Vector2>();
        inputAction.PlayerControls.LookAtStick.performed += ctx => stickInput = ctx.ReadValue<Vector2>();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void FixedUpdate()
    {
        FaceMouse();
        //FaceRightStick();
    }

    /// <summary>
    /// Looks at mouse position
    /// </summary>
    private void FaceMouse()
    {
        Vector3 mouseWorldPos;

        //Get the mouse position in world
        mouseWorldPos = cam.ScreenToWorldPoint(new Vector3(mouseInput.x, mouseInput.y, -Camera.main.transform.position.z));

        //Character face where the mouse is
        Vector2 facingDir = new Vector2(
            mouseWorldPos.x - transform.position.x,
            mouseWorldPos.y - transform.position.y
        );
        transform.up = facingDir;
    }

    /// <summary>
    /// Looks in the direction of the stick
    /// </summary>
    private void FaceRightStick()
    {
       //If the stick was moved
       if (stickInput.magnitude > 0.5f)
       {
           //Face in the direction the stick was moved
           transform.up = stickInput;
       }
    }

    //Enable and disable the input action
    private void OnEnable()
    {
        inputAction.Enable();
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }
}
