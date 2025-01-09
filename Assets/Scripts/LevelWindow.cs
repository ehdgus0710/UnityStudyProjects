using System;
using UnityEngine;
using UnityEngine.UI;

public class LevelWindow : GenericWindow
{
    [SerializeField]
    private Toggle[] levelToggles;
    private int level;


    private void OnEnable()
    {
        foreach (var toggle in levelToggles)
        {
            toggle.isOn = false;
        }

        levelToggles[level].isOn = true;
        firstSelected = levelToggles[level].gameObject;
    }

    protected override void Awake()
    {
        int index = 0;
        foreach (var toggle in levelToggles)
        {
            int currentIndex = index++;
            toggle.isOn = false;
            toggle.onValueChanged.AddListener((bool isOn) => { if (isOn) { OnSaveLevel(currentIndex); } });
        }

        OnLoadLevel();
    }

    public void OnSaveLevel(int saveLevel)
    {
        level = saveLevel;
        PlayerPrefs.SetInt("Level", saveLevel);
        PlayerPrefs.Save();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            windowManager.Open(Windows.Start);
        }
    }

    public void OnLoadLevel()
    {
        level = PlayerPrefs.GetInt("Level", -1);

        if (level == -1)
        {
            levelToggles[0].isOn = true;
            level = 0;
        }
        else
        {
            levelToggles[level].isOn = true;
        }
        firstSelected = levelToggles[level].gameObject;
    }
}
