using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{

#region Init Player
    private EventInstance sonPlayer;
    private RESULT testPlayer;
    public enum SoundPlayer {slowTime,   unslowTime,   stealComp,   giveComp,   projectionEnter,   projectionStay,   projectionExit}
    private string _slowTime = "event:/Player/Time/slowTime";
    private string _unslowTime = "event:/Player/Time/unslowTime";
    private string _stealComp = "event:/Player/Comp/stealComp";
    private string _giveComp = "event:/Player/Comp/giveComp";
    private string _projectionEnter = "event:/Player/Projection/projectionEnter";
    private string _projectionStay = "event:/Player/Projection/projectionStay";
    private string _projectionExit = "event:/Player/Projection/projectionExit";
    #endregion

#region Start()
    void Start()
    {
    }
#endregion

#region Son Player      PlayPlayerSound()

    //  PlayPlayerSound() est à utiliser lorsqu'un des son du joueur doit etre joué.
    //
    //  Tout les sons à appeler ici sont des sons finis, donc il ne doivent pas se joué en boucle.
    //  Si c'est le cas, aller voir dans la parti debug, plus bas.
    //
    //
    //  Sons disponible à ajouter dans le code:
    //  -   slowTime            : PlayPlayerSound(SoundPlayer.slowTime)                 ()
    //  -   unslowTime          : PlayPlayerSound(SoundPlayer.unslowTime)               ()
    //  -   stealComp           : PlayPlayerSound(SoundPlayer.stealComp)                ()
    //  -   giveComp            : PlayPlayerSound(SoundPlayer.giveComp)                 ()
    //  -   projectionEnter     : PlayPlayerSound(SoundPlayer.projectionEnter)          ()
    //  -   projectionStay      : PlayPlayerSound(SoundPlayer.projectionStay)           ()
    //  -   projectionExit      : PlayPlayerSound(SoundPlayer.projectionExit)           ()
    //
    //      Annotez quand les sons sont ajouté au code. (nom du script + line)

    public void PlayPlayerSound(SoundPlayer soundPlayer)
    {
        
        switch (soundPlayer)
        {
            case SoundPlayer.slowTime:
                sonPlayer = RuntimeManager.CreateInstance(_slowTime);
                break;
            case SoundPlayer.unslowTime:
                sonPlayer = RuntimeManager.CreateInstance(_unslowTime);
                break;
            case SoundPlayer.stealComp:
                sonPlayer = RuntimeManager.CreateInstance(_stealComp);
                break;
            case SoundPlayer.giveComp:
                sonPlayer = RuntimeManager.CreateInstance(_giveComp);
                break;
            case SoundPlayer.projectionEnter:
                sonPlayer = RuntimeManager.CreateInstance(_projectionEnter);
                break;
            case SoundPlayer.projectionStay:
                sonPlayer = RuntimeManager.CreateInstance(_projectionStay);
                break;
            case SoundPlayer.projectionExit:
                sonPlayer = RuntimeManager.CreateInstance(_projectionExit);
                break;
            default:
                UnityEngine.Debug.LogError("PlayPlayerSound, Argument manquant : Checkez la liste");
                break;
        }
        testPlayer = sonPlayer.start();
        if (testPlayer != RESULT.OK)
        {
            UnityEngine.Debug.LogError("PlayPlayerSound Error: Son non joué/manquant. ( Vous ne devriez au grand jamais voir cette erreur donc chill, mais dans le doute elle est là)");
        }
        sonPlayer.release();
    }
    #endregion



}
