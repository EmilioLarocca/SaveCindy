using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using FMODUnity;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private Button levelTwoButton;
    [SerializeField] private Button levelTwoButton2;
    [SerializeField] private Button endGameButton;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button playButton;
    [SerializeField] private GameObject keys;
    [SerializeField] private GameObject keys2;
    [SerializeField] private GameObject keys3;
    [SerializeField] private TextMeshProUGUI scoreHead;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreToLevel2;
    [SerializeField] private TextMeshProUGUI endScore;
    [SerializeField] private TextMeshProUGUI finalScore;
    [SerializeField] private TextMeshProUGUI highScoreHead;
    [SerializeField] private TextMeshProUGUI highScoreText;
    [SerializeField] private TextMeshProUGUI newHighScoreText;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private TextMeshProUGUI endgameText;
    [SerializeField] private TextMeshProUGUI newHighScore;
    [SerializeField] private TextMeshProUGUI timerHead;
    [SerializeField] private TextMeshProUGUI timeOverText;
    [SerializeField] private TextMeshProUGUI unlockedLevel2;
    [SerializeField] private TextMeshProUGUI livesText;
    [SerializeField] private TextMeshProUGUI livesAmount;
    [SerializeField] private TextMeshProUGUI lifeUp;
    [SerializeField] private TextMeshProUGUI youLose;

    public bool stopGame;
    public bool paused;
    public bool isCindyDead;
    private bool changeColor = false;
    private bool changeText = false;
    public bool isNewHighScore = false;
    public bool levelTwo = false;
    public bool restart = false;
    public bool firstTimeLV2 = true;
    public bool timerOn = false;

    public float timeLeft;
    public int livesLeft;
    public int score = -1;
    private int hiddenScore;
    private int enemiesLeft;

    public GameObject pauseScreen;
    public GameObject cindy;
    public GameObject gun;
    public GameObject pointer;
    public GameObject crosshair;
    private GameObject[] teleport;
    private GameObject[] emptyCorner;

    [SerializeField] private Music music;
    [SerializeField] private SpawnManager spawnManager;
    private ObjectPooler objectPooler;
    [SerializeField] private StudioEventEmitter lifeUpSound;

    void Awake()
    {
        stopGame = true;
        firstTimeLV2 = true;
        levelTwo = false;
        timerOn = false;
        restart = false;
        
        if (isCindyDead == true)
        {
            isCindyDead = false;
        }

        music.toStart = 1; // 1 is active; 0 is inactive
        music.completed = 0; // 0 = inactive; 1 = end level; 2 = end game
        music.toLevel = 0; // 1 = level 1; 2 = level 2
        music.toDeath = 0; // 1 is active; 0 is inactive
        
        SetUIOnAwake();

        if (FMODUnity.RuntimeManager.HasBankLoaded("Master"))
        {
            Debug.Log("Master Bank Loaded");
        }

        objectPooler = FindObjectOfType<ObjectPooler>().GetComponent<ObjectPooler>();

        emptyCorner = GameObject.FindGameObjectsWithTag("EmptyCorner");
        teleport = GameObject.FindGameObjectsWithTag("Teleport");

        //playButton.onClick.AddListener(StartGame);
        livesAmount.text = "0";

        // reset the scene
        cindy.SetActive(false);
        gun.SetActive(false);
        crosshair.SetActive(false);

        // activate empty corners
        foreach (var teleportObject in teleport)
        {
            teleportObject.SetActive(false);
        }
        foreach (var emptyCornerObject in emptyCorner)
        {
            emptyCornerObject.SetActive(true);
        }
    }

    public void StartGame()
    {
        stopGame = false;
        isCindyDead = false;
        restart = true;
        timerOn = true;

        // LEVEL 2
        if (levelTwo == true)
        {
            // reset music
            if (music.completed >= 1)
            {
                music.completed = 0;
            } 
            if (music.toStart == 1)
            {
                music.toStart = 0;
            } 
            if (music.toDeath == 1)
            {
                music.toDeath = 0;
            }

            // enable level 2 music
            if (music.toLevel < 2)
            {
                music.toLevel = 2;
            }

            // decrease life count
            if (firstTimeLV2  == true)
            {
                livesLeft = 0;
                firstTimeLV2 = false;
            } 
            else if (firstTimeLV2 == false)
            {
                UpdateLives(-1);
            }
            
            // replace empycorners with teleports
            foreach (var emptyCornerObject in emptyCorner)
            {
                if (emptyCornerObject.activeSelf)
                {
                    emptyCornerObject.SetActive(false);
                }
            }
            foreach (var teleportObject in teleport)
            {
                if (!teleportObject.activeSelf)
                {
                    teleportObject.SetActive(true);
                }
            }
            
            scoreToLevel2.gameObject.SetActive(false);
            unlockedLevel2.gameObject.SetActive(false);
            endScore.gameObject.SetActive(false);

            ActivateLevelTwoUIText();
        }
        
        // LEVEL 1
        else if (levelTwo == false)
        {
            // reset music
            if (music.completed >= 1)
            {
                music.completed = 0;
            }
            if (music.toStart == 1)
            {
                music.toStart = 0;
            } 
            if (music.toDeath == 1)
            {
                music.toDeath = 0;
            }

            // enable level 1 music
            if (music.toLevel == 0 || music.toLevel == 2)
            { 
                music.toLevel = 1;
            }
            
            score = 0;
            enemiesLeft = 34;
            UpdateScore(0);
            lifeUpSound.Stop();
            changeText = false;

            titleScreen.SetActive(false);
            playButton.gameObject.SetActive(false);
            keys.SetActive(false);
            keys3.SetActive(true);
            scoreToLevel2.gameObject.SetActive(true);

            // replace empycorners with teleports
            foreach (var emptyCornerObject in emptyCorner)
            {
                if (!emptyCornerObject.activeSelf)
                {
                    emptyCornerObject.SetActive(true);
                }
            }
            foreach (var teleportObject in teleport)
            {
                if (teleportObject.activeSelf)
                {
                    teleportObject.SetActive(false);
                }
            }
        }

        UpdateTimer(59);
        hiddenScore = 0;

        highScoreHead.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        scoreHead.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        timerHead.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        objectPooler.ResetPooledObjects();

        cindy.SetActive(true);
        gun.SetActive(true);

        cindy.GetComponent<MoveCindy>().GenerateSpawnPosition();
        AudioManagerUI.instance.PlayUiClick();
    }

    void ChangePaused()
    {
        if (!paused && !stopGame)
        {
            paused = true;
            pauseScreen.SetActive(true);
            Time.timeScale = 0;
            AudioManagerUI.instance.PlayUiPause();
            music.gamePaused = 0; // FMOD snapshot
        }
        else if (paused)
        {
            paused = false;
            pauseScreen.SetActive(false);
            Time.timeScale = 1;
            AudioManagerUI.instance.PlayUiPause();
            music.gamePaused = 1; // FMOD snapshot
        }
    }

    void Update()
    {
        if (titleScreen.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            StartGame();
        }
        if (gameOverText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return) ||
            timeOverText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return) ||
            endgameText.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            RestartGame();
        }

        if (Input.GetKeyDown(KeyCode.Z) && levelTwoButton2.gameObject.activeSelf)
        {
            RestartGame();
        }
        if (Input.GetKeyDown(KeyCode.X) && levelTwoButton2.gameObject.activeSelf)
        {
            EndGame();
        }
        
        if (stopGame)
        {
            pointer.SetActive(true);
            crosshair.SetActive(false);
        } else
        {
            pointer.SetActive(false);
            crosshair.SetActive(true);
        }

        if (timerOn)
        {
            Timer();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            ChangePaused();
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Call your exit game function
            ExitGame();
        }
    }

    void ExitGame()
    {
        // Implement your exit game logic here
#if UNITY_EDITOR
        // If running in the Unity Editor, stop playing the scene
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // If running a standalone build, quit the application
        Application.Quit();
#endif
    }

    void Timer()
    {
        // countdown
        if (timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            int updatedTime = UpdateTimer(Mathf.RoundToInt(timeLeft - 1));
            timerText.text = "" + updatedTime;
        }
        
        // time's over
        else
        {
            timeLeft = 0;
            timerOn = false;
            stopGame = true;

            highScoreHead.gameObject.SetActive(false);
            highScoreText.gameObject.SetActive(false);
            scoreHead.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            timerHead.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);

            if (scoreToLevel2.gameObject.activeSelf)
            {
                scoreToLevel2.gameObject.SetActive(false);
            }
            if (unlockedLevel2.gameObject.activeSelf)
            {
                unlockedLevel2.gameObject.SetActive(false);
            }
            if (livesText.gameObject.activeSelf)
            {
                livesText.gameObject.SetActive(false);
            }
            if (livesAmount.gameObject.activeSelf)
            {
                livesAmount.gameObject.SetActive(false);
            }
            if (lifeUp.gameObject.activeSelf)
            {
                lifeUp.gameObject.SetActive(false);
            }
            
            cindy.SetActive(false);
            gun.SetActive(false);
            crosshair.SetActive(false);
            
            EndGameBehaviour();
        }
    }

    public void EndGameBehaviour()
    {
        // level 1, 33 points
        if (score >= 33)
            {
            
            // level 1
            if (levelTwo == false && firstTimeLV2 == true)
            {
                if (isCindyDead == false) // TO LEVEL 2
                {
                    music.toLevel = 0;
                    music.completed = 1;
                    levelTwo = true;

                    keys3.SetActive(false);
                    timeOverText.gameObject.SetActive(true);
                    timeOverText.text = "TIME'S OVER";
                    endScore.gameObject.SetActive(true);
                    endScore.text = "SCORE: " + score;
                    endScore.color = new Color(80f / 255f, 180f / 255f, 90f / 255f); // green
                    levelTwoButton.gameObject.SetActive(true);
                    keys.SetActive(true);
                    //levelTwoButton.onClick.AddListener(RestartGame);
                }
                else if (isCindyDead == true) // GAME OVER
                {
                    GameOver();
                }
            }
                
            // level 2, more than 1 lives || CONTINUE LEVEL 2
            if (levelTwo == true && livesLeft >= 1)
            {
                firstTimeLV2 = false;

                if (isCindyDead == false)  // END GAME
                {
                    EndGame();
                }
                else if (isCindyDead == true) //REPLAY LEVEL 2
                {
                    stopGame = true;
                    timerOn = false;
                    
                    youLose.gameObject.SetActive(true);
                    DeactivateLevelTwoUIText();

                    endScore.gameObject.SetActive(true);
                    endScore.text = "SCORE: " + score;
                    endScore.color = new Color(180f / 255f, 80f / 255f, 84f / 255f); // red
                    endGameButton.gameObject.SetActive(true);
                    //endGameButton.onClick.AddListener(EndGame);
                    levelTwoButton2.gameObject.SetActive(true);
                    keys2.SetActive(true);
                    //levelTwoButton2.onClick.AddListener(RestartGame);
                }
            }

            // level 2, no more lives 
            if (levelTwo == true && livesLeft == 0 && firstTimeLV2 == false)
            {        
                if (isCindyDead == false)
                {
                    EndGame();
                }
                else if (isCindyDead == true)
                {
                    GameOver();
                }
            }
        }

        // level 1, less than 33 points
        if (score < 33 && isCindyDead == true)
        {
            GameOver();
        }
        else if (score < 33 && isCindyDead == false)
        {
            GameOver();
        }
    }

    void DeactivateLevelTwoUIText()
    {
        highScoreHead.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        scoreHead.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        livesAmount.gameObject.SetActive(false);
        lifeUp.gameObject.SetActive(false);
        timerHead.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
    }

    void ActivateLevelTwoUIText()
    {
        highScoreHead.gameObject.SetActive(true);
        highScoreText.gameObject.SetActive(true);
        scoreHead.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        livesText.gameObject.SetActive(true);
        livesAmount.gameObject.SetActive(true);
        lifeUp.gameObject.SetActive(true);
        timerHead.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);
    }

    void SetUIOnAwake()
    {
        titleScreen.SetActive(true);
        playButton.gameObject.SetActive(true);
        keys.SetActive(true);

        keys2.SetActive(false);
        keys3.SetActive(false);
        scoreHead.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        newHighScoreText.gameObject.SetActive(false);
        highScoreHead.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        timerHead.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);
        timeOverText.gameObject.SetActive(false);
        endScore.gameObject.SetActive(false);
        finalScore.gameObject.SetActive(false);
        scoreToLevel2.gameObject.SetActive(false);
        unlockedLevel2.gameObject.SetActive(false);
        livesText.gameObject.SetActive(false);
        livesAmount.gameObject.SetActive(false);
        lifeUp.gameObject.SetActive(false);
        youLose.gameObject.SetActive(false);
        endgameText.gameObject.SetActive(false);
        levelTwoButton2.gameObject.SetActive(false);
        endGameButton.gameObject.SetActive(false);
    }

    int UpdateTimer(int currentTime)
    {
        // sets time to integer, seconds
        currentTime += 1;
        int seconds = Mathf.RoundToInt(currentTime % 60); 
        return seconds;
    }

    public void UpdateScore(int scoreToAdd)
    {
        score += scoreToAdd;
        scoreText.text = (score).ToString();
        scoreToLevel2.text = (enemiesLeft - 1) + " to lv2";

        // counts enemies left to unlock level 2
        if (timerOn && levelTwo == false)
        {
            if (enemiesLeft > 0)
            {
                enemiesLeft -= 1;
            }
            if (enemiesLeft < 1)
            {
                scoreText.color = new Color(80f / 255f, 180f / 255f, 90f / 255f); // green
                scoreToLevel2.gameObject.SetActive(false);
                keys3.SetActive(false);
                unlockedLevel2.gameObject.SetActive(true);
            }
        }

        CheckHighScore();
        UpdateHighScoreText();
    }

    void CheckHighScore()
    {
        if (score > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", score);
            newHighScoreText.gameObject.SetActive(true);

            if (!isNewHighScore)
            {
                isNewHighScore = true;
                lifeUpSound.Play();
                changeText = true;
            }
        }
        else
        {
            isNewHighScore = false;
        }
    }

    void UpdateHighScoreText()
    {
        highScoreText.text = $"{PlayerPrefs.GetInt("HighScore", 0)}";
    }

    void FinalScore()
    {
        if (changeText == true)
        {
            newHighScore.gameObject.SetActive(true);
            newHighScore.text = "NEW HIGHSCORE: " + score;
        } else 
        {
            finalScore.gameObject.SetActive(true);
            finalScore.text = "SCORE: " + score;
        }
    }

    // triggers the lives count based on score
    public void HiddenScoreLV2(int hiddenScoreToAdd)
    {
        hiddenScore += hiddenScoreToAdd;

        // adds one life every 10 hiddenscores
        if (hiddenScore % 10 == 0)
        {
            UpdateLives(1);
        }
    }

    public void UpdateLives(int lifeToAdd)
    {
        livesLeft += lifeToAdd;
        livesAmount.text = "" + livesLeft;
        
        if (lifeToAdd > 0)
        {
            lifeUpSound.Play();
        }

        if (livesLeft > 0)
        {
            livesAmount.color = new Color(80f / 255f, 180f / 255f, 90f / 255f); // green
        }
        else if (livesLeft == 0)
        {
            livesAmount.color = new Color(180f / 255f, 80f / 255f, 84f / 255f); // red
        }
    }

    // stop footsteps sound
    void ResetEnemies()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            MoveEnemy moveEnemy = enemy.GetComponent<MoveEnemy>();
            if (moveEnemy != null && enemy.activeSelf)
            {
                moveEnemy.StopFootsteps();
            }

            Destroy(enemy);
        }
    }


    public void RestartGame()
    {
        spawnManager.waveNumber = 0;
        
        restart = false;
        timeLeft = 59;

        if (music.completed == 2)
        {
            music.toLevel = 1; 
        }

        // deactivate all texts and buttons

        if (gameOverText.gameObject.activeSelf)
        {
            gameOverText.gameObject.SetActive(false);
        }
        if (youLose.gameObject.activeSelf)
        {
            youLose.gameObject.SetActive(false);
        }                   
        if (endgameText.gameObject.activeSelf)
        {
            endgameText.gameObject.SetActive(false);
        }
        if (restartButton.gameObject.activeSelf)
        {
            restartButton.gameObject.SetActive(false);
        }
        if (levelTwoButton.gameObject.activeSelf)
        {
            levelTwoButton.gameObject.SetActive(false);
        }
        if (timeOverText.gameObject.activeSelf)
        {
            timeOverText.gameObject.SetActive(false);
        }
        if (endScore.gameObject.activeSelf)
        {
            endScore.gameObject.SetActive(false);
        }
        if (finalScore.gameObject.activeSelf)
        {
            finalScore.gameObject.SetActive(false);
        }
        if (levelTwoButton2.gameObject.activeSelf)
        {
            levelTwoButton2.gameObject.SetActive(false);
        }
        if (endGameButton.gameObject.activeSelf)
        {
            endGameButton.gameObject.SetActive(false);
        }
        if (newHighScore.gameObject.activeSelf)
        {
            newHighScore.gameObject.SetActive(false);
        }
        if (newHighScoreText.gameObject.activeSelf)
        {
            newHighScoreText.gameObject.SetActive(false);
        }
        if (keys2.activeSelf)
        {
            keys2.SetActive(false);
        }
        if (keys.activeSelf)
        {
            keys.SetActive(false);
        }
        
        scoreText.color = new Color(180f / 255f, 80f / 255f, 84f / 255f); // red

        // deactivate Cindy and Gun
        if (cindy.activeSelf)
        {
            cindy.SetActive(false);
        }
        if (gun.activeSelf)
        {
            gun.SetActive(false);
        }

        ResetEnemies();
        StartGame();
    }

    public void GameOver()
    {
        music.toLevel = 0;
        music.toDeath = 1;

        stopGame = true;
        firstTimeLV2 = true;
        levelTwo = false;
        timerOn = false;
        restart = false;
        isNewHighScore = false;
        
        gameOverText.gameObject.SetActive(true);
        FinalScore();
        restartButton.gameObject.SetActive(true);
        keys.SetActive(true);
        //restartButton.onClick.AddListener(RestartGame);

        highScoreHead.gameObject.SetActive(false);
        highScoreText.gameObject.SetActive(false);
        scoreHead.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);

        timerHead.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        if (keys3.activeSelf)
        {
            keys3.SetActive(false);
        }
        if (livesText.gameObject.activeSelf)
        {
            livesText.gameObject.SetActive(false);
        }
        if (livesAmount.gameObject.activeSelf)
        {
            livesAmount.gameObject.SetActive(false);
        }
        if (lifeUp.gameObject.activeSelf)
        {
            lifeUp.gameObject.SetActive(false);
        }
        if (scoreToLevel2.gameObject.activeSelf)
        {
            scoreToLevel2.gameObject.SetActive(false);
        }
        if (unlockedLevel2.gameObject.activeSelf)
        {
            unlockedLevel2.gameObject.SetActive(false);
        }
        if (levelTwoButton2.gameObject.activeSelf)
        {
            levelTwoButton2.gameObject.SetActive(false);
        }
        if (endGameButton.gameObject.activeSelf)
        {
            endGameButton.gameObject.SetActive(false);
        }
        if (endgameText.gameObject.activeSelf)
        {
            endgameText.gameObject.SetActive(false);
        }

        cindy.SetActive(false);
        gun.SetActive(false);
        crosshair.SetActive(false);
    }

    public void EndGame()
    {
        music.toLevel = 0;
        music.completed = 2;

        stopGame = true;
        firstTimeLV2 = true;
        levelTwo = false;
        timerOn = false;
        restart = false;
        isNewHighScore = false;

        if (youLose.gameObject.activeSelf)
        {
            youLose.gameObject.SetActive(false);
        }
        if (levelTwoButton2.gameObject.activeSelf)
        {
            levelTwoButton2.gameObject.SetActive(false);
        }
        if (endGameButton.gameObject.activeSelf)
        {
            endGameButton.gameObject.SetActive(false);
        }
        if (endScore.gameObject.activeSelf)
        {
            endScore.gameObject.SetActive(false);
        }
        if (keys2.activeSelf)
        {
            keys2.SetActive(false);
        }
        if (keys.activeSelf)
        {
            keys.SetActive(false);
        }
        
        endgameText.gameObject.SetActive(true);
        FinalScore();
        restartButton.gameObject.SetActive(true);
        keys.SetActive(true);
        //restartButton.onClick.AddListener(RestartGame);

        cindy.SetActive(false);
        gun.SetActive(false);
        crosshair.SetActive(false);
    }
}
