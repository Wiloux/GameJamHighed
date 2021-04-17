using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHelper : MonoBehaviour
{
    public static PlayerHelper instance;
    Player player;

    private void Awake() {instance = this;}
    private void Start()
    {
        player = GetComponent<Player>();
    }

    public float Score => player.score;

    public void AddPerfectJumpScore() { player.score += player.perfectTimingJump_ScoreGain; }
    public void AddSmashingEnemyScore() { player.score += player.smashingEnemy_ScoreGain; }
    public void AddWallDestructionScore() { player.score += player.breakingWall_ScoreGain; }

    public bool IsBeatingHighscore() { return player.beatingHighscore; }
}
