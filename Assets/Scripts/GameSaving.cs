﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public struct PlayerStats
{
    public float damage;
    public float hp;
    public float maxHp;
}

[System.Serializable]
public class EnemyAnalytics
{
    public enum Names { spider, snake, scorpion, zombie_1, zombie_2, zombie_3, zombie_4, knight_1, knight_2, knight_3, knight_4 };
    public GameObject prefab;
    public Names name;
    [HideInInspector] public bool show = false;
}

public class GameSaving : MonoBehaviour
{
    public static GameSaving instance;
    public event Action OnScoreChanged = () => { };
    public event Action OnEnemyDead = () => { };
    public event Action OnGameOver = () => { };
    public PlayerStats playerStats;
    public int score = 0;
    public int deadEnemies = 0;
    private Dictionary<string, int> enemiesDeadList = new Dictionary<string, int>();
    [SerializeField] private List<EnemyAnalytics> analiticsPrefabs;

    [HideInInspector]
    public string[] ENEMIES = System.Enum.GetNames(typeof(EnemyAnalytics.Names));
    void Awake()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1)
            DontDestroyOnLoad(this);

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        LoadDeadEnemies();        
    }

    public void AddScore(int value)
    {
        score += value;
        OnScoreChanged();
    }


    public void EnemyDead(string name)
    {
        deadEnemies += 1;
        OnEnemyDead();

        if (SceneManager.GetActiveScene().buildIndex == 1)
            return;

        if (enemiesDeadList.ContainsKey(name))
        {
            enemiesDeadList[name] += 1;
            return;
        }
        enemiesDeadList.Add(name, 1);
    }

    public void GameOver()
    {
        OnGameOver();
    }

    public void SaveCompleteTutorial()
    {
        PlayerPrefs.SetInt("@tutor", 1);
    }

    public void SaveDeadEnemies()
    {
        foreach (var item in enemiesDeadList)
        {
            if (item.Value > 0)
                PlayerPrefs.SetInt($"@{item.Key}", item.Value);
        }
    }

    private void LoadDeadEnemies()
    {
        foreach (string name in ENEMIES)
        {
            enemiesDeadList.Add(name, PlayerPrefs.GetInt($"@{name}", 0));
        }
    }

    public void ClearPlayerPrefs()
    {
        int tutorComplete = PlayerPrefs.GetInt("@tutor", 0);
        enemiesDeadList.Clear();
        int _score = PlayerPrefs.GetInt("@coins", 0);

        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("@coins", _score);
        PlayerPrefs.SetInt("@tutor", tutorComplete);
    }

    private void SetDeadCountToPrefabs()
    {
        foreach (var item in enemiesDeadList)
        {
            if (item.Value == 0)
                continue;

            EnemyAnalytics _tmp = analiticsPrefabs.Find(x => x.name.ToString() == item.Key);
            GameObject prefab = _tmp.prefab;
            prefab.GetComponentInChildren<Text>().text = $"x{item.Value}";
            _tmp.show = true;
        }
    }

    public List<GameObject> GetAnalyticsObjects()
    {
        SetDeadCountToPrefabs();
        return analiticsPrefabs.FindAll(x => x.show).ConvertAll(x => x.prefab);
    }
}
