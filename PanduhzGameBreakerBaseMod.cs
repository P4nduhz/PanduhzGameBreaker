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
using System.Diagnostics.Eventing.Reader;
using System.Security.Cryptography;

//All of my mods combined into one with a gui system
namespace PanduhzGameBreaker
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PanduhzGameBreakerBaseMod : BaseUnityPlugin
    {

        private const string modGUID = "Panduhz.GameBreaker";
        private const string modName = "Gamebreaker";
        private const string modVersion = "1.0.0";

        private readonly Harmony harmony = new Harmony(modGUID);
        public ManualLogSource mls;

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------







        //bools
        internal static ConfigEntry<bool> EnableinfiniteSprintHack;
        internal static ConfigEntry<bool> EnablenoFallDamageHack;
        internal static ConfigEntry<bool> EnablereachHack;
        internal static ConfigEntry<bool> EnabledrownHack;
        internal static ConfigEntry<bool> EnableinfiniteCredits;
        internal static ConfigEntry<bool> EnableShovelHack;
        internal static ConfigEntry<bool> EnableshipbuttonHack;
        internal static ConfigEntry<bool> EnablehinderHack;
        internal static ConfigEntry<bool> EnableGodModeHack;
        //internal static ConfigEntry<bool> EnableNightVision;
        //floats
        internal static ConfigEntry<float> jumpHack;
        internal static ConfigEntry<float> speedHack;
        internal static ConfigEntry<float> climbSpeedHack;
        internal static ConfigEntry<float> timetoholdHack;
        internal static ConfigEntry<float> doorpowerHack;
        internal static ConfigEntry<float> VowBridgeDurability;
        internal static ConfigEntry<float> FlashBangTimer;
        internal static ConfigEntry<float> playerThrowPower;
        internal static ConfigEntry<float> turretRotationSpeed;
        internal static ConfigEntry<float> turretRotationRange;
        //strings
        public static ConfigEntry<string> MenuKeyCode;


        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //float
        internal static float grabDistance;
        internal static float drowningTimer;
        internal static float jumpForce;
        internal static float sprintMeter;
        internal static float carryWeight;
        internal static float climbSpeed;
        internal static float cooldownTime;
        internal static float timeToHold;
        internal static float doorPowerDuration;
        internal static float bridgeDurability;
        internal static float TimeToExplode;
        internal static float throwPower;
        internal static float rotationRange;
        internal static float rotationSpeed;
        //int
        internal static int shovelHitForce;
        internal static int groupCredits;
        internal static int isMovementHindered;
        //bool
        internal static bool takingFallDamage;
        internal static bool twoHandedItemAllowed;
        internal static bool buttonsEnabled;
        internal static bool result2;
        //Light
        //internal static Light nightVision;


        internal static PanduhzGameBreakerBaseMod Instance;

        internal static GUILoader myGUI;
        private static bool hasGUISynced = false;

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public void Awake()
        {
            Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);


            mls.LogInfo(" ____   _    _   _ ____  _   _ _   _ _____   ____    _    __  __ _____");
            mls.LogInfo("|  _ \\ / \\  | \\ | |  _ \\| | | | | | |__  /  / ___|  / \\  |  \\/  | ____|");
            mls.LogInfo("| |_) / _ \\ |  \\| | | | | | | | |_| | / /  | |  _  / _ \\ | |\\/| |  _|");
            mls.LogInfo("|  __/ ___ \\| |\\  | |_| | |_| |  _  |/ /_  | |_| |/ ___ \\| |  | | |___");
            mls.LogInfo("| _|/_/___\\_\\_|_\\_|____/ \\___/|_|_|_/____|  \\____/_/   \\_\\_|  |_|_____|");
            mls.LogInfo("| __ )|  _ \\| ____|  / \\  | |/ / ____|  _ \\");
            mls.LogInfo("|  _ \\| |_) |  _|   / _ \\ | ' /|  _| | |_) |");
            mls.LogInfo("| |_) |  _ <| |___ / ___ \\| . \\| |___|  _ < ");
            mls.LogInfo("|____/|_| \\_\\_____/_/   \\_\\_|\\_\\_____|_| \\_\\ ");








            harmony.PatchAll(typeof(PanduhzGameBreakerBaseMod));

            var gameObject = new UnityEngine.GameObject("GUILoader");
            UnityEngine.Object.DontDestroyOnLoad(gameObject);
            gameObject.hideFlags = HideFlags.HideAndDontSave;
            gameObject.AddComponent<GUILoader>();
            myGUI = (GUILoader)gameObject.GetComponent("GUILoader");

            SetBindings();
            setGUIVars();
        }


        private void ResetValues()
        {

        }


        void setGUIVars()
        {
            //bools
            myGUI.guiEnableinfiniteSprintHack = EnableinfiniteSprintHack.Value;
            myGUI.guiEnabledrownHack = EnabledrownHack.Value;
            myGUI.guiEnablenoFallDamageHack = EnablenoFallDamageHack.Value;
            myGUI.guiEnablereachHack = EnablereachHack.Value;
            myGUI.guiEnableinfiniteCredits = EnableinfiniteCredits.Value;
            myGUI.guiEnableShovelHack = EnableShovelHack.Value;
            myGUI.guiEnableshipbuttonHack = EnableshipbuttonHack.Value;
            myGUI.guiEnablehinderHack = EnablehinderHack.Value;
            myGUI.guiEnableGodModeHack = EnableGodModeHack.Value;
            //floats
            myGUI.guijumpHack = jumpHack.Value;
            myGUI.guispeedHack = speedHack.Value;
            myGUI.guiclimbSpeedHack = climbSpeedHack.Value;
            myGUI.guitimetoholdHack = timetoholdHack.Value;
            myGUI.guidoorpowerHack = doorpowerHack.Value;
            myGUI.guiVowBridgeDurability = VowBridgeDurability.Value;
            myGUI.guiFlashBangTimer = FlashBangTimer.Value;
            myGUI.guiplayerThrowPower = playerThrowPower.Value;
            myGUI.guiturretRotationRange = turretRotationRange.Value;
            myGUI.guiturretRotationSpeed = turretRotationSpeed.Value;
            //strings
            myGUI.guiMenuKeyCode = MenuKeyCode.Value;
            //guisync
            hasGUISynced = true;
        }


        internal void UpdateCFGVarsFromGUI()
        {
            if (!hasGUISynced) { setGUIVars(); }
            //bools
            EnableinfiniteSprintHack.Value = myGUI.guiEnableinfiniteSprintHack;
            EnabledrownHack.Value = myGUI.guiEnabledrownHack;
            EnablenoFallDamageHack.Value = myGUI.guiEnablenoFallDamageHack;
            EnablereachHack.Value = myGUI.guiEnablereachHack;
            EnableinfiniteCredits.Value = myGUI.guiEnableinfiniteCredits;
            EnableShovelHack.Value = myGUI.guiEnableShovelHack;
            EnableshipbuttonHack.Value = myGUI.guiEnableshipbuttonHack;
            EnablehinderHack.Value = myGUI.guiEnablehinderHack;
            EnableGodModeHack.Value = myGUI.guiEnableGodModeHack;
            //EnableNightVision.Value = myGUI.guiEnableNightVision;
            //floats
            jumpHack.Value = myGUI.guijumpHack;
            speedHack.Value = myGUI.guispeedHack;
            climbSpeedHack.Value = myGUI.guiclimbSpeedHack;
            timetoholdHack.Value = myGUI.guitimetoholdHack;
            doorpowerHack.Value = myGUI.guidoorpowerHack;
            VowBridgeDurability.Value = myGUI.guiVowBridgeDurability;
            FlashBangTimer.Value = myGUI.guiFlashBangTimer;
            turretRotationRange.Value = myGUI.guiturretRotationRange;
            turretRotationSpeed.Value = myGUI.guiturretRotationSpeed;
            //strings
            playerThrowPower.Value = myGUI.guiplayerThrowPower;
            MenuKeyCode.Value = myGUI.guiMenuKeyCode;
        }

            private void SetBindings()
        {
            //bools
            EnableinfiniteSprintHack = Config.Bind("Hacks", "Player - enable infinite sprint hack", false, "");
            EnabledrownHack = Config.Bind("Hacks", "Player - enable drown hack", false, "");
            EnablenoFallDamageHack = Config.Bind("Hacks", "Player - enable no fall damage Hack", false, "");
            EnablereachHack = Config.Bind("Hacks", "Player - enable reach hack", false, "");
            EnableinfiniteCredits = Config.Bind("Hacks", "Player - enable infinite credits", false, "");
            EnableShovelHack = Config.Bind("Hacks", "Player - Shovel Hit Force", false, "");
            EnableshipbuttonHack = Config.Bind("Hacks", "Player - Ship Buttons", false, "");
            EnablehinderHack = Config.Bind("Hacks", "Player - Hinderance", false, "");
            EnableGodModeHack = Config.Bind("Hacks", "Player - God Mode", false, "");
            //EnableNightVision = Config.Bind("Hacks", "Player - Night Vision", false, "");
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            EnableinfiniteSprintHack.SettingChanged += InfiniteSprintPatch;
            EnabledrownHack.SettingChanged += DrownPatch;
            EnablenoFallDamageHack.SettingChanged += NoFallDamagePatch;
            EnablereachHack.SettingChanged += ReachPatch;
            EnableinfiniteCredits.SettingChanged += CreditsPatch;
            EnableShovelHack.SettingChanged += ShovelPatch; 
            EnableshipbuttonHack.SettingChanged += ShipButtons;
            EnablehinderHack.SettingChanged += HinderedPatch;
            EnableGodModeHack.SettingChanged += GodModePatch;
            //EnableNightVision.SettingChanged += NightVisionPatch;
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //floats
            jumpHack = Config.Bind("Hacks", "Player - Jump Force", 30f, new ConfigDescription("Base jump force for player", new AcceptableValueRange<float>(5.0f, 500.0f)));//20 is a pretty good jump force
            speedHack = Config.Bind("Hacks", "Player - Speed", 1f, new ConfigDescription("Base speed for player", new AcceptableValueRange<float>(0.01f, 1f)));//0.3 is pretty good speed not too buggy
            climbSpeedHack = Config.Bind("Hacks", "Player - Climb Speed", 2000f, new ConfigDescription("Base climb speed for player", new AcceptableValueRange<float>(4.0f, 2000.0f)));
            timetoholdHack = Config.Bind("Hacks", "Player - Interact", 0.0000000001f, new ConfigDescription("Time to hold interact button", new AcceptableValueRange<float>(0.0000000001f, 1.0f)));
            doorpowerHack = Config.Bind("Hacks", "Ship - Door Controls", 1f, new ConfigDescription("Time the ship door stays powered", new AcceptableValueRange<float>(1.0f, 20.0f)));
            VowBridgeDurability = Config.Bind("Hacks", "Vow - Bridge Durability", 1f, new ConfigDescription("How much weight the bridge on Vow can take", new AcceptableValueRange<float>(-100.0f, 100.0f)));
            FlashBangTimer = Config.Bind("Hacks", "Flashbang - Timer", 2.25f, new ConfigDescription("How long until the flashbang explode", new AcceptableValueRange<float>(0.0f, 360.0f)));
            playerThrowPower = Config.Bind("Hacks", "Player - Throw Power", 17.0f, new ConfigDescription("How far you can throw items", new AcceptableValueRange<float>(17.0f, 1000.0f)));
            turretRotationSpeed = Config.Bind("Hacks", "Turret Rotation Speed", 20.0f, new ConfigDescription("How fast the turret rotation", new AcceptableValueRange<float>(0.0f, 99999.0f)));
            turretRotationRange = Config.Bind("Hacks", "Turret Rotation Range", 45.0f, new ConfigDescription("How much the turret can rotate", new AcceptableValueRange<float>(0.0f, 360.0f)));
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            jumpHack.SettingChanged += JumpPatch;
            speedHack.SettingChanged += SpeedPatch;
            climbSpeedHack.SettingChanged += ClimbSpeedPatch;
            EnablehinderHack.SettingChanged += HinderedPatch;
            timetoholdHack.SettingChanged += InteractPatch;
            doorpowerHack.SettingChanged += ShipDoorPower;
            VowBridgeDurability.SettingChanged += BridgePatch;
            FlashBangTimer.SettingChanged += FlashbangPatch;
            playerThrowPower.SettingChanged += ThrowPatch;
            turretRotationSpeed.SettingChanged += TurretRotationSpeedPatch;
            turretRotationRange.SettingChanged += TurretRotationRangePatch;
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //strings
            MenuKeyCode = Config.Bind("Hacks", "Menu Key to open", "F2", "");
            //find way to change 
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ReachPatch(object sender, EventArgs e)
        {
            if (EnablereachHack.Value)
            {
                // this would be a variable you have stored
                grabDistance = 99999f;
            }
            else
            {
                grabDistance = 1;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void ReachPatchToggled(ref float ___grabDistance)
        {
            ___grabDistance = grabDistance;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ShovelPatch(object sender, EventArgs e)
        {
            if (EnableShovelHack.Value)
            {
                shovelHitForce = 999;
            }
            else
            {
                shovelHitForce = 1;
            }
        }

        [HarmonyPatch(typeof(Shovel), "ItemActivate")]
        [HarmonyPostfix]
        static void ShovelPatchToggled(ref int ___shovelHitForce)//boolean
        {
            ___shovelHitForce = shovelHitForce; 
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void DrownPatch(object sender, EventArgs e)
        {
            if (EnabledrownHack.Value)
            {
                drowningTimer = 1f;
            }
        }

        [HarmonyPatch(typeof(StartOfRound),"Update")]
        [HarmonyPostfix]
        static void DrownPatchToggled(ref float ___drowningTimer)//boolean
        {
                ___drowningTimer = drowningTimer;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void JumpPatch(object sender, EventArgs e)
        {
            jumpForce = jumpHack.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void JumpHackToggled(ref float ___jumpForce)//float
        {
            ___jumpForce = jumpForce;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void InfiniteSprintPatch(object sender, EventArgs e)
        {
            float sprintbefore = sprintMeter;
            if (EnableinfiniteSprintHack.Value)
            {
                sprintMeter = 1f;
            }
            else
            {
                sprintMeter = sprintbefore;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void InfiniteSprintToggled(ref float ___sprintMeter)//boolean
        {
            ___sprintMeter = sprintMeter;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void SpeedPatch(object sender, EventArgs e)
        {
            carryWeight = speedHack.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void SpeedToggled(ref float ___carryWeight)//float
        {
            ___carryWeight = carryWeight;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ClimbSpeedPatch(object sender, EventArgs e)
        {
            climbSpeed = climbSpeedHack.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void ClimbSpeedToggled(ref float ___climbSpeed)//float
        {
            ___climbSpeed = climbSpeed;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void NoFallDamagePatch(object sender, EventArgs e)
        {
            if (EnablenoFallDamageHack.Value)
            {
                takingFallDamage = false;
            }
            else
            {
                takingFallDamage = true;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void NoFallDamageToggled(ref bool ___takingFallDamage)//boolean
        {
            ___takingFallDamage = takingFallDamage;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void HinderedPatch(object sender, EventArgs e)
        {
            if (EnablehinderHack.Value)
            {
                isMovementHindered = 0;
            }
        }


        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void HinderToggled(ref int ___isMovementHindered)//int
        {
            ___isMovementHindered = isMovementHindered;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void CreditsPatch(object sender, EventArgs e)
        {
            if (EnableinfiniteCredits.Value)
            {
                groupCredits = 1000000;
            }
            else
            {
                groupCredits = 0;
            }
        }


        [HarmonyPatch(typeof(Terminal), "OnSubmit")]
        [HarmonyPostfix]
        private static void CreditsToggled(ref int ___groupCredits)//boolean
        {
            ___groupCredits = groupCredits;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void InteractPatch(object sender, EventArgs e)
        {
            twoHandedItemAllowed = true;
            cooldownTime = 0f;
            timeToHold = timetoholdHack.Value;
        }

        [HarmonyPatch(typeof(InteractTrigger), "Interact")]
        [HarmonyPostfix]

        static void InteractToggled(ref float ___cooldownTime, ref bool ___twoHandedItemAllowed, ref float ___timeToHold)//float
        {
            ___twoHandedItemAllowed = twoHandedItemAllowed;
            ___cooldownTime = cooldownTime;
            ___timeToHold = timeToHold;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ShipButtons(object sender, EventArgs e)
        {
            if (EnableshipbuttonHack.Value)
            {
                buttonsEnabled = true;
            }
            else
            {
                buttonsEnabled = false;
            }
        }
        static void ShipDoorPower(object sender, EventArgs e)
        {
            doorPowerDuration = doorpowerHack.Value;
        }

        [HarmonyPatch(typeof(HangarShipDoor), "Update")]
        [HarmonyPostfix]
        static void ShipControlsToggled(ref bool ___buttonsEnabled, ref float ___doorPowerDuration)//float and bool
        {
            ___doorPowerDuration = doorPowerDuration;
            ___buttonsEnabled = buttonsEnabled;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void BridgePatch(object sender, EventArgs e)
        {
            bridgeDurability = VowBridgeDurability.Value;
        }

        [HarmonyPatch(typeof(BridgeTrigger), "RemovePlayerFromBridge")]
        [HarmonyPostfix]
        static void BridgeToggled(ref float ___bridgeDurability)//float and bool
        {
            ___bridgeDurability = bridgeDurability;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void FlashbangPatch(object sender, EventArgs e)
        {
            TimeToExplode = FlashBangTimer.Value;
        }

        [HarmonyPatch(typeof(StunGrenadeItem), "Update")]
        [HarmonyPostfix]
        static void FlashBangToggled(ref float ___TimeToExplode)//float and bool
        {
            ___TimeToExplode = TimeToExplode;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        void GodModePatch(object sender, EventArgs e)
        {
            if (EnableGodModeHack.Value)
            {
                result2 = false;
            }
            else
            {
                result2 = true;
            }
        }
        
        [HarmonyPatch(nameof(PlayerControllerB.AllowPlayerDeath))]
        [HarmonyPostfix]
        static void GodModeToggled(ref bool __result)//float and bool
        {
            __result = result2;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void ThrowPatch(object sender, EventArgs e)
        {
            throwPower = playerThrowPower.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void ThrowToggle(ref float ___throwPower)//float
        {
            ___throwPower = throwPower;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        //private void NightVisionPatch(object sender, EventArgs e)
        //{
        //    if(EnableNightVision.Value)
        //    {
        //        nightVision.enabled = true;
        //    }
        //    else
        //    {
        //        nightVision.enabled = false;
        //      }


        //[HarmonyPatch(typeof(PlayerControllerB), "Update")]
        //[HarmonyPostfix]
        //static void NightVisionToggled(ref Light ___nightVision)
        //{
        //    ___nightVision = nightVision;
        //}

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void TurretRotationRangePatch(object sender, EventArgs e)
        {
            rotationRange = turretRotationRange.Value;
        }

        private void TurretRotationSpeedPatch(object sender, EventArgs e)
        {
            rotationSpeed = turretRotationSpeed.Value;
        }

        [HarmonyPatch(typeof(Turret), "ToggleTurretEnabled")]
        [HarmonyPostfix]
        static void TurretToggle(ref float ___rotationRange, ref float ___rotationSpeed)//float
        {
            ___rotationRange = rotationRange;
            ___rotationSpeed = rotationSpeed;
        }





    }
}
