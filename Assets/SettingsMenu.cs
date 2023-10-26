using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    [SerializeField]
    TMP_Dropdown resolution;
    [SerializeField]
    TMP_Dropdown windowType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DismissMenu()
    {
        //gameObject.SetActive(false);
        Destroy(gameObject);
    }

    private void OnEnable()
    {
        Time.timeScale = 0;
    }

    private void OnDestroy()
    {
        Time.timeScale = 1;
    }

    public void ApplyResolution()
    {
        var res_choice = resolution.options[resolution.value].text;
        //Debug.Log(res_choice);

        int x_index = res_choice.IndexOf("X");
        string width = res_choice.Substring(0, x_index - 1).Trim();
        string height = res_choice.Substring(x_index + 1).Trim();

        //Debug.Log("[" + width + "], [" + height + "]");

        bool isFullscreen = windowType.value == 1;

        Screen.SetResolution(int.Parse(width), int.Parse(height), isFullscreen);
    }
}
