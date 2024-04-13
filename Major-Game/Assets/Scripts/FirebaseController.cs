using UnityEngine.UI;
using UnityEngine;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FirebaseController : MonoBehaviour
{

    public GameObject loginPanel, signupPanel, notificationPanel;

    public InputField loginEmail, loginPassword, signupEmail, signupPassword, signupCPassword, signupUserName;

    public Text notif_Title_Text, notif_Message_Text;

    public void OpenLoginPanel() {

        loginPanel.SetActive(true);
        signupPanel.SetActive(false);
    }
    public void OpenSignUpPanel() {

        loginPanel.SetActive(false);
        signupPanel.SetActive(true);

    }
    public void LoginUser()
    {

        if (string.IsNullOrEmpty(loginEmail.text) && string.IsNullOrEmpty(loginPassword.text))
            showNotificationMessage("Error", "Empty fields");

        return;

    }

    // Do Login

    public void SignupUser()
    {

        if (string.IsNullOrEmpty(signupEmail.text) && string.IsNullOrEmpty(signupUserName.text) && string.IsNullOrEmpty(signupPassword.text) && string.IsNullOrEmpty(signupCPassword.text))
            showNotificationMessage("Error", "Empty fields");

        return;

    }

    // Do Signup




    private void showNotificationMessage(string title, string message)
    {
        notif_Title_Text.text = "" + title;
        notif_Message_Text.text = "" + message;

        notificationPanel.SetActive(true);
    }

    public void CloseNotif_Panel()
    {
        notif_Title_Text.text = "";
        notif_Message_Text.text = "";

        notificationPanel.SetActive(false);
    } 
}