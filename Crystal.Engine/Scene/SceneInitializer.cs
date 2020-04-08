using System;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Crystal.Framework;
using Crystal.Framework.Graphics;
using Crystal.Engine.Content;

namespace Crystal.Engine.Scene
{
    public class SceneInitializer
    {
        private readonly List<ObjectModel> systems, renderers;
        private readonly List<EntityModel> entities;
        private readonly List<InputAction> actions;
        private readonly Point size;
        private readonly IScaler scaler;

        // TODO: Rethink
        /// <summary>
        /// The fully classified name of the theme class that will
        /// be used as the scene default UI theme
        /// </summary>
        public string ThemeClass = "";

        public SceneInitializer(Point size,
                                IScaler scaler,
                                List<ObjectModel> systems,
                                List<ObjectModel> renderers,
                                List<EntityModel> entities,
                                List<InputAction> actions)
        {
            this.size = size;
            this.scaler = scaler;
            this.systems = systems;
            this.renderers = renderers;
            this.entities = entities;
            this.actions = actions;
        }

        public IEnumerable<ISystem> GetSystems(ContentManager cm)
        {
            foreach (var systemModel in systems)
            {
                yield return (ISystem)loadObject(systemModel, cm);
            }
        }

        public IEnumerable<IRenderer> GetRenderers(ContentManager cm)
        {
            foreach (var rendererModel in renderers)
            {
                yield return (IRenderer)loadObject(rendererModel, cm);
            }
        }

        public IEnumerable<IEntity> GetEntities(ContentManager cm)
        {
            foreach (var entityModel in entities)
            {
                var e = new Entity(entityModel.Name);
                foreach (var componentModel in entityModel.Components)
                {
                    e.Add((IComponent)loadObject(componentModel, cm));
                }
                yield return e;
            }
        }

        public IEnumerable<InputAction> GetActions()
        {
            return this.actions;
        }

        public IScaler GetScaler()
        {
            return this.scaler;
        }

        public Point GetSize()
        {
            return this.size;
        }

        private object loadObject(ObjectModel o, ContentManager cm)
        {
            try
            {
                if (o.Type.IsEnum)
                {
                    if (o.CtorArgs.Length == 1 && o.CtorArgs[0] is string enumValue)
                    {
                        return Enum.Parse(o.Type, enumValue);
                    }

                    throw new Exception("Invalid number of arguments to enum!");
                }

                return Activator.CreateInstance(
                    o.Type,
                    BindingFlags.CreateInstance |
                    BindingFlags.Public |
                    BindingFlags.Instance |
                    BindingFlags.OptionalParamBinding,
                    null,
                    loadObjects(
                        o.CtorArgs,
                        cm
                    ),
                    CultureInfo.CurrentCulture
                );
            }
            catch (MissingMethodException)
            {
                throw new Exception("The object you informed could not be created. " +
                                    $"Is this constructor call right?\n\"{o}\"");
            }
        }

        /// <summary>
        /// Instantiates objects from ObjectModels inside the arguments
        /// Also loads references to scene resources
        /// </summary>
        private object[] loadObjects(object[] args, ContentManager cm)
        {
            var res = new object[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                // args[i] is a object we have to build
                if (args[i] is ObjectModel c)
                {
                    res[i] = loadObject(c, cm);

                    // Edge case: if the object was
                    // of type Resource, we have to
                    // load it instead
                    if (res[i] is Resource r)
                    {
                        res[i] = cm.Load(r.Path);
                    }
                }
                // args[i] is a primitive
                else
                {
                    res[i] = args[i];
                }
            }

            return res;
        }

        public override string ToString()
        {
            var res = "";

            res += "\nSystems:\n";
            foreach (var sys in this.systems)
            {
                res += $"  - {sys}\n";
            }

            res += "\nRenderers:\n";
            foreach (var rend in this.renderers)
            {
                res += $"  - {rend}\n";
            }

            res += "\nEntities:\n";
            foreach (var e in this.entities)
            {
                res += $"  - {e.Name}\n";
                foreach (var component in e.Components)
                {
                    res += $"    - {component}\n";
                }
            }

            return res;
        }
    }

    /// <summary>
    /// A entity is just a name associated with a list
    /// of objects, that we can hopefuly cast to IComponent
    /// </summary>
    public struct EntityModel
    {
        public string Name;
        public List<ObjectModel> Components;
    }

    /// <summary>
    /// A object is basically just it's class and a array
    /// of parameters we pass to it's constructor
    /// </summary>
    public struct ObjectModel
    {
        public Type Type;
        public object[] CtorArgs;

        public override string ToString()
        {
            var res = $"new {this.Type}(";

            res += string.Join(
                ", ",
                this.CtorArgs.Select(arg =>
                {
                    if (arg is string)
                    {
                        return '"' + arg.ToString() + '"';
                    }
                    else if (arg is char)
                    {
                        return "'" + arg.ToString() + "'";
                    }
                    return arg != null ? arg.ToString() : "null";
                })
                .ToArray()
            );

            res += ")";

            return res;
        }
    }

    /// <summary>
    /// Represents a reference to a resource
    /// </summary>
    public struct Resource
    {
        public readonly string Path;

        public Resource(string path)
        {
            this.Path = path;
        }

        public override string ToString()
        {
            return $"Resource(\"{this.Path}\")";
        }
    }
}