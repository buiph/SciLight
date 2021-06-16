using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GM : MonoBehaviour
{
    public static GM Instance { get; private set; }

    [SerializeField] GameObject playerChar;
    [SerializeField] GameObject[] enemies;
    public float spawnDelay;
    private float _timeStamp;
    private int _upperRange;

    private int _currEnemyCount;
    internal int spawnCap;
    internal int killCount;

    [SerializeField] GameData scoreData;
    [SerializeField] TextMeshProUGUI killCountText;
    [SerializeField] private Button _resumeButton;
    [SerializeField] private Button _quitButton;
    [SerializeField] GameObject pausePanel;
    internal bool paused;

    private Vector2 _bounds;

    //Input actions
    PlayerInputAction inputAction;

    void Awake()
    {
        inputAction = new PlayerInputAction();
        inputAction.PlayerControls.Pause.performed += ctx => Pause();
    }

    void Start()
    {
        Scene currScene = SceneManager.GetSceneByName("SciLightScene");
        SceneManager.SetActiveScene(currScene);

        Assert.IsNotNull(scoreData);
        Assert.IsNotNull(killCountText);
        Assert.IsNotNull(_resumeButton);
        Assert.IsNotNull(_quitButton);
        Assert.IsNotNull(pausePanel);

        _resumeButton.onClick.AddListener( delegate{ Pause(); } );
        _quitButton.onClick.AddListener( delegate{ Quit(); } );

        // Screen bounds
        _bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));

        SpawnPlayer();
        _currEnemyCount = 0;
        spawnCap = 3;
        killCount = 0;

        paused = false;
        pausePanel.SetActive(false);
    }

    void OnEnable()
    {
        inputAction.Enable();

        AudioManager.Instance.Play("PlayerSpawn");
        AudioManager.Instance.PlayTheme("BattleTheme");

        spawnDelay = 2.5f;
        _timeStamp = Time.time;
        _upperRange = 1;
    }

    private void OnDisable()
    {
        inputAction.Disable();
    }
    
    void Update()
    {
        killCountText.text = killCount.ToString();

        if (Time.time >= _timeStamp + spawnDelay && _currEnemyCount < spawnCap)
        {
            _timeStamp = Time.time;

            StartCoroutine(SpawnEnemy(Random.Range(0, _upperRange)));
        }
    }

    /// <summary>
    /// Spawns the Player prefab
    /// </summary>
    void SpawnPlayer()
    {
        GameObject player = Instantiate(playerChar, Vector3.zero, playerChar.transform.rotation);
        player.GetComponent<MainCharControl>().OnDeath += GameOver; // Subcribe to OnDeath when the player spawns
    }

    /// <summary>
    /// Spawns an enemy into the scene at a random point
    /// </summary>
    IEnumerator SpawnEnemy(int i)
    {
        Vector3 targetPos = new Vector3(Random.Range(_bounds.x, _bounds.x * -1), Random.Range(_bounds.y, _bounds.y * -1), 0); //Find a random point in bounds

        // If that point is too close to player, find a different point
        while (Vector3.Distance(targetPos, playerChar.transform.position) <= 7)
        {
            targetPos = new Vector3(Random.Range(_bounds.x, _bounds.x * -1), Random.Range(_bounds.y, _bounds.y * -1), 0);
            yield return null;
        }
        
        AudioManager.Instance.Play("EnemySpawn");
        GameObject enemy = Instantiate(enemies[i], targetPos, enemies[i].transform.rotation);
        enemy.GetComponent<EnemyAI>().OnDeath += IncreaseKillCount; // Subcribe to OnDeath when the enemy spawns
        _currEnemyCount++;
    }

    /// <summary>
    /// Increase the killcount
    /// </summary>
    void IncreaseKillCount()
    {
        killCount++;
        _currEnemyCount--;
        scoreData.score = killCount;

        if (killCount == 5)
        {
            spawnDelay = 1.7f;
        }

        if (killCount == 10)
        {
            _upperRange = 2;
        }

        if (killCount == 15)
        {
            spawnDelay = 1.5f;
        }

        if (killCount == 20)
        {
            spawnCap = 4;
        }
    }

    /// <summary>
    /// Pauses the game
    /// </summary>
    public void Pause()
    {
        AudioManager.Instance.Play("Select");

        if (!paused)
        {
            pausePanel.gameObject.SetActive(true);
            Time.timeScale = 0;
            paused = true;
        }
        else
        {
            pausePanel.gameObject.SetActive(false);
            Time.timeScale = 1;
            paused = false;
        }
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void Quit()
    {
        AudioManager.Instance.Play("Select");

        Application.Quit();
    }

    /// <summary>
    /// Ends the game and load the game over scene
    /// </summary>
    void GameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("SciLightScene");
    }


}
