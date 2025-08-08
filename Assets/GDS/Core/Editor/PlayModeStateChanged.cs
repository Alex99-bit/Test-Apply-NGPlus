#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using GDS.Core.Events;

namespace GDS.Core.Editor {
#if UNITY_EDITOR
    //[InitializeOnLoad]
    public static class PlayModeStateChanged {

        static PlayModeStateChanged() {
            EditorApplication.playModeStateChanged += onPlayModeChange;
        }

        private static void onPlayModeChange(PlayModeStateChange state) {
            if (state == PlayModeStateChange.ExitingPlayMode) {
                EventBus.Global.Publish(new Reset());
            }
        }
    }
#endif
}
