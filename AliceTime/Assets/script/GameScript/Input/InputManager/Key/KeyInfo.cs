using UnityEngine;
using System.Collections;
using System;

namespace KamioriInput
{
	public class KeyInfo : InputSupport.InputParamBase<KeyInfo>
	{
		// key input frag
		public float Right{ get; set; } 
		public float Left{ get; set; }
		public float Up{ get; set; }
		public float Down{ get; set; }
		public int Jump{ get; set; }

        public override void UpdateParam(KeyInfo beforeParam, KeyInfo afterParam) {
            beforeParam.phase = afterParam.phase;
            beforeParam.Right = afterParam.Right;
            beforeParam.Up = afterParam.Up;
            beforeParam.Down = afterParam.Down;
            beforeParam.Jump = afterParam.Jump;
        }
    }
}
