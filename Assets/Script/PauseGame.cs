using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public GameObject pauseManager;
    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            pauseManager.SetActive(true);
        }

        if (Input.GetKeyDown(KeyCode.B))
        {
            pauseManager.SetActive(false);
        }
    }

    public void Resume()
    {
        pauseManager.SetActive(false);
    }
    public void Exit()
    {
        SceneManager.LoadScene(0);
    }
}
