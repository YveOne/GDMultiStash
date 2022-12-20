using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Threading;
using D3DHook.Hook.Common;

namespace D3DHook.Interface
{

    [Serializable]
    public delegate void MessageReceivedEvent(MessageReceivedEventArgs message);
    [Serializable]
    public delegate void DisconnectedEvent();
    [Serializable]
    public delegate void DrawOverlayEvent(DrawOverlayEventArgs args);
    [Serializable]
    public delegate void InitResourcesEvent(InitResourcesEventArgs args);
    [Serializable]
    public delegate void FrameDrawingEvent(float ms);
    
    [Serializable]
    public class CaptureInterface : MarshalByRefObject
    {
        /// <summary>
        /// The client process Id
        /// </summary>
        public int ProcessId { get; set; }

        #region Events

        #region Server-side Events

        /// <summary>
        /// Server event for sending debug and error information from the client to server
        /// </summary>
        public event MessageReceivedEvent RemoteMessage;

        public event FrameDrawingEvent FrameDrawing;

        #endregion

        #region Client-side Events

        /// <summary>
        /// Client event used to notify the hook to exit
        /// </summary>
        public event DisconnectedEvent Disconnected;

        /// <summary>
        ///     Client event used to (re-)draw an overlay in-game.
        /// </summary>
        public event DrawOverlayEvent DrawOverlay;

        public event InitResourcesEvent InitResources;
        

        #endregion

        #endregion

















        #region Public Methods


        /// <summary>
        /// Tell the client process to disconnect
        /// </summary>
        public void Disconnect()
        {
            SafeInvokeDisconnected();
        }

        /// <summary>
        /// Send a message to all handlers of <see cref="CaptureInterface.RemoteMessage"/>.
        /// </summary>
        /// <param name="messageType"></param>
        /// <param name="format"></param>
        /// <param name="args"></param>
        public void Message(MessageType messageType, string format, params object[] args)
        {
            Message(messageType, String.Format(format, args));
        }

        public void Message(MessageType messageType, string message)
        {
            SafeInvokeMessageRecevied(new MessageReceivedEventArgs(messageType, message));
        }

        /// <summary>
        /// Replace the in-game overlay with the one provided.
        /// 
        /// Note: this is not designed for fast updates (i.e. only a couple of times per second)
        /// </summary>
        /// <param name="overlay"></param>
        public void DrawOverlayInGame(IOverlay overlay)
        {
            SafeInvokeDrawOverlay(new DrawOverlayEventArgs()
            {
                Overlay = overlay
            });
        }

        public void Frame(float ms)
        {
            SafeInvokeFrameRecevied(ms);
        }

        public void InitializeResources(List<IResource> resources)
        {
            SafeInvokeInitResources(new InitResourcesEventArgs()
            {
                Resources = resources
            });
        }

        #endregion

        #region Private: Invoke message handlers

        private void SafeInvokeMessageRecevied(MessageReceivedEventArgs eventArgs)
        {
            if (RemoteMessage == null)
                return;         //No Listeners

            MessageReceivedEvent listener = null;
            Delegate[] dels = RemoteMessage.GetInvocationList();

            foreach (Delegate del in dels)
            {
                try
                {
                    listener = (MessageReceivedEvent)del;
                    listener.Invoke(eventArgs);
                }
                catch (Exception)
                {
                    //Could not reach the destination, so remove it
                    //from the list
                    RemoteMessage -= listener;
                }
            }
        }

        private void SafeInvokeDisconnected()
        {
            if (Disconnected == null)
                return;         //No Listeners

            DisconnectedEvent listener = null;
            Delegate[] dels = Disconnected.GetInvocationList();

            foreach (Delegate del in dels)
            {
                try
                {
                    listener = (DisconnectedEvent)del;
                    listener.Invoke();
                }
                catch (Exception)
                {
                    //Could not reach the destination, so remove it
                    //from the list
                    Disconnected -= listener;
                }
            }
        }

        private void SafeInvokeDrawOverlay(DrawOverlayEventArgs drawOverlayEventArgs)
        {
            if (DrawOverlay == null)
                return; //No Listeners

            DrawOverlayEvent listener = null;
            var dels = DrawOverlay.GetInvocationList();

            foreach (var del in dels)
            {
                try
                {
                    listener = (DrawOverlayEvent)del;
                    listener.Invoke(drawOverlayEventArgs);
                }
                catch (Exception)
                {
                    //Could not reach the destination, so remove it
                    //from the list
                    DrawOverlay -= listener;
                }
            }
        }

        private void SafeInvokeFrameRecevied(float ms)
        {
            if (FrameDrawing == null) return;         //No Listeners

            FrameDrawingEvent listener = null;
            Delegate[] dels = FrameDrawing.GetInvocationList();

            foreach (Delegate del in dels)
            {
                try
                {
                    listener = (FrameDrawingEvent)del;
                    listener.Invoke(ms);
                }
                catch (Exception)
                {
                    //Could not reach the destination, so remove it
                    //from the list
                    FrameDrawing -= listener;
                }
            }
        }

        private void SafeInvokeInitResources(InitResourcesEventArgs e)
        {
            if (InitResources == null) return;         //No Listeners

            InitResourcesEvent listener = null;
            Delegate[] dels = InitResources.GetInvocationList();

            foreach (Delegate del in dels)
            {
                try
                {
                    listener = (InitResourcesEvent)del;
                    listener.Invoke(e);
                }
                catch (Exception)
                {
                    InitResources -= listener;
                }
            }
        }

        #endregion

        /// <summary>
        /// Used to confirm connection to IPC server channel
        /// </summary>
        public DateTime Ping()
        {
            return DateTime.Now;
        }
    }


    /// <summary>
    /// Client event proxy for marshalling event handlers
    /// </summary>
    public class ClientCaptureInterfaceEventProxy : MarshalByRefObject
    {
        #region Event Declarations

        /// <summary>
        /// Client event used to notify the hook to exit
        /// </summary>
        public event DisconnectedEvent Disconnected;

        /// <summary>
        ///     Client event used to (re-)draw an overlay in-game.
        /// </summary>
        public event DrawOverlayEvent DrawOverlay;

        public event InitResourcesEvent InitResources;

        #endregion

        #region Lifetime Services

        public override object InitializeLifetimeService()
        {
            //Returning null holds the object alive
            //until it is explicitly destroyed
            return null;
        }

        #endregion


        public void DisconnectedProxyHandler()
        {
            Disconnected?.Invoke();
        }

        public void DrawOverlayProxyHandler(DrawOverlayEventArgs args)
        {
            DrawOverlay?.Invoke(args);
        }
        
        public void InitResourcesProxyHandler(InitResourcesEventArgs args)
        {
            InitResources?.Invoke(args);
        }
    }
}