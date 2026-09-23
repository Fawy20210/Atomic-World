using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputHandler3D : MonoBehaviour
{

    public TMP_InputField ParticleCountInput;
    public TMP_InputField upPartInput;
    public TMP_InputField downPartInput;
    public TMP_InputField boundsInput;

    public TMP_InputField TimeFactorInput;
    public TMP_InputField sizeInput;
    public TMP_InputField minDistInput;
    public TMP_InputField maxDistInput;
    public TMP_InputField dampeningInput;
    public TMP_InputField aInput;
    public TMP_InputField oInput;

    public TMP_InputField sizeScaleInput;
    public TMP_InputField UpColorInput;
    public TMP_InputField DownColorInput;
    public RawImage UpColor;
    public RawImage DownColor;


    public Controller3D controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    public void Reload()
    {
        controller.enabled = false;
        controller.ParticleCount = int.Parse(ParticleCountInput.text);
        controller.upPart = int.Parse(upPartInput.text);
        controller.downPart = int.Parse(downPartInput.text);
        controller.bounds = float.Parse(boundsInput.text);
        controller.enabled = true;
    }

    public void Apply()
    {
        controller.TimeFactor = float.Parse(TimeFactorInput.text);
        controller.size = float.Parse(sizeInput.text);
        controller.minDist = float.Parse(minDistInput.text);
        controller.maxDist = float.Parse(maxDistInput.text);
        controller.dampening = 1f - float.Parse(dampeningInput.text);
        controller.a = float.Parse(aInput.text);
        controller.o = float.Parse(oInput.text);
        controller.DoApply = true;
    }


    public void UpdateSizeScale()
    {
        controller.sizeScale = float.Parse(sizeScaleInput.text);
        controller.updateRender = true;
    }

    public void UpdateColors()
    {
        Color newCol;
        if(ColorUtility.TryParseHtmlString(UpColorInput.text+"ff", out newCol))
        {
            controller.UpColor=newCol;
            UpColor.color=newCol;
        }
        else
        {
            controller.UpColor=Color.red;
            UpColor.color=Color.red;
            UpColorInput.text = "#ff0000";
        }
        if(ColorUtility.TryParseHtmlString(DownColorInput.text+"ff", out newCol))
        {
            controller.DownColor=newCol;
            DownColor.color=newCol;
        }
        else
        {
            controller.DownColor=Color.blue;
            DownColor.color=Color.blue;
            DownColorInput.text = "#0000ff";
        }
        controller.updateColors=true;
    }

    public void PauseUpdate()
    {
        if(controller.pause){
            controller.pause=false;
        }
        else
        {
            controller.pause=true;
        }
    }

}
