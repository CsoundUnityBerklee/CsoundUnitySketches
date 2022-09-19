using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowGameObject : MonoBehaviour
{
    public GameObject objectToFollow;
    public Vector3 positionOffset;

    // Update is called once per frame
    void LateUpdate()
    {
        transform.position = objectToFollow.transform.position + positionOffset;
    }
}
