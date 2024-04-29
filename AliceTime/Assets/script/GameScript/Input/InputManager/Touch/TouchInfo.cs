using System;
using UnityEngine;

namespace KamioriInput
{
    public class TouchInfo : InputSupport.InputParamBase<TouchInfo>
    {
        public Vector3 currentScreenPosition { get; set; }
        public Vector3 deltaDistance { get; set; }
        public float deltaTime { get; set; }

        public TouchInfo()
        {

        }

        public TouchInfo(int id, Vector3 screenPosition)
        {
            this.id = id;
            this.currentScreenPosition = screenPosition;
        }

        public override void UpdateParam(TouchInfo beforeParam, TouchInfo afterParam)
        {
            beforeParam.phase = afterParam.phase;
            beforeParam.currentScreenPosition = afterParam.currentScreenPosition;
            beforeParam.deltaDistance = afterParam.deltaDistance;
            beforeParam.deltaTime = afterParam.deltaTime;
        }
    }
}