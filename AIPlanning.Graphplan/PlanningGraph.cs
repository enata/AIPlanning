using AIPlanning.Pddl;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AIPlanning.Graphplan
{
    public sealed class PlanningGraph
    {
        private readonly List<Layer<Proposition>> _propositionLayers = new List<Layer<Proposition>>();
        private readonly List<Layer<Action>> _actionLayers = new List<Layer<Action>>();
        private readonly Dictionary<int, List<PropositionLink>> _propositionLinks = new Dictionary<int, List<PropositionLink>>();
        private readonly Dictionary<int, List<ActionLink>> _actionLinks = new Dictionary<int, List<ActionLink>>();

        private readonly Dictionary<Action, HashSet<Proposition>> _actionPositiveEffects;
        private readonly Dictionary<Action, HashSet<Proposition>> _actionNegativeEffects;

        
        private readonly Dictionary<Action, PddlAction> _substitutedActions;
        private int _currentId;

        internal PlanningGraph(IEnumerable<PddlPredicateCall> initialState, Dictionary<Action, PddlAction> substitutedActions, ActionEffectsExtractor effectsExtractor)
        {
            _substitutedActions = substitutedActions;
            _actionPositiveEffects = effectsExtractor.ActionPositiveEffects;
            _actionNegativeEffects = effectsExtractor.ActionNegativeEffects;

            BuildLayer0(initialState);
        }

        public PlanningGraph(IEnumerable<PddlPredicateCall> initialState, Dictionary<Action, PddlAction> substitutedActions) 
            :this(initialState, substitutedActions, new ActionEffectsExtractor(substitutedActions))
        {}

        public List<Layer<Proposition>> PropositionLayers { get { return _propositionLayers; } }
        public List<Layer<Action>> ActionLayers { get { return _actionLayers; } }
        public Dictionary<int, List<PropositionLink>> PropositionLinks { get { return _propositionLinks; } }
        public Dictionary<int, List<ActionLink>> ActionLinks { get { return _actionLinks; } }

        public bool IsFixedPoint()
        {
            var result = _propositionLayers.Count > 1 &&
                         _propositionLayers[_propositionLayers.Count - 1].Elements.Count ==
                         _propositionLayers[_propositionLayers.Count - 2].Elements.Count;
            return result;
        }

        public void Expand()
        {

            var applicableActions = _substitutedActions.Where(sa => CheckPreconditions(sa.Value, sa.Key.Parameters)).ToArray();
            var lastPropositionLayer = _propositionLayers[_propositionLayers.Count - 1];

            var actionLayer = new Layer<Action>(applicableActions.Select(aa => aa.Key), _currentId);
            _actionLayers.Add(actionLayer);
            var propositionLinks = new List<PropositionLink>();
            var positiveEffects = new HashSet<Proposition>();
            var actionLinks = new List<ActionLink>();
            foreach (var applicableAction in applicableActions)
            {
                var actionPositiveEffects = _actionPositiveEffects[applicableAction.Key];
                var actionNegativeEffects = _actionNegativeEffects[applicableAction.Key];
                var action = applicableAction;
                var links =
                    actionPositiveEffects.Select(
                        ape => new PropositionLink(ape, action.Key, _currentId));
                propositionLinks.AddRange(links);
                positiveEffects.UnionWith(actionPositiveEffects);
                actionLinks.AddRange(
                    actionPositiveEffects.Select(
                        ape => new ActionLink(action.Key, ape, _currentId, false)));
                actionLinks.AddRange(
                    actionNegativeEffects.Select(ane => new ActionLink(action.Key, ane, _currentId, true)));
            }
            _propositionLinks[_currentId] = propositionLinks;
            _actionLinks[_currentId] = actionLinks;
            var nextLayerElements = lastPropositionLayer.Elements.Concat(positiveEffects);
            var propositionLayer = new Layer<Proposition>(nextLayerElements, _currentId);
            _propositionLayers.Add(propositionLayer);
            _currentId++;
        }

        

        private void BuildLayer0(IEnumerable<PddlPredicateCall> inititalState)
        {
            var elements = inititalState.Select(ps => new Proposition(ps.Name, ps.Parameters));
            var layer = new Layer<Proposition>(elements, _currentId);
            _propositionLayers.Add(layer);
            _currentId++;
        }

        

        private bool CheckPreconditions(PddlAction action, IEnumerable<string> substitutions)
        {
            var substitutionDictionary = new SubstitutionDictionary(action, substitutions);
            var clauseWalker = new SubstitutingClauseWalker(substitutionDictionary.DictionaryInstance);
            clauseWalker.Walk(action.Precondition);
            var lastLayer = _propositionLayers[_propositionLayers.Count - 1];
            var result = lastLayer.Elements.IsSupersetOf(clauseWalker.Included) &&
                         !lastLayer.Elements.Overlaps(clauseWalker.Excluded);
            return result;
        }
         

        public sealed class PropositionLink : Link<Proposition, Action>
        {
            internal PropositionLink(Proposition @from, Action to, int layer)
                : base(@from, to, layer)
            {
            }
        }

        public sealed class ActionLink : Link<Action,Proposition>
        {
            private readonly bool _not;


            internal ActionLink(Action @from, Proposition to, int layer, bool not)
                : base(@from, to, layer)
            {
                _not = not;
            }

            public bool Not
            {
                get { return _not; }
            }
        }

        public abstract class Link<TFrom, TTo> where TFrom: Method where TTo: Method
        {
            private readonly TFrom _from;
            private readonly int _layer;
            private readonly TTo _to;

            protected Link(TFrom @from, TTo to, int layer)
            {
                if (@from == null) throw new ArgumentNullException("from");
                if (to == null) throw new ArgumentNullException("to");

                _from = @from;
                _to = to;
                _layer = layer;
            }

            public TFrom From
            {
                get { return _from; }
            }

            public TTo To
            {
                get { return _to; }
            }

            public int LayerNumber
            {
                get { return _layer; }
            }
        }


        public sealed class Layer<T> where T:Method
        {
            private readonly HashSet<T> _elements;
            private readonly int _id;

            internal Layer(IEnumerable<T> elements, int id)
            {
                _id = id;
                _elements = new HashSet<T>(elements);
            }

            public HashSet<T> Elements
            {
                get { return _elements; }
            }

            public int Id
            {
                get { return _id; }
            }
        }

        public sealed class Proposition : Method
        {
            internal Proposition(string name, string[] parameters)
                : base(name, parameters)
            {
            }
        }

        public sealed class Action : Method
        {
            internal Action(string name, string[] parameters)
                : base(name, parameters)
            {
            }
        }

        public abstract class Method
        {
            private readonly string _name;
            private readonly string[] _parameters;

            protected Method(string name, string[] parameters)
            {
                if (name == null) throw new ArgumentNullException("name");
                if (parameters == null) throw new ArgumentNullException("parameters");

                _name = name;
                _parameters = parameters;
            }

            public string Name
            {
                get { return _name; }
            }


            public string[] Parameters
            {
                get { return _parameters; }
            }

            protected bool Equals(Method other)
            {
                return string.Equals(_name, other._name) && _parameters.Length == other._parameters.Length &&
                       _parameters.Zip(other._parameters, (p, o) => new Tuple<string, string>(p, o))
                                  .All(p => string.Equals(p.Item1, p.Item2));
            }

            public override bool Equals(object obj)
            {
                if (ReferenceEquals(null, obj)) return false;
                if (ReferenceEquals(this, obj)) return true;
                if (obj.GetType() != GetType()) return false;
                return Equals((Method) obj);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    var code = _name.GetHashCode();
                    var result = _parameters.Aggregate(code,
                                                       (aggregated, parameter) =>
                                                       aggregated*397 ^ parameter.GetHashCode());
                    return result;
                }
            }
        }
    }
}