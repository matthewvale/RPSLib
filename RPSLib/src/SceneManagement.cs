/// ------------------------------
/// Original Author: Matthew Vale
/// ------------------------------

using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace RPSLib
{

    /// <summary>
    /// Various methods to handle scene loading.
    /// </summary>
    public class SceneManagement
    {

        private static Stack<string> _sceneStack = new Stack<string>();
        private static string _targetScene = string.Empty;

        /// <summary>
        /// Load a given scene and optionally set it active.
        /// </summary>
        public static void LoadScene(string sceneName, LoadSceneMode loadMode, bool setActive = true)
        {
            try
            {
                SceneManager.LoadScene(sceneName, loadMode);
                if (setActive)
                {
                    _targetScene = sceneName;
                    SceneManager.sceneLoaded += OnSceneLoaded;
                }
                else
                {
                    ClearSceneStack();
                }
            }
            catch
            {
                Debug.Log("ActiveSceneHandler :: Could not load scene: " + sceneName + ". Does not exist, or is not in build settings.", Debug.Style.CriticalError);
                return;
            }
        }

        /// <summary>
        /// Sets a specific scene active. Determines the lighting and which scene new Objects are spawned in.
        /// </summary>
        public static void SetSceneActive(string sceneName)
        {
            try
            {
                SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneName));
                AddToSceneStack(sceneName);
            }
            catch
            {
                Debug.Log("ActiveSceneHandler :: Could not set active scene: " + sceneName + ". Does not exist, or is not in build settings.", Debug.Style.CriticalError);
            }
        }

        /// <summary>
        /// Unload the scene from memory and destroy all GameObjects in that scene.
        /// </summary>
        public static void UnloadScene(string sceneName)
        {
            try
            {
                SceneManager.UnloadSceneAsync(sceneName);
            }
            catch
            {
                Debug.Log("ActiveSceneHandler :: Could not unload scene: " + sceneName + ". Does not exist, or is not in build settings.", Debug.Style.CriticalError);
            }
        }

        /// <summary>
        /// Navigate back to the previously loaded scene and optionally unload this one.
        /// </summary>
        public static void NavigateSceneBack(bool unload)
        {
            if (_sceneStack == null || _sceneStack.Count == 0)
            {
                Debug.Log("SceneManagement :: Scene Stack is null or 0, no scene to go back to.", Debug.Style.Warning);
                return;
            }

            if (_sceneStack != null && _sceneStack.Count <= 1)
            {
                Debug.Log("SceneManagement :: Only 1 scene in the stack, nothing to go back to.", Debug.Style.Warning);
                return;
            }

            // Set the previous scene as the active scene
            if (_sceneStack != null && _sceneStack.Count >= 2)
            {
                string previousSceneToLoad = _sceneStack.First();
                SetSceneActive(previousSceneToLoad);
            }

            // Unload the current scene
            if (unload && _targetScene != null)
            {
                UnloadScene(_targetScene);
            }

            // Pop the unloaded scene
            RemoveCurrentSceneFromStack();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SetSceneActive(_targetScene);
        }

        private static void AddToSceneStack(string scene)
        {
            if (_sceneStack.Contains(scene))
            {
                return;
            }

            _sceneStack.Push(scene);

            //DebugStackState();
        }

        private static void RemoveCurrentSceneFromStack()
        {
            Debug.Log($"Removing {_sceneStack.Last()} from the Stack");
            _sceneStack.Pop();

            DebugStackState();
        }

        private static void ClearSceneStack()
        {
            _sceneStack.Clear();
        }

        private static void DebugStackState()
        {
            string stackString = "Order of Latest > Oldest: ";
            for (int i = 0; i < _sceneStack.Count; i++)
            {
                stackString += _sceneStack.ElementAt(i) + ", ";
            }
            Debug.Log("SceneStack :: " + stackString);
        }

    }

}