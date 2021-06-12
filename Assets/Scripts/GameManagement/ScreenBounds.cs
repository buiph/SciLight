using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenBounds : MonoBehaviour
{
    private Vector2 _bounds;

    void Start()
    {
        // Screen bounds
        _bounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.transform.position.z));
    }

    void LateUpdate()
    {
        Vector3 viewPos = transform.position;
        viewPos.x = Mathf.Clamp(viewPos.x, _bounds.x, _bounds.x * -1);
        viewPos.y = Mathf.Clamp(viewPos.y, _bounds.y, _bounds.y * -1);
        transform.position = viewPos;
    }
}
