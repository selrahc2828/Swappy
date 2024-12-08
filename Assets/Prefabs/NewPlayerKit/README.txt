Pour faire fonctionner ce player : 

desactiver/supprimer la main camera de la scene de base
Placer le ----Manager---- dans la scene
Placer les 2 préfabs Player et CameraHolder sur la scene
Placer le ---Canvas--- dans la scene

Dans le script Game Manager du prefab ----Manager---- renseigner :
    - Orientation de la categorie PlayerMouvementParameters (c'est l'enfant Orientation du prefab du player)
    - TimerSlowText de la categorie SlowTimer (Canvas >TimerSlow > CountSlow)
    - TimeProjectionText de la categorie PlayerProjectingParameter (Canvas > TimerProjection > CountProjection)
Dans le script PlayerMouvementStateMachine du prefab Player renseigner le ----Manager----
Dans le script PlayerCam de l'enfant MainCamera du prefab cameraHolder renseigner Orientation (c'est l'enfant Orientation du prefab du player)
Dans le script GrabObject du prefab Player renseigner :
    - la mainCamera, ainsi que HandlerPosition et InterractorZone qui se trouve en enfant de MainCamera > Interractor (juste pour voir la box à l'écran, sinon ils sont récupéré au runtime)
    - InteractText (Canvas > Interract)

Dans le script ComponentStealer renseigner MainCamera

Bien set les Layer à Ground pour les objets où l'avatar peut marcher/sauter
Les Prefabs et Objets Comportement on 2 materials dans leur Mesh Renderer, le 2e est le shader MATSH_Outline
