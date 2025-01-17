using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Asjc.JsonConfig
{
    /// <summary>
    /// Provides options to be used with <see cref="JsonConfig"/>.
    /// </summary>
    public sealed class JsonConfigOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonConfigOptions"/> class.
        /// </summary>
        public JsonConfigOptions() { }

        /// <summary>
        /// Copies the options from a <see cref="JsonConfigOptions"/> instance to a new instance.
        /// </summary>
        /// <param name="options">The options instance to copy options from.</param>
        public JsonConfigOptions(JsonConfigOptions options)
        {
            CreateNew = options.CreateNew;
            SaveNew = options.SaveNew;
            SerializerOptions = options.SerializerOptions;
        }

        /// <summary>
        /// Gets or sets a <see langword="bool"/> indicating whether to create and return a new config if the config file does not exist.
        /// </summary>
        public bool CreateNew { get; set; } = true;

        /// <summary>
        /// Gets or sets a <see langword="bool"/> indicating whether to save the newly created config immediately.
        /// </summary>
        public bool SaveNew { get; set; } = true;

        /// <summary>
        /// Gets or sets the specified <see cref="JsonSerializerOptions"/>.
        /// </summary>
        public JsonSerializerOptions SerializerOptions { get; set; } = new()
        {
            AllowTrailingCommas = true,
            Converters = { new JsonStringEnumConverter() },
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
            NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString,
            PropertyNameCaseInsensitive = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            WriteIndented = true
        };
    }
}
