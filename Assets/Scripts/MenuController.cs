using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuPanel; // ��ü �޴� �г�
    public GameObject optionPanel; // �ɼ� �г� (����)



    // �ٽ��ϱ� ��ư (�� �����)
    public void OnClickRetry()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


    // �޴� ���� (��: ESC ��)
    public void OpenMenu()
    {
        menuPanel.SetActive(true);
        Time.timeScale = 0f; // �Ͻ�����
    }

 

    // �ɼ� ��ư (�ɼ� �г� ����/�ݱ�)
    public void OnClickOption()
    {
        if (optionPanel != null)
            optionPanel.SetActive(!optionPanel.activeSelf);
    }
}
