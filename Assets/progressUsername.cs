using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class progressUsername : MonoBehaviour
{
    public Text progress_text;
    public InputField display_text; 
    
    void Start()
    {
        progress_text.text = PlayerPrefs.GetString("user_name");
    }

    public void Create()
    {
        progress_text.text = display_text.text;
        PlayerPrefs.SetString("user_name", progress_text.text);
        PlayerPrefs.Save();
    }
}