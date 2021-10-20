using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LaptopCodeButton : Interactable
{
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private short value = 1;
    [SerializeField] private GameObject WinText;

    private const string password = "0451";
    private int timerRed = 0;
    private int timerQuit = -1;

    void Start()
    {

    }

    // Update is called once per frame
    public override string GetDescription()
    {
        return "Press [F] to <color=green>press</color> the button";
    }

    public override void Interact()
    {
        if (textMesh.text.StartsWith("-"))
        {
            textMesh.text = textMesh.text.Substring(1) + value.ToString();
        }
        else
            return;

        if (!textMesh.text.StartsWith("-"))
        {
            if (textMesh.text == password)
            {
                WinText.SetActive(true);
                timerQuit = 200;
            }
            else
            {
                timerRed = 50;
                textMesh.color = Color.red;
            }
        }

    }

    private void FixedUpdate()
    {
        if (timerRed >= 0)
        {
            timerRed--;
            if (timerRed == 0)
            {
                textMesh.color = Color.white;
                textMesh.text = "----";
            }
        }

        if (timerQuit == -1)
            return;

        timerQuit--;
        if (timerQuit == 0)
        {
            Cursor.lockState = CursorLockMode.None;
            SceneManager.LoadScene("MenuScene");
        }
    }
}
