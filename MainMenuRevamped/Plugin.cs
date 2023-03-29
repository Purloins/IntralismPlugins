using System;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace MainMenuRevamped
{
    [BepInPlugin(PluginInfo.PLUGIN_GUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    public class Plugin : BaseUnityPlugin
    {
        private static ManualLogSource logger;
        private readonly Harmony harmony = new(PluginInfo.PLUGIN_GUID);
        private static Text newMapDescText;

        private void Awake()
        {
            logger = this.Logger;
            this.harmony.PatchAll(typeof(Plugin));
            logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} is loaded!");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(MenuScene), "Start")]
        public static void GameSceneStart(MenuScene __instance)
        {
            logger.LogInfo($"Plugin {PluginInfo.PLUGIN_GUID} game scene started");
            GameObject menuScene = __instance.mainMenuCanvas;
           
            // Main Menu
            GameObject mainMenuScrollRect = menuScene.transform.GetChild(0).gameObject;
            GameObject mainMenuContent = mainMenuScrollRect.transform.GetChild(0).gameObject;
            // Ranks and Challenges
            GameObject ranksAndChallenges = mainMenuContent.transform.GetChild(0).gameObject;
            // Singleplayer, multiplayer, workshop, store (SMWS)
            GameObject sMWS = mainMenuContent.transform.GetChild(1).gameObject;
            GameObject workshopAndStore = sMWS.transform.GetChild(2).gameObject;
            GameObject itemStore = workshopAndStore.transform.GetChild(1).gameObject;
            // Copyright
            GameObject copyrightText = mainMenuContent.transform.GetChild(4).gameObject;
            // ReportBug
            GameObject reportBug = mainMenuContent.transform.GetChild(5).gameObject;
            // Social media
            GameObject socialMedia = mainMenuContent.transform.GetChild(6).gameObject;
            // Last news and tips
            GameObject newsAndTips = mainMenuContent.transform.GetChild(2).gameObject;
            // Custom background
            Transform menuSceneParent = menuScene.transform.parent;
            for (int i = 0; i < menuSceneParent.childCount; i++)
            {
                menuSceneParent.GetChild(i).SetSiblingIndex(i + 1);
            }

            GameObject customBackground = new()
            {
                transform = {
                    parent = menuSceneParent,
                    localPosition = new Vector3(0, 0, 0),
                },
            };

            customBackground.transform.SetSiblingIndex(0);

            Image imageComponent = customBackground.AddComponent<Image>();
            LoadImageFromSkin imageFromSkin = customBackground.AddComponent<LoadImageFromSkin>();
            imageFromSkin.imageID = "menu/menu";
            imageComponent.rectTransform.sizeDelta = new Vector2(1920, 1080);

            ranksAndChallenges.SetActive(false);
            itemStore.SetActive(false);
            newsAndTips.SetActive(false);
            copyrightText.SetActive(false);
            reportBug.SetActive(false);
            socialMedia.SetActive(false);
        }
    }
}
