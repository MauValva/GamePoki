using System.Collections.Generic;
using UnityEngine;

namespace KrolStudio
{
    [System.Serializable]
    class ExplosionPartInfo
    {
        public string name;
        public List<Rigidbody> explodeParts;
        public List<Renderer> explodeRenderer;
    }
}