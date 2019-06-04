﻿/*
Copyright (c) 2019 Integrative Software LLC
Created: 6/2019
Author: Pablo Carbonell
*/

using System.Runtime.Serialization;

namespace Integrative.Lara.Delta
{
    [DataContract]
    sealed class ClearChildrenDelta : BaseDelta
    {
        [DataMember]
        public string ElementId { get; set; }

        public ClearChildrenDelta() : base(DeltaType.ClearChildren)
        {
        }

        public static void Enqueue(Element element)
        {
            if (element.QueueOpen)
            {
                element.Document.Enqueue(new ClearChildrenDelta
                {
                    ElementId = element.EnsureElementId(),
                });
            }
        }
    }
}