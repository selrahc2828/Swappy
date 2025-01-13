using FMOD.Studio;
using FMODUnity;
using UnityEngine;

    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance;
        
        #region Init Gestion Son
        private Bus m_busPlayer ;
        private Bus m_busSystem ;
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
        [SerializeField]
        private GameObject prefabSonImmuable;
        [SerializeField]
        private GameObject prefabSonBouncing;
        [SerializeField]
        private GameObject prefabSonRepulseTimer;
        [SerializeField]
        private GameObject prefabSonRepulseBoomer;

        #endregion
        #region Init Zic
        private bool isMusicPlay;
        #endregion
        #region InitRef

        //private void Start()
        //{
        //_busPlayer = RuntimeManager.GetBus(_SoundRef.busPlayer);
        //_busSystem = RuntimeManager.GetBus(_SoundRef.busSystem);
        //}
        #endregion

        #region Son Player
        //                                                                              In Code
        //
        //  PlayPlayerSound() est � utiliser lorsqu'un des son du joueur doit etre jou�.
        //
        //  Sons disponible � ajouter dans le code:
        //  -   slowTime            : PlayPlayerSound(SoundPlayer.slowTime)                 (GameManager.cs, ligne 247)
        //  -   unslowTime          : PlayPlayerSound(SoundPlayer.unslowTime)               (GameManager.cs, ligne 254)
        //  -   stealComp           : PlayPlayerSound(SoundPlayer.stealComp)                (ComportementStealer_proto.cs, ligne 106, ligne 202, ligne 212)
        //  -   giveComp            : PlayPlayerSound(SoundPlayer.giveComp)                 (ComportementStealer_proto.cs, ligne 138, ligne 153, ligne 239, ligne 254)
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
                    RuntimeManager.PlayOneShot("event:/Player/Time/SlowTime");
                    break;
                case SoundPlayer.unslowTime:
                    RuntimeManager.PlayOneShot("event:/Player/Time/UnslowTime");
                    break;
                case SoundPlayer.steal:
                    RuntimeManager.PlayOneShot("event:/Player/Comp/StealComp");
                    break;
                case SoundPlayer.give:
                    RuntimeManager.PlayOneShot("event:/Player/Comp/GiveComp");
                    break;
                case SoundPlayer.projectionEnter:
                    RuntimeManager.PlayOneShot("event:/Player/Projection/projectionEnter");
                    break;
                case SoundPlayer.projectionStay:
                    RuntimeManager.PlayOneShot("event:/Player/Projection/projectionStay");
                    break;
                case SoundPlayer.projectionExit:
                    RuntimeManager.PlayOneShot("event:/Player/Projection/projectionExit");
                    break;
                default:
                    Debug.LogError("SoundManager, ligne 73, PlaySoundPlayer(), Argument manquant : Checkez l'enum");
                    break;
            }

        }



        //                                                                              In Animation
        //
        //  PlaySoundFootstep() est � utiliser dans les animations de marche lorsque le pied touche le sol.
        //
        //      Annotez l'endroit o� footstep player est appelé                             (MovementState.cs, ligne 129, ligne 146)
        public void PlaySoundFootstep()
        {
            RuntimeManager.PlayOneShot("event:/Player/Moving/Footstep");
        }


        //  PlayPlayerJump() est � utiliser dans les animations de saut lorsque les pieds quittent le sol.
        //
        //      Annotez l'endroit o� footstep player est appel�                             (Jumping.cs, ligne 29)
        public void PlaySoundJump()
        {
            RuntimeManager.PlayOneShot("event:/Player/Moving/Jump");
        }


        //  PlayPlayerLand() est � utiliser dans les animations de saut lorsque les pieds touchent le sol.
        //
        //      Annotez l'endroit o� footstep player est appel�                             (Falling.cs, ligne 29)
        public void PlaySoundLand() 
        {
            RuntimeManager.PlayOneShot("event:/Player/Moving/Land");
        }




        #endregion
        #region Son System
        //  PlaySoundCollision() est � utiliser lorsqu'il y a une collision.
        //  Lors de l'appel de la fonction, vous devez ajouter en argement l'object de la collision pour que le son soit bien �mis aux bons endroits.
        //
        //      Annotez quand les sons sont ajout� au code. (ComportementState, ligne 52)
        public void PlaySoundCollision(GameObject gameObjet)
        {
            RuntimeManager.PlayOneShotAttached("event:/System/Collision", gameObject);
        }

        //  PlaySoundComponentPlace() est � utiliser lorsqu'un des comportements est pos� sur un objet, le player ou sur un objet grab.
        //  Lors de l'appel de la fonction, vous pouvez ajouter un argement supllémentaire pour choisir l'endroit o� est plac� le comportement, entre onObject, onPlayer et onGrab (default = onObject)
        //
        //  Tous les sons � appeler ici sont des sons finis, donc ils ne doivent pas se jou� en boucle.
        //
        //      Annotez quand les sons sont ajout� au code. (nom du script + line)
        public void PlaySoundComponentPlace(GameObject gameObjet,PlaceParamType soundCompPlace = PlaceParamType.repulse, PlaceParamOnWhatType onWhatItPlace = PlaceParamOnWhatType.onObject)
        {
            RuntimeManager.PlayOneShotAttached("event:/System/PLace", gameObject);
        }

        //  PlaySoundComponent() est � utiliser lorsqu'un des sons de comportement simple doit etre jou�.
        //  Lors de l'appel de la fonction, vous pouvez ajouter un argement en plus pour determiner l'endroit d'o� vient le son, (default = Vector3(0,0,0))
        //
        //  Tout les sons � appeler ici sont des sons finis, donc il ne doivent pas se jou� en boucle.
        //
        //
        //  Sons disponible � ajouter dans le code:
        //  -   repulseBoom         : PlayComponenentSound(SoundCompPlace.repulseBoom)              ()
        //  -   repulseTimer        : PlayComponenentSound(SoundCompPlace.repulseTimer)             ()
        //  -   immuableHit         : PlayComponenentSound(SoundCompPlace.immuableHit)              (C_Solo_Immuable, ligne 49)
        //  -   bounceHit           : PlayComponenentSound(SoundCompPlace.bounceHit)                (C_Solo_Bouncing, ligne 70)
        //  -   propelerStart       : PlayComponenentSound(SoundCompPlace.propelerStart)            (C_Solo_Rocket, ligne 7)
        //  -   aimantStart         : PlayComponenentSound(SoundCompPlace.aimantStart)              (C_Solo_Magnet, ligne 19)
        //
        //      Annotez quand les sons sont ajout� au code. (nom du script + line)
        public void PlaySoundComponenent(SoundComp soundComp, GameObject obj)
        {
            switch (soundComp)
            {
                case SoundComp.repulseTimer:
                    Instantiate(prefabSonRepulseTimer, obj.transform);
                    break;
                case SoundComp.repulseBoom:
                    Instantiate(prefabSonRepulseBoomer, obj.transform);
                    break;
                case SoundComp.immuableHit:
                    RuntimeManager.PlayOneShotAttached("event:/System/Componenent/BounceHit", obj);
                    break;
                case SoundComp.bounceHit:
                    RuntimeManager.PlayOneShotAttached("event:/System/Componenent/BounceHit",obj);
                    break;
                case SoundComp.propelerStart:
                    Instantiate(prefabSonPropeler, obj.transform);
                    break;
                case SoundComp.aimantStart:
                    Instantiate(prefabSonAimant, obj.transform);
                    break;
            }
        }

        #endregion
        #region Zic
        private void Update()
        {
            if (!isMusicPlay)
            {
                RuntimeManager.PlayOneShot("event:/Music/Sample Ref Sound");
                isMusicPlay = true;
            }
        
        }
        #endregion
        #region Gestion Du Son

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogWarning("Il y a plus d'une instance de SoundManager dans la scène");
                return;
            }
            Instance = this;
        }
        #endregion
    }

