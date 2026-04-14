using UnityEngine;
using UnityEngine.SceneManagement; 

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene(1); 
    }

    public void QuitGame()
    {
        Debug.Log("OYUN KAPANDI"); 
        Application.Quit(); 
    }
}
