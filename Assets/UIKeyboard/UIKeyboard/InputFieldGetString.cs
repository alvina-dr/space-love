using System;
using System.Collections;
using System.Collections.Generic;

using TMPro;

using UnityEngine;

public class InputFieldGetString : MonoBehaviour
{
    private TMP_InputField inputField;
    private KeyboardController kbdController;

    private void Start() {
        inputField = GetComponent<TMP_InputField>();
        kbdController = FindObjectOfType<KeyboardController>();
    }

    private void Update() {
        var temp = kbdController.typedString;
        if (temp.Length >= 0) {
            inputField.text = temp;
        }
    }
}
