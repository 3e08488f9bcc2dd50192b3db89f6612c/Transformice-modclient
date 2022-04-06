using System;
using System.Collections.Generic;
using System.Net;
using Newtonsoft.Json;

namespace tfmClientHook
{
	internal static class ServerInfo
	{
		public static void Clear() { }
		public static sbyte[] EncryptionKey;
		public static int[] EncryptionVector;
		public static IncomingTribullePackets IncomingTribulleCodes;
		public static OutgoingTribullePackets OutgoingTribulleCodes;
		public static Dictionary<int, string> TribulleCommunities;
		public static void Initialize()
		{
			ServerInfo.TribulleCommunities = new Dictionary<int, string>();
			using (WebClient webClient = new WebClient())
			{
				webClient.Proxy = null;
				webClient.Headers[HttpRequestHeader.Authorization] = "Basic YldtWEp4YzROUDBJMFE6WTluc2x2UiQ1TEc5YlJS";
				webClient.DownloadStringCompleted += delegate(object sender, DownloadStringCompletedEventArgs args)
				{
					if (args.Error == null)
					{
						ServerData serverData = JsonConvert.DeserializeObject<ServerData>(args.Result);
						ServerInfo.IncomingTribulleCodes = serverData.IncomingTribullePacketIds;
						ServerInfo.OutgoingTribulleCodes = serverData.OutgoingTribullePacketIds;
						serverData.Communities.ForEach(delegate(TribulleCommunity c)
						{
							ServerInfo.TribulleCommunities.Add(c.Id, c.Community.ToLowerInvariant());
						});
					}
				};
				webClient.DownloadStringAsync(new Uri("http://localhost/data/data.json"));
			}
		}
	}
}
