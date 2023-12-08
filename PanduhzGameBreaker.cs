using BepInEx.Logging;
using BepInEx;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DunGen;
using JetBrains.Annotations;
using PanduhzGameBreaker;
using PanduhzGameBreaker.Component;
using BepInEx.Configuration;
using GameNetcodeStuff;




//All of my mods combined into one with a gui system
namespace PanduhzGameBreaker
{
    [BepInPlugin(modGUID, modName, modVersion)]
    public class PanduhzGameBreakerBaseMod : BaseUnityPlugin
    {

        private const string modGUID = "PanduhzGameBreaker";
        private const string modName = "Gamebreaker";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        internal ManualLogSource mls;


        //bools
        private static ConfigEntry<bool> EnableinfiniteSprintHack;
        private static ConfigEntry<bool> EnablenoFallDamageHack;
        private static ConfigEntry<bool> EnablereachHack;
        private static ConfigEntry<bool> EnabledrownHack;
        private static ConfigEntry<bool> EnableinfiniteCredits;
        //floats
        private static ConfigEntry<float> jumpHack;
        private static ConfigEntry<float> speedHack;
        private static ConfigEntry<float> climbSpeedHack;
        //ints
        private static ConfigEntry<int> shovelHack;


        private static PanduhzGameBreakerBaseMod Instance;

        private static GUILoader myGUI;
        private static bool hasGUISynced = false;

        public void Awake()
        {
            Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);
            mls.LogInfo("Panduhz GameBreaker Successful");

            harmony.PatchAll(typeof(PanduhzGameBreakerBaseMod));
            var gameObject = new UnityEngine.GameObject("GUILoader");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            gameObject.AddComponent<GUILoader>();
            myGUI = (GUILoader)gameObject.GetComponent("GUILoader");
            SetBindings();
            setGUIVars();
        }


        void setGUIVars()
        {
            //bools
            myGUI.guiEnableinfiniteSprintHack = EnableinfiniteSprintHack.Value;
            myGUI.guiEnabledrownHack = EnabledrownHack.Value;
            myGUI.guiEnablenoFallDamageHack = EnablenoFallDamageHack.Value;
            myGUI.guiEnablereachHack = EnablereachHack.Value;
            myGUI.guiEnableinfiniteCredits = EnableinfiniteCredits.Value;
            //floats
            myGUI.guijumpHack = jumpHack.Value;
            myGUI.guispeedHack = speedHack.Value;
            myGUI.guiclimbSpeedHack = climbSpeedHack.Value;
            myGUI.guishovelHack = shovelHack.Value;
            hasGUISynced = true;
        }


        void UpdateCFGVarsFromGUI()
        {
            if (!hasGUISynced) { setGUIVars(); }
            //bools
            EnableinfiniteSprintHack.Value = myGUI.guiEnableinfiniteSprintHack;
            EnabledrownHack.Value = myGUI.guiEnabledrownHack;
            EnablenoFallDamageHack.Value = myGUI.guiEnablenoFallDamageHack;
            EnablereachHack.Value = myGUI.guiEnablereachHack;
            EnableinfiniteCredits.Value = myGUI.guiEnableinfiniteCredits;
            //floats
            jumpHack.Value = myGUI.guijumpHack;
            speedHack.Value = myGUI.guispeedHack;
            climbSpeedHack.Value = myGUI.guiclimbSpeedHack;
            //ints
            shovelHack.Value = myGUI.guishovelHack;
        }





        private void SetBindings()
        {
            //bools
            EnableinfiniteSprintHack = Config.Bind("Hacks", "Player - enable infinite sprint hack", true, "");
            EnabledrownHack = Config.Bind("Hacks", "Player - enable drown hack", true, "");
            EnablenoFallDamageHack = Config.Bind("Hacks", "Player - enable no fall damage Hack", true, "");
            EnablereachHack = Config.Bind("Hacks", "Player - enable reach hack", true, "");
            EnableinfiniteCredits = Config.Bind("Hacks", "Player - enable infinite credits", true, "");
            //floats
            jumpHack = Config.Bind("Hacks", "Player - Jump Force", 1f, new ConfigDescription("Base jump force for player", new AcceptableValueRange<float>(1f, 200f)));//20 is a pretty good jump force
            speedHack = Config.Bind("Hacks", "Player - Speed", 1f, new ConfigDescription("Base speed for player", new AcceptableValueRange<float>(0.01f, 1f)));//0.3 is pretty good speed not too buggy
            climbSpeedHack = Config.Bind("Hacks", "Player - Climb Speed", 4f, new ConfigDescription("Base climb speed for player", new AcceptableValueRange<float>(4f, 2000f)));
            //ints
            shovelHack = Config.Bind("Hacks", "Player - Shovel Hit Force", 1, new ConfigDescription("Base Shovel Hit Force", new AcceptableValueRange<int>(1, 100)));
        }

        [HarmonyPatch(typeof(Shovel), "ItemActivate")]
        [HarmonyPostfix]
        private static void ShovelPatch(ref int ___shovelHitForce)
        {
            ___shovelHitForce = shovelHack.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void ReachPatch(ref float ___grabDistance)
        {
            if (EnablereachHack.Value)
            {
                ___grabDistance = 99999f;
            }
        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void NoDrowning(ref float ___drowningTimer)
        {
            if (EnabledrownHack.Value)
            {
                ___drowningTimer = 1f;
            }
        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void IncreaseJumpForce(ref float ___jumpForce)
        {
            ___jumpForce = jumpHack.Value;

        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void InfiniteSprint(ref float ___sprintMeter)
        {
            if (EnableinfiniteSprintHack.Value)
            {
                ___sprintMeter = 1f;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void IncreaseSpeed(ref float ___carryWeight, bool ___isHoldingObject)
        {
            ___carryWeight = speedHack.Value;
        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void IncreaseClimbSpeed(ref float ___climbSpeed, ref bool ___isClimbingLadder)
        {
            ___climbSpeed = climbSpeedHack.Value;
        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        private static void NoFallDamage(ref bool ___isFallingFromJump, ref bool ___isFallingNoJump, ref int ___health)
        {
            if (EnablenoFallDamageHack.Value)
            {
                int healthBeforeFall = 120;
                if (___isFallingFromJump || ___isFallingNoJump)
                {
                    ___health = healthBeforeFall;
                }
                if (!___isFallingFromJump || !___isFallingNoJump)
                {
                    ___health = healthBeforeFall;
                }
            }
        }


        [HarmonyPatch(typeof(Terminal), "Start")]
        [HarmonyPostfix]
        private static void AddCredits(ref int ___groupCredits)
        {
            if (EnableinfiniteCredits.Value)
            {
                ___groupCredits = 1000000;
            }
        }


    }
}
