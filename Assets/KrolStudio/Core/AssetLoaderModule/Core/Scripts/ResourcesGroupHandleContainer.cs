using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    public class ResourcesGroupHandleContainer 
    {
        public readonly Dictionary<string, ResourceRequest> CompletedHandles = new();
        public readonly Dictionary<string, List<ResourceRequest>> AllHandles = new();
    }
}