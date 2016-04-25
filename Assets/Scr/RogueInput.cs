using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RogueInput
{
    public static Dictionary<string, KeyCode> buttons = new Dictionary<string, KeyCode>()
    {
        {"up", KeyCode.W },
        {"down", KeyCode.S },
        {"left", KeyCode.A },
        {"right", KeyCode.D }
    };

}
