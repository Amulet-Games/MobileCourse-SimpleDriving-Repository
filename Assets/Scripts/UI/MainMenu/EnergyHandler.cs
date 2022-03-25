using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace AG
{
    public class EnergyHandler : MonoBehaviour
    {
        [Header("UI_Button (Drops).")]
        public Button playBtn;

        [Header("UI_Text (Drops).")]
        public TMP_Text energyText;

        [Header("Mobile Notification")]
        public AndroidNotificationHandler androidNotif;

        [Header("Config.")]
        public int maxEnergy;
        public int energyRechargeDuration;      /// In Minute

        [Header("Status.")]
        int _curEnergy;

        [Header("Static.")]
        public const string EnergyKey = "Energy";
        public const string EnergyReadyKey = "EnergyReady";

        private void Start()
        {
            OnApplicationFocus(true);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus)
                return;

            CancelInvoke();

            _curEnergy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);
            if (_curEnergy == 0)
            {
                DisablePlayButton();

                string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, "");
                if (energyReadyString != "")
                {
                    DateTime energyReady = DateTime.Parse(energyReadyString);

                    /// If the current date/time has passed the point of energy filled date/time
                    if (DateTime.Now > energyReady)
                    {
                        FillEnergyImmediate();
                    }
                    else
                    {
                        Invoke("FillEnergySchedule", (energyReady - DateTime.Now).Seconds);
                    }
                }
            }

            SetEnergyText();
        }

        void FillEnergyImmediate()
        {
            /// Enable Play Button
            EnablePlayButton();

            /// Set current energy back to full energy
            _curEnergy = maxEnergy;

            /// Save it to PlayPref
            PlayerPrefs.SetInt(EnergyKey, _curEnergy);
        }

        void FillEnergySchedule()
        {
            FillEnergyImmediate();
        }

        void SetEnergyText()
        {
            energyText.text = _curEnergy.ToString();
        }

        void EnablePlayButton()
        {
            /// Disable Play Button
            playBtn.interactable = true;
        }

        void DisablePlayButton()
        {
            /// Disable Play Button
            playBtn.interactable = false;
        }

        public void DepleteEnergy()
        {
            /// Deplete energy
            _curEnergy--;

            /// Save it to PlayPref
            PlayerPrefs.SetInt(EnergyKey, _curEnergy);

            /// If energy is empty, set the energy ready date and save it.
            if (_curEnergy < 1)
            {
                DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeDuration);
                PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());

#if UNITY_ANDROID
                androidNotif.ScheduleNotification(energyReady);
#endif

            }
        }
    }
}