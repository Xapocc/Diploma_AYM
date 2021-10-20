using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public float interactionDistance;

    public TMPro.TextMeshProUGUI interactionText;
    public UnityEngine.UI.Image interactionHoldProgress;
    public GameObject interactionHoldGO;

    Camera cam;
    
    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        bool successfulHit = false;

        if (Physics.Raycast(ray, out hit, interactionDistance))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null && interactable.enabled)
            {

                HandleInteraction(interactable);
                interactionText.text = interactable.GetDescription();
                successfulHit = true;

                interactionHoldGO.SetActive(interactable.interactionType == Interactable.InteractionType.Hold);
               
            }
        }

        if (!successfulHit) interactionText.text = "";
        if (!successfulHit) interactionHoldGO.SetActive(false);
    }

    void HandleInteraction(Interactable interactable)
    {
        KeyCode key = KeyCode.F;
        switch (interactable.interactionType)
        {
            case Interactable.InteractionType.Click:
                if (Input.GetKeyDown(key))
                {
                    interactable.Interact();
                }
                break;
            case Interactable.InteractionType.Hold:
                if (Input.GetKey(key))
                {
                    interactable.IncreaseHoldTime();
                    if (interactable.GetHoldTime() > interactable.GetHoldDuration())
                    {
                        interactable.Interact();
                        interactable.ResetHoldTime();
                    }
                }
                else {
                    interactable.ResetHoldTime();
                }
                interactionHoldProgress.transform.localScale = new Vector3(interactable.GetHoldTime() / interactable.GetHoldDuration(), 1f, 1f);
                break;
            case Interactable.InteractionType.Minigame:
                    // Invoke minigame
                break;
            default:
                throw new System.Exception("Unsupportet type of interactable.");
        }
    }
}
