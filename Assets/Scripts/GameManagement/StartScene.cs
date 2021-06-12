using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;

public class StartScene : MonoBehaviour
{
    [SerializeField] private Button _startButton;
    [SerializeField] private Button _quitButton;

    void Start()
    {
        Assert.IsNotNull(_startButton);
        Assert.IsNotNull(_quitButton);

        _startButton.onClick.AddListener( delegate{ StartGame(); } );
        _quitButton.onClick.AddListener( delegate{ Quit(); } );

        AudioManager.Instance.PlayTheme("MenuTheme");
    }

    /// <summary>
    /// Starts the game
    /// </summary>
    public void StartGame()
    {
        AudioManager.Instance.Play("Select");

        SceneManager.LoadScene("SciLightScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("StartGame");
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        AudioManager.Instance.Play("Select");

        Application.Quit();
    }
}
