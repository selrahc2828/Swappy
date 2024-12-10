Pour faire fonctionner ce player : 

desactiver/supprimer la main camera de la scene de base
Placer le ----Manager---- dans la scene
Placer le ---Canvas--- dans la scene
Placer les 2 préfabs Player et CameraHolder sur la scene

Bien set les Layers à Ground pour les objets où l'avatar peut marcher/sauter
Les Prefabs et Objets Comportement ont 2 materials dans leur Mesh Renderer, le 2e est le shader MATSH_Outline

Optionnel: pour voir Gizmo de la box de detection du grab
Dans le script GrabObject du prefab Player renseigner :
    - la mainCamera, ainsi que HandlerPosition et InterractorZone qui se trouve en enfant de MainCamera > Interractor (juste pour voir la box à l'écran, sinon ils sont récupéré au runtime)

