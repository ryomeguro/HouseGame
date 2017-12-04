using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowSetting /*: MonoBehaviour*/ {

    [RuntimeInitializeOnLoadMethod]
    static void OnRuntimeMethodLoad()
    {
        Screen.SetResolution(1024, 576, false, 60);

    }
}
