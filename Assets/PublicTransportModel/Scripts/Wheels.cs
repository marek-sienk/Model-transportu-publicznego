using UnityEngine;
using System.Collections;

public class Wheels : MonoBehaviour
{
    public WheelCollider CurrentCollider;
    //private float RotationValue = 0;

    void Start()
    {

    }

    void Update()
    {
        //transform.Rotate(CurrentCollider.rpm * 2 * Mathf.PI / 60.0f * Time.deltaTime * Mathf.Rad2Deg, 0, 0, Space.Self);
        transform.Rotate(CurrentCollider.rpm  / 60f * 360f * Time.deltaTime , 0, 0, Space.Self);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, CurrentCollider.steerAngle - transform.localEulerAngles.z, transform.localEulerAngles.z);

        /*
         // odwzorowanie zawieszenia (niezgodne z Brake Sector)
        RaycastHit hit;
        Vector3 wheelPos;
        if (Physics.Raycast(CurrentCollider.transform.position, -CurrentCollider.transform.up, out hit, CurrentCollider.radius + CurrentCollider.suspensionDistance))
            wheelPos = hit.point + CurrentCollider.transform.up * CurrentCollider.radius;
        else
            wheelPos = CurrentCollider.transform.position - CurrentCollider.transform.up * CurrentCollider.suspensionDistance;

        transform.position = wheelPos;
       */
        


        //GetComponent<MeshFilter>().mesh.RecalculateBounds();

        //transform.Rotate(0, CurrentCollider.steerAngle, 0, Space.Self);

        //RotationValue += CurrentCollider.rpm * (360 / 60) * Time.deltaTime;

        //transform.Rotate(Vector3.right, CurrentCollider.rpm * 2 * Mathf.PI / 60.0f * Time.deltaTime * Mathf.Rad2Deg, Space.Self);
        //transform.Rotate(Vector3.right, CurrentCollider.rpm , Space.Self);
    }

}
