using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CMF
{
    //This camera input class is an example of how to get input from a connected mouse using Unity's default input system;
    //It also includes an optional mouse sensitivity setting;
    public class CameraMouseInput : CameraInput
    {
        //Mouse input axes;
        public string mouseHorizontalAxis = "Mouse X";
        public string mouseVerticalAxis = "Mouse Y";

        //Invert input options;
		public bool invertHorizontalInput = false;
		public bool invertVerticalInput = false;

        //Use this value to fine-tune mouse movement;
        //All mouse input will be multiplied by this value;
        public float mouseInputMultiplier = 0.01f;

        public FixedTouchField _TouchField;

        private void Start()
        {
            if (InputController.instance != null)
            {
                _TouchField = InputController.instance._TouchField;
            }
        }

        public override float GetHorizontalCameraInput()
        {
            //Get raw mouse input;
            float _input = 0;
            if (_TouchField != null)
            {
                _input = _TouchField.TouchDist.x;
            }
            else
            {
                _input = Input.GetAxisRaw(mouseHorizontalAxis);
            }
            
            //Apply mouse sensitivity;
            //_input *= mouseInputMultiplier;

            //Invert input;
            if(invertHorizontalInput)
                _input *= -1f;

            return _input;
        }

        public override float GetVerticalCameraInput()
        {
           //Get raw mouse input;
           float _input = 0;
           if (_TouchField != null)
           {
               _input = _TouchField.TouchDist.y;
           }
           else
           {
               _input = -Input.GetAxisRaw(mouseVerticalAxis);
           }
           
            //Apply mouse sensitivity;
            //_input *= mouseInputMultiplier;

            //Invert input;
            if(invertVerticalInput)
                _input *= -1f;

            return _input;
        }
    }
}
