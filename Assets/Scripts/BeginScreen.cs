using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginScreen : MonoBehaviour
{
    private PlayerControls controls;

    public GameObject deathScreen;
    public GameObject beginScreen;
    public GameObject UI;

    public TMPro.TextMeshProUGUI playerName;

    public TMPro.TMP_InputField name;

    private void Awake()
    {
        Time.timeScale = 0;

        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Gameplay.Enable();
    }

    private void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    public void Activate()
    {
        UI.SetActive(false);
        deathScreen.SetActive(false);
        beginScreen.SetActive(true);
    }

    public void InputName()
    {
        playerName.text = name.text.ToUpper() ?? "DIO";

        UI.SetActive(true);
        Time.timeScale = 1f;

        beginScreen.SetActive(false);
    }
}