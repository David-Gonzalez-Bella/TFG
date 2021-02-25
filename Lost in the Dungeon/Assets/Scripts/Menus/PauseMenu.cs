using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public void TimeScaleStop() => Time.timeScale = 0.0f;
    public void TimeScaleResume() => Time.timeScale = 1.0f;
}
