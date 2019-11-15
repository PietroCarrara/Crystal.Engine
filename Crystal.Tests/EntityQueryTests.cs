using System.Linq;
using NUnit.Framework;
using Crystal.Framework.ECS;
using Crystal.Framework.ECS.Query;

namespace Tests
{
    public class EntityQueryTests
    {
        [Test]
        public void TestContainsAll()
        {
            var s = new Scene("test");

            var eABC = s.Entity()
                .With(new ComponentA())
                .With(new ComponentB())
                .With(new ComponentC());

            var eAAC = s.Entity()
                .With(new ComponentA())
                .With(new ComponentA())
                .With(new ComponentC());


            var resABC = new EntityQuery()
                .HasComponents(
                    typeof(ComponentA),
                    typeof(ComponentB),
                    typeof(ComponentC)
                )
                .Run(s);
            
            var resAA = new EntityQuery()
                .HasComponents(
                    typeof(ComponentA),
                    typeof(ComponentA)
                )
                .Run(s);

            var resAC = new EntityQuery()
                .HasComponents(
                    typeof(ComponentA),
                    typeof(ComponentC)
                )
                .Run(s);

            Assert.True(resABC.Contains(eABC) && resABC.Count() == 1);
            Assert.True(resAA.Contains(eAAC) && resAA.Count() == 1);
            Assert.True(resAC.Contains(eABC) && resAC.Contains(eAAC) && resAC.Count() == 2);
        }
    }

    class ComponentA : IComponent { }
    class ComponentB : IComponent { }
    class ComponentC : IComponent { }
}