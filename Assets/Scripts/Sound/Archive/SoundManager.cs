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

    private RESULT _debug;

#endregion
    #region Init Comportement
    public enum SoundCompPlace { repulsePlace, immuablePlace, bouncePlace, propelerPlace, aimantPlace }
    public enum OnWhatItPlace  { onObject, onPlayer, onGrab }
    private EventInstance _sonCompPlace;
    private RESULT _testCompPlace;  
    private string _repulsePlace = "event:/System/Componenent/Place/repulsePlace";
    private string _immuablePlace = "event:/System/Componenent/Place/immuablePlace";
    private string _bouncePlace = "event:/System/Componenent/Place/bouncePlace";
    private string _propelerPlace = "event:/System/Componenent/Place/propelerPlace";
    private string _aimantPlace = "event:/System/Componenent/Place/aimantPlace";
    public enum SoundComp { repulseBoom,  immuableHit, bounceHit, propelerStart, aimantStart,repulseTimer, }
    private EventInstance _sonComp;
    private RESULT _testComp;
    private string repulseBoom = "event:/System/Componenent/repulseBoom";
    private string repulseTimer = "event:/System/Componenent/repulseTimer";
    private string immuableHit = "event:/System/Componenent/immuableHit";
    private string bounceHit = "event:/System/Componenent/bounceHit";
    private string propelerStart = "event:/System/Componenent/propelerStart";
    private string aimantStart = "event:/System/Componenent/aimantStart";

    private EventInstance _sonAimant;
    private RESULT _testAimant;
    private EventInstance _sonPropeler;
    private RESULT _testPropeler;
    private EventInstance _sonRepulseTimer;
    private RESULT _testRepulseTimer;
    #endregion
    #region Init Collision
    private EventInstance _sonCollision;
    private RESULT _testCollisionSound;
    private string _collision = "event:/System/Collision";
    #endregion

    private void Start()
    {
        _SoundRef = GetComponent<SoundRef>();
    }

    #region Son Player

    //                                                                              In Code
    //
    //
    //  PlayPlayerSound() est � utiliser lorsqu'un des son du joueur doit etre jou�.
    //
    //  Tout les sons � appel� ici sont des sons finis, donc il ne doivent pas se jou� en boucle.
    //
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
    //
    //  PlaySoundFootstep() est � utiliser dans les animations de marche lorsque le pieds touche le sol.
    //
    //  Son finit, donc pas de boucle, en th�orie
    //
    //      Annotez l'endroit o� footstep player est appel�                             ()
    public void PlaySoundFootstep()
    {
        RuntimeManager.PlayOneShot(_SoundRef.footstep);
    }


    //  PlayPlayerJump() est � utiliser dans les animations de saut lorsque les pieds quitte le sol.
    //
    //  Son finit, donc pas de boucle, en th�orie
    //
    //      Annotez l'endroit o� footstep player est appel�                             ()
    public void PlayPlayerJump()
    {
        RuntimeManager.PlayOneShot(_SoundRef.jump);
    }


    //  PlayPlayerLand() est � utiliser dans les animations de saut lorsque les pieds touche le sol.
    //
    //  Son finit, donc pas de boucle, en th�orie
    //
    //      Annotez l'endroit o� footstep player est appel�                             ()
    public void PlayPlayerLand() 
    {
        RuntimeManager.PlayOneShot(_SoundRef.land);
    }




    #endregion
    #region Son Componenet Collectif 
    //  PlayComponentPlace() est � utiliser lorsqu'un des comportement est pos� sur un objet, le player ou sur un objet grab.
    //  Lors de l'appel de la fonction, vous pouvez ajouter un argement en plus pour choisir l'endroit o� est plac� le comportement, entre onObject, onPlayer et onGrab (default = onObject)
    //
    //  Tout les sons � appeler ici sont des sons finis, donc il ne doivent pas se jou� en boucle.
    //
    //
    //  Sons disponible � ajouter dans le code:
    //  -   repulsePlace        : PlayComponentPlaceSound(SoundCompPlace.repulsePlace)          ()
    //  -   immuablePlace       : PlayComponentPlaceSound(SoundCompPlace.immuablePlace)         ()
    //  -   bouncePlace         : PlayComponentPlaceSound(SoundCompPlace.bouncePlace)           ()
    //  -   propelerPlace       : PlayComponentPlaceSound(SoundCompPlace.propelerPlace)         ()
    //  -   aimantPlace         : PlayComponentPlaceSound(SoundCompPlace.aimantPlace)           ()
    //
    //      Annotez quand les sons sont ajout� au code. (nom du script + line)
    public void PlayComponentPlaceSound(SoundCompPlace soundCompPlace, OnWhatItPlace onWhatItPlace = OnWhatItPlace.onObject)
    {
        switch(soundCompPlace)
        {
            case SoundCompPlace.repulsePlace:
                _sonCompPlace = RuntimeManager.CreateInstance(_repulsePlace);
                break;
            case SoundCompPlace.immuablePlace:
                _sonCompPlace = RuntimeManager.CreateInstance(_immuablePlace);
                break;
            case SoundCompPlace.bouncePlace:
                _sonCompPlace = RuntimeManager.CreateInstance(_bouncePlace);
                break;
            case SoundCompPlace.propelerPlace:
                _sonCompPlace = RuntimeManager.CreateInstance(_propelerPlace);
                break;
            case SoundCompPlace.aimantPlace:
                _sonCompPlace = RuntimeManager.CreateInstance(_aimantPlace);
                break;
            default:
                UnityEngine.Debug.LogError("PlayPlayerSound, Argument manquant : Checkez la liste");
                break;
        }
        switch(onWhatItPlace) 
        {
            case OnWhatItPlace.onObject:
                _sonCompPlace.setParameterByName("Type", 0);
                break;
            case OnWhatItPlace.onPlayer:
                _sonCompPlace.setParameterByName("Type", 1);
                break;
            case OnWhatItPlace.onGrab:
                _sonCompPlace.setParameterByName("Type", 2);
                break;
        }
        _testCompPlace = _sonCompPlace.start();
        if (_testCompPlace!= RESULT.OK)
        {
            UnityEngine.Debug.LogError("PlayComponentPlaceSound Error: Son non jou�/manquant. ( Vous ne devriez au grand jamais voir cette erreur donc chill, mais dans le doute elle est l�)");
        }
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
    //  -   propelerStart       : PlayComponenentSound(SoundCompPlace.propelerStart)            ()
    //  -   aimantStart         : PlayComponenentSound(SoundCompPlace.aimantStart)              ()
    //
    //      Annotez quand les sons sont ajout� au code. (nom du script + line)
    public void PlayComponenentSound(SoundComp soundComp, Vector3 positon = default)
    {
        switch (soundComp)
        {
            case SoundComp.repulseBoom:
                _sonComp = RuntimeManager.CreateInstance(repulseBoom);
                break;
            case SoundComp.repulseTimer:
                _sonComp = RuntimeManager.CreateInstance(repulseTimer);
                break;
            case SoundComp.immuableHit:
                _sonComp = RuntimeManager.CreateInstance(immuableHit);
                break;
            case SoundComp.bounceHit:
                _sonComp = RuntimeManager.CreateInstance(bounceHit);
                break;
            //case SoundComp.propelerStart:
            //    _sonComp = RuntimeManager.CreateInstance(propelerStart);
            //    break;
            //case SoundComp.aimantStart:
            //    _sonComp = RuntimeManager.CreateInstance(aimanatStart);
            //    break;
        }
        _sonComp.set3DAttributes(RuntimeUtils.To3DAttributes(positon));
        _testComp = _sonComp.start();
        if (_testComp != RESULT.OK)
        {
            UnityEngine.Debug.LogError("PlayComponentSound Error: Son non jou�/manquant. ( Vous ne devriez au grand jamais voir cette erreur donc chill, mais dans le doute elle est l�)");
        }
    }
    #endregion
    #region Son Comportement Individuel
    //  Sons Long/en Loop, dont la position doit etre actualiser � chaque frame.
    //  Sons en question:
    //  -   Aimant Start                : PlayAimantSound(transform.position)                   ()
    //  -   Aimant SetPosition          : SetPositionAimantSound(transform.position)            ()
    //  -   Aimant Stop                 : StopAimantSound()                                     ()
    //  -   Propeler Start              : PlayPropelerSound(transform.position)                 ()
    //  -   Propeler SetPosition        : SetPositionPropelerSound(transform.position)          ()
    //  -   Propeler Stop               : StopPropelerSound()                                   ()
    //  -   RepulseTime Start           : PlayRepulseTimeSound(transform.position)              ()
    //  -   RepulseTime SetPosition     : SetPositionRepulseTimeSound(transform.position)       ()
    //  -   RepulseTime Stop            : StopRepulseTimeSound()                                ()
    //
    //      Annotez quand les sons sont ajout� au code. (nom du script + line)
    public void PlayAimmantSound(Vector3 position = default)
    {
        _sonAimant = RuntimeManager.CreateInstance(aimantStart);
        _sonAimant.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _testAimant = _sonAimant.start();
        if ( _testAimant != RESULT.OK)
        {
            UnityEngine.Debug.LogError("PlayAimantSound, Pas de son jou�/trouv�, c'est pas bon signe ");
        }
    }
    public void SetPositionAimantSound(Vector3 position = default)
    {
        _testAimant = _sonAimant.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        {
            UnityEngine.Debug.LogError("SetPositionAimantSound, Pas de son jou�/trouv�, c'est pas bon signe ");
        }
    }
    public void StopAimantSound()
    {
        _sonAimant.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _sonAimant.release();
    }

    public void PlayPropelerSound(Vector3 position = default)
    {
        _sonPropeler = RuntimeManager.CreateInstance(propelerStart);
        _sonPropeler.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _testPropeler = _sonPropeler.start();
        if (_testPropeler != RESULT.OK)
        {
            UnityEngine.Debug.LogError("PlayPropelerSound, Pas de son jou�/trouv�, c'est pas bon signe ");
        }
    }
    public void SetPositionPropelerSound(Vector3 position = default)
    {
        _testPropeler = _sonPropeler.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        {
            UnityEngine.Debug.LogError("SetPositionPropelerSound, Pas de son jou�/trouv�, c'est pas bon signe ");
        }
    }
    public void StopPropelerSound()
    {
        _sonPropeler.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _sonPropeler.release();
    }

    public void PlayRepulseTimerSound(Vector3 position = default)
    {
        _sonRepulseTimer = RuntimeManager.CreateInstance(propelerStart);
        _sonRepulseTimer.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        _testRepulseTimer = _sonRepulseTimer.start();
        if (_testRepulseTimer != RESULT.OK)
        {
            UnityEngine.Debug.LogError("PlayRepulseTimerSound, Pas de son jou�/trouv�, c'est pas bon signe ");
        }
    }
    public void SetPositionRepulseTimerSound(Vector3 position = default)
    {
        _testRepulseTimer = _sonRepulseTimer.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        {
            UnityEngine.Debug.LogError("SetPositionRepulseTimerSound, Pas de son jou�/trouv�, c'est pas bon signe ");
        }
    }
    public void StopRepulseTimerSound()
    {
        _sonRepulseTimer.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        _sonRepulseTimer.release();
    }
    #endregion
    #region Son Collison
    //  Son de collision de tout les objets, � appel� lorsqu'il y a une collision.
    //  Argument n�cessaire :  Position (Vector3) + est-ce que l'objet est magik/� un comportement sp�ciale (bool)  
    public void PLayCollisionSound(Vector3 position = default, bool isMagik = false)
    {
        _sonCollision = RuntimeManager.CreateInstance(_collision);
        _sonCollision.set3DAttributes(RuntimeUtils.To3DAttributes(position));
        if (isMagik)
        {
            _sonCollision.setParameterByName("Type", 1);
        }
        else
        {
            _sonCollision.setParameterByName("Type", 0);
        }
        _testCollisionSound = _sonCollision.start();
        if ( _testCollisionSound != RESULT.OK)
        {
            UnityEngine.Debug.LogError("PLayCollisionSound, Pas de son jou�/trouv�, des probl�me de collision");
        }
        _sonCollision.release();
    }
    #endregion


}
