﻿/*
Copyright (c) 2019 Integrative Software LLC
Created: 10/2019
Author: Pablo Carbonell
*/

using Integrative.Lara.Tools;
using System;
using System.Net;

namespace Integrative.Lara.Main
{
    /// <summary>
    /// Represents a hosted Lara application
    /// </summary>
    public sealed class Application : IDisposable
    {
        readonly Published _published = new Published();

        /// <summary>
        /// Defines default error pages
        /// </summary>
        public ErrorPages ErrorPages { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        public Application()
        {
            ErrorPages = new ErrorPages(_published);
            ErrorPages.PublishErrorImage();
        }

        internal Published GetPublished() => _published;

        /// <summary>
        /// Removes all published addresses
        /// </summary>
        public void ClearAllPublished()
        {
            _published.ClearAll();
        }

        /// <summary>
        /// Disposable implementation
        /// </summary>
        public void Dispose()
        {
            ClearAllPublished();
            _published.Dispose();
        }

        #region Publishing

        /// <summary>
        /// Publishes a page.
        /// </summary>
        /// <param name="address">The URL address of the page.</param>
        /// <param name="pageFactory">Handler that creates instances of the page</param>
        public void PublishPage(string address, Func<IPage> pageFactory)
            => _published.Publish(address, new PagePublished(pageFactory));

        /// <summary>
        /// Publishes static content.
        /// </summary>
        /// <param name="address">The URL address of the content.</param>
        /// <param name="content">The static content to be published.</param>
        public void PublishFile(string address, StaticContent content)
            => _published.Publish(address, content);

        /// <summary>
        /// Publishes a web service
        /// </summary>
        /// <param name="content">Web service settings</param>
        public void PublishService(WebServiceContent content)
        {
            content = content ?? throw new ArgumentNullException(nameof(content));
            _published.Publish(content);
        }

        /// <summary>
        /// Unpublishes an address and its associated content.
        /// </summary>
        /// <param name="path">The path.</param>
        public void UnPublish(string path)
        {
            _published.UnPublish(path);
        }

        /// <summary>
        /// Publishes all classes marked with the attributes [LaraPage] and [LaraWebService]
        /// </summary>
        public void PublishAssemblies()
            => AssembliesReader.LoadAssemblies(this);

        /// <summary>
        /// Unpublished a web service
        /// </summary>
        /// <param name="address">The URL address of the web service</param>
        /// <param name="method">The HTTP method of the web service</param>
        public void UnPublish(string address, string method)
        {
            address = address ?? throw new ArgumentNullException(nameof(address));
            method = method ?? throw new ArgumentNullException(nameof(method));
            _published.UnPublish(address, method);
        }

        internal bool TryGetNode(string path, out IPublishedItem item)
            => _published.TryGetNode(path, out item);

        internal bool TryGetConnection(Guid guid, out Connection connection)
            => _published.Connections.TryGetConnection(guid, out connection);

        internal Connection CreateConnection(IPAddress remoteIp)
            => _published.Connections.CreateConnection(remoteIp);

        internal void ClearEmptyConnection(Connection connection)
            => _published.Connections.ClearEmptyConnection(connection);

        #endregion

        #region Web Components

        /// <summary>
        /// Registers a specific web component
        /// </summary>
        /// <param name="options"></param>
        public void PublishComponent(WebComponentOptions options)
        {
            options = options ?? throw new ArgumentNullException(nameof(options));
            _published.Publish(options);
        }

        /// <summary>
        /// Unregisters a specific web component
        /// </summary>
        /// <param name="tagName"></param>
        public void UnPublishWebComponent(string tagName)
            => _published.UnPublishWebComponent(tagName);

        internal bool TryGetComponent(string tagName, out Type type)
            => _published.TryGetComponent(tagName, out type);

        #endregion
    }
}