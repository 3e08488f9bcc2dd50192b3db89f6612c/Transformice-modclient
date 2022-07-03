using System;
using System.Collections.Generic;

namespace Transformice.tfmStandalone
{
	public sealed class SanctionCategory
	{
		public string Description { get; set; }
		public List<Sanction> Sanctions { get; }
		
		public SanctionCategory()
		{
			this.Sanctions = new List<Sanction>();
		}
	}
}
