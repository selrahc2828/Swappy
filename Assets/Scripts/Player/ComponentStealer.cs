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
    public CameraController camController;

    [Header("Raycast")]
    public LayerMask hitLayer;
    private Ray _ray;
    private RaycastHit _hit;

    [HideInInspector]
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

    private void OnEnable()
    {

    }

    void Start()
    {
        lastSteal.text = "";
        mainCam = camController.mainCamera;
        steals = new Dictionary<MonoBehaviour, System.Type>();

    }

    void Update()
    {
        RaycastHit _hit;

        //Vector3 newMousePos = new Vector3(Mathf.Abs(Input.mousePosition.x), Mathf.Abs(Input.mousePosition.y), Mathf.Abs(Input.mousePosition.z));
        _ray = mainCam.ScreenPointToRay(Input.mousePosition);

        LineRendererETDebug();

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer))
        {
            if (_hit.collider == null)
            {
                surface.SetText("null");
                return;
            }
            surface.SetText(_hit.collider.gameObject.name);
        }
        else
        {
            surface.SetText("");
        }
    }

    public void CopyStealComp()
    {
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
                    lastSteal.text = "";

                }
                #endregion

                foreach (MonoBehaviour component in components)
                {
                    //on sauvegarde son type (le nom du script qu'on va voler s'y trouve)
                    type = component.GetType();
                    //on sauvgarde les field (les valeurs des variables du script qu'on va voler)
                    FieldInfo stealableField = type.GetField("stealable");

                    //s'il y a une valeurs dans stealableField et que cette valeur est un booléen
                    if (stealableField != null && stealableField.FieldType == typeof(bool))
                    {
                        //on copie le booléen stealable du script volé
                        bool isStealable = (bool)stealableField.GetValue(component);

                        //on verifie qu'il est true
                        if (isStealable)
                        {
                            //on ajoute une entrée au dictionnaire avec en clé le component et en value son type
                            steals.Add(component, type);
                            // Désactive le script en le mettant inactif
                            component.enabled = false;
                            lastSteal.text += component.name + " - " + type + "\n";
                        }
                    }
                }                

            }
        }
    }

    public void PasteComp()
    {
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            //Debug.LogWarning("Hit ray donné : " + _hit.collider.name);

            if (_hit.collider == null || _hit.collider.CompareTag("NotInteract"))
            {
                return;
            }
                //si l'objectStolen existe (on a bien un script volé)
                if (objectStolen != null && objectStolen.GetComponent<Collider>() != null)
                {
                    GameObject objectGiven = _hit.collider.gameObject;

                    //on parcour le dictionnaire des scripts a appliquer
                    foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
                    {
                        // Ajoute dynamiquement un script du même type sur l'objet cible
                        Component newComponent = objectGiven.AddComponent(script.Value);

                        //on parcour les variable du script qu'on vien d'ajouter
                        foreach (FieldInfo field in script.Value.GetFields(BindingFlags.Public | BindingFlags.Instance))
                        {
                            //on leur donne les même valeur qu'au moment ou on a volé le script
                            field.SetValue(newComponent, field.GetValue(script.Key));
                        }
                        //on détruit le script dans l'objet d'origine
                        Destroy(script.Key);
                        Debug.Log("Script " + script.Value.Name + " copié sur " + objectGiven.name + " et supprimé de " + objectStolen.name);
                    }
                    //on remet a zero les dictionnaire et les objets sauvegardé
                    steals.Clear();
                    lastSteal.text = "";
                    components = null;
                    objectStolen = null;
                }
        }
    }

    public void PasteAtMe()
    {
        //si l'objectStolen existe (on a bien un script volé)
        if (objectStolen != null && objectStolen.GetComponent<Collider>() != null)
        {
            GameObject objectGiven = gameObject; //à modif plus tard

            //on parcour le dictionnaire des scripts a appliquer
            foreach (KeyValuePair<MonoBehaviour, System.Type> script in steals)
            {
                // Ajoute dynamiquement un script du même type sur l'objet cible
                Component newComponent = objectGiven.AddComponent(script.Value);

                //on parcour les variable du script qu'on vien d'ajouter
                foreach (FieldInfo field in script.Value.GetFields(BindingFlags.Public | BindingFlags.Instance))
                {
                    //on leur donne les même valeur qu'au moment ou on a volé le script
                    field.SetValue(newComponent, field.GetValue(script.Key));
                }
                //on détruit le script dans l'objet d'origine
                Destroy(script.Key);
                Debug.Log("Script " + script.Value.Name + " copié sur " + objectGiven.name + " et supprimé de " + objectStolen.name);
            }
            //on remet a zero les dictionnaire et les objets sauvegardé
            steals.Clear();
            lastSteal.text = "";
            components = null;
            objectStolen = null;
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
