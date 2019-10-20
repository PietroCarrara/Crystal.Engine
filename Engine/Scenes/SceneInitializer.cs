using System;
using System.Linq;
using System.Reflection;
using System.Globalization;
using System.Collections.Generic;
using Crystal.ECS;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Crystal.Engine.Scenes
{
    public class SceneInitializer
    {
        public static Dictionary<string, SceneInitializer> Initializers { get; private set; }
            = new Dictionary<string, SceneInitializer>();

        public readonly string Name;

        public Dictionary<string, string> Resources = new Dictionary<string, string>();

        public List<Type> Systems = new List<Type>();

        public List<Type> Renderers = new List<Type>();

        /// <summary>
        /// List of Entities
        /// A Entity is a name string and a List of Components
        /// A Component is a Type (the Component class) and a List of objects (the constructor parameters)
        /// </summary>
        public List<Tuple<string, List<Tuple<Type, List<object>>>>> Entities
            = new List<Tuple<string, List<Tuple<Type, List<object>>>>>();

        public SceneInitializer(string name)
        {
            this.Name = name;

            Initializers.Add(name, this);
        }

        /// <summary>
        /// Initializes a scene
        /// </summary>
        /// <param name="s">The scene to be initalized</param>
        public void Initialize(Scene scene, ContentManager content)
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
                var e = scene.Entity(entity.Item1);

                foreach (var component in entity.Item2)
                {
                    var c = (IComponent)Activator.CreateInstance(
                        component.Item1,
                        BindingFlags.CreateInstance|
                        BindingFlags.Public|
                        BindingFlags.Instance|
                        BindingFlags.OptionalParamBinding|
                        BindingFlags.CreateInstance,
                        null,
                        loadReferences(
                            component.Item2.ToArray(),
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
        /// Replaces objects of type SceneResource with the resource they actually point to
        /// </summary>
        /// <returns></returns>
        private object[] loadReferences(object[] args, Scene scene)
        {
            var res = new object[args.Length];

            for (var i = 0; i < args.Length; i++)
            {
                if (args[i] is SceneResource r)
                {
                    res[i] = scene.Resource<object>(r.Name);
                }
                else
                {
                    res[i] = args[i];
                }
            }

            return res;
        }
    }

}