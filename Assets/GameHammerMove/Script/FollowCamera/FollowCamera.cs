using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Transform Taget;

    public Vector3 offest;

    public float value;


    private void LateUpdate()
    {
        Vector3 pos = Taget.position + offest;

        transform.position = Vector3.Lerp(transform.position, pos, value * Time.deltaTime);
    }

}
