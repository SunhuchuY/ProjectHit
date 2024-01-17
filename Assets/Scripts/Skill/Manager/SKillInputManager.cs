using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SKillInputManager : MonoBehaviour
{
    public bool[] isInput = new bool[4];

    private void Update()
    {
        if (!isInput[0])
        {
            isInput[0] = Input.GetKeyDown(KeyCode.Q);
        }
    }
}