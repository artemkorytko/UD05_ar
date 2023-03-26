using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class FieldPositionController : MonoBehaviour
{
   private ARPlaneManager _arPlaneManager; //  создаст повержность
   private ARRaycastManager _arRaycastManager; // бросит на поверхность рейкаст 
   private ARAnchorManager _arAnchorManager; // привяжет объект к точке рейкаста

   public bool IsActive { get; set; } // можем привязываться пока активны ... ы? кто?

   
   public void Initialize(ARRaycastManager arRaycastManager, ARPlaneManager arPlaneManager,
      ARAnchorManager arAnchorManager)
   {
      _arRaycastManager = arRaycastManager;
      _arPlaneManager = arPlaneManager;
      _arAnchorManager = arAnchorManager;
   }

   private void Update()
   {
      if (!IsActive) return;

      // найти центр экрана !!!
      var position = new Vector2(Screen.currentResolution.width * 0.5f, Screen.currentResolution.width * 0.5f);

      // массив возвращает плагин, все чего он нарейкастил по пути
      List<ARRaycastHit> arRaycastHits = new List<ARRaycastHit>();
      
      // третье - пытается взять полигон на поверхности!
      // где, передали массив, и к чему мы пытаемся получить рейкасты - нам надо треугольники на поверхности
      if (_arRaycastManager.Raycast(position, arRaycastHits, TrackableType.PlaneWithinPolygon))
      {
         // структура
         // с кем мы срейкастились - есть позиция и айдишник
         // Pose - это типо трансформ без скейла
         Pose pose = arRaycastHits[0].pose; // позиция
         TrackableId id = arRaycastHits[0].trackableId; // id поверхности
         
         // рейкаст попал на поверхность и берем ее id
         ARPlane arPlane = _arPlaneManager.GetPlane(id); // получаем поверхность на которую маы попали по его id
         ARAnchor point = _arAnchorManager.AttachAnchor(arPlane, pose); // точка на поверхности
         // привязка к позиции куда попали - якорь относительно этой точки

         gameObject.transform.position = point.transform.position; // поставили и повернули на эту точку
         gameObject.transform.rotation = pose.rotation;

      }
   }

}
