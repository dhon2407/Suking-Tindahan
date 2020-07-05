using System;
using System.Collections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace Preview
{
    public class Rotator : MonoBehaviour
    {
        [SerializeField, MinValue(0.0)] private float speed = 0.2f;
        [SerializeField, MinValue(0.01)] private float stepAngle = 0.1f;

        public void SpeedUp(float rateOfIncrease, float duration, UnityAction callback)
        {
            float originalSpeed = stepAngle;
            LeanTween.value(gameObject, originalSpeed, originalSpeed * rateOfIncrease, duration)
                .setOnUpdate(UpdateSpeed)
                .setEase(LeanTweenType.easeInOutCubic).setOnComplete(
                    () =>
                    {
                        stepAngle = originalSpeed;
                        callback.Invoke();
                    });
        }

        private void UpdateSpeed(float newSpeed)
        {
            stepAngle = newSpeed;
        }

        private Transform _transform;

        private IEnumerator StartRotation()
        {
            while (true)
            {
                _transform.RotateAround(_transform.position, _transform.up, stepAngle);
                yield return new WaitForSeconds(speed);
            }
        }

        private void Awake()
        {
            _transform = transform;
        }

        private void OnEnable()
        {
            StartCoroutine(StartRotation());
        }
    }
}