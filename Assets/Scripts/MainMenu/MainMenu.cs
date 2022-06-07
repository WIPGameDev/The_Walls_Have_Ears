using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    private GameController gameController;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    public void StartGame ()
    {
        gameController.StartNewGame();
    }

    public void ContinueGame ()
    {
        gameController.LoadCheckPoint();
    }

    public void QuitGame ()
    {
        gameController.ExitGame();
    }
}
