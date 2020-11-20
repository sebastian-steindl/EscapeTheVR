using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Events;

class ResetElementEvent : UnityEvent<DragObject>
{
    public DragObject DragObject;

    private static ResetElementEvent events = new ResetElementEvent();
    public static ResetElementEvent getInstance() {
        return events;
    }
    private ResetElementEvent()
    {
    }
}
