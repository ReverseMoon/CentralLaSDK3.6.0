
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;
using VRC.Udon;

namespace HashStudiosSimuLogo.Scripts
{

    /// <summary>
    /// Copyright (c) 2023 Hash Studios LLC. All rights reserved.
    /// This code is the sole property of Hash Studios LLC and is protected by copyright laws.
    /// It may not be used, reproduced, or distributed without the express written permission of Hash Studios LLC.
    ///
    /// Any unauthorized use of this code is strictly prohibited and may result in legal action.
    ///
    /// This copyright notice must be included in any copies or reproductions of this code.
    ///
    /// NOTE: This copyright notice does not apply to any dependencies or
    /// external libraries used in this code, such as UdonSharp, VRC SDK, or VRChat.
    /// The copyright and licensing terms of these dependencies apply to their respective code.
    /// Hash Studios LLC is not claiming any rights to these dependencies.
    /// </summary>

    public class U_ShowWorldLogo_OnSpawn_Main : UdonSharpBehaviour
    {
        [Space]
        [Space]
        [Space]
        [Header("like the world name and logo.)")]
        [Header("(Make sure to include information in your image")]
        [Space]
        [Space]
        [Space]
        [Header("--- WORLD INFORMATION ---")]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        public Sprite WorldLogo;
        public Sprite Background;
        public string Category1_Label;
        public string Category1_Text;
        public string Category2_Label;
        public string Category2_Text;
        public string Category3_Label;
        public string Category3_Text;
        [Space]
        [Space]
        [Space]
        [Header("world logo to appear after spawning in-game.)")]
        [Header("(This is the amount of time it will take for the")]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        public int timeToDisplayAfterSpawn;
        [Space]
        [Space]
        [Space]
        [Header("(This is how long the logo will stay after it appears.)")]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        [Space]
        public float timeToDisplayLogo;
        void Start()
        {

        }
    }
}