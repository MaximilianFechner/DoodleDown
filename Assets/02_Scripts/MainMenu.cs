using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(0);
    }

    public void ExitGame()
    {
        Debug.Log("Close Game");
        Application.Quit();
    }
}
