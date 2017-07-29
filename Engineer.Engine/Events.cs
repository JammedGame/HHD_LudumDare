using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engineer.Engine
{
    public class EventsPackage
    {
        private ExternEventPackage _Extern;
        private List<EventHandlersPackage> _EventList;
        public ExternEventPackage Extern
        {
            get
            {
                return _Extern;
            }

            set
            {
                _Extern = value;
            }
        }
        public List<EventHandlersPackage> EventList
        {
            get
            {
                return _EventList;
            }
            set
            {
                _EventList = value;
            }
        }
        public EventsPackage()
        {
            this._Extern = new ExternEventPackage();
            this._EventList = new List<EventHandlersPackage>();
        }
        public EventsPackage(List<EventHandlersPackage> EventList)
        {
            this._Extern = new ExternEventPackage();
            this._EventList = EventList;
        }
        public EventsPackage(EventsPackage SE, Scene ParentScene)
        {
            this._Extern = SE._Extern;
            this._EventList = new List<EventHandlersPackage>();
            for (int i = 0; i < SE._EventList.Count; i++) _EventList.Add(new EventHandlersPackage(SE._EventList[i], ParentScene));
        }
        public List<ScriptSceneObject> Events(string ID)
        {
            for (int i = 0; i < _EventList.Count; i++)
            {
                if (_EventList[i].ID == ID) return _EventList[i].Events;
            }
            return null;
        }
    }
    public class EventHandlersPackage
    {
        private string _ID;
        private List<ScriptSceneObject> _Events;
        public string ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }
        public List<ScriptSceneObject> Events
        {
            get
            {
                return _Events;
            }
            set
            {
                _Events = value;
            }
        }
        public EventHandlersPackage()
        {
            this.ID = Guid.NewGuid().ToString();
            this.Events = new List<ScriptSceneObject>();
        }
        public EventHandlersPackage(string ID)
        {
            this.ID = ID;
            this.Events = new List<ScriptSceneObject>();
        }
        public EventHandlersPackage(EventHandlersPackage SEH, Scene ParentScene)
        {
            this.ID = SEH.ID;
            this._Events = new List<ScriptSceneObject>();
            for (int i = 0; i < SEH._Events.Count; i++) _Events.Add(new ScriptSceneObject(SEH._Events[i], ParentScene));
        }
        public static List<EventHandlersPackage> NewSceneEventsPackage()
        {
            List<EventHandlersPackage> EventList = new List<EventHandlersPackage>();
            EventList.Add(new EventHandlersPackage("Closing"));
            EventList.Add(new EventHandlersPackage("KeyDown"));
            EventList.Add(new EventHandlersPackage("KeyUp"));
            EventList.Add(new EventHandlersPackage("KeyPress"));
            EventList.Add(new EventHandlersPackage("Load"));
            EventList.Add(new EventHandlersPackage("MouseDown"));
            EventList.Add(new EventHandlersPackage("MouseUp"));
            EventList.Add(new EventHandlersPackage("MouseClick"));
            EventList.Add(new EventHandlersPackage("MouseMove"));
            EventList.Add(new EventHandlersPackage("MouseWheel"));
            EventList.Add(new EventHandlersPackage("RenderFrame"));
            EventList.Add(new EventHandlersPackage("Resize"));
            EventList.Add(new EventHandlersPackage("TimerTick"));
            return EventList;
        }
        public static List<EventHandlersPackage> NewDrawnSceneObjectEventsPackage()
        {
            List<EventHandlersPackage> EventList = new List<EventHandlersPackage>();
            EventList.Add(new EventHandlersPackage("MouseDown"));
            EventList.Add(new EventHandlersPackage("MouseUp"));
            EventList.Add(new EventHandlersPackage("MouseClick"));
            EventList.Add(new EventHandlersPackage("MouseMove"));
            return EventList;
        }
    }
    public delegate void GameEventHandler(Game CurrentGame, EventArguments Args);
    public class ExternEventPackage
    {
        public event GameEventHandler Closing;
        public event GameEventHandler KeyDown;
        public event GameEventHandler KeyUp;
        public event GameEventHandler KeyPress;
        public event GameEventHandler Load;
        public event GameEventHandler MouseDown;
        public event GameEventHandler MouseUp;
        public event GameEventHandler MouseClick;
        public event GameEventHandler MouseMove;
        public event GameEventHandler MouseWheel;
        public event GameEventHandler RenderFrame;
        public event GameEventHandler Resize;
        public event GameEventHandler TimerTick;
        public event GameEventHandler OperationProgress;
        public event GameEventHandler OperationFinished;
        public ExternEventPackage()
        {
            Closing = new GameEventHandler(OnInvoke);
            KeyDown = new GameEventHandler(OnInvoke);
            KeyUp = new GameEventHandler(OnInvoke);
            KeyPress = new GameEventHandler(OnInvoke);
            Load = new GameEventHandler(OnInvoke);
            MouseDown = new GameEventHandler(OnInvoke);
            MouseUp = new GameEventHandler(OnInvoke);
            MouseClick = new GameEventHandler(OnInvoke);
            MouseMove = new GameEventHandler(OnInvoke);
            MouseWheel = new GameEventHandler(OnInvoke);
            RenderFrame = new GameEventHandler(OnInvoke);
            TimerTick = new GameEventHandler(OnInvoke);
            Resize = new GameEventHandler(OnInvoke);
            OperationProgress = new GameEventHandler(OnInvoke);
            OperationFinished = new GameEventHandler(OnInvoke);
        }
        public void OnInvoke(Game CurrentGame, EventArguments Args)
        {

        }
        public void Invoke(string EventName, Game CurrentGame, EventArguments Args)
        {
            if (EventName == "Closing") Closing.Invoke(CurrentGame, Args);
            if (EventName == "KeyDown") KeyDown.Invoke(CurrentGame, Args);
            if (EventName == "KeyUp") KeyUp.Invoke(CurrentGame, Args);
            if (EventName == "KeyPress") KeyPress.Invoke(CurrentGame, Args);
            if (EventName == "Load") Load.Invoke(CurrentGame, Args);
            if (EventName == "MouseDown") MouseDown.Invoke(CurrentGame, Args);
            if (EventName == "MouseUp") MouseUp.Invoke(CurrentGame, Args);
            if (EventName == "MouseClick") MouseClick.Invoke(CurrentGame, Args);
            if (EventName == "MouseMove") MouseMove.Invoke(CurrentGame, Args);
            if (EventName == "MouseWheel") MouseWheel.Invoke(CurrentGame, Args);
            if (EventName == "RenderFrame") RenderFrame.Invoke(CurrentGame, Args);
            if (EventName == "Resize") Resize.Invoke(CurrentGame, Args);
            if (EventName == "TimerTick") TimerTick.Invoke(CurrentGame, Args);
            if (EventName == "OperationProgress") OperationProgress.Invoke(CurrentGame, Args);
            if (EventName == "OperationFinished") OperationFinished.Invoke(CurrentGame, Args);
        }
    }
}
