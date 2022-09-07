using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private Transform entryContainer;
    private Transform entryTemplate;
    private List<HighscoreEntry> highscoreEntryList;
    private List<Transform> highscoreEntryTransformList;

    private void Awake()
    {
        entryContainer = transform.Find("HighscoreContainer");
        entryTemplate = entryContainer.Find("HighscoreTemplate");

        entryTemplate.gameObject.SetActive(false);

        highscoreEntryList = new List<HighscoreEntry>();
        /*
        {
            new HighscoreEntry{name = "AAA", time = 0 },
            new HighscoreEntry{name = "ABA", time = 10 },
            new HighscoreEntry{name = "ABC", time = 60 },
            new HighscoreEntry{name = "DAN", time = 105 },
            new HighscoreEntry{name = "AMI", time = 120 },
            new HighscoreEntry{name = "DAM", time = 180 },
            new HighscoreEntry{name = "MUH", time = 240 },
            new HighscoreEntry{name = "DIO", time = -9 },
        };*/

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        for (int i = 0; i < highscoreEntryList.Count; i++)
        {
            for (int j = i + 1; j < highscoreEntryList.Count; j++)
            {
                if (highscoreEntryList[i].time > highscoreEntryList[j].time)
                {
                    HighscoreEntry tmp = highscoreEntryList[i];
                    highscoreEntryList[i] = highscoreEntryList[j];
                    highscoreEntryList[j] = tmp;
                }
            }
        }
        highscoreEntryTransformList = new List<Transform>();
        foreach (HighscoreEntry highscoreEntry in highscoreEntryList)
        {
            CreateHighscoreEntryTransform(highscoreEntry, entryContainer, highscoreEntryTransformList);
        }
        /*
        Highscores highscores = new Highscores{highscoreEntryList = highscoreEntryList};
        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
        Debug.Log(PlayerPrefs.GetString("highscoreTable"));
        */
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transformlist)
    {
        float templateHeight = 20f;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransform = entryTransform.GetComponent<RectTransform>();

        entryRectTransform.anchoredPosition = new Vector3(0, -templateHeight * transformlist.Count);
        entryTransform.gameObject.SetActive(true);

        int rank = 1+transformlist.Count;
        string rankString;
        switch (rank)
        {
            default: rankString = rank + "TH"; break;

            case 1: rankString = "1ST"; break;
            case 2: rankString = "2ND"; break;
            case 3: rankString = "3RD"; break;
        }

        entryTransform.Find("Position").GetComponent<TMPro.TextMeshProUGUI>().text = rankString;

        string name = highscoreEntry.name;
        entryTransform.Find("Name").GetComponent<TMPro.TextMeshProUGUI>().text = name;

        float time = highscoreEntry.time;
        float minutes = Mathf.FloorToInt(time / 60);
        float seconds = Mathf.FloorToInt(time % 60);
        entryTransform.Find("Time").GetComponent<TMPro.TextMeshProUGUI>().text =
            string.Format("{0:00}:{1:00}", minutes, seconds);

        transformlist.Add(entryTransform);
    }

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntryList;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public string name;
        public float time;
    }
}