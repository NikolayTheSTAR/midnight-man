using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorCursor : MonoBehaviour
{
    public void SetTransformData(CursorTransformData data)
    {
        transform.rotation = Quaternion.Euler(data.Rotation);
        transform.localScale = data.Scale;
    }

    public void SetFlip(bool toRight)
    {
        transform.localScale = new Vector3(toRight ? -1 : 1, 1, 1);
    }
}