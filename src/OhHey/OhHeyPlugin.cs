// Copyright (c) 2025 MeiHasCrashed
// SPDX-License-Identifier: AGPL-3.0-or-later

using Dalamud.Plugin;
using Dalamud.Plugin.Services;
using JetBrains.Annotations;
using Microsoft.Extensions.DependencyInjection;
using OhHeyFixed.Core.IoC;
using OhHeyFixed.Listeners;
using OhHeyFixed.Services;
using OhHeyFixed.UI;

namespace OhHeyFixed;

[UsedImplicitly(ImplicitUseKindFlags.InstantiatedNoFixedConstructorSignature)]
public sealed class OhHeyFixedPlugin : IDalamudPlugin
{
    private readonly IServiceProvider _provider;
    public OhHeyFixedPlugin(IDalamudPluginInterface pluginInterface)
    {
        var services = new ServiceCollection();
        services
            .AddSingleton(pluginInterface)
            .AddDalamudService<IPluginLog>()
            .AddDalamudService<IFramework>()
            .AddDalamudService<IClientState>()
            .AddDalamudService<IObjectTable>()
            .AddDalamudService<ITargetManager>()
            .AddDalamudService<IGameInteropProvider>()
            .AddDalamudService<IDataManager>()
            .AddDalamudService<IChatGui>()
            .AddDalamudService<ICommandManager>()
            .AddDalamudService<ICondition>()
            .AddSingleton<ConfigurationService>()
            .AddSingleton<EmoteListener>()
            .AddSingleton<EmoteService>()
            .AddSingleton<TargetListener>()
            .AddSingleton<TargetService>()
            .AddDalamudWindow<ConfigurationWindow>()
            .AddDalamudWindow<MainWindow>()
            .AddSingleton<KeyedWindowService>()
            .AddSingleton<WindowService>()
            .AddSingleton<ChatCommandService>();

        _provider = services.BuildServiceProvider();
        _ = _provider.GetRequiredService<WindowService>();
        _ = _provider.GetRequiredService<ChatCommandService>();
    }

    public void Dispose()
    {
        (_provider as IDisposable)?.Dispose();
    }
}
