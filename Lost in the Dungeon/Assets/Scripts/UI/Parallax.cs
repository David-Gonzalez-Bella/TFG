using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Parallax : MonoBehaviour
{
    private RawImage rawImage;
    public float parallaxSpeed;

    private void Awake()
    {
        rawImage = GetComponent<RawImage>();
        parallaxSpeed *= Time.deltaTime;
    }

    private void Update()
    {
        rawImage.uvRect = new Rect(rawImage.uvRect.x + parallaxSpeed, 0.0f, 1.0f, 1.0f);
    }
}
