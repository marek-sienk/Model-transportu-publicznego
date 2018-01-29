using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool brake;
    public bool steering;
}
public class SteerScript : MonoBehaviour {

    public List<AxleInfo> axleInfos;
    public Vector3 centerOfMass;
    public ArrayList path;
    public Transform pathGroup;
    public GameObject leftDoor;
    public GameObject rightDoor;
    public GameObject Seats;
    public Camera vehicleCamera;
    public float maxSteer = 15;
    public int currentPathObj;
    public float distFromPoint;
    public float maxTourqe = 15;
    public float currentSpeed;
    public float topSpeed = 150;
    public float deccelaration = 10;
    public float brakes;
    public float maxDoorOpen;
    public float minSafeDistance = 10;
    public float midSensorStartPoint = 5;
    public float sideSensorDistance = 2;
    public float frontSensorAngle = 30;
    public bool inSector = false;

    private bool doorClosed = true;
    private Vector3 doorPosition;
    private bool[] isOccupied;
    private bool brake = false;
    private bool brakeWhenCollision = false;
    private int flag = 0;

	void Start () {
        GetComponent<Rigidbody>().centerOfMass = centerOfMass;
        GetPath();
        doorPosition = leftDoor.transform.localPosition;
        isOccupied = new bool[Seats.transform.childCount];
        vehicleCamera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
        GetSteer();
        Move();
        AvoidCollision();
	}

    public void GetPath(){
        Transform[] path_objs = pathGroup.GetComponentsInChildren<Transform>();
        path = new ArrayList();

        foreach (var path_obj in path_objs)
        {
            if (path_obj != pathGroup)
            {
                path.Add(path_obj);
            }
        }
    }

    public void GetSteer()
    {
        foreach (var axleInfo in axleInfos)
        {
            Vector3 steerVector = transform.InverseTransformPoint(new Vector3(((Transform)path[currentPathObj]).position.x, transform.position.y, ((Transform)path[currentPathObj]).position.z));
            //print(maxSteer * (steerVector.x / steerVector.magnitude));
            //print("<color=red>"+axleInfo.rightWheel.steerAngle+"</color>");

            //if (steerVector.x / steerVector.magnitude < -0.01 || steerVector.x / steerVector.magnitude > 0.01)
            //{
            //    if(axleInfo.steering)
            //        if ((steerVector.x / steerVector.magnitude < 0 && axleInfo.rightWheel.steerAngle > maxSteer * (steerVector.x / steerVector.magnitude))
            //            || (steerVector.x / steerVector.magnitude > 0 && axleInfo.rightWheel.steerAngle < maxSteer * (steerVector.x / steerVector.magnitude)))
            //        {
            //            axleInfo.leftWheel.steerAngle += steerVector.x / steerVector.magnitude;
            //            axleInfo.rightWheel.steerAngle += steerVector.x / steerVector.magnitude;
            //        }
            //}
            //else
            //{
                float newSteer = maxSteer * (steerVector.x / steerVector.magnitude);
                if (axleInfo.steering)
                {
                    axleInfo.leftWheel.steerAngle = newSteer;
                    axleInfo.rightWheel.steerAngle = newSteer;
                }
            //}

            if (steerVector.magnitude <= distFromPoint)
            {
                if (!((Transform)path[currentPathObj]).CompareTag("Stop"))
                {
                    currentPathObj++;
                }
                else
                {
                    if (currentSpeed > 1)
                        brake = true;
                    else
                        StartCoroutine(Wait());
                }
                if (currentPathObj >= path.Count)
                {
                    currentPathObj = 0;
                } 
            }
        }
    }

    public void Move()
    {
        foreach (var axleInfo in axleInfos)
        {
            currentSpeed = 2 * Mathf.PI * axleInfo.rightWheel.radius * axleInfo.rightWheel.rpm * 60 / 1000;
            currentSpeed = Mathf.Round(currentSpeed);
            if (brake || brakeWhenCollision)
            {
                Brake(brakes);
            }
            else
            {
                if (currentSpeed <= topSpeed && !inSector)
                {
                    if (axleInfo.motor)
                    {
                        axleInfo.leftWheel.motorTorque = maxTourqe;
                        axleInfo.rightWheel.motorTorque = maxTourqe;
                    }
                    if (axleInfo.brake)
                    {
                        axleInfo.leftWheel.brakeTorque = 0;
                        axleInfo.rightWheel.brakeTorque = 0;
                    }

                }
                else if(!inSector)
                {
                    Brake(deccelaration);
                }
            }
        }
        
    }

    public void Brake(float brakePower)
    {
        foreach (var axleInfo in axleInfos)
        {
            if (axleInfo.brake)
            {
                axleInfo.leftWheel.brakeTorque = brakePower;
                axleInfo.rightWheel.brakeTorque = brakePower;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = 0;
                axleInfo.rightWheel.motorTorque = 0;
            }
        }
    }

    void AvoidCollision()
    {
        Vector3 pos;
        RaycastHit hit;
        Vector3 rightAngle = Quaternion.AngleAxis(frontSensorAngle, transform.up) * transform.forward;
        Vector3 leftAngle = Quaternion.AngleAxis(-frontSensorAngle, transform.up) * transform.forward;
        flag = 0;

        if (transform.GetComponent<Rigidbody>().velocity.sqrMagnitude / 3 < 8)
            minSafeDistance = 8;
        else
            minSafeDistance = transform.GetComponent<Rigidbody>().velocity.sqrMagnitude / 3;

        //mid sensor
        pos = transform.position;
        pos += transform.forward * midSensorStartPoint;
        if (Physics.Raycast(pos, transform.forward, out hit, minSafeDistance) && hit.transform.root == transform.root)
        {
            if (!hit.transform.Equals("Terrain") && hit.transform.root.transform != pathGroup.transform)
                flag++;
            Debug.DrawLine(pos, hit.point,Color.white);
        }

        //right forward sensor
        pos += transform.right * sideSensorDistance;
        if (Physics.Raycast(pos, transform.forward, out hit, minSafeDistance) && hit.transform.root == transform.root)
        {
            if (!hit.transform.Equals("Terrain") && hit.transform.root.transform != pathGroup.transform)
                flag++;
            Debug.DrawLine(pos, hit.point, Color.white);
        }

        //right angle sensor
        if (Physics.Raycast(pos, rightAngle, out hit, minSafeDistance) && hit.transform.root == transform.root)
        {
            if (!hit.transform.Equals("Terrain") && hit.transform.root.transform != pathGroup.transform)
                flag++;
            Debug.DrawLine(pos, hit.point, Color.white);
        }

        //left forward sensor
        pos -= transform.right * sideSensorDistance * 2;
        if (Physics.Raycast(pos, transform.forward, out hit, minSafeDistance) && hit.transform.root == transform.root)
        {
            if (!hit.transform.Equals("Terrain") && hit.transform.root.transform != pathGroup.transform)
                flag++;
            Debug.DrawLine(pos, hit.point, Color.white);
        }

        //left angle sensor
        if (Physics.Raycast(pos, leftAngle, out hit, minSafeDistance) && hit.transform.root == transform.root)
        {
            if (!hit.transform.Equals("Terrain") && hit.transform.root.transform != pathGroup.transform)
                flag++;
            Debug.DrawLine(pos, hit.point, Color.white);
        }

        if (flag != 0)
            brakeWhenCollision = true;
        else
            brakeWhenCollision = false;
    }


    void OpenDoor()
    {
        if (doorClosed && transform.GetComponent<Rigidbody>().velocity.sqrMagnitude < 1)
        {
            if (leftDoor.transform.localPosition.z > doorPosition.z - maxDoorOpen)
            {
                leftDoor.transform.Translate(new Vector3(0, 0, -0.1f * Time.deltaTime));
                rightDoor.transform.Translate(new Vector3(0, 0, 0.1f * Time.deltaTime));
                if (leftDoor.transform.localPosition.x < doorPosition.x + 0.3f)
                {
                    leftDoor.transform.Translate(new Vector3(0.1f * Time.deltaTime, 0, 0));
                    rightDoor.transform.Translate(new Vector3(0.1f * Time.deltaTime, 0, 0));
                }
            }
            
            if (leftDoor.transform.localPosition.z <= doorPosition.z - maxDoorOpen)
                doorClosed = false;
        }
    }

    public void CloseDoor()
    {
        if (!doorClosed)
        {
            if (leftDoor.transform.localPosition.z < doorPosition.z)
            {
                leftDoor.transform.Translate(new Vector3(0, 0, 0.1f * Time.deltaTime));
                rightDoor.transform.Translate(new Vector3(0, 0, -0.1f * Time.deltaTime));
            }
            if (leftDoor.transform.localPosition.z >= doorPosition.z)
            {
                if (leftDoor.transform.localPosition.x > doorPosition.x)
                {
                    leftDoor.transform.Translate(new Vector3(-0.1f * Time.deltaTime, 0, 0));
                    rightDoor.transform.Translate(new Vector3(-0.1f * Time.deltaTime, 0, 0));
                }
                else
                {
                    doorClosed = true;
                    currentPathObj++;
                    brake = false;
                }
            }
        }
    }

    IEnumerator Wait()
    {
        if (doorClosed)
            OpenDoor();
        else
        {
            yield return new WaitForSeconds(5);
            CloseDoor();
        }
    }

    public bool IsClosed()
    {
        return doorClosed;
    }

    public GameObject ChooseSeat(int i)
    {
        if (i < Seats.transform.childCount)
            return Seats.transform.GetChild(i).gameObject;
        else
            return null;
    }

    public bool getSeatStatus(int i)
    {
        return isOccupied[i];
    }

    public void setSeatStatus(int i, bool s)
    {
        isOccupied[i] = s;
    }

    public GameObject getDoor()
    {
        return leftDoor;
    }

    public Camera getCamera()
    {
        return vehicleCamera;
    }
}
