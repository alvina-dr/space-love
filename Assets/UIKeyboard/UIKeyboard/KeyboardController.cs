using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

public class KeyboardController : MonoBehaviour
{
    public string typedString = "";
    
    public void GetKey(char key) {
        Debug.Log(key);
        typedString += key;
    }

    public void Backspace() {
        if (typedString.Length > 0) {
            typedString = typedString.Remove(typedString.Length - 1, 1);
        }
    }
    
}
