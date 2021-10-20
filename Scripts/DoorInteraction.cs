using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteraction : Interactable
{
    [SerializeField] private bool isOpened;

    [SerializeField] private int activationTime = 100;
    [SerializeField] private float angle = 110f;

    private int activationRate = 0;
    private float angleStep;

    // Start is called before the first frame update
    void Start()
    {
        if (activationTime <= 0)
        {
            activationTime = 1;
        }
        angleStep = angle / activationTime;
        if(isOpened)
            (this.GetComponentsInParent<Transform>().GetValue(1) as Transform).Rotate(new Vector3(0f, angle, 0f));
    }

    void FixedUpdate()
    {
        if (activationRate > 0)
        {
            if (isOpened)
            {
                (this.GetComponentsInParent<Transform>().GetValue(1) as Transform).Rotate(new Vector3(0f, angleStep, 0f));
            }
            else
            {
                (this.GetComponentsInParent<Transform>().GetValue(1) as Transform).Rotate(new Vector3(0f, -angleStep, 0f));
            }
            activationRate--;
        }
    }

    public override string GetDescription()
    {
        switch (GetSpecialRequirement())
        {
            case SpecialRequirements.MainEntranceKeyObtained:
                if (!GameProgression.IsMainEntranceKeyObtained())
                {
                    return "You need the <color=yellow>key</color> to open this door";
                }
                break;
        }

        if (isOpened) return "Press [F] to <color=red>close</color> the door";
        return "Press [F] to <color=green>open</color> the door";

    }

    public override void Interact()
    {
        switch (GetSpecialRequirement())
        {
            case SpecialRequirements.MainEntranceKeyObtained:
                if (!GameProgression.IsMainEntranceKeyObtained())
                    return;
                break;
        }

        isOpened = !isOpened;
        if (activationRate == 0)
            activationRate = activationTime;
        else
            activationRate = activationTime - activationRate;
    }
}
