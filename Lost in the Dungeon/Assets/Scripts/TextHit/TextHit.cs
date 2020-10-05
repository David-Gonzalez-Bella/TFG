using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TextMesh))]
public class TextHit : MonoBehaviour
{
    private string sortingLayer = "Mid"; //We cant change this in the inspector, so we'll do it here
    private float maxHeight = 1.5f;
    private float verticalSpeed = 0.9f;
    private float lifeTime = 0.5f;
    private bool fading = false;
    private Vector3 movement;

    private Renderer rend;
    private TextMesh tm;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        tm = GetComponent<TextMesh>();
    }

    void Start()
    {
        rend.sortingLayerName = sortingLayer;
        rend.sortingOrder = 5;
        movement = new Vector3(0.0f, verticalSpeed, 0.0f);
    }

    private void FixedUpdate()
    {
        if (this.transform.localPosition.y < maxHeight)
        {
            this.transform.localPosition += movement * Time.deltaTime;
        }
        else
        {
            if (!fading)
            {
                fading = true;
                StartCoroutine(StartFading());
                Destroy(this.gameObject, lifeTime);
            }
        }
    }

    //Coroutines
    IEnumerator StartFading()
    {
        
        Color currentColor = tm.color;
        for (float alpha = 1; alpha > 0; alpha -= 0.08f)
        {
            currentColor.a = alpha;
            tm.color = currentColor;
            yield return new WaitForEndOfFrame();
        }
    }
}
