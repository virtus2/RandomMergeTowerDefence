using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerMove : MonoBehaviour
{
    private TowerDataViewer towerDataViewer;
    private Camera mainCamera;
    private Ray ray;
    private RaycastHit hit;
    private Transform hitTransform = null;
    private Vector3 originalPosition;
    private Tile originalTile;
    private int layerMask;
    private bool isMoving;

    private Vector3 clickStartPosition;
    private Vector3 clickEndPosition;

    public bool IsMoving => isMoving;
    private void Awake()
    {
        mainCamera = Camera.main;
        layerMask = 1 << LayerMask.NameToLayer("Tower");
    }

    public void Setup(TowerDataViewer towerDataViewer)
    {
        this.towerDataViewer = towerDataViewer;
    }
    private void OnMouseDown()
    {
        clickStartPosition = Input.mousePosition;
        originalPosition = transform.position;
        ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        towerDataViewer.ShowPanel(transform);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            if (hit.transform.CompareTag("Tile"))
            {
                originalTile = hit.transform.GetComponent<Tile>();
            }
        }
    }
    private void OnMouseDrag()
    {
        if(Vector3.Distance(clickStartPosition, Input.mousePosition) > 3f)
        {
            isMoving = true;

            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            newPosition.z = -1;
            gameObject.transform.position = newPosition;

            towerDataViewer.HidePanel();

        }
    }

    private void OnMouseUp()
    {
        clickEndPosition = Input.mousePosition;
        if(Vector3.Distance(clickStartPosition, clickEndPosition) > 3f)
        {
            isMoving = false;
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            int layerMask = 1 << LayerMask.NameToLayer("Tower");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
            {
                if (hit.transform.CompareTag("Tile"))
                {
                    Tile tile = hit.transform.GetComponent<Tile>();
                    if (tile.IsTowerBuilt)
                    {
                        gameObject.transform.position = originalPosition;
                        TowerWeapon movingTower = gameObject.GetComponent<TowerWeapon>();
                        TowerWeapon tileTower = tile.OwnTower.GetComponent<TowerWeapon>();
                        
                        if((movingTower.TowerType == tileTower.TowerType) && (movingTower.Level == tileTower.Level)
                            && originalTile.transform.position != tile.transform.position)
                        {
                            tileTower.LevelUp();
                            //towerDataViewer.OnClickTowerUpgrade();

                            Lean.Pool.LeanPool.Despawn(gameObject);
                            originalTile.OwnTower = null;
                            originalTile.IsTowerBuilt = false;
                        }
                    }
                    else
                    {
                        hitTransform = hit.transform;
                        Vector3 newPosition = new Vector3(tile.transform.position.x, tile.transform.position.y, -1);
                        gameObject.transform.position = newPosition;
                        tile.IsTowerBuilt = true;
                        tile.OwnTower = gameObject;
                        originalTile.IsTowerBuilt = false;

                    }
                }
                else
                {
                    gameObject.transform.position = originalPosition;
                }
            }
            else
            {
                gameObject.transform.position = originalPosition;

            }
        }
        else
        {
            towerDataViewer.ShowPanel(transform);
            transform.position = originalPosition;
        }
    }
}
