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

    public TMPro.TextMeshProUGUI elapsedTime;
    public Timer timer;
    private float time;

    public TMPro.TMP_InputField name;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();

        time = timer.GetTime() + 1;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        elapsedTime.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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

    public void InputName()
    {
        string playerName = name.text ?? "DIO";

        table.AddHighscoreEntry(time, playerName.ToUpper());

        var scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}