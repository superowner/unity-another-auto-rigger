﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AnotherAutoRigger;

namespace AnotherAutoRigger
{
    [System.Serializable]
    public class RuntimeHelperTranslate : MonoBehaviour
    {
        [Header("[Pose Reader]")]
        [Space(5)]
        public YawPitchRoll poseReader;

        [Header("[User Attributes]")]
        [Space(5)]
        public float multiplier = 1;

        [Header("[Offset Attributes]")]
        [Space(5)]
        public float offsetX;
        public float offsetY;
        public float offsetZ;

        [Header("[Multiplier Attributes]")]
        [Space(5)]
        public int _directionMultiplier = 1;
        public int _blockMultiplier = 1;

        [Header("[Curve Attributes]")]
        [Space(5)]
        public float _negValue = -1;
        public float _posValue = 1;

        [Header("[Default Attributes]")]
        [Space(5)]
        public int _defaultAxis;
        public float _defaultPositionX;
        public float _defaultPositionY;
        public float _defaultPositionZ;

        private Vector3 defaultPosition;
        private Vector3 startPosition;
        private AnimationCurve defaultCurve;
        private float defaultLength;

        void Start()
        {
            // construct default values
            defaultPosition = new Vector3(_defaultPositionX, _defaultPositionY, _defaultPositionZ);
            defaultLength = defaultPosition.magnitude;
            defaultCurve = new AnimationCurve(
                new Keyframe(180, 0),
                new Keyframe(170, _negValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(120, _negValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(-120, _posValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(-170, _posValue * _directionMultiplier * _blockMultiplier),
                new Keyframe(-180, 0)
            );

            // construct start position
            Vector3 offsetPosition = new Vector3(offsetX, offsetY, offsetZ) * _blockMultiplier;
            startPosition = defaultPosition + offsetPosition;
        }

        void Update()
        {
            // validate pose reader
            if (poseReader == null)
                return;

            // extract twist value on prefered axis
            Vector3 yawPitchRoll = poseReader.GetYawPitchRoll();

            float twistValue = yawPitchRoll[_defaultAxis];
            float twistOffset = defaultCurve.Evaluate(twistValue) * defaultLength * multiplier;

            // set local position
            transform.localPosition = startPosition + new Vector3(0, twistOffset, 0);
        }
    }
}