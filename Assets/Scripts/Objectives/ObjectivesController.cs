using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectivesController : MonoBehaviour
{
    private GameController gameController;

    [SerializeField] private RectTransform objectivesList;

    [SerializeField] private TMP_Text header;
    [SerializeField] private TMP_Text objective1;
    [SerializeField] private TMP_Text objective2;
    [SerializeField] private TMP_Text objective3;

    private void Awake()
    {
        objectivesList.gameObject.SetActive(false);
    }

    private void Start()
    {
        gameController = FindObjectOfType<GameController>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Objectives"))
        {
            objectivesList.gameObject.SetActive(true);
        }
        if (Input.GetButtonUp("Objectives"))
        {
            objectivesList.gameObject.SetActive(false);
        }
    }

    public void SetObjectiveOne (string newText)
    {
        objective1.text = newText;
    }

    public void SetObjectiveTwo(string newText)
    {
        objective2.text = newText;
    }

    public void SetObjectiveThree(string newText)
    {
        objective3.text = newText;
    }
}
