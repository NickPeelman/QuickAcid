﻿using System;

namespace QuickAcid
{
	public static class QAcidRunnerToLinq
	{
		public static QAcidRunner<TValueTwo> Select<TValueOne, TValueTwo>(
			this QAcidRunner<TValueOne> runner,
			Func<TValueOne, TValueTwo> selector)
		{
			if (runner == null)
				throw new ArgumentNullException("runner");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return
				s =>
				{
					if (s.Failed)
						return new QAcidResult<TValueTwo>(s, default(TValueTwo));
					return new QAcidResult<TValueTwo>(s, selector(runner(s).Value));
				}; 
			
		}

		// This is the Bind function
		public static QAcidRunner<TValueTwo> SelectMany<TValueOne, TValueTwo>(
			this QAcidRunner<TValueOne> runner,
			Func<TValueOne, QAcidRunner<TValueTwo>> selector)
		{
			if (runner == null)
				throw new ArgumentNullException("runner");
			if (selector == null)
				throw new ArgumentNullException("selector");

			return
				s =>
					{
						if (s.Failed)
							return new QAcidResult<TValueTwo>(s, default(TValueTwo));
						return selector(runner(s).Value)(s);
					};
		}

		public static QAcidRunner<TValueThree> SelectMany<TValueOne, TValueTwo, TValueThree>(
			this QAcidRunner<TValueOne> runner,
			Func<TValueOne, QAcidRunner<TValueTwo>> selector,
			Func<TValueOne, TValueTwo, TValueThree> projector)
		{
			if (runner == null)
				throw new ArgumentNullException("runner");
			if (selector == null)
				throw new ArgumentNullException("selector");
			if (projector == null)
				throw new ArgumentNullException("projector");

			return runner.SelectMany(x => selector(x).Select(y => projector(x, y)));
		}
	}
}
