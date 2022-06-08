using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DadsNoteUI : MonoBehaviour
{
    private GameController gameController;
    [SerializeField] private string schoolLevelName;
    [SerializeField] private string schoolLevelMarker;

    public void PauseForNote ()
    {
        gameController = FindObjectOfType<GameController>();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.Confined;
        gameController.GameState = GameState.MENU;
    }

    public void Continue ()
    {
        Time.timeScale = 1f;
        gameController.LoadLevel(schoolLevelName, schoolLevelMarker);
    }
}
