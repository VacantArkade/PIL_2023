using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public GameObject settingsMenu;

    public float volumeSFX = 1.0f;
    public float volumeBGM = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenSettingsMenu()
    {
        /*var existingMenu = FindObjectOfType<SettingsMenu>();
        if (existingMenu != null)
        {
            existingMenu.gameObject.SetActive(true);
            return;
        }*/
            
        Instantiate(settingsMenu);
    }

    public void ResetLevel()
    {
        StartCoroutine(RestartLevel());
    }

    public void BackToMenu()
    {

    }

    public void PrepNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        var scene = SceneManager.LoadSceneAsync(nextLevelIndex);
        scene.allowSceneActivation = false;
    }

    public IEnumerator NextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        var scene = SceneManager.LoadSceneAsync(nextLevelIndex);
        scene.allowSceneActivation = false;

        var fade = FindObjectOfType<FadeCanvas>();

        fade.SetDesiredAlpha(3);

        while(fade.Fading)
        {
            //Debug.Log("fading");
            yield return new WaitForSeconds(0.1f);
        }

        scene.allowSceneActivation = true;
    }

    IEnumerator RestartLevel()
    {
        //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        var fade = FindObjectOfType<FadeCanvas>();

        fade.SetDesiredAlpha(1);

        //while (fade.Fading)
        {
            //Debug.Log("fading");
            yield return new WaitForSeconds(2f);
        }

        Debug.Log("restarting");
        //scene.allowSceneActivation = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        int nextLevelIndex = currentLevelIndex + 1;
        SceneManager.LoadScene(nextLevelIndex);
    }
}
