using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class RecordPlayer : MonoBehaviour
{
    private PlayerControls controls;

    public GameObject DeathScreen;
    public GameObject UI;

    public HighScoreTable table;

    public TMPro.TextMeshProUGUI playername;
    public TMPro.TextMeshProUGUI usernameDeathscreen;
    public TMPro.TextMeshProUGUI elapsedTime;
    public Timer timer;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();

        string name = playername.text ?? "DIO";
        float time = timer.GetTime() + 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);

        elapsedTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        usernameDeathscreen.text = name.ToUpper();

        table.AddHighscoreEntry(time, name.ToUpper());
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public void ActivateDeathScreen()
    {
        UI.SetActive(false);
        DeathScreen.SetActive(true);
    }

    public void PlayAgain()
    {
        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}