using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartSceneButtons : MonoBehaviour
{
   public void StartButton()
   {
       SceneManager.LoadScene("MainScene");
   }
    //게임종료로직
    public void QuitGame()
    { 
        Application.Quit();
    }
}
