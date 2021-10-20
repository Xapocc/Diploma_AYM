using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CodePadButtonInteractable : Interactable
{
    [SerializeField] private TextMeshPro textMesh;
    [SerializeField] private short value = 1;
    [SerializeField] private GameObject door;

    private const string password = "695225";
    private int timerRed = 0;


    [SerializeField] private int activationTime = 100;
    [SerializeField] private float angle = 110f;

    private int activationRate = 0;
    private float angleStep;


    void Start()
    {
        if (activationTime <= 0)
        {
            activationTime = 1;
        }
        angleStep = angle / activationTime;
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
                if (activationRate == 0)
                    activationRate = activationTime;
                else
                    activationRate = activationTime - activationRate;
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
                textMesh.color = Color.green;
                textMesh.text = "------";
            }
        }

        if (activationRate > 0)
        {
            door.transform.Rotate(new Vector3(0f, angleStep, 0f));
            activationRate--;
        }


    }
}
