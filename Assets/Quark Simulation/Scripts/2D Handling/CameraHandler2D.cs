using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraHandler2D : MonoBehaviour
{
    
    public float MovementScale = 1;
    public float MovementScaleFactor = 2;
    public TMP_InputField MovementScaleInput;
    InputAction moveAction;
    InputAction ScrollAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        ScrollAction = InputSystem.actions.FindAction("Scroll");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 moveValue = moveAction.ReadValue<Vector3>();
        Vector2 scrollValue = ScrollAction.ReadValue<Vector2>();
        if (scrollValue.y < 0)
        {
            MovementScale /= MovementScaleFactor;        
            MovementScaleInput.text = MovementScale.ToString();
        } else if(scrollValue.y > 0)
        {
            MovementScale *= MovementScaleFactor;
            MovementScaleInput.text = MovementScale.ToString();   
        }

        transform.position += new Vector3(moveValue.x, moveValue.z,moveValue.y) * MovementScale;
    }
    public void ResetCamera()
    {
        transform.position = new Vector3(0,0,-40);
        transform.eulerAngles = new Vector3(0,0,0);
    }
    public void setMovementScale()
    {
        float inp;
        if (float.TryParse(MovementScaleInput.text, out inp))
        {
            MovementScale = inp;
        }
        else
        {
            MovementScaleInput.text = MovementScale.ToString();
        }
    }
}
