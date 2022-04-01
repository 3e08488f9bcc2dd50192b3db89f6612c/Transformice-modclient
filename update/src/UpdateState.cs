using System;

namespace Update
{
	public enum UpdateState
	{
		CheckingForTransformiceExe,
		Downloading, // Download the executable from server
		Updating // Extract and replace it.
	}
}
