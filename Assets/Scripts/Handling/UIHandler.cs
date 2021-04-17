using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    public enum Mode { playing, pause, options};
    private Mode mode;

    [SerializeField] TMP_Text scoreDisplayer;

    [SerializeField] GameObject gameUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] GameObject optionsMenu;

    GameHandler gameHandler;
    PlayerHelper playerHelper;

    // Start is called before the first frame update
    void Start()
    {
        gameHandler = GameHandler.instance;
        playerHelper = PlayerHelper.instance;
    }

    // Update is called once per frame
    void Update()
    {
        scoreDisplayer.text = "Score: " + ((int)playerHelper.Score).ToString();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Escape();
        }
    }

    public void Escape()
    {
        switch (mode)
        {
            case Mode.playing:
                gameUI.SetActive(false);
                pauseMenu.SetActive(true);
                mode = Mode.pause;
                break;
            case Mode.pause:
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
