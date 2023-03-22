using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace AR
{
    public class FieldPositionController : MonoBehaviour
    {
        private ARRaycastManager _arRaycastManager; // бросит луч
        private ARPlaneManager _arPlaneManager; // определит поверхность
        private ARAnchorManager _arAnchorManager; // привяжет объект к точке рейкаст
        
        public bool IsActive { get; set; }

        public void Initialize(ARAnchorManager anchorManager, ARRaycastManager arRaycastManager,
            ARPlaneManager arPlaneManager)
        {
            _arAnchorManager = anchorManager;
            _arPlaneManager = arPlaneManager;
            _arRaycastManager = arRaycastManager;
        }

        private void Update()
        {
            if(!IsActive) return;
            
            var position = new Vector2(Screen.currentResolution.width * 0.5f, Screen.currentResolution.height * 0.5f);
            List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
            
            if (_arRaycastManager.Raycast(position, arRaycastHits, TrackableType.PlaneWithinPolygon))
            {
                Pose pose = arRaycastHits[0].pose;
                TrackableId id = arRaycastHits[0].trackableId;
                
                ARPlane arPlane = _arPlaneManager.GetPlane(id);
                ARAnchor point = _arAnchorManager.AttachAnchor(arPlane, pose);
                
                gameObject.transform.position = point.transform.position;
                gameObject.transform.rotation = pose.rotation;
            }

        }
    }
}