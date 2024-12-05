Pour faire fonctionner ce player : 

desactiver/supprimer la main camera de la scene de base
Placer le ----Manager---- dans la scene
placer les 2 préfabs Player et CameraHolder sur la scene

Dans le script Game Manager du prefab ----Manager---- renseigner Orientation de la categorie PlayerMouvementParameters (c'est l'enfant Orientation du prefab du player)
Dans le script PlayerMouvementStateMachine du prefab Player renseigner le ----Manager----
Dans le script PlayerCam de k'enfant MainCamera du prefab cameraHolder renseigner Orientation (c'est l'enfant Orientation du prefab du player)