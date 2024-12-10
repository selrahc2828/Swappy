using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{

#region InitPlayer
    public enum SoundPlayer
        {
            slowTime,
            unslowTime,
            stealComp,
            giveComp,
            projectionEnter,
            projectionStay,
            projectionExit
        }

    private EventInstance _slowTime;
    private EventInstance _unslowTime;
    private EventInstance _stealComp;
    private EventInstance _giveComp;
    private EventInstance _projectionEnter;
    private EventInstance _projectionStay;
    private EventInstance _projectionExit;
#endregion

#region Start()
    void Start()
    {
        _slowTime = RuntimeManager.CreateInstance("event:/Player/Time/slowTime");
        _unslowTime = RuntimeManager.CreateInstance("event:/Player/Time/unslowTime");
        _stealComp = RuntimeManager.CreateInstance("event:/Player/Comp/stealComp");
        _giveComp = RuntimeManager.CreateInstance("event:/Player/Comp/giveComp");
        _projectionEnter = RuntimeManager.CreateInstance("event:/Player/Projection/projectionEnter");
        _projectionStay = RuntimeManager.CreateInstance("event:/Player/Projection/projectionStay");
        _projectionExit = RuntimeManager.CreateInstance("event:/Player/Projection/projectionExit");
    }
#endregion

#region Son Player      PlayPlayerSound()

    //  PlayPlayerSound() est à utiliser lorsqu'un des son du joueur doit etre joué.
    //
    //  Tout les sons à appeler ici sont des sons finis, donc il ne doivent pas se joué en boucle.
    //  Si c'est le cas, aller voir dans la parti debug, plus bas.
    //
    //  Si certain sont ne se lance pas essayez d'utiliser les RuntimeManager.PlayOneShot().    (pensez, dans ce cas à retirer les DEBUG et à me le dire.)
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

    public void PlayPlayerSound(SoundPlayer soundPlayer, Vector3 position=default)
    {
        switch (soundPlayer)
        {
            case SoundPlayer.slowTime:
                _slowTime.set3DAttributes(RuntimeUtils.To3DAttributes(position));
                _slowTime.start();          //RuntimeManager.PlayOneShot("event:/Player/Time/slowTime");                    DEBUG
                break;
            case SoundPlayer.unslowTime:
                _unslowTime.start();        //RuntimeManager.PlayOneShot("event:/Player/Time/unslowTime");                  DEBUG
                break;
            case SoundPlayer.stealComp:
                _stealComp.start();         //RuntimeManager.PlayOneShot("event:/Player/Comp/stealComp");                   DEBUG
                break;
            case SoundPlayer.giveComp:
                _giveComp.start();          //RuntimeManager.PlayOneShot("event:/Player/Comp/giveComp");                    DEBUG
                break;
            case SoundPlayer.projectionEnter:
                _projectionEnter.start();   //RuntimeManager.PlayOneShot("event:/Player/Projection/projectionEnter");       DEBUG
                break;
            case SoundPlayer.projectionStay:
                _projectionStay.start();    //RuntimeManager.PlayOneShot("event:/Player/Projection/projectionStay");        DEBUG
                break;
            case SoundPlayer.projectionExit:
                _projectionExit.start();    //RuntimeManager.PlayOneShot("event:/Player¨/Projection/projectionExit");       DEBUG
                break;
            default:
                UnityEngine.Debug.LogError("PlayPlayerSound, Argument manquant : Checkez la liste");
                break;
        }
    }
    #endregion
#region Son Player      Debug 

    //  StopPlayerSound() n'est pas à utiliser dans les script, si il est necessaire pour que ça marche c'est que ya un problème.
    // 
    //  La fonction peut s'appeler avec une surchager bool, pour determiner si le son doit s'arreter d'un coup (true) ou plus lentement(false), par default c'est false.
    //  Si vous avez besoin de ça dites le moi, parceque vous ne devriez pas.
    //
    //  Sons disponible à ajouter dans le code, si necessaire:
    //  -   slowTime            : StopPlayerSound(SoundPlayer.slowTime)                 ()
    //  -   unslowTime          : StopPlayerSound(SoundPlayer.unslowTime)               ()
    //  -   stealComp           : StopPlayerSound(SoundPlayer.stealComp)                ()
    //  -   giveComp            : StopPlayerSound(SoundPlayer.giveComp)                 ()
    //  -   projectionEnter     : StopPlayerSound(SoundPlayer.projectionEnter)          ()
    //  -   projectionStay      : StopPlayerSound(SoundPlayer.projectionStay)           ()
    //  -   projectionExit      : StopPlayerSound(SoundPlayer.projectionExit)           ()
    //
    //      Annotez si les sons sont ajouté au code. (nom du script + line)

    public void StopPlayerSound(SoundPlayer soundPlayer,bool stopMode = false)
    {
        if (stopMode)
        {
            switch (soundPlayer)
            {
                case SoundPlayer.slowTime:
                    _slowTime.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                case SoundPlayer.unslowTime:
                    _unslowTime.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                case SoundPlayer.stealComp:
                    _stealComp.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                case SoundPlayer.giveComp:
                    _giveComp.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                case SoundPlayer.projectionEnter:
                    _projectionEnter.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                case SoundPlayer.projectionStay:
                    _projectionStay.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                case SoundPlayer.projectionExit:
                    _projectionExit.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
                    break;
                default:
                    UnityEngine.Debug.LogError("PlayPlayerSound, Argument manquant : Checkez la liste");
                    break;
            }
        }
        else
        {
            switch (soundPlayer)
            {
                case SoundPlayer.slowTime:
                    _slowTime.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                case SoundPlayer.unslowTime:
                    _unslowTime.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                case SoundPlayer.stealComp:
                    _stealComp.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                case SoundPlayer.giveComp:
                    _giveComp.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                case SoundPlayer.projectionEnter:
                    _projectionEnter.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                case SoundPlayer.projectionStay:
                    _projectionStay.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                case SoundPlayer.projectionExit:
                    _projectionExit.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
                    break;
                default:
                    UnityEngine.Debug.LogError("PlayPlayerSound, Argument manquant : Checkez la liste");
                    break;
            }
        }
    }
#endregion


}
