using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{
    private SoundRef _SoundRef;

    #region Init Player
    public enum SoundPlayer { slowTime, unslowTime, steal, give, projectionEnter, projectionStay, projectionExit }
#endregion
    #region Init System
    public enum PlaceParamType { repulse, immuable, bounce, propeler, aimant }
    public enum PlaceParamOnWhatType  { onObject, onPlayer, onGrab }
    public enum SoundComp { repulseBoom,  immuableHit, bounceHit, propelerStart, aimantStart,repulseTimer, }
    #endregion

    private void Start()
    {
        _SoundRef = GetComponent<SoundRef>();
    }

    #region Son Player
    //                                                                              In Code
    //
    //  PlayPlayerSound() est � utiliser lorsqu'un des son du joueur doit etre jou�.
    //
    //  Sons disponible � ajouter dans le code:
    //  -   slowTime            : PlayPlayerSound(SoundPlayer.slowTime)                 ()
    //  -   unslowTime          : PlayPlayerSound(SoundPlayer.unslowTime)               ()
    //  -   stealComp           : PlayPlayerSound(SoundPlayer.stealComp)                ()
    //  -   giveComp            : PlayPlayerSound(SoundPlayer.giveComp)                 ()
    //  -   projectionEnter     : PlayPlayerSound(SoundPlayer.projectionEnter)          ()
    //  -   projectionStay      : PlayPlayerSound(SoundPlayer.projectionStay)           ()
    //  -   projectionExit      : PlayPlayerSound(SoundPlayer.projectionExit)           ()
    //
    //      Annotez quand les sons sont ajout� au code. (nom du script + line)
    public void PlaySoundPlayer(SoundPlayer soundPlayer = default)
    {
        switch (soundPlayer)
        {
            case SoundPlayer.slowTime:
                RuntimeManager.PlayOneShot(_SoundRef.slowTime);
                break;
            case SoundPlayer.unslowTime:
                RuntimeManager.PlayOneShot(_SoundRef.unslowTime);
                break;
            case SoundPlayer.steal:
                RuntimeManager.PlayOneShot(_SoundRef.steal);
                break;
            case SoundPlayer.give:
                RuntimeManager.PlayOneShot(_SoundRef.give);
                break;
            case SoundPlayer.projectionEnter:
                RuntimeManager.PlayOneShot(_SoundRef.projectionEnter);
                break;
            case SoundPlayer.projectionStay:
                RuntimeManager.PlayOneShot(_SoundRef.projectionStay);
                break;
            case SoundPlayer.projectionExit:
                RuntimeManager.PlayOneShot(_SoundRef.projectionExit);
                break;
            default:
                UnityEngine.Debug.LogError("SoundManager, ligne 73, PlaySoundPlayer(), Argument manquant : Checkez l'enum");
                break;
        }

    }



    //                                                                              In Animation
    //
    //  PlaySoundFootstep() est � utiliser dans les animations de marche lorsque le pieds touche le sol.
    //
    //      Annotez l'endroit o� footstep player est appel�                             ()
    public void PlaySoundFootstep()
    {
        RuntimeManager.PlayOneShot(_SoundRef.footstep);
    }


    //  PlayPlayerJump() est � utiliser dans les animations de saut lorsque les pieds quitte le sol.
    //
    //      Annotez l'endroit o� footstep player est appel�                             ()
    public void PlaySoundJump()
    {
        RuntimeManager.PlayOneShot(_SoundRef.jump);
    }


    //  PlayPlayerLand() est � utiliser dans les animations de saut lorsque les pieds touche le sol.
    //
    //      Annotez l'endroit o� footstep player est appel�                             ()
    public void PlaySoundLand() 
    {
        RuntimeManager.PlayOneShot(_SoundRef.land);
    }




    #endregion
    #region Son System
    
    //  PlayComponentPlace() est � utiliser lorsqu'un des comportement est pos� sur un objet, le player ou sur un objet grab.
    //  Lors de l'appel de la fonction, vous pouvez ajouter un argement en plus pour choisir l'endroit o� est plac� le comportement, entre onObject, onPlayer et onGrab (default = onObject)
    //
    //  Tout les sons � appeler ici sont des sons finis, donc il ne doivent pas se jou� en boucle.
    //
    //      Annotez quand les sons sont ajout� au code. (nom du script + line)
    public void PlayComponentPlaceSound(GameObject gameObjet,PlaceParamType soundCompPlace = PlaceParamType.repulse, PlaceParamOnWhatType onWhatItPlace = PlaceParamOnWhatType.onObject)
    {
        RuntimeManager.PlayOneShotAttached(_SoundRef.place, gameObject);
    }

    //  PlayComponent() est � utiliser lorsqu'un des son de comportement simple doit etre jou�.
    //  Lors de l'appel de la fonction, vous pouvez ajouter un argement en plus pour determiner l'endroit d'o� vient le son, (default = Vector3(0,0,0))
    //
    //  Tout les sons � appeler ici sont des sons finis, donc il ne doivent pas se jou� en boucle.
    //
    //
    //  Sons disponible � ajouter dans le code:
    //  -   repulseBoom         : PlayComponenentSound(SoundCompPlace.repulseBoom)              ()
    //  -   repulseTimer        : PlayComponenentSound(SoundCompPlace.repulseTimer)             ()
    //  -   immuableHit         : PlayComponenentSound(SoundCompPlace.immuableHit)              ()
    //  -   bounceHit           : PlayComponenentSound(SoundCompPlace.bounceHit)                ()
    //
    //      Annotez quand les sons sont ajout� au code. (nom du script + line)
    public void PlayComponenentSound(SoundComp soundComp, GameObject gameObject)
    {
        switch (soundComp)
        {
            case SoundComp.repulseTimer:
                RuntimeManager.PlayOneShotAttached(_SoundRef.repulseTimer, gameObject);
                break;
            case SoundComp.repulseBoom:
                RuntimeManager.PlayOneShotAttached(_SoundRef.repulseTimer, gameObject);
                break;
            case SoundComp.immuableHit:
                RuntimeManager.PlayOneShotAttached(_SoundRef.immuableHit, gameObject);
                break;
            case SoundComp.bounceHit:
                RuntimeManager.PlayOneShotAttached(_SoundRef.bounceHit, gameObject);
                break;
        }
    }
    #endregion
}
