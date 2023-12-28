using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace FlushTemplate
{
    [CreateAssetMenu(fileName = "TemplateSettings", order = 0)]
    public class TemplateSettings : ScriptableObject
    {
        public enum PanelAnimation
        {
            Scale,
            Slide
        }
        public float defaultBoinkDuration = 0.2f;
        [Header("Upgrade Panel Animation")]
        public float defaultFadeAlpha = 0.9f;
        public PanelAnimation panelAnimation = PanelAnimation.Slide;
        [Header("Canvas Coin")]
        public float canvasCoinInitialMoveTime = 0.3f;
        public float canvasCoinMoveTime = 1.5f;
        public int canvasCoinDelay = 15;
        public float canvasCoinRange = 10f;
        public AnimationCurve canvasCoinCurve;
    }
}
