﻿using System;
using QuickMGenerate;
using QuickMGenerate.UnderTheHood;
using Xunit;

namespace QuickAcid.Tests
{
	public class MultipleMethods
	{
		public class BugHouse
		{
			private string bug;
			public bool RunInt(int a)
			{
				bug += "1";
				if (bug.EndsWith("1221") && a == 6)
					throw new Exception(); 
				return true;
			}

			public bool RunString(string a)
			{
				bug += "2";
				if (bug.EndsWith("122") && a == "p")
					throw new Exception(); 
				return true;
			}
		}

		[Fact]
		public void BugHouseError()
		{

			var test =
				from bughouse in QAcid.OnceOnlyInput("bughouse", () => new BugHouse())
				from funcOne in
					QAcid.Choose(
						"Choose",

						from i in MGen.Int(0, 10).ShrinkableInput("int")
						from runInt in QAcid.Act("bughouse.RunInt", () => bughouse.RunInt(i))
						from specOne in QAcid.Spec("returns true", () => runInt)
						select Unit.Instance,

						from str in MGen.String(1, 1).ShrinkableInput("string")
						from runString in QAcid.Act("bughouse.RunString", () => bughouse.RunString(str))
						from specTwo in QAcid.Spec("returns true", () => runString)
						select Unit.Instance)

				select Unit.Instance;

			test.Verify(100, 100);
		}

		[Fact]
		public void BugHouseErrorStringyfied()
		{

			var test =
				from bughouse in "bughouse".OnceOnlyInput(() => new BugHouse())
				from funcOne in
					"Choose".Choose(
						from i in "int".ShrinkableInput(MGen.Int(0, 10))
						from runInt in "bughouse.RunInt".Act(() => bughouse.RunInt(i))
						from specOne in "returns true".Spec(() => runInt)
						select Unit.Instance,

						from str in "string".ShrinkableInput(MGen.String(1, 1))
						from runString in "bughouse.RunString".Act(() => bughouse.RunString(str))
						from specTwo in "returns true".Spec(() => runString)
						select Unit.Instance)

				select Unit.Instance;

			test.Verify(100, 100);
		}
	}
}