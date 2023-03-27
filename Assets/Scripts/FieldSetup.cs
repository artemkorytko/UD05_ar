using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldSetup : MonoBehaviour
{
    public void ChangeRotation(float value)
    {
        transform.Rotate(transform.up * value);
    }

    public void ChangeScale(float value)
    {
        var scale = transform.localScale;
        
        scale.x += value;
        scale.y += value;
        scale.z += value;

        scale.x = Mathf.Clamp(scale.x, 0.1f, float.MaxValue);
        scale.y = Mathf.Clamp(scale.y, 0.1f, float.MaxValue);
        scale.z = Mathf.Clamp(scale.z, 0.1f, float.MaxValue);
        
        transform.localScale = scale;
    }
}