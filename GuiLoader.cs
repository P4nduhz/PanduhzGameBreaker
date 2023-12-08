using BepInEx.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PanduhzGameBreaker.Component
{
    internal class GUILoader : MonoBehaviour
    {
        private KeyboardShortcut openCloseMenu;
        private bool isMenuOpen;

        internal bool wasKeyDown;

        private int toolbarInt = 0;
        private string[] toolbarStrings = new string[] { "Hacks" };

        private int MENUWIDTH = 600;
        private int MENUHEIGHT = 800;
        private int MENUX;
        private int MENUY;
        private int ITEMWIDTH = 300;
        private int CENTERX;

        //bool
        public bool guiEnablenoFallDamageHack;
        public bool guiEnablereachHack;
        public bool guiEnabledrownHack;
        public bool guiEnableinfiniteSprintHack;
        public bool guiEnableinfiniteCredits;
        //floats
        public float guijumpHack;
        public float guispeedHack;
        public float guiclimbSpeedHack;
        //ints
        public int guishovelHack;

        private GUIStyle menuStyle;
        private GUIStyle buttonStyle;
        private GUIStyle labelStyle;
        private GUIStyle toggleStyle;
        private GUIStyle hScrollStyle;


        private void Awake()
        {
            openCloseMenu = new KeyboardShortcut(KeyCode.Insert);
            isMenuOpen = false;
            // this isn't pygame.. only need the screenwidth and height
            MENUX = (Screen.width / 2); //- (MENUWIDTH / 2);
            MENUY = (Screen.height / 2); // - (MENUHEIGHT / 2);
            CENTERX = MENUX + ((MENUWIDTH / 2) - (ITEMWIDTH / 2));
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
                GUI.Box(new Rect(MENUX, MENUY, MENUWIDTH, MENUHEIGHT), "PanduhzGameBreaker", menuStyle);

                toolbarInt = GUI.Toolbar(new Rect(MENUX, MENUY - 30, MENUWIDTH, 30), toolbarInt, toolbarStrings, buttonStyle);

                switch (toolbarInt)
                {
                    case 0:

                        //bools
                        guiEnableinfiniteSprintHack = GUI.Button(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 30, ITEMWIDTH, 30), "Enable Infinite Sprint ", buttonStyle);
                        guiEnabledrownHack = GUI.Button(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 60, ITEMWIDTH, 30), "Enable No Drowning ", buttonStyle);
                        guiEnablenoFallDamageHack = GUI.Button(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 90, ITEMWIDTH, 30), "Enable No Fall Damage ", buttonStyle);
                        guiEnablereachHack = GUI.Button(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 120, ITEMWIDTH, 30), "Enable Reach ", buttonStyle);
                        guiEnableinfiniteCredits = GUI.Button(new Rect(MENUX + (MENUWIDTH / 2), MENUY + 150, ITEMWIDTH, 30), "Enable Infinite Credits ", buttonStyle);
                        //floats
                        guispeedHack = GUI.HorizontalSlider(new Rect(CENTERX, MENUY + 160, ITEMWIDTH, 30), guispeedHack, 0.1f, 1.0f);
                        GUI.Label(new Rect(CENTERX, MENUY + 130, ITEMWIDTH, 30), "Speed strength of the player " + guispeedHack.ToString(), labelStyle);
                        GUI.Label(new Rect(CENTERX, MENUY + 200, ITEMWIDTH, 30), "Jump strength of the player " + guijumpHack.ToString(), labelStyle);
                        guijumpHack = GUI.HorizontalSlider(new Rect(CENTERX, MENUY + 230, ITEMWIDTH, 30), guijumpHack, 1.0f, 500.0f);
                        GUI.Label(new Rect(CENTERX, MENUY + 270, ITEMWIDTH, 30), "Jester cranking speed " + guiclimbSpeedHack.ToString(), labelStyle);
                        guiclimbSpeedHack = GUI.HorizontalSlider(new Rect(CENTERX, MENUY + 300, ITEMWIDTH, 30), guiclimbSpeedHack, 4.0f, 2000.0f);
                        //ints
                        break;
                }

            }

        }
    }
}