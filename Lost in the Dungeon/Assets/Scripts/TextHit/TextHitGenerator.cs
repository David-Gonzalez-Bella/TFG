using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextHitGenerator : MonoBehaviour
{
    public static TextHitGenerator sharedInstance { get; private set; }
    public TextHit textHit;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    public void CreateTextHit(Color color, Transform parent, string damage)
    {
        textHit.GetComponent<TextMesh>().color = color;
        textHit.GetComponent<TextMesh>().text = damage.ToString();
        Instantiate(textHit, parent.transform.position + new Vector3(0f, 1.12f, 0f), Quaternion.identity, parent);
    }
}
