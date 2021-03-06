using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public enum Mode { menu, playing, pause, options, gameOver};
    private Mode mode;
    private Mode lastMode;

    [SerializeField] TMP_Text scoreDisplayer;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;
    [Space(5)]
    [SerializeField] GameObject gameOverMenu;
    [SerializeField] TMP_Text gameOverScoreDisplayer;
    [SerializeField] TMP_Text gameOverHighscoreDisplayer;

    public List<AudioClip> NewHighscoreEffects = new List<AudioClip>();

    GameHandler gameHandler;
    PlayerHelper playerHelper;

    float currentPlayerScore = 0;
    bool vLineSaid;
    float spd = 1;

    public Animator HighscoreAnim;
    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameHandler.instance;
        playerHelper = PlayerHelper.instance;
        mode = GameHandler.playing ? Mode.playing : Mode.menu;

        mainMenu.SetActive(!GameHandler.playing);
        gameUI.SetActive(GameHandler.playing);
    }

    // Update is called once per frame
    void Update()
    {
        if(GameHandler.playing)
        {
            if (!playerHelper.IsDead)
            {
                scoreDisplayer.text = "Score: " + ((int)playerHelper.Score).ToString();

                if (Input.GetKeyDown(KeyCode.Escape))
                {
                    Escape();
                }
            }
            else
            {
                if (!gameOverMenu.activeSelf)
                {
                    mode = Mode.gameOver;
                    gameOverMenu.SetActive(true);
                    if((int)playerHelper.Score == PlayerPrefs.GetInt("Highscore", 0))
                    {
                        gameOverHighscoreDisplayer.text = PlayerPrefs.GetInt("OldHighscore", 0).ToString();
                    }
                    else { gameOverHighscoreDisplayer.text = PlayerPrefs.GetInt("Highscore", 0).ToString(); }
                }
                else
                {
                    if(currentPlayerScore < (int)playerHelper.Score)
                    {
                        spd += 0.1f;
                        currentPlayerScore += 1 * spd * Time.deltaTime;
                    }
                    if (currentPlayerScore > Convert.ToInt32(gameOverHighscoreDisplayer.text))
                    {
                        gameOverHighscoreDisplayer.text = ((int)currentPlayerScore).ToString();
                    }
                    gameOverScoreDisplayer.text = ((int)currentPlayerScore).ToString();

                    if((int)currentPlayerScore == PlayerPrefs.GetInt("Highscore", (int)playerHelper.Score) && !vLineSaid)
                    {
                        HighscoreAnim.SetTrigger("HS");
                        SoundManager.Instance.PlayUISoundEffect(NewHighscoreEffects[UnityEngine.Random.Range(0, NewHighscoreEffects.Count)]);
                        vLineSaid = true;
                    }
                }
            }
        }
    }

    private void ChangeMode(Mode newMode)
    {
        lastMode = mode;
        mode = newMode;
    }

    public void StartGame()
    {
        GameHandler.instance.StartGame();
        mainMenu.SetActive(false);
        gameUI.SetActive(true);
        ChangeMode(Mode.playing);
    }

    public void Escape()
    {
        switch (mode)
        {
            case Mode.playing:
                SoundManager.Instance.PauseUnPauseMusic(true);
                gameUI.SetActive(false);
                pauseMenu.SetActive(true);
                ChangeMode(Mode.pause);
                break;
            case Mode.pause:
                SoundManager.Instance.PauseUnPauseMusic(false);
                gameUI.SetActive(true);
                pauseMenu.SetActive(false);
                gameHandler.SetPause(false);
                ChangeMode(Mode.playing);
                break;
            case Mode.options:
                optionsMenu.SetActive(false);
                if(lastMode == Mode.menu) { mainMenu.SetActive(true);}
                else if(lastMode == Mode.pause) { pauseMenu.SetActive(true);}
                ChangeMode(Mode.pause);
                break;
        }
    }

    public void OpenOptionsFromPauseMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        ChangeMode(Mode.options);
    }

    public void OpenOptionsFromMainMenu()
    {
        optionsMenu.SetActive(true);
        mainMenu.SetActive(false);
        ChangeMode(Mode.options);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
