﻿using System.Text.Json;
using System.Text.Json.Serialization;

namespace Asjc.JsonConfig
{
    /// <summary>
    /// Provides an implementation for quickly loading and saving configurations via JSON.
    /// </summary>
    public abstract class JsonConfig
    {
        /// <summary>
        /// Gets or sets the path to the config file.
        /// </summary>
        [JsonIgnore]
#pragma warning disable CS8618
        public string Path { get; set; }
#pragma warning restore CS8618

        /// <summary>
        /// Gets or sets the default <see cref="Path"/> for the current class.
        /// </summary>
        protected virtual string DefaultPath => $"{GetType().Name}.json";

        /// <summary>
        /// Gets or sets the options to use.
        /// </summary>
        [JsonIgnore]
#pragma warning disable CS8618
        public JsonConfigOptions Options { get; set; }
#pragma warning restore CS8618

        /// <summary>
        /// Gets or sets the default <see cref="Options"/> for the current class.
        /// </summary>
        protected virtual JsonConfigOptions DefaultOptions => GlobalOptions;

        /// <summary>
        /// Gets or sets the global default <see cref="Options"/>.
        /// </summary>
        public static JsonConfigOptions GlobalOptions { get; set; } = new();

        /// <summary>
        /// Gets the JSON string representation of the current object.
        /// </summary>
        [JsonIgnore]
        public string Json => JsonSerializer.Serialize(this, GetType());

        /// <summary>
        /// Occurs when reading a config file.
        /// </summary>
        public event Action<JsonConfig>? Reading;

        /// <summary>
        /// Occurs when creating a new config.
        /// </summary>
        public event Action<JsonConfig>? Creating;

        /// <summary>
        /// Occurs when the config is loaded.
        /// </summary>
        public event Action<JsonConfig>? Loaded;

        /// <summary>
        /// Occurs before saving the config.
        /// </summary>
        public event Action<JsonConfig>? BeforeSave;

        /// <summary>
        /// Occurs after saving the config.
        /// </summary>
        public event Action<JsonConfig>? AfterSave;

        /// <summary>
        /// Raises the <see cref="Reading"/> event.
        /// </summary>
        protected virtual void OnReading() => Reading?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Creating"/> event.
        /// </summary>
        protected virtual void OnCreating() => Creating?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="Loaded"/> event.
        /// </summary>
        protected virtual void OnLoaded() => Loaded?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="BeforeSave"/> event.
        /// </summary>
        protected virtual void OnBeforeSave() => BeforeSave?.Invoke(this);

        /// <summary>
        /// Raises the <see cref="AfterSave"/> event.
        /// </summary>
        protected virtual void OnAfterSave() => AfterSave?.Invoke(this);

        /// <summary>
        /// Load the config generally.
        /// </summary>
        /// <typeparam name="T">The type of config to be loaded.</typeparam>
        /// <returns>The loaded config.</returns>
        public static T? Load<T>() where T : JsonConfig, new() => Load<T>(new T().DefaultPath, new T().DefaultOptions);

        /// <inheritdoc cref="Load{T}()"/>
        public static T? Load<T>(string path) where T : JsonConfig, new() => Load<T>(path, new T().DefaultOptions);

        /// <inheritdoc cref="Load{T}()"/>
        public static T? Load<T>(JsonConfigOptions options) where T : JsonConfig, new() => Load<T>(new T().DefaultPath, options);

        /// <inheritdoc cref="Load{T}()"/>
        public static T? Load<T>(string path, JsonConfigOptions options) where T : JsonConfig, new()
        {
            T? config = default;
            if (File.Exists(path))
                config = Read<T>(path, options);
            else if (options.CreateNew)
                config = Create<T>(path, options);
            config?.OnLoaded();
            return config;
        }

        /// <summary>
        /// Load the config only by reading the config file.
        /// </summary>
        /// <typeparam name="T">The type of config to be loaded.</typeparam>
        /// <returns>The loaded config.</returns>
        public static T? Read<T>() where T : JsonConfig, new() => Read<T>(new T().DefaultPath, new T().DefaultOptions);

        /// <inheritdoc cref="Read{T}()"/>
        public static T? Read<T>(string path) where T : JsonConfig, new() => Read<T>(path, new T().DefaultOptions);

        /// <inheritdoc cref="Read{T}()"/>
        public static T? Read<T>(JsonConfigOptions options) where T : JsonConfig, new() => Read<T>(new T().DefaultPath, options);

        /// <inheritdoc cref="Read{T}()"/>
        public static T? Read<T>(string path, JsonConfigOptions options) where T : JsonConfig, new()
        {
            string json = File.ReadAllText(path);
            T? config = JsonSerializer.Deserialize<T>(json, options.SerializerOptions);
            if (config != null)
            {
                config.OnReading();
                config.Path = path;
                config.Options = options;
            }
            return config;
        }

        /// <summary>
        /// Load the config only by creating a new one.
        /// </summary>
        /// <typeparam name="T">The type of config to be loaded.</typeparam>
        /// <returns>The loaded config.</returns>
        public static T Create<T>() where T : JsonConfig, new() => Create<T>(new T().DefaultPath, new T().DefaultOptions);

        /// <inheritdoc cref="Create{T}()"/>
        public static T Create<T>(string path) where T : JsonConfig, new() => Create<T>(path, new T().DefaultOptions);

        /// <inheritdoc cref="Create{T}()"/>
        public static T Create<T>(JsonConfigOptions options) where T : JsonConfig, new() => Create<T>(new T().DefaultPath, options);

        /// <inheritdoc cref="Create{T}()"/>
        public static T Create<T>(string path, JsonConfigOptions options) where T : JsonConfig, new()
        {
            T config = new();
            config.OnCreating();
            config.Path = path;
            config.Options = options;
            if (options.SaveNew)
                config.Save();
            return config;
        }

        /// <summary>
        /// Save the current config to a file.
        /// </summary>
        public void Save() => Save(Path, Options);

        /// <inheritdoc cref="Save()"/>
        public void Save(string path) => Save(path, Options);

        /// <inheritdoc cref="Save()"/>
        public void Save(JsonConfigOptions options) => Save(Path, options);

        /// <inheritdoc cref="Save()"/>
        public void Save(string path, JsonConfigOptions options)
        {
            OnBeforeSave();
            string json = JsonSerializer.Serialize(this, GetType(), options.SerializerOptions);
            File.WriteAllText(path, json);
            OnAfterSave();
        }

        /// <inheritdoc/>
        public override string ToString() => Json;
    }
}
