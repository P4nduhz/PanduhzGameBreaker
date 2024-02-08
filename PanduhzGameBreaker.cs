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
using System.CodeDom;

//All of my mods combined into one with a gui system
namespace PanduhzGameBreaker
{
    [BepInPlugin(modGUID, modName, modVersion)]
    [HarmonyPatch(typeof(PlayerControllerB))]
    public class PanduhzGameBreakerBaseMod : BaseUnityPlugin
    {

        private const string modGUID = "Panduhz.GameBreaker";
        private const string modName = "Gamebreaker";
        private const string modVersion = "1.0.6";
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        internal PlayerControllerB Player;
        //internal PlayerControllerB PlayerCamera;
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------




        private readonly Harmony harmony = new Harmony(modGUID);
        public ManualLogSource mls;

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------



        internal List<string> unlockableShipUpgrades = new List<string> { "Loud Horn", "Signal Translator", "Teleporter", "Inverse Teleporter" };
        internal List<string> unlockableShipItems = new List<string> { "Cupboard", "File Cabinet", "Bunkbeds", "Plushie pajama man", "Shower", "Table", "Television", "Romantic Table", "Cozy Lights", "Record Player", "Jack-o-lantern", "Welcome Mat", "Toilet", "Goldfish" };
        internal List<string> unlockableShipSuits = new List<string> { "Pajama suit", "Hazard suit", "Green Suit" };

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
        internal static ConfigEntry<bool> ToggleShipLights;
        internal static ConfigEntry<bool> EnableInfiniteShotgunAmmo;
        internal static ConfigEntry<bool> EnableFlyHack;
        internal static ConfigEntry<bool> toggleShipDoors;
        internal static ConfigEntry<bool> EnableheavyBleeding;
        internal static ConfigEntry<bool> EnableFreeCamHack;
        internal static ConfigEntry<bool> PlayerLookInputBool;
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
        //int
        internal static int shovelHitForce;
        internal static int groupCredits;
        internal static int isMovementHindered;
        internal static int shellsloaded;
        //bool
        internal static bool takingFallDamage;
        internal static bool buttonsEnabled;
        internal static bool result2;
        internal static bool areLightsOn;
        internal static bool flyHack;
        internal static bool bleedingHeavily;
        internal static bool freeCam;
        internal static bool lookInput;
        //


        internal static PanduhzGameBreakerBaseMod Instance;

        internal static GUILoader myGUI;
        private static bool hasGUISynced = false;

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        public void Awake()
        {
            Instance = this;
            mls = BepInEx.Logging.Logger.CreateLogSource(modGUID);

            mls.LogInfo("\n" +
                "\r\n  ____   _    _   _ ____  _   _ _   _ _____   ____    _    __  __ _____" +
                "\r\n |  _ \\ / \\  | \\ | |  _ \\| | | | | | |__  /  / ___|  / \\  |  \\/  | ____|" +
                "\r\n | |_) / _ \\ |  \\| | | | | | | | |_| | / /  | |  _  / _ \\ | |\\/| |  _|" +
                "\r\n |  __/ ___ \\| |\\  | |_| | |_| |  _  |/ /_  | |_| |/ ___ \\| |  | | |___" +
                "\r\n | _|/_/___\\_\\_|_\\_|____/ \\___/|_|_|_/____|  \\____/_/   \\_\\_|  |_|_____|" +
                "\r\n | __ )|  _ \\| ____|  / \\  | |/ / ____|  _ \\" +
                "\r\n |  _ \\| |_) |  _|   / _ \\ | ' /|  _| | |_) |" +
                "\r\n | |_) |  _ <| |___ / ___ \\| . \\| |___|  _ < " +
                "\r\n |____/|_| \\_\\_____/_/   \\_\\_|\\_\\_____|_| \\_\\ " +
                "\r\n" +
                "\r\n version " + modVersion + "\r\n");






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
            myGUI.guiEnableShovelHack = EnableShovelHack.Value;
            myGUI.guiEnableshipbuttonHack = EnableshipbuttonHack.Value;
            myGUI.guiEnablehinderHack = EnablehinderHack.Value;
            myGUI.guiEnableGodModeHack = EnableGodModeHack.Value;
            myGUI.guiToggleShipLights = ToggleShipLights.Value;
            myGUI.guiEnableInfiniteShotgunAmmo = EnableInfiniteShotgunAmmo.Value;
            myGUI.guiEnableFlyHack = EnableFlyHack.Value;
            myGUI.guitoggleShipDoors = toggleShipDoors.Value;
            myGUI.guiEnableheavyBleeding = EnableheavyBleeding.Value;
            myGUI.guiEnableFreeCamHack = EnableFreeCamHack.Value;
            myGUI.guiPlayerLookInputBool = PlayerLookInputBool.Value;
            //floats
            myGUI.guijumpHack = jumpHack.Value;
            myGUI.guispeedHack = speedHack.Value;
            myGUI.guiclimbSpeedHack = climbSpeedHack.Value;
            myGUI.guitimetoholdHack = timetoholdHack.Value;
            myGUI.guidoorpowerHack = doorpowerHack.Value;
            myGUI.guiVowBridgeDurability = VowBridgeDurability.Value;
            myGUI.guiFlashBangTimer = FlashBangTimer.Value;
            myGUI.guiplayerThrowPower = playerThrowPower.Value;
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
            ToggleShipLights.Value = myGUI.guiToggleShipLights;
            EnableInfiniteShotgunAmmo.Value = myGUI.guiEnableInfiniteShotgunAmmo;
            EnableFlyHack.Value = myGUI.guiEnableFlyHack;
            toggleShipDoors.Value = myGUI.guitoggleShipDoors;
            EnableheavyBleeding.Value = myGUI.guiEnableheavyBleeding;
            EnableFreeCamHack.Value = myGUI.guiEnableFreeCamHack;
            PlayerLookInputBool.Value = myGUI.guiPlayerLookInputBool;
            //floats
            jumpHack.Value = myGUI.guijumpHack;
            speedHack.Value = myGUI.guispeedHack;
            climbSpeedHack.Value = myGUI.guiclimbSpeedHack;
            timetoholdHack.Value = myGUI.guitimetoholdHack;
            doorpowerHack.Value = myGUI.guidoorpowerHack;
            VowBridgeDurability.Value = myGUI.guiVowBridgeDurability;
            FlashBangTimer.Value = myGUI.guiFlashBangTimer;
            playerThrowPower.Value = myGUI.guiplayerThrowPower;
            //strings
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
            ToggleShipLights = Config.Bind("Hacks", "Ship - Lights", false, "");
            EnableInfiniteShotgunAmmo = Config.Bind("Hacks", "Player - Shotgun Ammo", false, "");
            EnableFlyHack = Config.Bind("Hacks", "Player - Flying", false, "");
            toggleShipDoors = Config.Bind("Hacks", "Ship - Door Toggle", false, "");
            EnableheavyBleeding = Config.Bind("Hacks", "Player - Heavy Bleeding", false, "");
            EnableFreeCamHack = Config.Bind("Hacks", "Player - Free Cam Hack", false, "");
            PlayerLookInputBool = Config.Bind("Hacks", "Player - Look Input Toggle", false, "");
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
            ToggleShipLights.SettingChanged += ShipLightsPatch;
            EnableInfiniteShotgunAmmo.SettingChanged += ShotgunToggled;
            EnableFlyHack.SettingChanged += FlyHackToggled;
            toggleShipDoors.SettingChanged += ShipDoorPatch;
            EnableheavyBleeding.SettingChanged += BloodPatch;
            EnableFreeCamHack.SettingChanged += FreeCamPatch;
            PlayerLookInputBool.SettingChanged += disableLookInputPatch;
            //EnableNightVision.SettingChanged += NightVisionPatch;
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //floats
            jumpHack = Config.Bind("Hacks", "Player - Jump Force", 15f, new ConfigDescription("Base jump force for player", new AcceptableValueRange<float>(15.0f, 500.0f)));//20 is a pretty good jump force
            speedHack = Config.Bind("Hacks", "Player - Speed", 1f, new ConfigDescription("Base speed for player", new AcceptableValueRange<float>(0.01f, 1f)));//0.3 is pretty good speed not too buggy
            climbSpeedHack = Config.Bind("Hacks", "Player - Climb Speed", 4f, new ConfigDescription("Base climb speed for player", new AcceptableValueRange<float>(4.0f, 2000.0f)));
            timetoholdHack = Config.Bind("Hacks", "Player - Interact", 1f, new ConfigDescription("Time to hold interact button", new AcceptableValueRange<float>(0.0000000001f, 1.0f)));
            doorpowerHack = Config.Bind("Hacks", "Ship - Door Controls", 1f, new ConfigDescription("Time the ship door stays powered", new AcceptableValueRange<float>(0.0f, 1.0f)));
            VowBridgeDurability = Config.Bind("Hacks", "Vow - Bridge Durability", 1f, new ConfigDescription("How much weight the bridge on Vow can take", new AcceptableValueRange<float>(0.0f, 1.0f)));
            FlashBangTimer = Config.Bind("Hacks", "Flashbang - Timer", 2.25f, new ConfigDescription("How long until the flashbang explode", new AcceptableValueRange<float>(0.0f, 360.0f)));
            playerThrowPower = Config.Bind("Hacks", "Player - Throw Power", 17.0f, new ConfigDescription("How far you can throw items", new AcceptableValueRange<float>(17.0f, 1000.0f)));
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
            //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
            //strings
            MenuKeyCode = Config.Bind("Hacks", "Menu Key to open, cannot be changed at the moment working on this", "F2", "");
            //find way to change 
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        //disable look input
        //ref bool ___disableLookInput

        private void disableLookInputPatch(object sender, EventArgs e)
        {
            lookInput = PlayerLookInputBool.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void disableLookInputToggled(ref bool ___disableLookInput)
        {
            ___disableLookInput = lookInput;
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
                grabDistance = 5f;
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

        [HarmonyPatch(typeof(StartOfRound), "Update")]
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
                groupCredits = int.MaxValue;
            }
            else
            {
                groupCredits = int.MinValue;
            }
        }


        [HarmonyPatch(typeof(Terminal), "Update")]
        [HarmonyPostfix]
        private static void CreditsToggled(ref int ___groupCredits)//boolean
        {
            ___groupCredits = groupCredits;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void InteractPatch(object sender, EventArgs e)
        {
            cooldownTime = 0f;
            timeToHold = timetoholdHack.Value;
        }

        [HarmonyPatch(typeof(InteractTrigger), "Interact")]
        [HarmonyPostfix]

        static void InteractToggled(ref float ___cooldownTime, ref float ___timeToHold)//float
        {
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

        [HarmonyPatch(typeof(BridgeTrigger), "Update")]
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
        //ship lights
        private void ShipLightsPatch(object sender, EventArgs e)
        {
            if (ToggleShipLights.Value)
            {
                areLightsOn = true;
            }
            else
            {
                areLightsOn = false;
            }
        }
        [HarmonyPatch(typeof(ShipLights), "SetShipLightsClientRpc")]
        [HarmonyPostfix]
        static void ShipLightsToggled(ref bool ___areLightsOn)
        {
            ___areLightsOn = areLightsOn;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void ShotgunToggled(object sender, EventArgs e)
        {
            if (EnableInfiniteShotgunAmmo.Value)
            {
                shellsloaded = 999;
            }
            else
            {
                shellsloaded = 0;
            }
        }

        [HarmonyPatch(typeof(ShotgunItem), "ItemActivate")]
        [HarmonyPostfix]
        static void ShotgunItemActivatePatch(ShotgunItem __instance)
        {
            __instance.shellsLoaded = shellsloaded;
        }
        [HarmonyPatch(typeof(ShotgunItem), "ShootGun")]
        [HarmonyPostfix]
        public static void PatchShootGun(ShotgunItem __instance)
        {
            __instance.shellsLoaded = shellsloaded;
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void FlyHackToggled(object sender, EventArgs e)
        {
            if (EnableFlyHack.Value)
            {
                flyHack = true;
            }
            else
            {
                flyHack = false;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]


        internal static void FlyHackFunction(ref float ___movementSpeed)//default 0.5
        {
            GetLocalPlayerController();
            if (flyHack)
            {
                float flyingspeed = ___movementSpeed * Time.deltaTime;

                if (UnityInput.Current.GetKey(KeyCode.LeftShift))
                {
                    flyingspeed *= 2f;
                }
                if (UnityInput.Current.GetKey(KeyCode.W))
                {
                    PanduhzGameBreakerBaseMod.Instance.Player.playerRigidbody.transform.position += PanduhzGameBreakerBaseMod.Instance.Player.playerGlobalHead.transform.forward * flyingspeed;
                }
                if (UnityInput.Current.GetKey(KeyCode.A))
                {
                    PanduhzGameBreakerBaseMod.Instance.Player.playerRigidbody.transform.position += PanduhzGameBreakerBaseMod.Instance.Player.playerGlobalHead.transform.right * -flyingspeed;
                }
                if (UnityInput.Current.GetKey(KeyCode.S))
                {
                    PanduhzGameBreakerBaseMod.Instance.Player.playerRigidbody.transform.position += PanduhzGameBreakerBaseMod.Instance.Player.playerGlobalHead.transform.forward * -flyingspeed;
                }
                if (UnityInput.Current.GetKey(KeyCode.D))
                {
                    PanduhzGameBreakerBaseMod.Instance.Player.playerRigidbody.transform.position += PanduhzGameBreakerBaseMod.Instance.Player.playerGlobalHead.transform.right * flyingspeed;
                }
                if (UnityInput.Current.GetKey(KeyCode.Space))
                {
                    PanduhzGameBreakerBaseMod.Instance.Player.playerRigidbody.transform.position += Vector3.up * flyingspeed;
                }
                if (UnityInput.Current.GetKey(KeyCode.LeftControl))
                {
                    PanduhzGameBreakerBaseMod.Instance.Player.playerRigidbody.transform.position += PanduhzGameBreakerBaseMod.Instance.Player.playerGlobalHead.transform.up * -flyingspeed;
                }
                PanduhzGameBreakerBaseMod.Instance.Player.ResetFallGravity();
            }
            else
            {

            }
        }

        internal static void GetLocalPlayerController()
        {
            PanduhzGameBreakerBaseMod.Instance.Player = GameNetworkManager.Instance.localPlayerController;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void ShipDoorPatch(object sender, EventArgs e)
        {
            if (toggleShipDoors.Value)
            {
                RoundManager.Instance.playersManager.SetShipDoorsClosed(closed: true);
            }
            else
            {
                RoundManager.Instance.playersManager.SetShipDoorsClosed(closed: false);
            }
        }


        [HarmonyPatch(typeof(HangarShipDoor), "Update")]
        [HarmonyPostfix]

        internal static void DoorToggle()
        {
            //RoundManager.Instance.playersManager.SetShipDoorsClosed(closed: true);
        }

        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void UnlockAllPatch(object sender, EventArgs e)
        {
            //
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void UnlockToggled()//float
        {
            //PanduhzGameBreakerBaseMod.Instance.unlockableShipItems
            //PanduhzGameBreakerBaseMod.Instance.unlockableShipUpgrades
            //PanduhzGameBreakerBaseMod.Instance.unlockableShipSuits
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


        private void BloodPatch(object sender, EventArgs e)
        {
            bleedingHeavily = EnableheavyBleeding.Value;
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]
        static void BloodToggled(ref bool ___bleedingHeavily)//float
        {
            ___bleedingHeavily = bleedingHeavily;
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        private void FreeCamPatch(object sender, EventArgs e)
        {
            if (EnableFreeCamHack.Value)
            {
                freeCam = true;

            }
            else
            {
                freeCam = false;
            }
        }

        [HarmonyPatch(typeof(PlayerControllerB), "Update")]
        [HarmonyPostfix]

        internal static void FreeCamToggled(ref bool ___isFreeCamera)//default 0.5
        {
            ___isFreeCamera = freeCam;

            if (freeCam)
            {

            }
            else
            {

            }












        }

        internal static void SetFreeCam()
        {
            //PanduhzGameBreakerBaseMod.Instance.PlayerCamera = 
        }
        //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------


    }
}
