using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public struct RogueInput
{
    public static Dictionary<string, KeyCode> buttons = new Dictionary<string, KeyCode>()
    {
        {"up", KeyCode.UpArrow },
        {"down", KeyCode.DownArrow },
        {"left", KeyCode.LeftArrow },
        {"right", KeyCode.RightArrow }
    };

}
