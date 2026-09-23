using TMPro;
using UnityEngine;

public class InputValidation2D : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void KeepAboveZeroInt(TMP_InputField inputField)
    {
        int value;
        if (!int.TryParse(inputField.text, out value))
        {
            inputField.text = "1";
        } else if (value < 1)
        {
            inputField.text = "1";
        }
    }
}
