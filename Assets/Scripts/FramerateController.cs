using UnityEngine;

public class FramerateController : MonoBehaviour
{
    public int targetFrameRate = 60;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}