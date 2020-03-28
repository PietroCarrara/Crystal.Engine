using System;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Crystal.Framework;
using Crystal.Engine.Input;
using Crystal.Engine.Graphics;
using Crystal.Engine.UI.Themes;

namespace Crystal.Engine.SceneUtil
{
    public class SceneInitializer
    {
        public static Dictionary<string, SceneInitializer> Initializers { get; private set; }
            = new Dictionary<string, SceneInitializer>();

        public readonly string Name;

        public Dictionary<string, string> Resources = new Dictionary<string, string>();

        public List<ObjectModel> Systems = new List<ObjectModel>();

        public List<ObjectModel> Renderers = new List<ObjectModel>();

        public List<EntityModel> Entities = new List<EntityModel>();

        public List<InputAction> Actions = new List<InputAction>();

        /// <summary>
        /// The fully classified name of the theme class that will
        /// be used as the scene default UI theme
        /// </summary>
        public string ThemeClass = "";

        public SceneInitializer(string name)
        {
            this.Name = name;

            Initializers.Add(name, this);
        }

        /// <summary>
        /// Initializes a scene
        /// </summary>
        /// <param name="game">The game</param>
        /// <param name="scene">The scene to be initalized</param>
        public void Initialize(CrystalGame game, Scene scene)
        {
            // Load resources
            foreach (var res in this.Resources)
            {
                scene.AddResource(
                    res.Key,
                    game.Content.Load<object>(res.Value)
                );
            }

            // Add systems
            foreach (var system in this.Systems)
            {
                var s = loadObject(system, scene);
                scene.Add((ISystem)s);
            }

            // Add renderers
            foreach (var renderer in this.Renderers)
            {
                var r = loadObject(renderer, scene);
                scene.Add((IRenderer)r);
            }

            // Create entities
            foreach (var entity in this.Entities)
            {
                var e = scene.Entity(entity.Name);

                foreach (var component in entity.Components)
                {
                    var c = (IComponent)loadObject(component, scene);

                    e.Add(c);
                }
            }

            // Add Actions
            foreach (var action in this.Actions)
            {
                scene.Actions.Add(action);
            }

            // TODO: The rest of this initialization should be done with Crystal.Framework.LowLevel

            // Initialize the game drawer
            scene.Drawer = new CrystalDrawer(game);

            scene.Content = game.Content;

            // TODO: Load named theme
            scene.Theme = this.ThemeClass == "" ? new KenneyTheme() : null;
            scene.Theme.Load(scene.Content);

            // Call the user-defined code
            // for system initialization
            scene.Initialize();
        }

        private object loadObject(ObjectModel o, Scene s)
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
                    BindingFlags.OptionalParamBinding |
                    BindingFlags.CreateInstance,
                    null,
                    loadModels(
                        o.CtorArgs,
                        s
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
        private object[] loadModels(object[] args, Scene scene)
        {
            var res = new object[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                // args[i] is a object we have to build
                if (args[i] is ObjectModel c)
                {
                    res[i] = loadObject(c, scene);

                    // Edge case: if the object was
                    // of type Resource, we have to
                    // load it instead
                    if (res[i] is Resource r)
                    {
                        res[i] = scene.Resource<object>(r.Name);
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

            res += $"Name:\n  - {this.Name}\n";

            res += "\nSystems:\n";
            foreach (var sys in this.Systems)
            {
                res += $"  - {sys}\n";
            }

            res += "\nRenderers:\n";
            foreach (var rend in this.Renderers)
            {
                res += $"  - {rend}\n";
            }

            res += "\nEntities:\n";
            foreach (var e in this.Entities)
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

}

// Make the resource type accessible
// in a namespace that makes sense
namespace Crystal.Engine
{
    /// <summary>
    /// Represents a reference to a scene resource
    /// </summary>
    public struct Resource
    {
        public string Name;

        public Resource(string name)
        {
            this.Name = name;
        }

        public override string ToString()
        {
            return $"Scene.Resource(\"{this.Name}\")";
        }
    }
}