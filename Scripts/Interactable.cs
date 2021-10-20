using UnityEngine;
using UnityEditor;



// If there will be time


//[CustomEditor(typeof(Interactable))]
//public class EditorInteractable : Editor
//{
//    //AttackType type;
//    Interactable interactable;

//    private void OnEnable()
//    {
//        interactable = (Interactable)target;
//    }

//    public override void OnInspectorGUI()
//    {
//        EditorGUILayout.BeginVertical();

//        interactable.interactionType = (Interactable.InteractionType)EditorGUILayout.EnumPopup("Interaction type", interactable.interactionType);


//        if (interactable.interactionType == Interactable.InteractionType.Hold)
//        {
//            float a = EditorGUILayout.FloatField("Hold duration", interactable.GetHoldDuration());
//            interactable.SetHoldDuration(a);
//        }

//        EditorGUILayout.EndVertical();
//    }
//}

public abstract class Interactable : MonoBehaviour
{
    public enum InteractionType
    {
        Click,
        Hold,
        Minigame
    }

    public enum SpecialRequirements
    {
        None,
        MainEntranceKeyObtained
    }

    float holdTime;
    public InteractionType interactionType;
    [SerializeField] private float holdDuration = 1f;
    [SerializeField] private SpecialRequirements specialRequirement = 0;
    public abstract string GetDescription();
    public abstract void Interact();

    public void IncreaseHoldTime() => holdTime += Time.deltaTime;
    public void ResetHoldTime() => holdTime = 0f;
    public float GetHoldTime() => holdTime;
    public float GetHoldDuration() => holdDuration;
    public void SetHoldDuration(float value) => holdDuration = value;

    public SpecialRequirements GetSpecialRequirement() => specialRequirement;
    public void SetSpecialRequirement(SpecialRequirements value) => specialRequirement = value;

}