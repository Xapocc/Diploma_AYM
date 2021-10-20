using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[AddComponentMenu("Camera-Control/Player1MouseLook")]

public class Player1MouseLook : MonoBehaviour
{
    private enum RotationAxes {MouseXandY = 0, MouseX = 1, MouseY = 2};
    [SerializeField] private RotationAxes axes = RotationAxes.MouseXandY;
    [SerializeField] private bool IsTurned = false;
    //Чувствительность
    [SerializeField] private float sensitivityX = 2f;
    [SerializeField] private float sensitivityY = 2f;

    [SerializeField] private float minimumX = -360f;
    [SerializeField] private float maximumX = 360f;

    [SerializeField] private float minimumY = -360f;
    [SerializeField] private float maximumY = 360f;

    private float rotationX = 0f;
    private float rotationY = 0f;

    //переменная, содержащая тип вращения
    Quaternion originalRotation;
    // Start is called before the first frame update
    void Start()
    {
        if(GetComponent<Rigidbody>())
        {
            GetComponent<Rigidbody>().freezeRotation = true;
        }
        originalRotation = transform.localRotation;
    }

    public static float ClampAngle (float angle, float min, float max)
    {
        while(angle < -360F) 
        {
            angle += 360F;
        }

        while(angle > 360F)
        {
            angle -= 360F;
        }

        // Debug.Log("Clamping angle = " + angle +
        // " min = " + min +
        // " max = " + max +
        // " to " + Mathf.Clamp(angle, min, max));
        
        return Mathf.Clamp(angle, min, max);
    }
    // Update is called once per frame
    void Update()
    {
        Cursor.lockState = CursorLockMode.Locked;

        if( axes == RotationAxes.MouseXandY)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);
            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, !IsTurned ? Vector3.up : Vector3.right);
            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, !IsTurned ? -Vector3.right : Vector3.up);
        
            transform.localRotation = originalRotation*xQuaternion*yQuaternion;
        }

        else if( axes == RotationAxes.MouseX)
        {
            rotationX += Input.GetAxis("Mouse X") * sensitivityX;

            rotationX = ClampAngle(rotationX, minimumX, maximumX);

            Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);
            
            transform.localRotation = originalRotation*xQuaternion;
        }

        else if( axes == RotationAxes.MouseY)
        {
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;

            rotationY = ClampAngle(rotationY, minimumY, maximumY);

            Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, -Vector3.right);
        
            transform.localRotation = originalRotation*yQuaternion;
        }
    }
}
