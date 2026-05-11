// Copyright (c) 2025 MeiHasCrashed
// SPDX-License-Identifier: AGPL-3.0-or-later

using Dalamud.Plugin;
using Dalamud.Plugin.Services;

namespace OhHeyFixed.Services;

public class ConfigurationService
{
    private readonly IDalamudPluginInterface _pluginInterface;
    private readonly IPluginLog _logger;

    public OhHeyFixedConfiguration Configuration { get; }

    public event EventHandler<OhHeyFixedConfiguration>? ConfigurationChanged;

    public ConfigurationService(IPluginLog logger, IDalamudPluginInterface pluginInterface)
    {
        _logger = logger;
        _pluginInterface = pluginInterface;
        Configuration = LoadConfiguration();
    }

    private OhHeyFixedConfiguration LoadConfiguration()
    {
        if (_pluginInterface.GetPluginConfig() is not OhHeyFixedConfiguration config)
        {
            config = new OhHeyFixedConfiguration();
            _logger.Info("No existing configuration found. Creating new configuration with version {ConfigurationVersion}.",
                config.Version);
            _pluginInterface.SavePluginConfig(config);
        }
        else
        {
            _logger.Info("Configuration loaded. Version: {Version}", config.Version);
        }

        return config;
    }

    public void Save()
    {
        _pluginInterface.SavePluginConfig(Configuration);
        _logger.Debug("Configuration saved.");
        OnConfigurationChanged();
    }


    private void OnConfigurationChanged()
    {
        var handler = ConfigurationChanged;
        try
        {
            handler?.Invoke(this, Configuration);
        }
        catch (Exception ex)
        {
            _logger.Error(ex, "Error invoking ConfigurationChanged event.");
        }
    }
}
