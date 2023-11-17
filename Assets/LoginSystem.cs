// LoginSystem.cs
using UnityEngine;
using UnityEngine.UI;

public class LoginSystem : MonoBehaviour
{
    public InputField usernameInput;

    public void SaveUsername()
    {
        string username = usernameInput.text;
        // Save the username (you can use PlayerPrefs or other data-saving methods)
        PlayerPrefs.SetString("Username", username);
    }
}
