﻿using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Reflection;

namespace BeatSaberTweaks
{
    class OneColour : MonoBehaviour
    {
        public static OneColour Instance;

        MainSettingsModel model;
        bool rumble = false;

        public static void OnLoad(Transform parent)
        {
            if (Instance != null) return;
            new GameObject("One Colour").AddComponent<OneColour>().transform.parent = parent;
        }

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                SceneManager.activeSceneChanged += SceneManagerOnActiveSceneChanged;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(this);
            }
        }

        public void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            if (scene.buildIndex == TweakManager.MainScene)
            {
                if (model == null)
                {
                    model = Resources.FindObjectsOfTypeAll<MainSettingsModel>().FirstOrDefault();
                    rumble = model.controllersRumbleEnabled;
                }
                model.controllersRumbleEnabled = rumble;
            }
            if (scene.buildIndex == TweakManager.GameScene && Settings.OneColour &&  TweakManager.IsPartyMode())
            {
                PlayerController _playerController = FindObjectOfType<PlayerController>();
                Saber left = _playerController.leftSaber;
                Saber right = _playerController.rightSaber;

                rumble = model.controllersRumbleEnabled;
                model.controllersRumbleEnabled = false;

                if (left != null && right != null)
                {
                    GameObject gameObject = Instantiate(right.gameObject);
                    Saber component = gameObject.GetComponent<Saber>();
                    gameObject.transform.parent = left.transform.parent;
                    gameObject.transform.localPosition = Vector3.zero;
                    gameObject.transform.localRotation = Quaternion.identity;
                    left.gameObject.SetActive(false);
                    ReflectionUtil.SetPrivateField(_playerController, "_leftSaber", component);
                    //var type = ReflectionUtil.GetPrivateField<SaberTypeObject>(right, "_saberType");
                    //ReflectionUtil.SetPrivateField(component, "_saberType", type);
                }
            }
        }
    } 
}
