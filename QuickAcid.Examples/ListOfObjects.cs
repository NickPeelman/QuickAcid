﻿using System.Collections.Generic;
using System.Linq;
using QuickMGenerate;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickAcid.Examples
{
    public class ListOfObjects
    {
        [Fact]
        public void Check()
        {
            TheRun.Verify(10, 20);
        }

        private static readonly QAcidRunner<Unit> TheRun =
            from container in "setup".OnceOnlyInput(() => new TheContainer())
            from addRemove in "add/remove".Choose(Add(container), Remove(container))
            select Unit.Instance;

        private static QAcidRunner<Unit> Add(TheContainer container)
        {
            return 
                from input in "add input".Act(() => MGen.One<TheObject>().Generate())
                from add in "add".Act(() => container.Add(input))
                from added in "added".Spec((() => container.List.Contains(input)))
                select Unit.Instance;
        }

        private static QAcidRunner<Unit> Remove(TheContainer container)
        {
            return (
                from index in "index".Input(MGen.Int(0, container.List.Count - 1))
                from input in "remove input".Act(() => container.List[index])
                from remove in "remove".Act(() => container.Remove(input))
                from removed in "removed".Spec((() => !container.List.Contains(input)))
                select Unit.Instance)
                .If(() => container.List.Any());
        }

        public class TheContainer
        {
            public List<TheObject> List { get; set; }

            public TheContainer()
            {
                List = new List<TheObject>();
            }

            public void Add(TheObject obj)
            {
                List.Add(obj);
            }

            public void Remove(TheObject obj)
            {
                var ix = List.IndexOf(obj);
                if (ix == 0)
                    return;
                List.Remove(obj);
            }
        }

        public class TheObject
        {
            public int AnInt { get; set; }
        }
    }
}