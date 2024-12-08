using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Reflection;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.HID;

public class ComponentStealer : MonoBehaviour
{
    private Controls controls;

    [Header("Raycast")]
    public LayerMask hitLayer;
    private Ray _ray;

    public Camera mainCam;

    [Header("Properties")]
    public GameObject objectStolen;
    public Dictionary<MonoBehaviour, System.Type> steals;
    public System.Type type;
    public MonoBehaviour[] components;

    [Header("Debug")]
    public TextMeshProUGUI surface;
    public TextMeshProUGUI lastSteal;
    public Transform rayPointStart;
    public LineRenderer line;


    void Start()
    {
        controls = GameManager.controls;

        controls.Player.ActionSlot1.performed += VolDeComportement;
        controls.Player.ActionSlot2.performed += ApplicationDeComportementSurObjet;
        controls.Player.ApplicationDeComportementSurPlayer.performed += ApplicationDeComportementSurPlayer;
        controls.Player.ViderSlots.performed += ViderComportementDuPlayer;

        //lastSteal.text = "";
        steals = new Dictionary<MonoBehaviour, System.Type>();

    }

    private void OnDisable()
    {
        controls.Player.ActionSlot1.performed -= VolDeComportement;
        controls.Player.ActionSlot2.performed += ApplicationDeComportementSurObjet;
        controls.Player.ApplicationDeComportementSurPlayer.performed += ApplicationDeComportementSurPlayer;
        controls.Player.ViderSlots.performed += ViderComportementDuPlayer;
    }

    void Update()
    {
        _ray = mainCam.ScreenPointToRay(Input.mousePosition);
    }
    private void VolDeComportement(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            RaycastHit _hit;

            _ray = mainCam.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
            {
                if (_hit.collider == null || _hit.collider.CompareTag("NotInteract"))
                {
                    return;
                }

                if (_hit.collider.gameObject.GetComponent<Comportment>())
                {
                    objectStolen = _hit.collider.gameObject;
                    components = objectStolen.GetComponents<MonoBehaviour>();

                    #region pour voler un objet a la fois
                    if (steals != null)
                    {
                        //on parcour le dictionnaire
                        foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
                        {
                            //si le monobehavior existe encore (verification obligatoire en cas de destruction de objectStolen)
                            if (script.Key != null)
                            {
                                //on le réactive
                                script.Key.enabled = true;
                            }
                            else
                            {
                                //on arrete de parcourir le dictionnaire car il contiens des valeurs appartenant a un objet détruit
                                break;
                            }
                        }
                        //on clear le dictionnaire
                        steals.Clear();

                    }
                    #endregion

                    foreach (MonoBehaviour component in components)
                    {
                        //on sauvegarde son type (le nom du script qu'on va voler s'y trouve)
                        type = component.GetType();
                        //on sauvgarde les field (les valeurs des variables du script qu'on va voler)
                        FieldInfo stealableField = type.GetField("stealable");
                        //on sauvegarde le game object d'origine du comportement
                        FieldInfo originalOwnerField = type.GetField("originalOwner");

                        //s'il y a une valeurs dans stealableField et que cette valeur est un booléen, on vérifie aussi que la variable originalOwner est un gameObject
                        if (stealableField != null && stealableField.FieldType == typeof(bool) && originalOwnerField.FieldType == typeof(GameObject))
                        {
                            //on modifie la variable originalOwner avant de copier l'état du script d'origine
                            originalOwnerField.SetValue(component, objectStolen);

                            //on copie le booléen stealable du script volé
                            bool isStealable = (bool)stealableField.GetValue(component);


                            //on verifie qu'il est true
                            if (isStealable)
                            {

                                //on ajoute une entrée au dictionnaire avec en clé le component et en value son type
                                steals.Add(component, type);
                                // Désactive le script en le mettant inactif
                                component.enabled = false;
                            }
                        }
                    }
                }
            }
        }
    }

    private void ApplicationDeComportementSurObjet(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("ok");
            RaycastHit _hit;

            if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
            {
                //Debug.LogWarning("Hit ray donné : " + _hit.collider.name);

                if (_hit.collider == null || _hit.collider.CompareTag("NotInteract"))
                {
                    if(_hit.collider.CompareTag("NotInteract"))
                    {
                        Debug.Log("pas ok interact");
                        return;
                    }
                    Debug.Log("pas ok null");
                    return;
                }

                ApplicationDeComportement(_hit.collider.gameObject);
            }
        }
    }

    private void ApplicationDeComportementSurPlayer(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            ApplicationDeComportement(gameObject);
        }
    }

    private void ApplicationDeComportement(GameObject _objectGiven)
    {
        //si l'objectStolen existe (on a bien un script volé)
        if (objectStolen != null && objectStolen.GetComponent<Collider>() != null)
        {
            //on parcour le dictionnaire des scripts a appliquer
            foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
            {
                // Ajoute dynamiquement un script du même type sur l'objet cible
                Component newComponent = _objectGiven.AddComponent(script.Value);

                //on parcour les variable du script qu'on vien d'ajouter
                foreach (FieldInfo field in script.Value.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    //on leur donne les même valeur qu'au moment ou on a volé le script
                    field.SetValue(newComponent, field.GetValue(script.Key));
                }
                //on détruit le script dans l'objet d'origine
                Destroy(script.Key);
                Debug.Log("Script " + script.Value.Name + " copié sur " + _objectGiven.name + " et supprimé de " + objectStolen.name);
            }
            //on remet a zero les dictionnaire et les objets sauvegardé
            steals.Clear();
            components = null;
            objectStolen = null;
        }
    }

    private void ViderComportementDuPlayer(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            components = gameObject.GetComponents<MonoBehaviour>();

            foreach (MonoBehaviour component in components)
            {
                //on sauvegarde son type (le nom du script qu'on va voler s'y trouve)
                type = component.GetType();//on sauvgarde les field (les valeurs des variables du script qu'on va voler)
                FieldInfo stealableField = type.GetField("stealable");
                //on sauvegarde le game object d'origine du comportement
                FieldInfo originalOwnerField = type.GetField("originalOwner");

                //s'il y a une valeurs dans stealableField et que cette valeur est un booléen, on vérifie aussi que la variable originalOwner est un gameObject
                if (stealableField != null && stealableField.FieldType == typeof(bool) && originalOwnerField.FieldType == typeof(GameObject))
                {
                    //on copie le booléen stealable du script volé
                    bool isStealable = (bool)stealableField.GetValue(component);

                    //on copie la variable originalOwner
                    GameObject originalOwner = (GameObject)originalOwnerField.GetValue(component);

                    //on verifie qu'il est true
                    if (isStealable)
                    {
                        if(originalOwner != null)
                        {
                            // Ajoute dynamiquement un script du même type sur l'objet cible
                            Component newComponent = originalOwner.AddComponent(type);

                            //on parcour les variable du script qu'on vien d'ajouter
                            foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.Instance))
                            {
                                //on leur donne les même valeur qu'au moment ou on a volé le script
                                field.SetValue(newComponent, field.GetValue(component));
                            }
                        }
                        Destroy(component);
                        Debug.Log("Script " + type.Name + " renvoyé sur " + component.name + " et supprimé du Player");
                    }
                }
            }
            components = null;
        }
    }

    public void ResetListeComp()
    {
        if (steals != null)
        {
            //on parcour le dictionnaire
            foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
            {
                //si le monobehavior existe encore (verification obligatoire en cas de destruction de objectStolen)
                if (script.Key != null)
                {
                    //on le réactive
                    script.Key.enabled = true;
                }
                else
                {
                    //on arrete de parcourir le dictionnaire car il contiens des valeurs appartenant a un objet détruit
                    break;
                }
            }
        }
        steals.Clear();
        components = null;
        objectStolen = null;

        // Supprime tous les comportements sur le GameObject
        // (voir pour réactiver ceux des script de base)
        Comportment[] attachedComponents = gameObject.GetComponents<Comportment>();
        foreach (MonoBehaviour component in attachedComponents)
        {
            Destroy(component);
        }

        lastSteal.text = "";

    }
}
