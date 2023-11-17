using UnityEngine;
using UnityEngine.UI;

public class ButtonController : MonoBehaviour
{
    public Button LoginButton; // Reference to the LoginButton in the Unity Editor

    public GameObject MainMenu; // Reference to the MainMenu panel

    private bool loginButtonEnabled = false; // A flag to keep track of the button's state

    public void OnButtonClick()
    {
        if (!loginButtonEnabled)
        {
            // Enable the LoginButton if it's not already enabled
            LoginButton.gameObject.SetActive(true);

            // Set the flag to true to indicate that the button is now enabled
            loginButtonEnabled = true;
        }
        else
        {
            // Handle the second click logic to switch to the MainMenu panel
            MainMenu.SetActive(true); // Activate the MainMenu panel
        }
    }
}
