using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class KeyboardUsageExample : MonoBehaviour
{
    public KeyboardController keyboard;
    public TMP_InputField outputText;

    private void Update() {
        outputText.text = keyboard.typedString;
    }
}
