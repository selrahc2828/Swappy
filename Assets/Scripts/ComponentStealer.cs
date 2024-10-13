using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Reflection;
using UnityEngine.SceneManagement;

public class ComponentStealer : MonoBehaviour
{
    public TextMeshProUGUI surface;
    public TextMeshProUGUI lastSteal;

    private Vector3 rayTail;
    private Ray _ray;

    public Controller controller;
    public CameraController camController;

    [Header("Raycast et line render")]
    private RaycastHit _hit;
    [HideInInspector]
    public float radius;
    public LineRenderer line;
    public float longueur = 10f;
    public LayerMask hitLayer;

    //public bool isStealing;
    public List<Component> listComportement = new List<Component>();
    private Camera mainCam;
    public Transform rayPointStrat;
    public Transform castStealerPoint;//à suppr

    [Header("Variation")]
    [Tooltip("Vol ou copie")]
    public bool isSteal;

    private void OnEnable()
    {

    }

    void Start()
    {
        lastSteal.text = "";
        mainCam = camController.mainCamera;

    }

    void Update()
    {
        RaycastHit _hit;

        mainCam = camController.Brain.OutputCamera;

        //Vector3 newMousePos = new Vector3(Mathf.Abs(Input.mousePosition.x), Mathf.Abs(Input.mousePosition.y), Mathf.Abs(Input.mousePosition.z));
        _ray = mainCam.ScreenPointToRay(Input.mousePosition);

        //rayTail = mainCam.transform.position + mainCam.transform.forward * longueur;

        float maxDistance = 500f;
        // Si le raycast touche un objet
        if (Physics.Raycast(_ray, out _hit, maxDistance, hitLayer))
        {
            // Positionner les points du LineRenderer pour dessiner la ligne
            line.SetPosition(0, rayPointStrat.position);  // Début de la ligne (caméra)
            line.SetPosition(1, _hit.point);  // Fin de la ligne (point touché par le rayon)

            Debug.DrawLine(mainCam.transform.position, _hit.point, Color.red);
        }
        else
        {
            // Si rien n'est touché, on dessine la ligne jusqu'à la distance max du raycast
            Vector3 farPoint = _ray.GetPoint(maxDistance);
            line.SetPosition(0, rayPointStrat.position);  // Début de la ligne (caméra)
            line.SetPosition(1, farPoint);  // Fin de la ligne (point éloigné)
            Debug.DrawLine(mainCam.transform.position, farPoint, Color.green);

        }

        //if (Physics.SphereCast(_ray, radius, out _hit, Mathf.Infinity)) //mask

        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer))// (Physics.Raycast(castStealerPoint.position, castStealerPoint.forward, out _hit, Mathf.Infinity, hitLayer))
        {
            if (_hit.collider == null)
            {
                surface.SetText("null");
                return;
            }
            //Debug.Log(_hit.collider.gameObject.name);
            //nameText.text = _hit.collider.gameObject.name;
            surface.SetText(_hit.collider.gameObject.name);
        }
        else
        {
            surface.SetText("");
        }
    }

    public void CopyStealComp()
    {
        //Debug.Log("StealComp  _mvtData.type : " + _mvtData.type);
        //isStealing = true;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            if (_hit.collider == null)
            {
                return;
            }

            if (_hit.collider.gameObject.GetComponent<Comportment>())
            {
                //Debug.Log(_hit.collider.gameObject.GetComponent<Comportement>().typeComp);

                Comportment[] listComp = _hit.collider.gameObject.GetComponents<Comportment>();

                foreach (Comportment item in listComp)
                {
                    listComportement.Add(item);

                    lastSteal.text += item.name + " - " + item.typeComp + "\n";

                    if (isSteal)
                        Destroy(item);// ou disable
                }
            }
        }
    }

    public void PasteComp()
    {
        //Debug.Log("PasteComp  _mvtData.type : " + _mvtData.type);
        //isStealing = false;
        if (Physics.Raycast(_ray, out _hit, Mathf.Infinity, hitLayer)) //mask
        {
            if (_hit.collider == null || _hit.collider.GetComponent<Rigidbody>() == null)
            {
                return;
            }

            // Désactiver chaque script
            foreach (Comportment component in listComportement)
            {
                System.Type type = component.GetType();
                //FieldInfo stealableField = type.GetField("stealable");//nope

                // Ajoute dynamiquement un script du même type sur l'objet cible
                Component newComp = _hit.collider.gameObject.AddComponent(type);


                foreach (FieldInfo field in type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance))
                {
                    field.SetValue(newComp, field.GetValue(component));
                }
            }
        }
    }

    void OnDrawGizmos()
    {
        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(mainCam.transform.position, mainCam.transform.position + mainCam.transform.forward * 10);
    }
}
