using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameHandler : MonoBehaviour
{
    public static GameHandler instance;
    private void Awake(){ instance = this; }

    public static bool playing;
    public bool paused;

    public GameObject destructionParticles;

    private void Start()
    {
        SetPause(!playing);
        if (playing) ProceduralGeneration.instance.StartGeneration();
    }

    private void Update()
    {
        if (playing)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
    }

    public void StartGame()
    {
        SetPause(false);
        ProceduralGeneration.instance.StartGeneration();
        playing = true;
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

#if UNITY_EDITOR
[CustomEditor(typeof(GameHandler))] public class GameHandlerInspector : Editor
{
    int newHighscore;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GUILayout.Space(10);
        if(GUILayout.Button("Reset Highscore")) { PlayerPrefs.SetInt("Highscore", 0); }
        GUILayout.Space(2);
        GUILayout.BeginHorizontal();
        if(GUILayout.Button("Set score")) { PlayerPrefs.SetInt("Highscore", newHighscore); Debug.Log("New best score: " + newHighscore); }
        newHighscore = EditorGUILayout.IntField(newHighscore);
        GUILayout.EndHorizontal();
    }
}
#endif
