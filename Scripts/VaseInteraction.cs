using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VaseInteraction : Interactable
{
    [SerializeField] private bool hasKey;
    private DateTime vaseChechedDT = DateTime.Now.AddDays(-1);
    private string description = "Press [F] to <color=yellow>search</color> for the key";



    // Start is called before the first frame update
    void Start()
    {
        //this.GetComponent<VaseInteraction>().enabled = false;
    }

    void FixedUpdate()
    {
        if (GameProgression.IsMainEntranceKeyObtained() && !this.hasKey)
            this.enabled = false;

        if (this.interactionType == InteractionType.Click && DateTime.Now.Second - vaseChechedDT.Second >= 5)
        {
            description = "Press [F] to <color=yellow>search</color> for the key";
            this.interactionType = InteractionType.Hold;
        }
    }

    public override string GetDescription()
    {
       return description;
    }

    public override void Interact()
    {
        if (hasKey)
        {
            // "player found the key" code
            if (hasKey)
            {
                GameProgression.CompleteMainEntranceKeyObtained();
                description = "You found the <color=yellow>key</color>!";
                this.interactionType = InteractionType.Click;
            }
        }
        else
        {
            // the fun part
            vaseChechedDT = DateTime.Now;
            description = "";
            this.interactionType = InteractionType.Click;
        }
    }
}
