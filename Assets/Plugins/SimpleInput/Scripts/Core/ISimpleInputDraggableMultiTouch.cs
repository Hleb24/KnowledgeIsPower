using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace SimpleInputNamespace {
  public interface ISimpleInputDraggableMultiTouch {
    bool OnUpdate(List<PointerEventData> mousePointers, List<PointerEventData> touchPointers, ISimpleInputDraggableMultiTouch activeListener);
    int Priority { get; }
  }
}