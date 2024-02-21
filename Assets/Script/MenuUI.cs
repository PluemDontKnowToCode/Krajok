using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuUI : MonoBehaviour
{
    public Animator sceneTransition;
    [SerializeField] private float transitionTime;

    [Header("SoundSetting")]
    public static float soundSetting;
    public static float volumeSetting;
    public Slider volumeSlider, SFXSlider;
    public GameObject BGM;
    public GameObject cutscene;
    /*public void GUIDebug()
    {
        Debug.Log("Clicked");
    }*/
    //We just wanted to check if it's working properly. :/

    public void StartGame(string sceneName)
    {
        StartCoroutine(LoadingScreen(sceneName));
    }
    private void Update()
    {
        soundSetting = SFXSlider.value / 100;
        volumeSetting = volumeSlider.value / 100;
        BGM.GetComponent<AudioSource>().volume = volumeSetting;
    }
    public void ExitGame()
    {
        Application.Quit();
    }

    public void BackToMainMenu(string sceneName)
    {
        SceneManager.LoadSceneAsync(sceneName);
    }

    IEnumerator LoadingScreen(string sceneName)
    {
        sceneTransition.SetTrigger("Play");
        yield return new WaitForSeconds(transitionTime);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        while (!operation.isDone)
        {
            yield return null;
        }
    }
}
