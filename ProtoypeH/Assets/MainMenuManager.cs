using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [Header("Main Menu Objects")]
    [SerializeField] private GameObject _loadingBarObject;
    [SerializeField] private Image _loadingBar;
    [SerializeField] private GameObject[] _ObjectsToHide;

    [Header("Scenes to load")]
    [SerializeField] private SceneField _persistentGameplay;
    [SerializeField] private SceneField _levelScene ;
    [SerializeField] private SceneField _levelSceneExtra ;

    private List<AsyncOperation> _scenesToLoad = new List<AsyncOperation>();
    private void Awake()
    {
        //_loadingBarObject.SetActive(false);
    }

    // Update is called once per frame
    public void StartGame()
    {
        HideMenu();
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_persistentGameplay));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelScene, LoadSceneMode.Additive));
        _scenesToLoad.Add(SceneManager.LoadSceneAsync(_levelSceneExtra, LoadSceneMode.Additive));

        StartCoroutine(ProgressLoadingBar());
    }

    private void HideMenu(){
        for(int i = 0; i < _ObjectsToHide.Length; i++)
        {
            _ObjectsToHide[i].SetActive(false);
        }

    }
    private IEnumerator ProgressLoadingBar()
    {
        float loadProgress = 0f;
        for(int i = 0; i < _scenesToLoad.Count; i++)
        {
            while(!_scenesToLoad[i].isDone)
            {
                loadProgress += _scenesToLoad[i].progress;
                _loadingBar.fillAmount = loadProgress / _scenesToLoad.Count;
                yield return null;
            }
        }
    }
}
