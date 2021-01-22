using System;
using EMS.Core.API;
using Grpc.Net.Client;

namespace TestConsole
{
	class Program
	{
		static void Main(string[] args)
		{
            using GrpcChannel channel = GrpcChannel.ForAddress("https://localhost:5001");
            Teams.TeamsClient client = new Teams.TeamsClient(channel);

            TeamData request = new TeamData
            {
                Name = "Test",
                Description = string.Empty
            };

            EMS.Common.Protos.BaseResponse response = client.AddAsync(request);
		}
	}
}
