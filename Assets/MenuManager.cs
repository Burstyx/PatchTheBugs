using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{ 
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        SceneManager.LoadScene(2);
    }

    public void LeaveGame()
    {
        Application.Quit();
    }
}
