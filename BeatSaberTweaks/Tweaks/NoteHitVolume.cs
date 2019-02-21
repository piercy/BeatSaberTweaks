using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IllusionPlugin;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using System.Collections;
using CustomUI.BeatSaber;

namespace BeatSaberTweaks
{
    public class NoteHitVolume : MonoBehaviour
    {
        public static NoteHitVolume Instance;

        const string goodCutString = "_goodCutVolume";
        const string badCutString = "_badCutVolume";

        public static void OnLoad(Transform parent)
        {
            if (Instance != null) return;
            new GameObject("Note Volume").AddComponent<NoteHitVolume>().transform.parent = parent;
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

        private void SceneManagerOnActiveSceneChanged(Scene arg0, Scene scene)
        {
            try
            {
                if (SceneUtils.isGameScene(scene))
                {

                    var pauseMenuManager = Resources.FindObjectsOfTypeAll<PauseMenuManager>().First();
                    if (pauseMenuManager != null)
                    {
                        var continueButton = ReflectionUtil.GetPrivateField<Button>(pauseMenuManager, "_continueButton");

                        var decrementButton = Instantiate(continueButton, continueButton.transform.parent);
                        decrementButton.onClick.AddListener(decrementVolume_Click);
                        decrementButton.SetButtonText("Volume--");

                        var incrementButton = Instantiate(continueButton, continueButton.transform.parent);
                        incrementButton.onClick.AddListener(incrementVolume_Click);
                        incrementButton.SetButtonText("Volume++");

                        continueButton.onClick.AddListener(continueButton_clicked);
                    }
                    StartCoroutine(WaitForLoad());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Tweaks (NoteVolume) done fucked up: " + e);
            }
        }

        private void incrementVolume_Click()
        {
            Settings.NoteHitVolume = Settings.NoteHitVolume + 0.1f;
            Settings.NoteMissVolume = Settings.NoteHitVolume + 0.1f;

            if (Settings.NoteHitVolume > 1f)
                Settings.NoteHitVolume = 1f;

            Settings.NoteMissVolume = Settings.NoteMissVolume + 0.1f;
            if (Settings.NoteMissVolume > 1f)
                Settings.NoteMissVolume = 1f;
        }
        private void decrementVolume_Click()
        {
            Settings.NoteHitVolume = Settings.NoteHitVolume - 0.1f;
            Settings.NoteMissVolume = Settings.NoteHitVolume - 0.1f;

            if (Settings.NoteHitVolume < 0f)
                Settings.NoteHitVolume = 0f;


            Settings.NoteMissVolume = Settings.NoteMissVolume + 0.1f;
            if (Settings.NoteMissVolume < 0f)
                Settings.NoteMissVolume = 0f;
        }
        private void continueButton_clicked()
        {
            // just call existing BeastSaberTweaks method
            LoadingDidFinishEvent();
        }

        private IEnumerator WaitForLoad()
        {
            bool loaded = false;
            while (!loaded)
            {
                var resultsViewController = Resources.FindObjectsOfTypeAll<ResultsViewController>().FirstOrDefault();

                if (resultsViewController == null)
                {
                    Plugin.Log("resultsViewController is null!", Plugin.LogLevel.DebugOnly);
                    yield return new WaitForSeconds(0.01f);
                }
                else
                {
                    Plugin.Log("Found resultsViewController!", Plugin.LogLevel.DebugOnly);
                    loaded = true;
                }
            }
            LoadingDidFinishEvent();
        }

        private void LoadingDidFinishEvent()
        {
            try
            {
                var pool = Resources.FindObjectsOfTypeAll<NoteCutSoundEffect>();
                foreach (var effect in pool)
                {
                    /*
                    if (normalVolume == 0)
                    {
                        normalVolume = ReflectionUtil.GetPrivateField<float>(effect, goodCutString);
                        normalMissVolume = ReflectionUtil.GetPrivateField<float>(effect, badCutString);

                        Plugin.Log("Normal hit volumes =" + normalVolume, Plugin.LogLevel.DebugOnly);
                        Plugin.Log("Normal miss volumes =" + normalMissVolume, Plugin.LogLevel.DebugOnly);
                    }
                    */
                    ReflectionUtil.SetPrivateField(effect, goodCutString, Settings.NoteHitVolume);
                    ReflectionUtil.SetPrivateField(effect, badCutString, Settings.NoteMissVolume);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
