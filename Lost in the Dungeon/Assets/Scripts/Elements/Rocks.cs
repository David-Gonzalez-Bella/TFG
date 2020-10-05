using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Rocks : MonoBehaviour
{
    public static Rocks sharedInstance { get; private set; }
    public List<Transform> rocks;
    public bool pathCleared = false;

    private void Awake()
    {
        if (sharedInstance == null)
            sharedInstance = this;
    }

    public void ClearPath()
    {
        if (!pathCleared)
        {
            pathCleared = true;
            foreach (Transform rock in rocks)
            {
                if (rock.tag.CompareTo("Rock") != 0)
                {
                    rock.GetComponent<SpriteRenderer>().enabled = false;
                    if (rock.GetComponent<CapsuleCollider2D>() != null) { rock.GetComponent<CapsuleCollider2D>().enabled = false; }
                }
            }
        }
    }

    public void BlockPath()
    {
        foreach (Transform rock in rocks)
        {
            if (rock.tag.CompareTo("Rock") != 0)
            {
                rock.GetComponent<SpriteRenderer>().enabled = true;
                if (rock.GetComponent<CapsuleCollider2D>() != null) { rock.GetComponent<CapsuleCollider2D>().enabled = true; }
            }
        }
    }

    [ContextMenu("Autofill Rocks")]
    public void AutofillRocks()
    {
        rocks = FindObjectsOfType<Transform>().Where(t => t.name.Contains("Rock")).ToList();
    }
}
