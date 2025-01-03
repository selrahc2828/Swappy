//--------------------------------------------------------------------
// Ce script Unity illustre l'utilisation des "Programmer Sounds" et 
// de la table audio dans FMOD pour jouer dynamiquement des sons.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices; // Nécessaire pour travailler avec des pointeurs et la mémoire gérée.

class ScriptUsageProgrammerSounds : MonoBehaviour
{
    // Déclaration d'une variable pour stocker un callback d'événement FMOD.
    FMOD.Studio.EVENT_CALLBACK dialogueCallback;

    // Référence à l'événement FMOD défini dans le projet (chemin vers l'événement dans FMOD Studio).
    public FMODUnity.EventReference EventName;

#if UNITY_EDITOR
    // Cette méthode est appelée automatiquement dans l'éditeur Unity lorsque le script est réinitialisé.
    // Ici, elle initialise la référence de l'événement.
    void Reset()
    {
        EventName = FMODUnity.EventReference.Find("event:/Character/Radio/Command");
    }
#endif

    // Méthode appelée au démarrage du script.
    void Start()
    {
        // Crée explicitement un délégué pour le callback et le stocke afin d'éviter 
        // qu'il ne soit libéré par le garbage collector pendant son utilisation.
        dialogueCallback = new FMOD.Studio.EVENT_CALLBACK(DialogueEventCallback);
    }

    // Fonction pour jouer un dialogue en utilisant une clé spécifique.
    void PlayDialogue(string key)
    {
        // Crée une nouvelle instance de l'événement FMOD spécifié.
        var dialogueInstance = FMODUnity.RuntimeManager.CreateInstance(EventName);

        // Verrouille la clé en mémoire (empêche le garbage collector de la déplacer) et
        // passe un pointeur vers cette clé en tant que "user data" à l'instance de l'événement.
        GCHandle stringHandle = GCHandle.Alloc(key);
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        // Définit le callback à appeler pour cet événement.
        dialogueInstance.setCallback(dialogueCallback);

        // Démarre l'événement (joue le son).
        dialogueInstance.start();

        // Libère l'instance après démarrage, car elle est maintenant gérée par FMOD.
        dialogueInstance.release();
    }

    // Callback statique appelé par FMOD pour gérer les sons programmés.
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        // Récupère l'instance d'événement associée au pointeur donné.
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // Récupère les données utilisateur associées à cet événement (pointeur vers la clé).
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);

        // Convertit le pointeur en un objet C# (ici une clé sous forme de chaîne).
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        string key = stringHandle.Target as string;

        // Gère les différents types de callbacks FMOD.
        switch (type)
        {
            // Cas où un son doit être créé dynamiquement.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                {
                    // Définit les modes pour créer un son (par ex., en boucle, non bloquant).
                    FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;

                    // Récupère les paramètres du son à partir du pointeur fourni.
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                    // Vérifie si la clé contient une extension de fichier (exemple : "audio.mp3").
                    if (key.Contains("."))
                    {
                        // Crée un son à partir d'un fichier dans le dossier StreamingAssets.
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);

                        if (soundResult == FMOD.RESULT.OK)
                        {
                            // Associe le son à l'événement et met à jour les paramètres.
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = -1; // Pas de sous-son utilisé ici.
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    else
                    {
                        // Cas où la clé fait référence à une entrée dans la table audio.
                        FMOD.Studio.SOUND_INFO dialogueSoundInfo;
                        var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);

                        if (keyResult != FMOD.RESULT.OK)
                        {
                            break; // Quitte si la clé est invalide.
                        }

                        // Crée un son à partir des informations de la table audio.
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);

                        if (soundResult == FMOD.RESULT.OK)
                        {
                            // Associe le son et ses informations à l'événement.
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    break;
                }

            // Cas où un son programmé doit être détruit.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release(); // Libère les ressources associées au son.
                    break;
                }

            // Cas où l'événement est détruit : libère la mémoire associée à la clé.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    stringHandle.Free(); // Libère la mémoire verrouillée pour la clé.
                    break;
                }
        }

        // Retourne le résultat OK pour indiquer que le callback a réussi.
        return FMOD.RESULT.OK;
    }

    // Méthode appelée chaque frame.
    void Update()
    {
        // Joue un dialogue en fonction de la touche pressée.
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayDialogue("640148main_APU Shutdown"); // Dialogue 1.
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayDialogue("640165main_Lookin At It"); // Dialogue 2.
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayDialogue("640169main_Press to ATO"); // Dialogue 3.
        }
    }
}
