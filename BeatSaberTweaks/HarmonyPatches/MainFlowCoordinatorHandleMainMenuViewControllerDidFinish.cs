using Harmony;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRUI;
using BS_Utils;
using UnityEngine;
using System.Reflection;

namespace BeatSaberTweaks.HarmonyPatches
{
    [HarmonyPatch(typeof(MainFlowCoordinator))]
    [HarmonyPatch("HandleMainMenuViewControllerDidFinish")]
    [HarmonyPatch(new Type[] { typeof(MainMenuViewController), typeof(MainMenuViewController.MenuButton) })]
    class MainFlowCoordinatorHandleMainMenuViewControllerDidFinish
    {
        protected static MainFlowCoordinator instance = null;
        protected static Traverse instanceForReflection = null;

        static bool Prefix(MainFlowCoordinator __instance, MainMenuViewController viewController, MainMenuViewController.MenuButton subMenuType)
        {

            // This is mostly an exact copy of the original function
            // The only difference is the quit button

            instance = __instance;
            instanceForReflection = Traverse.Create(__instance);

            switch (subMenuType)
            {
                case MainMenuViewController.MenuButton.SoloFreePlay:
                    //this.PresentFlowCoordinatorOrAskForTutorial(this._soloFreePlayFlowCoordinator);
                    __instance.PresentFlowCoordinatorOrAskForTutorial(instanceForReflection.Field("_soloFreePlayFlowCoordinator").GetValue<SoloFreePlayFlowCoordinator>());
                    break;
                case MainMenuViewController.MenuButton.Party:
                    //this.PresentFlowCoordinatorOrAskForTutorial(this._partyFreePlayFlowCoordinator);
                    __instance.PresentFlowCoordinatorOrAskForTutorial(instanceForReflection.Field("_partyFreePlayFlowCoordinator").GetValue<PartyFreePlayFlowCoordinator>());
                    break;
                case MainMenuViewController.MenuButton.Settings:
                    //base.PresentFlowCoordinator(this._settingsFlowCoordinator, null, false, false);
                    var _settingsFlowCoordinator = instanceForReflection.Field("_settingsFlowCoordinator").GetValue<SettingsFlowCoordinator>();
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "PresentFlowCoordinator", new object[] { _settingsFlowCoordinator, null, false, false });
                    break;
                case MainMenuViewController.MenuButton.PlayerSettings:
                    //base.SetRightScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetRightScreenViewController", new object[] { null, false });

                    //base.SetLeftScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetLeftScreenViewController", new object[] { null, false });

                    //base.PresentViewController(__instance._playerSettingsViewController, null, false);
                    var _playerSettingsViewController = instanceForReflection.Field("_playerSettingsViewController").GetValue<PlayerSettingsViewController>();
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "PresentViewController", new object[] { _playerSettingsViewController, null, false });

                    break;
                case MainMenuViewController.MenuButton.FloorAdjust:
                    //base.SetRightScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetRightScreenViewController", new object[] { null, false });

                    //base.SetLeftScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetLeftScreenViewController", new object[] { null, false });

                    //base.PresentViewController(__instance._floorAdjustViewController, null, false);
                    var _floorAdjustViewController = instanceForReflection.Field("_floorAdjustViewController").GetValue<FloorAdjustViewController>();
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "PresentViewController", new object[] { _floorAdjustViewController, null, false });
                    break;
                case MainMenuViewController.MenuButton.HowToPlay:
                    //base.SetRightScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetRightScreenViewController", new object[] { null, false });

                    //base.SetLeftScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetLeftScreenViewController", new object[] { null, false });

                    //base.PresentViewController(__instance._howToPlayViewController, null, false);
                    var _howToPlayViewController = instanceForReflection.Field("_howToPlayViewController").GetValue<HowToPlayViewController>();
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "PresentViewController", new object[] { _howToPlayViewController, null, false });

                    break;
                case MainMenuViewController.MenuButton.Credits:
                    //this._menuSceneSetupData.ShowCredits(null, null);
                    var _menuSceneSetupData = instanceForReflection.Field("_menuSceneSetupData").GetValue<MenuSceneSetupDataSO>();
                    _menuSceneSetupData.ShowCredits(null, null);
                    break;
                case MainMenuViewController.MenuButton.Quit:
                    //base.SetRightScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetRightScreenViewController", new object[] { null, false });

                    //base.SetLeftScreenViewController(null, false);
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "SetLeftScreenViewController", new object[] { null, false });

                    // This probably isn't going to work

                    // Unsubscribe the tutorial event

                    var _simpleDialogPromptViewController = instanceForReflection.Field("_simpleDialogPromptViewController").GetValue<SimpleDialogPromptViewController>();

                    /*
                    try
                    {
                        var instanceType = __instance.GetType();

                        if (instanceType == null)
                        {
                            Console.WriteLine("instanceType is null!");
                            break;
                        }
                        try
                        {
                            var instanceMethod = instanceType.GetMethod("HandleSimpleDialogPromptViewControllerDidFinish", BindingFlags.Instance | BindingFlags.Public);

                            if (instanceMethod == null)
                            {
                                Console.WriteLine("instanceMethod is null!");
                                break;
                            }
                            try
                            {
                                var instanceDelegate = instanceMethod.CreateDelegate(typeof(Action<SimpleDialogPromptViewController, bool>), __instance);
                                
                                try
                                {
                                    var HandleSimpleDialogPromptViewControllerDidFinish = (Action<SimpleDialogPromptViewController, bool>)instanceDelegate;
                                }
                                catch (Exception e)
                                {
                                    Console.WriteLine("HandleSimpleDialogPromptViewControllerDidFinish");
                                    Console.WriteLine(e);
                                }
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("instanceDelegate");
                                Console.WriteLine(e);
                            }
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("instanceMethod");
                            Console.WriteLine(e);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("instanceType");
                        Console.WriteLine(e);
                    }
                    */

                    var HandleSimpleDialogPromptViewControllerDidFinish =
                        (Action<SimpleDialogPromptViewController, bool>)__instance.GetType()
                        .GetMethod("HandleSimpleDialogPromptViewControllerDidFinish", BindingFlags.Instance | BindingFlags.Public)
                        .CreateDelegate(typeof(Action<SimpleDialogPromptViewController, bool>), __instance);

                    _simpleDialogPromptViewController.didFinishEvent -= HandleSimpleDialogPromptViewControllerDidFinish;

                    _simpleDialogPromptViewController.didFinishEvent += QuitApp;

                    _simpleDialogPromptViewController.Init("Quit", "Are you sure you want to quit?", "Quit", "Cancel");
                    BS_Utils.Utilities.ReflectionUtil.InvokeMethod(__instance, "PresentViewController", new object[] { _simpleDialogPromptViewController, null, false });
                    break;
            }

            // Return false to tell Harmony not to run the original function as well
            return false;
        }

        public static void QuitApp(SimpleDialogPromptViewController arg1, bool result)
        {
            Plugin.Log("QuitApp result:" + result, Plugin.LogLevel.DebugOnly);
            if (result)
            {
                Application.Quit();
            }
            else
            {
                if (instance == null)
                {
                    Plugin.Log("instance is null in QuitApp! This shouldn't happen!", Plugin.LogLevel.Error);
                    return;
                }

                var _mainMenuViewController = instanceForReflection.Field("_mainMenuViewController").GetValue<MainMenuViewController>();
                var _playerStatisticsViewController = instanceForReflection.Field("_playerStatisticsViewController").GetValue<PlayerStatisticsViewController>();
                var _releaseInfoViewController = instanceForReflection.Field("_releaseInfoViewController").GetValue<ReleaseInfoViewController>();

                BS_Utils.Utilities.ReflectionUtil.InvokeMethod(instance, "PresentViewController", new object[] { _mainMenuViewController, null, false });
                BS_Utils.Utilities.ReflectionUtil.InvokeMethod(instance, "SetRightScreenViewController", new object[] { _playerStatisticsViewController, false });
                BS_Utils.Utilities.ReflectionUtil.InvokeMethod(instance, "SetLeftScreenViewController", new object[] { _releaseInfoViewController, false });
            }

        }
    }
}
