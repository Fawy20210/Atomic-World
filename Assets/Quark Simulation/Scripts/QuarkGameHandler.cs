using UnityEngine;

public class QuarkGameHandler : MonoBehaviour
{
    public Canvas UI3D;
    public CameraHandler3D cameraHandler3D;
    public Controller3D controller3D;
    public Canvas UI2D;
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
        UI2D.enabled = true;
        cameraHandler2D.enabled = true;
        controller2D.enabled = true;
    }
    public void Start3D()
    {
        SelectionScreen.enabled = false;
        UI3D.enabled = true;
        cameraHandler3D.enabled = true;
        controller3D.enabled = true;
    }
    public void EndAll()
    {
        SelectionScreen.enabled = true;
        UI2D.enabled = false;
        cameraHandler2D.enabled = false;
        controller2D.enabled = false;
        controller2D.pause = false;
        UI3D.enabled = false;
        cameraHandler3D.enabled = false;
        controller3D.enabled = false;
        controller3D.pause = false;
    }
    public void QuitGame()
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
