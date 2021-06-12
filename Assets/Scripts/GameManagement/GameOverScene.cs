using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using TMPro;

public class GameOverScene : MonoBehaviour
{
    [SerializeField] GameData scoreData;

    [SerializeField] private TextMeshProUGUI _gameOverTxt;
    [SerializeField] private TextMeshProUGUI _scoreReportTxt;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _quitButton; 

    void Start()
    {
        Assert.IsNotNull(_restartButton);
        Assert.IsNotNull(_quitButton);

        _restartButton.onClick.AddListener( delegate{ Restart(); } );
        _quitButton.onClick.AddListener( delegate{ Quit(); } );

        _gameOverTxt.text = "Game Over";
        _scoreReportTxt.text = "Score: " + scoreData.score;

        AudioManager.Instance.Play("GameOver");
        AudioManager.Instance.PlayTheme("GameOverTheme");
    }

    public void Restart()
    {
        AudioManager.Instance.Play("Select");

        SceneManager.LoadScene("SciLightScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("GameOver");
    }

    public void Quit()
    {
        AudioManager.Instance.Play("Select");

        Application.Quit();
    }
}
