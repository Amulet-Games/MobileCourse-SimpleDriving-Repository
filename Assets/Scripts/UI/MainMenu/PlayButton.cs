using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace AG
{
    public class PlayButton : MonoBehaviour
    {
        public EnergyHandler energyHandler;

        public void UIButton_Play()
        {
            energyHandler.DepleteEnergy();

            SceneManager.LoadScene(1);
        }
    }
}