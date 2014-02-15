using System;
using System.Collections.Generic;
using System.Linq;
using AIPlanning.Core;
using AIPlanning.Core.Interfaces;

namespace AIPlanning.Search
{
    internal sealed class PathTracker<T>
    {
        private readonly Dictionary<ISearchNode<T>, NodeTraceInfo> _nodeInfoStorage = new Dictionary<ISearchNode<T>, NodeTraceInfo>();

        public void TrackNode(ISearchNode<T> node, ISearchNode<T> parent)
        {
            if(node==null) throw new ArgumentNullException("node");
            if(parent != null && !_nodeInfoStorage.ContainsKey(parent))
                throw new ArgumentException("Untracked parent");

            int parentDepth = parent == null ? -1 : _nodeInfoStorage[parent].Depth;
            var nodeInfo = new NodeTraceInfo(parentDepth + 1, parent);
            _nodeInfoStorage[node] = nodeInfo;
        }

        public bool IsTracked(ISearchNode<T> node)
        {
            if(node == null) throw new ArgumentNullException("node");

            return _nodeInfoStorage.ContainsKey(node);
        }

        public Path<T> GetPath(ISearchNode<T> node)
        {
            if(node == null) throw new ArgumentNullException("node");
            if(!_nodeInfoStorage.ContainsKey(node)) 
                throw new ArgumentException("Untracked node");

            var nodePath = TrackPath(node)
                .Reverse();
            var nodeDepth = _nodeInfoStorage[node].Depth;

            var result = new Path<T>(nodePath, nodeDepth);
            return result;
        }

        public int GetDepth(ISearchNode<T> node)
        {
            if (node == null) throw new ArgumentNullException("node");
            if (!_nodeInfoStorage.ContainsKey(node))
                throw new ArgumentException("Untracked node");

            return _nodeInfoStorage[node].Depth;
        }

        private IEnumerable<ISearchNode<T>> TrackPath(ISearchNode<T> node)
        {
            var currentNode = node;
            while (currentNode != null)
            {
                yield return currentNode;
                currentNode = _nodeInfoStorage[currentNode].Parent;
            }
        }

        private struct NodeTraceInfo
        {
            public readonly int Depth;
            public readonly ISearchNode<T> Parent;

            public NodeTraceInfo(int depth, ISearchNode<T> parent) : this()
            {
                Depth = depth;
                Parent = parent;
            }
        }
    }
}