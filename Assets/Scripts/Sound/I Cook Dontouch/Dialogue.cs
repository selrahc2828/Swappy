//--------------------------------------------------------------------
// Ce script Unity illustre l'utilisation des "Programmer Sounds" et 
// de la table audio dans FMOD pour jouer dynamiquement des sons.
//--------------------------------------------------------------------

using System;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices; // N�cessaire pour travailler avec des pointeurs et la m�moire g�r�e.

class ScriptUsageProgrammerSounds : MonoBehaviour
{
    // D�claration d'une variable pour stocker un callback d'�v�nement FMOD.
    FMOD.Studio.EVENT_CALLBACK dialogueCallback;

    // R�f�rence � l'�v�nement FMOD d�fini dans le projet (chemin vers l'�v�nement dans FMOD Studio).
    public FMODUnity.EventReference EventName;

#if UNITY_EDITOR
    // Cette m�thode est appel�e automatiquement dans l'�diteur Unity lorsque le script est r�initialis�.
    // Ici, elle initialise la r�f�rence de l'�v�nement.
    void Reset()
    {
        EventName = FMODUnity.EventReference.Find("event:/Character/Radio/Command");
    }
#endif

    // M�thode appel�e au d�marrage du script.
    void Start()
    {
        // Cr�e explicitement un d�l�gu� pour le callback et le stocke afin d'�viter 
        // qu'il ne soit lib�r� par le garbage collector pendant son utilisation.
        dialogueCallback = new FMOD.Studio.EVENT_CALLBACK(DialogueEventCallback);
    }

    // Fonction pour jouer un dialogue en utilisant une cl� sp�cifique.
    void PlayDialogue(string key)
    {
        // Cr�e une nouvelle instance de l'�v�nement FMOD sp�cifi�.
        var dialogueInstance = FMODUnity.RuntimeManager.CreateInstance(EventName);

        // Verrouille la cl� en m�moire (emp�che le garbage collector de la d�placer) et
        // passe un pointeur vers cette cl� en tant que "user data" � l'instance de l'�v�nement.
        GCHandle stringHandle = GCHandle.Alloc(key);
        dialogueInstance.setUserData(GCHandle.ToIntPtr(stringHandle));

        // D�finit le callback � appeler pour cet �v�nement.
        dialogueInstance.setCallback(dialogueCallback);

        // D�marre l'�v�nement (joue le son).
        dialogueInstance.start();

        // Lib�re l'instance apr�s d�marrage, car elle est maintenant g�r�e par FMOD.
        dialogueInstance.release();
    }

    // Callback statique appel� par FMOD pour g�rer les sons programm�s.
    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT DialogueEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, IntPtr instancePtr, IntPtr parameterPtr)
    {
        // R�cup�re l'instance d'�v�nement associ�e au pointeur donn�.
        FMOD.Studio.EventInstance instance = new FMOD.Studio.EventInstance(instancePtr);

        // R�cup�re les donn�es utilisateur associ�es � cet �v�nement (pointeur vers la cl�).
        IntPtr stringPtr;
        instance.getUserData(out stringPtr);

        // Convertit le pointeur en un objet C# (ici une cl� sous forme de cha�ne).
        GCHandle stringHandle = GCHandle.FromIntPtr(stringPtr);
        string key = stringHandle.Target as string;

        // G�re les diff�rents types de callbacks FMOD.
        switch (type)
        {
            // Cas o� un son doit �tre cr�� dynamiquement.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.CREATE_PROGRAMMER_SOUND:
                {
                    // D�finit les modes pour cr�er un son (par ex., en boucle, non bloquant).
                    FMOD.MODE soundMode = FMOD.MODE.LOOP_NORMAL | FMOD.MODE.CREATECOMPRESSEDSAMPLE | FMOD.MODE.NONBLOCKING;

                    // R�cup�re les param�tres du son � partir du pointeur fourni.
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));

                    // V�rifie si la cl� contient une extension de fichier (exemple : "audio.mp3").
                    if (key.Contains("."))
                    {
                        // Cr�e un son � partir d'un fichier dans le dossier StreamingAssets.
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(Application.streamingAssetsPath + "/" + key, soundMode, out dialogueSound);

                        if (soundResult == FMOD.RESULT.OK)
                        {
                            // Associe le son � l'�v�nement et met � jour les param�tres.
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = -1; // Pas de sous-son utilis� ici.
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    else
                    {
                        // Cas o� la cl� fait r�f�rence � une entr�e dans la table audio.
                        FMOD.Studio.SOUND_INFO dialogueSoundInfo;
                        var keyResult = FMODUnity.RuntimeManager.StudioSystem.getSoundInfo(key, out dialogueSoundInfo);

                        if (keyResult != FMOD.RESULT.OK)
                        {
                            break; // Quitte si la cl� est invalide.
                        }

                        // Cr�e un son � partir des informations de la table audio.
                        FMOD.Sound dialogueSound;
                        var soundResult = FMODUnity.RuntimeManager.CoreSystem.createSound(dialogueSoundInfo.name_or_data, soundMode | dialogueSoundInfo.mode, ref dialogueSoundInfo.exinfo, out dialogueSound);

                        if (soundResult == FMOD.RESULT.OK)
                        {
                            // Associe le son et ses informations � l'�v�nement.
                            parameter.sound = dialogueSound.handle;
                            parameter.subsoundIndex = dialogueSoundInfo.subsoundindex;
                            Marshal.StructureToPtr(parameter, parameterPtr, false);
                        }
                    }
                    break;
                }

            // Cas o� un son programm� doit �tre d�truit.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROY_PROGRAMMER_SOUND:
                {
                    var parameter = (FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.PROGRAMMER_SOUND_PROPERTIES));
                    var sound = new FMOD.Sound(parameter.sound);
                    sound.release(); // Lib�re les ressources associ�es au son.
                    break;
                }

            // Cas o� l'�v�nement est d�truit : lib�re la m�moire associ�e � la cl�.
            case FMOD.Studio.EVENT_CALLBACK_TYPE.DESTROYED:
                {
                    stringHandle.Free(); // Lib�re la m�moire verrouill�e pour la cl�.
                    break;
                }
        }

        // Retourne le r�sultat OK pour indiquer que le callback a r�ussi.
        return FMOD.RESULT.OK;
    }

    // M�thode appel�e chaque frame.
    void Update()
    {
        // Joue un dialogue en fonction de la touche press�e.
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
