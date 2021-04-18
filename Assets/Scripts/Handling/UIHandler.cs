using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public enum Mode { playing, pause, options, gameOver};
    private Mode mode;

    [SerializeField] TMP_Text scoreDisplayer;

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
    }

    // Update is called once per frame
    void Update()
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
            mode = Mode.gameOver;
            gameOverMenu.SetActive(true);


            if(currentPlayerScore < (int)playerHelper.Score)
            {
                spd += 0.1f;
                currentPlayerScore += 1 * spd * Time.deltaTime;
            }
            gameOverScoreDisplayer.text = ((int)currentPlayerScore).ToString();
            gameOverHighscoreDisplayer.text = PlayerPrefs.GetInt("Highscore", (int)playerHelper.Score).ToString();

            if((int)currentPlayerScore == PlayerPrefs.GetInt("Highscore", (int)playerHelper.Score) && !vLineSaid)
            {
                HighscoreAnim.SetTrigger("HS");
                SoundManager.Instance.PlayUISoundEffect(NewHighscoreEffects[Random.Range(0, NewHighscoreEffects.Count)]);
                vLineSaid = true;
            }
        }
    }

    public void Escape()
    {
        switch (mode)
        {
            case Mode.playing:
                SoundManager.Instance.PauseUnPauseMusic(true);
                gameUI.SetActive(false);
                pauseMenu.SetActive(true);
                mode = Mode.pause;
                break;
            case Mode.pause:
                SoundManager.Instance.PauseUnPauseMusic(false);
                gameUI.SetActive(true);
                pauseMenu.SetActive(false);
                gameHandler.SetPause(false);
                mode = Mode.playing;
                break;
            case Mode.options:
                pauseMenu.SetActive(true);
                optionsMenu.SetActive(false);
                mode = Mode.pause;
                break;
        }
    }

    public void OpenOptionsFromPauseMenu()
    {
        optionsMenu.SetActive(true);
        pauseMenu.SetActive(false);
        mode = Mode.options;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

}
