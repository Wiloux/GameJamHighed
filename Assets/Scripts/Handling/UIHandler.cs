using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIHandler : MonoBehaviour
{
    [SerializeField] TMP_Text scoreDisplayer;

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
    }
}
