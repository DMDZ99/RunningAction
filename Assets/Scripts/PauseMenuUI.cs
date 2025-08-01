using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuU : MonoBehaviour
{
    public GameObject pausePanel;   // 일시정지 패널
    public GameObject optionPanel;  // 옵션 패널
    public GameObject restart; //재시작 패널

    private bool isPaused = false;

    void Update()
    {
        // ESC 키로 일시정지
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused)
        {
            // 일시정지 해제
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            // 일시정지 시작
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    // 게임 재시작
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    // 옵션 열기
    public void OpenOptions()
    {
        optionPanel.SetActive(true);
    }

    // 옵션 닫기
    public void CloseOptions()
    {
        optionPanel.SetActive(false);
    }

    // 게임 다시 시작 (씬 재로드)
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
