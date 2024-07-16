using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
	//This character movement input class is an example of how to get input from a keyboard to control the character;
    public class CharacterKeyboardInput : CharacterInput
    {
		public string horizontalInputAxis = "Horizontal";
		public string verticalInputAxis = "Vertical";
		public KeyCode jumpKey = KeyCode.Space;

		//If this is enabled, Unity's internal input smoothing is bypassed;
		public bool useRawInput = true;
		
		private VariableJoystick _joystick;

		private bool isJumpPresed;
		private void Start()
		{
			if (InputController.instance != null)
			{
				_joystick = InputController.instance._Joystick;
			}
		}

		public override float GetHorizontalMovementInput()
		{
			if (_joystick != null)
			{
				return _joystick.Horizontal;
			}
			else
			{
				if (useRawInput)
					return Input.GetAxisRaw(horizontalInputAxis);
				else
					return Input.GetAxis(horizontalInputAxis);
			}
		}

		public override float GetVerticalMovementInput()
		{
			if (_joystick != null)
			{
				return _joystick.Vertical;
			}
			else
			{
				if (useRawInput)
					return Input.GetAxisRaw(verticalInputAxis);
				else
					return Input.GetAxis(verticalInputAxis);
			}
		}

		public void Jump(bool value)
		{
			isJumpPresed = value;
		}

		public override bool IsJumpKeyPressed()
		{
			if (_joystick != null)
			{
				return isJumpPresed;
			}

			return Input.GetKey(jumpKey);
		}
    }
}
