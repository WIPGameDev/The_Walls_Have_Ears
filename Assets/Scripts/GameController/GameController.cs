using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public enum GameState
{
    MENU,
    PLAYING,
    EXITING,
    LOADING,
    PAUSED
}

public class GameController : MonoBehaviour
{
    public static bool isGamePaused = false;

    [SerializeField] private GameState gameState = GameState.PLAYING;

    [SerializeField] private string currentScene;
    [SerializeField] private string nextScene;

    [SerializeField] private GameObject player;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private GameObject pauseMenu;

    public UnityEvent OnGameStart;
    public UnityEvent OnGameLoading;

    public UnityEvent OnSceneLoading;
    public UnityEvent OnSceneLoaded;

    public UnityEvent OnMainMenu;
    public UnityEvent OnCheckPointLoad;

    public UnityEvent OnGamePause;
    public UnityEvent OnGamePauseResume;

    public UnityEvent OnGameOver;

    public string CurrentScene { get => currentScene; }
    public string NextScene { get => nextScene; }
    public GameObject LoadingScreen { get => loadingScreen; }
    public GameObject PauseMenu { get => pauseMenu; }

    void Awake()
    { 

    }

    void Start()
    {
        UpdateScenes();
        if (Application.isEditor)
        {

        }
        else
        {
            LoadMainMenu();
        }
    }

    void Update()
    {
        switch (gameState)
        {
            case GameState.MENU:
                {
                    //Menu Stuff
                    break;
                }
            case GameState.PLAYING:
                {
                    if (Input.GetButtonDown("Cancel"))
                    {
                        PauseGame();
                    }
                    break;
                }
            case GameState.PAUSED:
                {
                    if (Input.GetButtonDown("Cancel"))
                    {
                        ResumeGame();
                    }
                    break;
                }
            case GameState.LOADING:
                {
                    //Loading stuff, maybe animate loading screen or something.
                    break;
                }
            case GameState.EXITING:
                {
                    //Exiting stuff, maybe wait for a checkpoint or some data to be saved.
                    break;
                }
        }
    }

    public void LoadMainMenu()
    {
        //nextScene = "MainMenu";
        //StartCoroutine(LoadingMainMenu(currentScene, nextScene));
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void StartNewGame()
    {
        //nextScene = "EntryArea";
        //StartCoroutine(LoadingLevel("MainMenu", "EntryArea", "StartMarker"));
    }

    public void LoadLevel(string levelName, string markerName)
    {
        nextScene = levelName;
        StartCoroutine(LoadingLevel(currentScene, nextScene, markerName));
    }

    public void SaveCheckPoint()
    {
        Debug.Log("Save Checkpoint");
    }

    public void LoadCheckPoint()
    {
        Debug.Log("Load Checkpoint");
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        pauseMenu.SetActive(true);
        gameState = GameState.PAUSED;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        gameState = GameState.PLAYING;
    }

    public void ExitGame()
    {
        Application.Quit(0);
    }

    public void GameOver()
    {

    }

    public void UpdateScenes()
    {
        if (SceneManager.sceneCount > 1)
        {
            currentScene = SceneManager.GetSceneAt(1).name;
        }
    }

    private IEnumerator LoadingMainMenu(string currentScene, string targetScene)
    {
        gameState = GameState.LOADING;
        Coroutine loadingScene = StartCoroutine(LoadingScene(currentScene, targetScene));
        yield return loadingScene;
        gameState = GameState.MENU;
        UpdateScenes();
    }

    private IEnumerator LoadingLevel(string currentScene, string targetScene, string targetMarker)
    {
        gameState = GameState.LOADING;
        Coroutine loadingScene = StartCoroutine(LoadingScene(currentScene, targetScene));
        yield return loadingScene;
        if (player != null)
        {
            GameObject marker = GameObject.Find(targetMarker);
            if (marker != null)
            {
                player.transform.SetPositionAndRotation(marker.transform.position, marker.transform.rotation);
            }
            player.SetActive(true);
        }
        gameState = GameState.PLAYING;
        UpdateScenes();
    }

    private IEnumerator LoadingScene(string currentScene, string targetScene)
    {
        LoadingScreen.SetActive(true);
        if (player != null)
            player.SetActive(false);
        Scene sceneToUnload = SceneManager.GetSceneByName(currentScene);

        if (sceneToUnload.IsValid() && sceneToUnload.isLoaded)
        {
            AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(sceneToUnload);
            while (!asyncUnload.isDone)
            {
                yield return null;
            }
        } else
        {
            Debug.Log("No current scene assign.");
        }

        int sceneToLoadIndex = SceneUtility.GetBuildIndexByScenePath("Assets/Scenes/" + targetScene + ".unity");
        if (sceneToLoadIndex != -1)
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneToLoadIndex, LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        } else
        {
            Debug.Log("Scene name does not have a matching scene at Assets/Scenes/" + targetScene + ".unity");
        }
        LoadingScreen.SetActive(false);
    }
}