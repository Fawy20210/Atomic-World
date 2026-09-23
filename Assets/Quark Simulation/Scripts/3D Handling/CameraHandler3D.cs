using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler3D : MonoBehaviour
{
    public float sensitivity = 1;
    InputAction moveAction;
    InputAction lookAction;
    InputAction LeftClickAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        LeftClickAction = InputSystem.actions.FindAction("Click");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveValue = moveAction.ReadValue<Vector3>();
        Vector2 lookValue = lookAction.ReadValue<Vector2>() * sensitivity;
        if (LeftClickAction.IsPressed())
        {
            transform.Rotate(new Vector2(-lookValue.y,lookValue.x),Space.Self);        
        }

        transform.Translate(moveValue);
    }
    public void ResetCamera()
    {
        transform.position = new Vector3(0,0,-40);
        transform.eulerAngles = new Vector3(0,0,0);
    }
}
