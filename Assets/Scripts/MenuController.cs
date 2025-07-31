using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // 전체 메뉴 패널
    public GameObject optionPanel; // 옵션 패널 (선택)



    // 다시하기 버튼 (씬 재시작)
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // 메뉴 열기 (예: ESC 등)
    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // 일시정지
    }

 

    // 옵션 버튼 (옵션 패널 열기/닫기)
    public void OnClickOption()
    {
        if (optionPanel != null)
            optionPanel.SetActive(!optionPanel.activeSelf);
    }
}
