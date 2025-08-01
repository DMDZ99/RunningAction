using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuU : MonoBehaviour
{
    public GameObject pausePanel;   // �Ͻ����� �г�
    public GameObject optionPanel;  // �ɼ� �г�
    public GameObject restart; //����� �г�

    private bool isPaused = false;

    void Update()
    {
        // ESC Ű�� �Ͻ�����
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
            // �Ͻ����� ����
            Time.timeScale = 1f;
            isPaused = false;
        }
        else
        {
            // �Ͻ����� ����
            Time.timeScale = 0f;
            isPaused = true;
        }
    }

    // ���� �����
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
    }

    // �ɼ� ����
    public void OpenOptions()
    {
        optionPanel.SetActive(true);
    }

    // �ɼ� �ݱ�
    public void CloseOptions()
    {
        optionPanel.SetActive(false);
    }

    // ���� �ٽ� ���� (�� ��ε�)
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
