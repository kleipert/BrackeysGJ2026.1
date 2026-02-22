using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneInit : MonoBehaviour
{
    private bool _initDone;
    private int _playerHPCurrent;
    private int _playerHPMax;

    [SerializeField] private bool _isBossRoom;
    [SerializeField] private GameObject _noCollisionTilemap;
    [SerializeField] private GameObject _healthIncreaseItem;
    [SerializeField] private int _sceneId;
    private bool _healthItemCollected;
    private bool _cutscenePlayed;

    private void Awake()
    {
        Scene scene = SceneManager.GetSceneByName("StatsScene");
        if (!scene.IsValid())
        {
            StartCoroutine(LoadStatScene());
        }
    }

    private void Start()
    {
        SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(_sceneId));
    }

    private IEnumerator LoadStatScene()
    {
        yield return SceneManager.LoadSceneAsync("StatsScene", LoadSceneMode.Additive);
    }

    private void Update()
    {
        if (_initDone) return;

        _playerHPMax = GameStats.Instance ? GameStats.Instance.PlayerHPMax : 3;
        HealthManager.Instance.SetMaxHealth(_playerHPMax);
        HealthManager.Instance.Heal(10);
        SetupLevel();
        _initDone = true;
        
    }

    private void SetupLevel()
    {
        if (_isBossRoom || !GameStats.Instance) return;

        if (GameStats.Instance.GetHealthItemPickedUp(SceneManager.GetActiveScene().buildIndex))
        {
            if(_noCollisionTilemap)
                _noCollisionTilemap.SetActive(false);
            if(_healthIncreaseItem)
                _healthIncreaseItem?.SetActive(false);
        }
        else
        {
            if(_noCollisionTilemap)
                _noCollisionTilemap?.SetActive(true);
            if(_healthIncreaseItem)
                _healthIncreaseItem.SetActive(true);
        }
        
        
        
    }
}
