using UnityEngine;

public class QuarkGameHandler : MonoBehaviour
{
    public CameraHandler3D cameraHandler3D;
    public Controller3D controller3D;
    public CameraHandler2D cameraHandler2D;
    public Controller2D controller2D;

    public Canvas SelectionScreen;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Start2D()
    {
        SelectionScreen.enabled = false;
        cameraHandler2D.enabled = true;
        controller2D.enabled = true;
    }
    public void Start3D()
    {
        SelectionScreen.enabled = false;
        cameraHandler3D.enabled = true;
        controller3D.enabled = true;
    }
    public void EndAll()
    {
        SelectionScreen.enabled = true;
        cameraHandler2D.enabled = false;
        controller2D.enabled = false;
        cameraHandler3D.enabled = false;
        controller3D.enabled = false;
    }
    public void Quit()
    {
        
    }
}
