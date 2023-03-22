using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FieldPositionController : MonoBehaviour
{
    private ARPlaneManager _arPlaneManager; //определит поверхность
    private ARRaycastManager _arRaycastManager; //бросит рейкаст на поверхность
    private ARAnchorManager _arAnchorManager; //привяжен объект к точке рейкаста
    
    public bool IsActive { get; set; }

    public void Initialize(ARPlaneManager arPlaneManager, ARRaycastManager arRaycastManager, ARAnchorManager arAnchorManager)
    {
        _arPlaneManager = arPlaneManager;
        _arRaycastManager = arRaycastManager;
        _arAnchorManager = arAnchorManager;
    }

    private void Update()
    {
        if(!IsActive) return;


        var position = new Vector2(Screen.currentResolution.width * 0.5f, Screen.currentResolution.height * 0.5f);

        List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();

        if (_arRaycastManager.Raycast(position, arRaycastHits, TrackableType.PlaneWithinPolygon))
        {
            Pose pose = arRaycastHits[0].pose; //позиция
            TrackableId id = arRaycastHits[0].trackableId; //id поверхности
            
            ARPlane arPlane = _arPlaneManager.GetPlane(id); //поверхность
            ARAnchor point = _arAnchorManager.AttachAnchor(arPlane, pose); //точка на повехности
            
            gameObject.transform.position = point.transform.position;
            gameObject.transform.rotation = pose.rotation;
        }
        
    }
}
