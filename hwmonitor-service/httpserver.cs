/*
 * 
 * Based on microsoft documentation with some code from project openhardware monitor
 * 
 */

using hwmonitor_service;
using System;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;

namespace httpserver
{
    public class HttpServer
    {
        private HttpListener listener;
        private int listenerPort;
        private Thread listenerThread;

        public HttpServer(int port)
        {
            listenerPort = port;
            
            try
            {
                listener = new HttpListener();
                listener.IgnoreWriteExceptions = true;
            }
            catch (PlatformNotSupportedException)
            {
                listener = null;
            }
        }

        public bool PlatformNotSupported
        {
            get
            {
                return listener == null;
            }
        }

        public Boolean StartHTTPListener()
        {
            if (PlatformNotSupported)
                return false;

            try
            {
                if (listener.IsListening)
                    return true;

                string prefix = "http://+:" + listenerPort + "/";
                listener.Prefixes.Clear();
                listener.Prefixes.Add(prefix);
                listener.Start();

                if (listenerThread == null)
                {
                    listenerThread = new Thread(HandleRequests);
                    listenerThread.Start();
                }
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public Boolean StopHTTPListener()
        {
            if (PlatformNotSupported)
                return false;

            try
            {
                listenerThread.Abort();
                listener.Stop();
                listenerThread = null;
            }
            catch (HttpListenerException)
            {
            }
            catch (ThreadAbortException)
            {
            }
            catch (NullReferenceException)
            {
            }
            catch (Exception)
            {
            }
            return true;
        }

        private void HandleRequests()
        {

            while (listener.IsListening)
            {
                var context = listener.BeginGetContext(
                  new AsyncCallback(ListenerCallback), listener);
                context.AsyncWaitHandle.WaitOne();
            }
        }

        private void ListenerCallback(IAsyncResult result)
        {
            HttpListener listener = (HttpListener)result.AsyncState;
            if (listener == null || !listener.IsListening)
                return;

            // Call EndGetContext to complete the asynchronous operation.
            HttpListenerContext context;
            try
            {
                context = listener.EndGetContext(result);
            }
            catch (Exception)
            {
                return;
            }

            HttpListenerRequest request = context.Request;

            var requestedFile = request.RawUrl.Substring(1);

            SendJSON(context.Response);
        }
        private void SendJSON(HttpListenerResponse response)
        {

            string JSON;
            hwmonitor_list newList = new hwmonitor_list();
            JSON = newList.GetSystemInfo();

            var responseContent = JSON;
            byte[] buffer = Encoding.UTF8.GetBytes(responseContent);

            response.AddHeader("Cache-Control", "no-cache");

            response.ContentLength64 = buffer.Length;
            response.ContentType = "application/json";

            try
            {
                Stream output = response.OutputStream;
                output.Write(buffer, 0, buffer.Length);
                output.Close();
            }
            catch (HttpListenerException)
            {
            }

            response.Close();
        }



        public int ListenerPort
        {
            get { return listenerPort; }
            set { listenerPort = value; }
        }

        ~HttpServer()
        {
            if (PlatformNotSupported)
                return;

            StopHTTPListener();
            listener.Abort();
        }

        public void Quit()
        {
            if (PlatformNotSupported)
                return;

            StopHTTPListener();
            listener.Abort();
        }
    }
}
