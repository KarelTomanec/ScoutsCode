using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBox : MonoBehaviour {

    public Vector3 center;
    public Vector3 size;

    public Vector3 GetSpawnPoint()
    {
        bool empty = false;
        Vector3 pos = Vector3.zero;

        while (!empty)
        {
            pos = center + new Vector3(
                Random.Range(-size.x / 2, size.x / 2),
                Random.Range(-size.y / 2, size.y / 2),
                Random.Range(-size.z / 2, size.z / 2)
            );
            pos.y = 0;
            empty = Physics.CheckSphere(pos, 1);
        }

        return pos;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawCube(center, size);
    }


}
