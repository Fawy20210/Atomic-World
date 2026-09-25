using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class CameraHandler3D : MonoBehaviour
{
    public float sensitivity = 1;
    public float MovementScale = 1;
    public float MovementScaleFactor = 2;

    public TMP_InputField MovementScaleInput;
    InputAction moveAction;
    InputAction lookAction;
    InputAction LeftClickAction;
    InputAction ScrollAction;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveAction = InputSystem.actions.FindAction("Move");
        lookAction = InputSystem.actions.FindAction("Look");
        LeftClickAction = InputSystem.actions.FindAction("Click");
        ScrollAction = InputSystem.actions.FindAction("Scroll");
    }

    // Update is called once per frame
    void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject()){
            Vector2 lookValue = lookAction.ReadValue<Vector2>() * sensitivity;
            Vector2 scrollValue = ScrollAction.ReadValue<Vector2>();
            if (scrollValue.y < 0)
            {
                MovementScale /= MovementScaleFactor;        
                MovementScaleInput.text = MovementScale.ToString();
                //Debug.Log((scrollValue.y, MovementScale));      
            } else if(scrollValue.y > 0)
            {
                MovementScale *= MovementScaleFactor;
                MovementScaleInput.text = MovementScale.ToString();
                //Debug.Log((scrollValue.y, MovementScale));      
            }

            if (LeftClickAction.IsPressed())
            {
                transform.Rotate(new Vector2(-lookValue.y,lookValue.x),Space.Self);        
            }
        }

        Vector3 moveValue = moveAction.ReadValue<Vector3>();
        transform.Translate(moveValue * MovementScale);
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
