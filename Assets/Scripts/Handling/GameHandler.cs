using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    private void Awake(){ instance = this; }

    public bool paused;

    float timer = 0;

    public GameObject destructionParticles;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        if (!paused)
        {
            timer += Time.deltaTime;
        }
    }

    public void SetPause(bool pause)
    {
        paused = pause;
        Time.timeScale = paused ? 0: 1;
    }
    private void TogglePause() { SetPause(!paused); }

    public IEnumerator Fade(GameObject toFade, float fadeDuration, string mode = "in")
    {
        Renderer[] renderers = toFade.GetComponentsInChildren<Renderer>();
        Color[] colors = new Color[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            Color color = renderers[i].material.color;
            color.a = mode == "in" ? 0 : 1;

            colors[i] = color;
        }

        while (true)
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                float diff = (1f / fadeDuration) * Time.deltaTime;

                colors[i].a += mode == "in" ? diff : -diff;
                renderers[i].material.color = colors[i];
            }

            if (mode == "in" && colors[0].a >= 1) break;
            else if (mode != "in" && colors[0].a <= 0) break;

            yield return null;
        }
    }
}
