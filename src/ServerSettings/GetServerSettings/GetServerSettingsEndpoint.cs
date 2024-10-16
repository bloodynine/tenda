﻿using MongoDB.Entities;
using Tenda.Shared;
using Tenda.Shared.Models;
using Tenda.Users;

namespace Tenda.ServerSettings.GetServerSettings;

public class GetServerSettingsEndpoint : EndpointWithoutRequest<GetServerSettingsResponse>
{
    public override void Configure()
    {
        Get("/api/settings/server");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var settings = await DB.Find<ServerSettingsDoc>().ExecuteFirstAsync(ct);
        if (settings is null)
        {
            settings ??= new ServerSettingsDoc() { AllowSignUps = false, UseKeyCloak = false };
            await DB.SaveAsync(settings, cancellation: ct);
        }

        var users = await DB.CountAsync<User>(cancellation: ct);
        await SendAsync(new GetServerSettingsResponse(settings, users == 0), cancellation: ct);
    }
}