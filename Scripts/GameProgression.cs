using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameProgression
{
    [SerializeField] private static bool mainEntranceKeyObtained = false;
    [SerializeField] private static bool IsInMinigame = false;

    public static bool IsMainEntranceKeyObtained() => mainEntranceKeyObtained;
    public static void CompleteMainEntranceKeyObtained() => mainEntranceKeyObtained = true;

    public static bool GetIsInMinigame() => IsInMinigame;
    public static void SetIsInMinigame(bool newState) => IsInMinigame = newState;
}
