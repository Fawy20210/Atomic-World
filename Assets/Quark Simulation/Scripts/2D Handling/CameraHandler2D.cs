using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler2D : MonoBehaviour
{
    InputAction moveAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveValue = moveAction.ReadValue<Vector3>();

        transform.position += new Vector3(moveValue.x, moveValue.z,moveValue.y);
    }
}
