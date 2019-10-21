using System;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Crystal.ECS;
using Microsoft.Xna.Framework.Content;

namespace Crystal.Engine.SceneUtil
{
    public class SceneInitializer
    {
        public static Dictionary<string, SceneInitializer> Initializers { get; private set; }
            = new Dictionary<string, SceneInitializer>();

        public readonly string Name;

        public Dictionary<string, string> Resources = new Dictionary<string, string>();

        public List<Type> Systems = new List<Type>();

        public List<Type> Renderers = new List<Type>();

        public List<EntityModel> Entities = new List<EntityModel>();

        public SceneInitializer(string name)
        {
            this.Name = name;

            Initializers.Add(name, this);
        }

        /// <summary>
        /// Initializes a scene
        /// </summary>
        /// <param name="s">The scene to be initalized</param>
        public void Initialize(ECS.Scene scene, ContentManager content)
        {
            // Load resources
            foreach (var res in this.Resources)
            {
                scene.AddResource(res.Key, content.Load<object>(res.Value));
            }

            // Add systems
            foreach (var system in this.Systems)
            {
                scene.Add((ISystem)Activator.CreateInstance(system));
            }

            // Add renderers
            foreach (var renderer in this.Renderers)
            {
                scene.Add((IRenderer)Activator.CreateInstance(renderer));
            }

            // Create entities
            foreach (var entity in this.Entities)
            {
                var e = scene.Entity(entity.Name);

                foreach (var component in entity.Components)
                {
                    var c = (IComponent)Activator.CreateInstance(
                        component.Type,
                        BindingFlags.CreateInstance |
                        BindingFlags.Public |
                        BindingFlags.Instance |
                        BindingFlags.OptionalParamBinding |
                        BindingFlags.CreateInstance,
                        null,
                        loadReferences(
                            component.CtorArgs,
                            scene
                        ),
                        CultureInfo.CurrentCulture
                    );

                    e.Add(c);
                }
            }

            scene.Initialized = true;
        }

        /// <summary>
        /// Instantiates objects from ObjectModels inside the arguments
        /// Also loads references to scene resources
        /// </summary>
        private object[] loadReferences(object[] args, ECS.Scene scene)
        {
            var res = new object[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] is Resource r)
                {
                    res[i] = scene.Resource<object>(r.Name);
                }
                else if (args[i] is ObjectModel c)
                {
                    res[i] = loadReferences(c.CtorArgs, scene);
                }
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
    /// A list is just a name associated with a list 
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
                this.CtorArgs.Select(arg => arg.ToString()).ToArray()
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