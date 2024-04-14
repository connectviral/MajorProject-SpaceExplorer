using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class PasswordConfirmation : MonoBehaviour
{
    public InputField passwordField;
    public InputField confirmPasswordField;
    public Text passwordCheckText;
    public UnityEngine.UI.Button submitButton;
    public void CheckPasswords()
    {
        string p1 = passwordField.text;
        string p2 = confirmPasswordField.text;
        if(p1 == p2)
        {
            passwordCheckText.text = "Passwords match";
        }
        else
        {
            passwordCheckText.text = "Passwords do not match. Make sure they match";
        }
    }
}
