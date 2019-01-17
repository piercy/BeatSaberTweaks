﻿using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace BeatSaberTweaks
{
    public class IngameTimeSpentClock : MonoBehaviour
    {
        public static IngameTimeSpentClock Instance;

        private static GameObject _IngameTimeSpentClockCanvas = null;
        private static TextMeshProUGUI _Text = null;
        private static Vector3 _TimePos;
        private static Quaternion _TimeRot;
        private static float _TimeSize;
        private static bool _HideWhilePlaying;
        private static String _MessageTemplate;

        private TimeSpan _TimeSpent;
        private Coroutine _CUpdateIngameTimeSpentClock;

        private bool _IsPlayerIngame;

        public static void OnLoad(Transform parent)
        {
            if (Instance != null) return;
            Plugin.Log("Creating IngameTimeSpentClock.", Plugin.LogLevel.DebugOnly);
            new GameObject("IngameTimeSpentClock").AddComponent<IngameTimeSpentClock>().transform.parent = parent;
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Plugin.Log("IngameTimeSpentClock awake.", Plugin.LogLevel.DebugOnly);
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
                _TimePos = Settings.IngameTimeSpentClockPosition;
                _TimeRot = Settings.IngameTimeSpentClockRotation;
                _TimeSize = Settings.IngameTimeSpentClockFontSize;
                _HideWhilePlaying = Settings.HideIngameTimeSpentClockIngame;
                _MessageTemplate = Settings.IngameTimeSpentClockMessageTemplate;
                _TimeSpent = new TimeSpan(0);
                _IsPlayerIngame = false;
            }
            else
                Destroy(this);
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            Plugin.Log("IngameTimeSpentClock SceneManagerOnActiveSceneChanged: " + arg0.name + " " + scene.name, Plugin.LogLevel.DebugOnly);
            try
            {
                if (SceneUtils.isMenuScene(scene) && _IngameTimeSpentClockCanvas == null)
                {
                    Plugin.Log("Creating the IngameTimeSpentClock object... ", Plugin.LogLevel.DebugOnly);
                    _IngameTimeSpentClockCanvas = new GameObject();
                    DontDestroyOnLoad(_IngameTimeSpentClockCanvas);
                    _IngameTimeSpentClockCanvas.AddComponent<Canvas>();

                    _IngameTimeSpentClockCanvas.name = "IngameTimeSpentClock Canvas";
                    _IngameTimeSpentClockCanvas.transform.position = _TimePos;
                    _IngameTimeSpentClockCanvas.transform.rotation = _TimeRot;
                    _IngameTimeSpentClockCanvas.transform.localScale = new Vector3(0.02f, 0.02f, 1.0f);

                    var textGO = new GameObject();
                    textGO.transform.SetParent(_IngameTimeSpentClockCanvas.transform);
                    textGO.transform.localPosition = Vector3.zero;
                    textGO.transform.localRotation = Quaternion.identity;
                    textGO.transform.localScale = Vector3.one;

                    _Text = textGO.AddComponent<TextMeshProUGUI>();
                    _Text.name = "IngameTimeSpentClock Text";
                    _Text.alignment = TextAlignmentOptions.Center;
                    _Text.fontSize = _TimeSize;
                    _Text.text = "You didn't play yet during this session.";

                    _IngameTimeSpentClockCanvas.SetActive(Settings.ShowIngameTimeSpentClock);
                }
            }
            catch (Exception e)
            {
                Plugin.Log("IngameTimeSpentClock error: " + e, Plugin.LogLevel.DebugOnly);
            }

            if (_IngameTimeSpentClockCanvas != null)
            {
                _IsPlayerIngame = SceneUtils.isGameScene(scene);
                if (_HideWhilePlaying)
                    _IngameTimeSpentClockCanvas.SetActive(!_IsPlayerIngame);

                if (SceneUtils.isGameScene(scene))
                {
                    if (_CUpdateIngameTimeSpentClock != null)
                        StopCoroutine(_CUpdateIngameTimeSpentClock);
                    _CUpdateIngameTimeSpentClock = StartCoroutine(UpdateIngameTimeSpentClock());
                }
                else if (SceneUtils.isMenuScene(scene))
                {
                    if (_CUpdateIngameTimeSpentClock != null)
                    {
                        StopCoroutine(_CUpdateIngameTimeSpentClock);
                        _CUpdateIngameTimeSpentClock = null;
                    }
                }
            }
        }

        public void Update()
        {
            if (_IngameTimeSpentClockCanvas != null && Settings.ShowIngameTimeSpentClock != _IngameTimeSpentClockCanvas.activeSelf && !_IsPlayerIngame)
                _IngameTimeSpentClockCanvas.SetActive(Settings.ShowIngameTimeSpentClock);
        }

        public IEnumerator UpdateIngameTimeSpentClock()
        {
            Plugin.Log("IngameTimeSpentClock UpdateIngameTimeSpentClock function called.", Plugin.LogLevel.DebugOnly);
            while (_Text != null)
            {
                _TimeSpent = _TimeSpent.Add(new TimeSpan(0, 0, 1));
                String timeDisplay;
                if (_TimeSpent.Hours == 0)
                {
                    if (_TimeSpent.Minutes == 0)
                        timeDisplay = string.Format("{0:00}s", _TimeSpent.Seconds);
                    else
                        timeDisplay = string.Format("{0:00}m {1:00}s", _TimeSpent.Minutes, _TimeSpent.Seconds);
                }
                else
                    timeDisplay = string.Format("{0:00}h {1:00}m {2:00}s", _TimeSpent.Hours, _TimeSpent.Minutes, _TimeSpent.Seconds);

                _Text.text = _MessageTemplate.Replace("%TIME%", timeDisplay);
                yield return new WaitForSeconds(1f);
            }
        }
    }
}
