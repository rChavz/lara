﻿/*
Copyright (c) 2019-2020 Integrative Software LLC
Created: 5/2019
Author: Pablo Carbonell
*/

using System;
using System.Threading.Tasks;

namespace Integrative.Lara
{
    // ReSharper disable once InconsistentNaming
    internal sealed class JSBridge : IJSBridge
    {
        private readonly PageContext _parent;

        public string? EventData { get; internal set; } = string.Empty;

        public JSBridge(PageContext parent)
        {
            _parent = parent;
        }

        public void Submit(string javaScriptCode, string? payload = null)
        {
            _parent.Document.Enqueue(new SubmitJsDelta
            {
                Code = javaScriptCode,
                Payload = payload
            });
        }

        [Obsolete("Use instead AddMessageListener() and RemoveMessageListener().")]
        public void OnMessage(string key, Func<Task> handler)
        {
            _parent.Document.OnMessage(key, handler);
        }

        public void AddMessageListener(string messageId, Func<MessageEventArgs, Task> handler)
        {
            _parent.Document.AddMessageListener(messageId, handler);
        }

        public void RemoveMessageListener(string messageId, Func<MessageEventArgs, Task> handler)
        {
            _parent.Document.RemoveMessageListener(messageId, handler);
        }

        public void ServerEventsOn() => _parent.Document.ServerEventsOn();

        public Task ServerEventsOff() => _parent.Document.ServerEventsOff();

    }
}
