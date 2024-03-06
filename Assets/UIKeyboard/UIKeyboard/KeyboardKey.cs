using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class KeyboardKey : MonoBehaviour
{
	//Place this on the buttons that act as the keys for the keyboard
	private char keyCharacter;
	private KeyboardController kbdController;

	private void Start() {
		keyCharacter = gameObject.name.ToCharArray()[0];
		kbdController = GetComponentInParent<KeyboardController>(); 
		GetComponent<Button>().onClick.AddListener(KeyPressed);
	}

	public void KeyPressed() {
		if (gameObject.name == "Backspace") {
			kbdController.Backspace();
		} else {
			kbdController.GetKey(keyCharacter);
		}
		
	}
}
