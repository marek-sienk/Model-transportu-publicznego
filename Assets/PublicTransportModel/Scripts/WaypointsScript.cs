using UnityEngine;
using System.Collections;

public class WaypointsScript : MonoBehaviour {

    public ArrayList path;
    public Color rayColor = Color.white;

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;

        Transform[] path_objs = transform.GetComponentsInChildren<Transform>();
        path = new ArrayList();

        foreach (var path_obj in path_objs)
        {
            if (path_obj != transform)
            {
                path.Add(path_obj);
            }
        }

        for (int i = 0; i < path.Count; i++)
        {
            Vector3 pos = ((Transform)path[i]).position;
            if (i > 0)
            {
                Vector3 prev = ((Transform)path[i - 1]).position;
                Gizmos.DrawLine(prev, pos);
                Gizmos.DrawWireSphere(pos, 0.3f);
            }
        }
    }
}
