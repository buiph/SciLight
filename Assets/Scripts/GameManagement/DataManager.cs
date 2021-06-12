using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance { get; private set; }

    public LeaderScoreContainer myContainer;

    [SerializeField] private GameObject _learderboardPanel;
    [SerializeField] private Button _backButton;

    public TextMeshProUGUI[] leaderName;
    public TextMeshProUGUI[] leaderScore;
    
    private void Awake()
    {
        if (Instance)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        Assert.IsNotNull( _learderboardPanel );
        Assert.IsNotNull( _backButton );

        _backButton.onClick.AddListener(delegate{ ToggleLeaderboardPanel(); });

        _learderboardPanel.SetActive(false);

        UIInvoker.OnOpenLeaderboard += ToggleLeaderboardPanel;

        myContainer = new LeaderScoreContainer();

        //If running the game for the first time, create a save file
        if (!File.Exists("SaveFiles/Leaderboard.xml"))
        {
            Directory.CreateDirectory("SaveFiles");
        }
        
        LoadData();
    }

    void OnDestroy()
    {
        UIInvoker.OnOpenLeaderboard -= ToggleLeaderboardPanel;
    }

    public void ToggleLeaderboardPanel()
    {
        AudioManager.Instance.Play("ButtonClick");
        _learderboardPanel.SetActive(!_learderboardPanel.activeSelf);
    }

    public void SaveData()
    {
        Stream stream = File.Open("SaveFiles/Leaderboard.xml", FileMode.Create);
        XmlSerializer serializer = new XmlSerializer(typeof(LeaderScoreContainer));
        serializer.Serialize(stream, myContainer);
        stream.Close();
    }

    public void LoadData()
    {
        // If the XML file exists then load the data.
        if (File.Exists("SaveFiles/Leaderboard.xml"))
        {
            Stream stream = File.Open("SaveFiles/Leaderboard.xml", FileMode.Open);
            XmlSerializer serializer = new XmlSerializer(typeof(LeaderScoreContainer));
            myContainer = serializer.Deserialize(stream) as LeaderScoreContainer;
            stream.Close();

            UpdateLeaderboardDisplay();
        }
        else
        {
            ClearLeaderBoard();
        }
    }

    /// <summary>
    /// Write the leader infomations on the leaderboard display
    /// </summary>
    public void UpdateLeaderboardDisplay()
    {
        // Displays the names and scores on the leaderboard
        for (int i = 0; i < myContainer.leaders.Length; i++)
        {
            leaderName[i].text = myContainer.leaders[i].GetName();
            leaderScore[i].text = myContainer.leaders[i].GetScore().ToString();
        }
    }

    /// <summary>
    /// Completely wipe the leaderboard data
    /// </summary>
    public void ClearLeaderBoard()
    {
        for (int i = 0; i < myContainer.leaders.Length; i++)
        {
            myContainer.leaders[i].SetName("");
            myContainer.leaders[i].SetScore(0);

            // Updates the leaderboard display
            leaderName[i].text = myContainer.leaders[i].GetName();
            leaderScore[i].text = myContainer.leaders[i].GetScore().ToString();

            SaveData();
        }
    }

    /// <summary>
    /// Sort the leaderboard with the latest highscore
    /// </summary>
    /// <param name="checkScore"></param>
    /// <param name="checkName"></param>
    public void SortTopScores(int checkScore, string checkName)
    {
        int tempScore;
        string tempName;

        // Sort from lowest position
        // If current score is higher than the next score, swap their position
        for (int i = 0; i < myContainer.leaders.Length; i++)
        {
            if (myContainer.leaders[i].GetScore() >= checkScore || myContainer.leaders[i].GetScore() == 0f)
            {
                tempScore = myContainer.leaders[i].GetScore();
                tempName = myContainer.leaders[i].GetName();

                myContainer.leaders[i].SetScore(checkScore);
                myContainer.leaders[i].SetName(checkName);

                checkScore = tempScore;
                checkName = tempName;
            }
        }
    }

    public bool IsNewHighScore(int score)
    {
        if (score > myContainer.leaders[myContainer.leaders.Length - 1].GetScore())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
