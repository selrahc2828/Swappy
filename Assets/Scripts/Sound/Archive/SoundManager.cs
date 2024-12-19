using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using FMOD.Studio;

public class SoundManager : MonoBehaviour
{

    #region Init Gestion Son
    private Bus _busPlayer ;
    private Bus _busSystem ;
    #endregion
    #region Init Player
    public enum SoundPlayer { slowTime, unslowTime, steal, give, projectionEnter, projectionStay, projectionExit }
#endregion
    #region Init System
    public enum PlaceParamType { repulse, immuable, bounce, propeler, aimant }
    public enum PlaceParamOnWhatType  { onObject, onPlayer, onGrab }
    public enum SoundComp { repulseTimer, repulseBoom,  immuableHit, bounceHit, propelerStart, aimantStart }
    [SerializeField]
    private GameObject prefabSonPropeler;
    [SerializeField]
    private GameObject prefabSonAimant;

    #endregion
    #region InitRef
    private SoundRef _SoundRef;
    private void Start()
    {
        _SoundRef = GetComponent<SoundRef>();
        //_busPlayer = RuntimeManager.GetBus(_SoundRef.busPlayer);
        //_busSystem = RuntimeManager.GetBus(_SoundRef.busSystem);
    }
    #endregion

    #region Son Player
    //                                                                              In Code
    //
    //  PlayPlayerSound() est à utiliser lorsqu'un des son du joueur doit etre joué.
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
    //  PlaySoundFootstep() est à utiliser dans les animations de marche lorsque le pieds touche le sol.
    //
    //      Annotez l'endroit où footstep player est appelé                             ()
    public void PlaySoundFootstep()
    {
        RuntimeManager.PlayOneShot(_SoundRef.footstep);
    }


    //  PlayPlayerJump() est à utiliser dans les animations de saut lorsque les pieds quitte le sol.
    //
    //      Annotez l'endroit où footstep player est appelé                             ()
    public void PlaySoundJump()
    {
        RuntimeManager.PlayOneShot(_SoundRef.jump);
    }


    //  PlayPlayerLand() est à utiliser dans les animations de saut lorsque les pieds touche le sol.
    //
    //      Annotez l'endroit où footstep player est appelé                             ()
    public void PlaySoundLand() 
    {
        RuntimeManager.PlayOneShot(_SoundRef.land);
    }




    #endregion
    #region Son System
    //  PlaySoundCollision() est à utiliser lorsqu'il y a une collision.
    //  Lors de l'appel de la fonction, vous devez ajouter en argement l'object de la collision pour que le son soit bien émis au bons endroits.
    //
    //      Annotez quand les sons sont ajouté au code. (nom du script + line)
    public void PlaySoundCollision(GameObject gameObjet)
    {
        RuntimeManager.PlayOneShotAttached(_SoundRef.collision, gameObject);
    }

    //  PlaySoundComponentPlace() est à utiliser lorsqu'un des comportement est posé sur un objet, le player ou sur un objet grab.
    //  Lors de l'appel de la fonction, vous pouvez ajouter un argement en plus pour choisir l'endroit où est placé le comportement, entre onObject, onPlayer et onGrab (default = onObject)
    //
    //  Tout les sons à appeler ici sont des sons finis, donc il ne doivent pas se joué en boucle.
    //
    //      Annotez quand les sons sont ajouté au code. (nom du script + line)
    public void PlaySoundComponentPlace(GameObject gameObjet,PlaceParamType soundCompPlace = PlaceParamType.repulse, PlaceParamOnWhatType onWhatItPlace = PlaceParamOnWhatType.onObject)
    {
        RuntimeManager.PlayOneShotAttached(_SoundRef.placeComp, gameObject);
    }

    //  PlaySoundComponent() est à utiliser lorsqu'un des son de comportement simple doit etre joué.
    //  Lors de l'appel de la fonction, vous pouvez ajouter un argement en plus pour determiner l'endroit d'où vient le son, (default = Vector3(0,0,0))
    //
    //  Tout les sons à appeler ici sont des sons finis, donc il ne doivent pas se joué en boucle.
    //
    //
    //  Sons disponible à ajouter dans le code:
    //  -   repulseBoom         : PlayComponenentSound(SoundCompPlace.repulseBoom)              ()
    //  -   repulseTimer        : PlayComponenentSound(SoundCompPlace.repulseTimer)             ()
    //  -   immuableHit         : PlayComponenentSound(SoundCompPlace.immuableHit)              ()
    //  -   bounceHit           : PlayComponenentSound(SoundCompPlace.bounceHit)                ()
    //  -   propelerStart       : PlayComponenentSound(SoundCompPlace.propelerStart)            ()
    //  -   aimantStart         : PlayComponenentSound(SoundCompPlace.aimantStart)              ()
    //
    //      Annotez quand les sons sont ajouté au code. (nom du script + line)
    public void PlaySoundComponenent(SoundComp soundComp, GameObject gameObject)
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
            case SoundComp.propelerStart:
                Instantiate(prefabSonPropeler, gameObject.transform);
                break;
            case SoundComp.aimantStart:
                Instantiate(prefabSonAimant, gameObject.transform);
                break;
        }
    }

    #endregion
    #region Gestion Du Son
    private void Update()
    {
    }
    #endregion
}
