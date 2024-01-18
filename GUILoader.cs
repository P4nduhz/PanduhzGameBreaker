using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using static IngamePlayerSettings;

namespace PanduhzGameBreaker.Component
{
    internal class GUILoader : MonoBehaviour
    {
        private KeyboardShortcut openCloseMenu;
        private bool isMenuOpen;

        internal bool wasKeyDown;

        private int toolbarInt = 0;
        private string[] toolbarStrings = new string[] { "Hacks", "Player Management", "Information & Credits" };

        private int MENUWIDTH = 600;
        private int MENUHEIGHT = 800;
        private int MENUX;
        private int MENUY;
        private int MENUWIDTH40;
        private int MENUX20;
        private int ITEMWIDTH = 300;
        private int CENTERX;








        
        //bools
        public bool guiEnablenoFallDamageHack;
        public bool guiEnablereachHack;
        public bool guiEnabledrownHack;
        public bool guiEnableinfiniteSprintHack;
        public bool guiEnableinfiniteCredits;
        public bool guiEnableShovelHack;
        public bool guiEnableshipbuttonHack;
        public bool guiEnablehinderHack;
        public bool guiEnableGodModeHack;
        //public bool guiEnableNightVision;
        //floats
        public float guijumpHack;
        public float guispeedHack;
        public float guiclimbSpeedHack;
        public float guitimetoholdHack;
        public float guidoorpowerHack;
        public float guiVowBridgeDurability;
        public float guiFlashBangTimer;
        public float guiplayerThrowPower;
        public float guiturretRotationRange;
        public float guiturretRotationSpeed;
        //strings
        public string guiMenuKeyCode;




        



        private GUIStyle menuStyle;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;
        private GUIStyle toggleStyle;
        private GUIStyle hScrollStyle;


        private void Awake()
        {
            openCloseMenu = new KeyboardShortcut(KeyCode.F2);//find way to make type into variable //guimenukeycode
            isMenuOpen = false;
            // this isn't pygame.. only need the screenwidth and height
            MENUX = (Screen.width / 2); //- (MENUWIDTH / 2);
            MENUY = (Screen.height / 2); // - (MENUHEIGHT / 2);
            CENTERX = MENUX + ((MENUWIDTH / 2) - (ITEMWIDTH / 2));
            MENUX20 = MENUX + 20;
            MENUWIDTH40 = MENUWIDTH - 40;
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];
            for (int i = 0; i < pix.Length; ++i)
            {
                pix[i] = col;
            }
            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();
            return result;
        }

        private void intitializeMenu()
        {
            if (menuStyle == null)
            {
                menuStyle = new GUIStyle(GUI.skin.box);
                buttonStyle = new GUIStyle(GUI.skin.button);
                labelStyle = new GUIStyle(GUI.skin.label);
                toggleStyle = new GUIStyle(GUI.skin.toggle);
                hScrollStyle = new GUIStyle(GUI.skin.horizontalSlider);

                menuStyle.normal.textColor = Color.white;
                menuStyle.normal.background = MakeTex(2, 2, new Color(0.01f, 0.01f, 0.1f, .9f));
                menuStyle.fontSize = 18;
                menuStyle.normal.background.hideFlags = HideFlags.HideAndDontSave;

                buttonStyle.normal.textColor = Color.white;
                buttonStyle.fontSize = 18;

                labelStyle.normal.textColor = Color.white;
                labelStyle.normal.background = MakeTex(2, 2, new Color(0.01f, 0.01f, 0.1f, .9f));
                labelStyle.fontSize = 18;
                labelStyle.alignment = TextAnchor.MiddleCenter;
                labelStyle.normal.background.hideFlags = HideFlags.HideAndDontSave;

                toggleStyle.normal.textColor = Color.white;
                toggleStyle.fontSize = 18;

                hScrollStyle.normal.textColor = Color.white;
                hScrollStyle.normal.background = MakeTex(2, 2, new Color(0.0f, 0.0f, 0.2f, .9f));
                hScrollStyle.normal.background.hideFlags = HideFlags.HideAndDontSave;

            }
        }


        public void Update()
        {
            PanduhzGameBreakerBaseMod.Instance.UpdateCFGVarsFromGUI();
            // Much better than onPressed
            // removes jitter, ensures menu always toggles when key is released
            if (openCloseMenu.IsDown())
            {
                if (!wasKeyDown)
                {
                    wasKeyDown = true;
                }
            }
            if (openCloseMenu.IsUp())
            {
                if (wasKeyDown)
                {
                    wasKeyDown = false;
                    isMenuOpen = !isMenuOpen;
                    if (isMenuOpen)
                    {
                        Cursor.visible = true;
                        Cursor.lockState = CursorLockMode.Confined;
                    }
                    else
                    {
                        Cursor.visible = false;
                        //Cursor.lockState = CursorLockMode.Locked;
                    }
                }
            }

        }


        public void OnGUI()
        {
            // oddly enough doesn't work here
            if (menuStyle == null) { intitializeMenu(); }

            if (isMenuOpen)
            {
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                int offset = 200;
                string Disclaimer = "*Still in development";
                string Discord = " Discord Panduhz8044";
                string Youtube = " https://www.youtube.com/@panduhz1583";
                string Github = " https://Github.com/p4nduhz";
                string Steam = " https://steamcommunity.com/id/panduhz420";
                string Minx1 = " Huge thanks to Minx this mod would not be possible without him";
                string Minx2 = " https://www.youtube.com/@iMinx";
                string ReccomendedSettings = " Recommended Settings:";
                string Setting1 = " Jump Hack: 30.0";
                string Setting2 = " Speed Hack: 0.3";
                string Setting3 = " Climb Speed Hack: 2000.0";
                string Setting4 = " Hinder Hack 0.3";
                string Setting5 = " Interact Hack 0.05";
                string Setting6 = " Door Power: depends on use case if unsure 1";
                GUI.Box(new Rect(MENUX, MENUY, MENUWIDTH, MENUHEIGHT), "PanduhzGameBreaker", menuStyle);
                //-------------------------------------------------------------------------------------------------------------------------------------------------------------------------
                toolbarInt = GUI.Toolbar(new Rect(MENUX, MENUY - 30, MENUWIDTH, 30), toolbarInt, toolbarStrings, buttonStyle);

                switch (toolbarInt)
                {
                    case 0:
                        //Hacks
                        GUI.Label(new Rect(MENUX + MENUWIDTH - 200, MENUY, 200, 30), Disclaimer, labelStyle);
                        //bools
                        guiEnableinfiniteSprintHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 30, ITEMWIDTH, 30), guiEnableinfiniteSprintHack, "Infinite Sprint", buttonStyle);
                        guiEnabledrownHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 70, ITEMWIDTH, 30), guiEnabledrownHack, "No Drowning", buttonStyle);
                        guiEnablenoFallDamageHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 110, ITEMWIDTH, 30), guiEnablenoFallDamageHack, "No Fall Damage", buttonStyle);
                        guiEnablereachHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 150, ITEMWIDTH, 30), guiEnablereachHack, "Reach", buttonStyle);
                        guiEnableinfiniteCredits = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 190, ITEMWIDTH, 30), guiEnableinfiniteCredits, "Infinite Credits", buttonStyle);
                        guiEnableShovelHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 230, ITEMWIDTH, 30), guiEnableShovelHack, "Shovel Damage", buttonStyle);
                        guiEnableshipbuttonHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 270, ITEMWIDTH, 30), guiEnableshipbuttonHack, "Ship Buttons", buttonStyle);
                        guiEnablehinderHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 310, ITEMWIDTH, 30), guiEnablehinderHack, "Player Hinderance", buttonStyle);
                        guiEnableGodModeHack = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 350, ITEMWIDTH, 30), guiEnableGodModeHack, "God Mode", buttonStyle);
                        //guiEnableNightVision = GUI.Toggle(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 390, ITEMWIDTH, 30), guiEnableNightVision, "Night Vision", buttonStyle);
                        //floats
                        guijumpHack = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 70, ITEMWIDTH, 30), guijumpHack, 5.0f, 500.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 30, ITEMWIDTH, 30), " Player Jump: " + guijumpHack.ToString(), labelStyle);
                        //
                        guispeedHack = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 140, ITEMWIDTH, 30), guispeedHack, 0.01f, 1.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 100, ITEMWIDTH, 30), " Player Speed: " + guispeedHack.ToString(), labelStyle);
                        //
                        guiclimbSpeedHack = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 210, ITEMWIDTH, 30), guiclimbSpeedHack, 4.0f, 2000.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 170, ITEMWIDTH, 30), " Player Climb Speed: " + guiclimbSpeedHack.ToString(), labelStyle);
                        //
                        guitimetoholdHack = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 280, ITEMWIDTH, 30), guitimetoholdHack, 0.0000000001f, 1.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 240, ITEMWIDTH, 30), " Interact time: " + guitimetoholdHack.ToString(), labelStyle);
                        //
                        guidoorpowerHack = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 350, ITEMWIDTH, 30), guidoorpowerHack, 1.0f, 20f);
                        GUI.Label(new Rect(MENUX, MENUY + 310, ITEMWIDTH, 30), " Ship Door Power: " + guidoorpowerHack.ToString(), labelStyle);
                        //
                        guiVowBridgeDurability = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 420, ITEMWIDTH, 30), guiVowBridgeDurability, -100.0f, 100.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 380, ITEMWIDTH, 30), " Vow Bridge Durability: " + guiVowBridgeDurability.ToString(), labelStyle);
                        //
                        guiFlashBangTimer = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 490, ITEMWIDTH, 30), guiFlashBangTimer, 0.0f, 360.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 450, ITEMWIDTH, 30), " Stun Grenade Timer: " + guiFlashBangTimer.ToString(), labelStyle);
                        //
                        guiplayerThrowPower = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 560, ITEMWIDTH, 30), guiplayerThrowPower, 17.0f, 1000.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 520, ITEMWIDTH, 30), " Player Throw Power: " + guiplayerThrowPower.ToString(), labelStyle);
                        //
                        guiturretRotationSpeed = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 630, ITEMWIDTH, 30), guiturretRotationSpeed, 0.0f, 99999.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 590, ITEMWIDTH, 30), " Turret Rotation Speed: " + guiturretRotationSpeed.ToString(), labelStyle);
                        //
                        guiturretRotationRange = GUI.HorizontalSlider(new Rect(MENUX, MENUY + 700, ITEMWIDTH, 30), guiturretRotationRange, 0.0f, 360.0f);
                        GUI.Label(new Rect(MENUX, MENUY + 660, ITEMWIDTH, 30), " Turret Rotation Range: " + guiturretRotationRange.ToString(), labelStyle);
                        break;
                    case 1:
                        //Player Management
                        GUI.Label(new Rect(MENUX + MENUWIDTH - offset, MENUY, offset, 30), Disclaimer, labelStyle);
                        
                        break;
                    case 2:
                        //Information & Credits
                        GUI.Label(new Rect(MENUX + MENUWIDTH - offset, MENUY, offset, 30), Disclaimer, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 40, MENUWIDTH40, 30), Discord, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 70, MENUWIDTH40, 30), Youtube, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 100, MENUWIDTH40, 30), Github, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 130, MENUWIDTH40, 30), Steam, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 160, MENUWIDTH40, 30), ReccomendedSettings, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 190, MENUWIDTH40, 30), Setting1, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 215, MENUWIDTH40, 30), Setting2, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 240, MENUWIDTH40, 30), Setting3, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 265, MENUWIDTH40, 30), Setting4, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 290, MENUWIDTH40, 30), Setting5, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 315, MENUWIDTH40, 30), Setting6, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 350, MENUWIDTH40, 30), Minx1, labelStyle);
                        GUI.Label(new Rect(MENUX20, MENUY + 380, MENUWIDTH40, 30), Minx2, labelStyle);
                        break;
                }

            }

        }
    }
}