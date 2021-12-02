using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectDetector : MonoBehaviour
{
    [SerializeField]
    private TowerSpawner towerSpawner;
    [SerializeField]
    private TowerDataViewer towerDataViewer;

    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;

    private void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        // Check if there is a touch
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            // Check if finger is over a UI element
            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
            {
                return;
            }
            ray = mainCamera.ScreenPointToRay(touch.position);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
            }
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                towerDataViewer.HidePanel();
            }
            hitTransform = null;
        }
        /*
        if (Input.GetMouseButtonUp(0))
        {
            if(EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                hitTransform = hit.transform;
            }
            if (hitTransform == null || hitTransform.CompareTag("Tower") == false)
            {
                Debug.Log("HIDE PANEL");
                towerDataViewer.HidePanel();
            }
            hitTransform = null;
        }    
        */
    }
}
