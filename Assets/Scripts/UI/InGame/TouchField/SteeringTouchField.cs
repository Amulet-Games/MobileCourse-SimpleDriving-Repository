using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AG
{
    public class SteeringTouchField : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [Header("Steer Value")]
        [SerializeField] int onEnterSteerVal;
        [SerializeField] int onExitSteerVal;

        [Header("Ref")]
        [ReadOnlyInspector] protected Car _car;

        #region Callbacks.
        private void Start()
        {
            GetRefs();
        }
        #endregion
        
        #region Pointer Enter / Exit.
        public void OnPointerEnter(PointerEventData eventData)
        {
            _car.Steer(onEnterSteerVal);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _car.Steer(onExitSteerVal);
        }
        #endregion

        #region Setup.
        void GetRefs()
        {
            _car = Car.singleton;
        }
        #endregion
    }
}