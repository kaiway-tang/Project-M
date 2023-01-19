using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int currentScene, nextScene;
    public static Vector3 spawnPosition;
    private void Awake()
    {
        nextScene = -1;
        currentScene = SceneManager.GetActiveScene().buildIndex;
    }
    public static void LoadScene(string sceneName)
    {
        LoadScene(SceneManager.GetSceneByName(sceneName).buildIndex);
    }
    public static void LoadScene(int ID)
    {
        Player.m_HP = Player.playerScript.GetHP();
        SceneManager.LoadScene(ID);
    }

    public static void LoadCurrentScene()
    {
        LoadScene(currentScene);
    }

    public static void LoadNextScene()
    {
        if (nextScene != -1)
        {
            LoadScene(nextScene);
        }
    }



    public static int GetCurrentScene()
    {
        return currentScene;
    }
}
