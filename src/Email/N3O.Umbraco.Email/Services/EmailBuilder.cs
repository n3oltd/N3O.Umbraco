using N3O.Umbraco.Scheduler;
using System;

namespace N3O.Umbraco.Email {
	public class EmailBuilder : IEmailBuilder {
		private readonly Lazy<IBackgroundJob> _backgroundJob;

		public EmailBuilder(Lazy<IBackgroundJob> backgroundJob) {
			_backgroundJob = backgroundJob;
		}
		
		public IFluentEmailBuilder Create() {
			return new FluentEmailBuilder(_backgroundJob);
		}
	}
}
