using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVInteractable : Interactable
{
    private const string defaultDesc = "Press [F] to <color=yellow>play</color> the video game";
    private string description = defaultDesc;

    public delegate void TVInteracted();
    public static event TVInteracted TVInteractedEvent;

    private void Start()
    {
        TVMinigame.GameQuittedEvent += QuitGame;
        TVMinigame.GameWonEvent += GameWon;
    }

    public override string GetDescription()
    {
        return description;
    }

    public override void Interact()
    {
        description = string.Empty;
        TVInteractedEvent?.Invoke();
    }

    private void QuitGame()
    {
        description = defaultDesc;
    }

    private void GameWon()
    {
        this.enabled = false;
    }

}
