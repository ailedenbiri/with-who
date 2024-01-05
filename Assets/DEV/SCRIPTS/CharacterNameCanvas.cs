using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterNameCanvas : MonoBehaviour
{
    private void Start()
    {
        // Calculate the lossy scale factor of the parent
        Vector3 parentLossyScale = transform.parent.lossyScale;

        // Calculate the desired local scale for the target object
        Vector3 targetLocalScale = new Vector3(
            0.031f / parentLossyScale.x,
            0.031f / parentLossyScale.y,
            0.031f / parentLossyScale.z
        );

        // Apply the calculated local scale to the target object
        transform.localScale = targetLocalScale;
    }
}
