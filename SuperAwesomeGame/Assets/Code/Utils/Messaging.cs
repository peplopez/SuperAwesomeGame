using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

static public class Messaging
{
	public delegate void Callback( GameObject sender, GameObject receiver, GameEvent gameEvent, object param );	
	
	public enum Filter
	{
		All,
		OnlyForMe
	}

	public enum Priority
	{
		High = 0,
		Medium = 1,
		Low = 2
	}
	
	class Listener
	{
		public GameObject subscriber;
		public Callback callback;
		public Filter filter;
		public Priority priority;
		public string componentName;
	}
		
    private static Dictionary<GameEvent, List<Listener>> eventTable = new Dictionary<GameEvent, List<Listener>>();
	
	//------------------------------------------------------------------------
    static public void AddListener( GameEvent gameEvent, Callback handler, Filter filter )
    {
		Delegate d = handler;
		
		// intenta inferir el GameObject al que pertenece el handler si es que pertenece a alguno
		GameObject go = null;
		Component component = d.Target as Component;
		if( component != null )
		{
			go = component.gameObject;
		}
		
		AddListener( go, gameEvent, handler, filter, Priority.Medium );
    }
	
	//------------------------------------------------------------------------
    static public void AddListener( GameObject subscriber, GameEvent gameEvent, Callback handler, Filter filter, Priority priority )
    {
		if( subscriber == null )
		{
			Debug.LogWarning( "[Event] El handler no esta asociado a un GameObject, ¿seguro que es lo quieres hacer?" );
		}
		
		List<Listener> listeners;
		if( !eventTable.TryGetValue( gameEvent, out listeners ) )
		{
			eventTable.Add( gameEvent, new List<Listener>() );
			listeners = eventTable[gameEvent];
		}
		
		Listener listener = new Listener();
		listener.subscriber = subscriber;
		listener.filter = filter;
		listener.callback = handler;
		listener.priority = priority;
		if(handler.Target is Component)
		{
			listener.componentName = (handler.Target as Component).ToString();
		}
		else
		{
			listener.componentName = handler.Target.GetType().ToString() + handler.Target.GetHashCode().ToString ();
		}

		//Debug.Log("[Core] Added Listener from '" + listener.componentName + "' for event '" + gameEvent.ToString() + "'.");
		listeners.Add( listener );
	}	
	
	//------------------------------------------------------------------------
    static public void RemoveListener( GameEvent gameEvent, Callback handler )
    {
		List<Listener> listeners;
		if( !eventTable.TryGetValue( gameEvent, out listeners ) )
		{
			return;
		}
		
		Listener foundListener = null;
		foreach( Listener listener in listeners )
		{
			if( listener.callback == handler )
			{
				foundListener = listener;
				break;
			}
		}
		
		if( foundListener != null )
			listeners.Remove( foundListener );
    }
	
	//------------------------------------------------------------------------
	static public void Send(GameObject sender, GameObject receiver, GameEvent gameEvent, object param)
	{
	
			//string name = receiver == null ? "null" : receiver.name;
			//Debug.Log("[Event] Send message *" + gameEvent.ToString() + "* " + "to " + name);
		

		List<Listener> listeners;
		if (!eventTable.TryGetValue(gameEvent, out listeners))
		{
			//Debug.LogWarning( "[Core] No listeners found for event" + gameEvent.ToString() );
			return;
		}

		foreach (var p in Enum.GetValues(typeof(Priority)))
		{
			Priority currentPriority = (Priority)p;

			List<Listener> currentPriorityListeners = listeners.Where(l => l.priority == currentPriority).ToList();

			foreach (Listener listener in currentPriorityListeners)
			{
				bool send = false;

				if (listener.filter == Filter.All)
				{
					send = true;
				}
	
				if (listener.filter == Filter.OnlyForMe)
				{
					if (listener.subscriber == receiver || receiver == null)
					{
						send = true;
					}
				}

				if (send == true)
				{
					/*
					if( listener.subscriber != null && !listener.subscriber.activeInHierarchy )
						Debug.LogWarning(string.Format("[Core] Se ha enviado un mensaje '{0}' al objeto '{1}' que esta inactivo.", gameEvent.ToString(), listener.componentName) );
					*/

					if( listener.subscriber == null )
						Debug.LogWarning(string.Format("[Core] Se ha intentado enviar un mensaje '{0}' al objeto '{1}' que ya no existe.", gameEvent.ToString(), listener.componentName));
					else
						listener.callback(sender, receiver, gameEvent, param);
				}
			}
		}
	} // Send

	public static void CleanUp()
	{
		foreach (var e in eventTable)
		{
			foreach (Listener listener in e.Value)
			{
				if (listener.subscriber == null)
					Debug.LogError("[Core] Orphan Listener in '" + listener.componentName + "' for event '" + e.Key.ToString() + "'. Call RemoveListener() to cleanup message.");
			}
		}
	}
}