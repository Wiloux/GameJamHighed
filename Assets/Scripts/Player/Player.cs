using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private new Collider2D collider;
    PlayerMovement controller;
    public bool isDead = false;

    public bool isInBuilding;
    [Header("Score")]
    [Space(10)]
    public float score;
    public bool beatingHighscore;
    [Space(10)]
    public int breakingWall_ScoreGain = 5;
    public int smashingEnemy_ScoreGain = 10;
    public int perfectTimingJump_ScoreGain = 7;
    public float scoreGainBySecond = 1;


    public List<AudioClip> PunchEffects = new List<AudioClip>();
    public List<AudioClip> DeathEffects = new List<AudioClip>();
    public List<AudioClip> WalkGEffects = new List<AudioClip>();
    public List<AudioClip> WalkWEffects = new List<AudioClip>();


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        collider = GetComponent<Collider2D>();
        controller = GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            if (Input.GetKeyDown(KeyCode.X) && !controller.isTackling)
            {
                SoundManager.Instance.PlaySoundEffect(PunchEffects[Random.Range(0, PunchEffects.Count)]);
                controller.isTackling = true;
                animator.SetTrigger("Attack");
            }

            if (transform.position.y < -1 || rb.velocity == Vector2.zero)
            {
                Die();
            }

            score += Time.deltaTime * scoreGainBySecond;

        }
        else
        {
            if (!beatingHighscore && score > PlayerPrefs.GetInt("Highscore", 0))
            {
                beatingHighscore = true; 
            }
                if (Input.anyKeyDown)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void WalkSFX()
    {
        if (isInBuilding)
        {
            SoundManager.Instance.PlaySoundEffect(WalkWEffects[Random.Range(0, WalkWEffects.Count)]);
        }
        else
        {
            SoundManager.Instance.PlaySoundEffect(WalkGEffects[Random.Range(0, WalkGEffects.Count)]);
        }
  
    }
    public void StopAttack()
    {
        controller.isTackling = false;
    }

    public void Die()
    {
        // temp


        if (!isDead)
        {
            SoundManager.Instance.PlaySoundEffect(DeathEffects[Random.Range(0, DeathEffects.Count)]);
            SoundManager.Instance.PlayUISoundEffect(SoundManager.Instance.GameOver);
            SoundManager.Instance.PauseUnPauseMusic(true);
        }
        controller.speed = 0;
        GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        rb.AddForce(new Vector2(-10, 10), ForceMode2D.Impulse);
        isDead = true;
        controller.anim.SetTrigger("Dead");

        int currentHighscore = PlayerPrefs.GetInt("Highscore", 0);
        if (score > currentHighscore) { PlayerPrefs.SetInt("OldHighscore", currentHighscore); PlayerPrefs.SetInt("Highscore", (int)score);  }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<FloorTrigger>() != null) { isInBuilding = !isInBuilding; /*Debug.Log("in building = " + isInBuilding);*/ }
    }
}
