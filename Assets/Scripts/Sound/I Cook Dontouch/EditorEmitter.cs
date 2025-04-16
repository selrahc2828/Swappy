// using System;
// using System.Collections;
// using System.Collections.Generic;
// using FMODUnity;
// using UnityEditor;
// using UnityEngine;
// using UnityEngine.Serialization;
//
// [CustomEditor(typeof(emitter))]
// public class EditorEmitter : Editor
// {
//         public override void OnInspectorGUI()
//         {
//             emitter script = (emitter)target;
//
//             
//              script.Action = (FMODMusicManager.MusicAction) EditorGUILayout.EnumPopup(script.Action, GUILayout.Width(100));
//
//
//             if (script.Action  == FMODMusicManager.MusicAction.None)
//             {
//                 
//             }
//             if (script.Action == FMODMusicManager.MusicAction.Play)
//             {
//                 script.musicInstance = 
//             }
//             if (script.Action  == FMODMusicManager.MusicAction.Stop)
//             {
//                 
//             }
//             if (script.Action  == FMODMusicManager.MusicAction.Switch)
//             {
//                 
//             }
//
//
//             // Pour sauvegarder les changements
//             if (GUI.changed)
//             {
//                 EditorUtility.SetDirty(target);
//             }
//         }
//     
//
// }
