using UnityEngine;

public class LightSwitch : Interactable
{
    public Light m_Light;
    public bool isOn;
    public float angle = 10;

    // Start is called before the first frame update
    void Start()
    {
        this.transform.Rotate(-angle, this.transform.localRotation.y, this.transform.localRotation.z);
        UpdateLight();
    }

    void UpdateLight()
    {
        m_Light.enabled = isOn;

            this.transform.Rotate(angle * (isOn ? -2 : 2), this.transform.localRotation.y, this.transform.localRotation.z);

    }

    // Update is called once per frame
    public override string GetDescription()
    {
        if (isOn) return "Press [F] to turn <color=red>off</color> the light";
        return "Press [F] to turn <color=green>on</color> the light";
    }

    public override void Interact()
    {
        isOn = !isOn;
        UpdateLight();
    }
}
