using UnityEngine;

public class Mirror : MonoBehaviour
{
    public Transform mirrorCam;
    public Transform playerCam;

    private void Update()
    {
        CalculateRotation();
    }

    public void CalculateRotation()
    {
        Vector3 dir = (playerCam.position - transform.position).normalized;
        Quaternion rot = Quaternion.LookRotation(dir);
        Quaternion rot90 = new Quaternion(0, 180, 0, 0);

        rot.eulerAngles = (rot90.eulerAngles - (transform.eulerAngles - rot.eulerAngles));

        mirrorCam.localRotation = rot;
    }
}