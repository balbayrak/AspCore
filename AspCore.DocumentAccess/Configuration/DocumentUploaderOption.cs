namespace AspCore.DocumentAccess.Configuration
{
    public class DocumentUploaderOption : DocumentAccessOption
    {
        /// <summary>
        /// This key used for validation info. Get info with configuration helper.
        /// </summary>
        public string uploaderConfigurationKey { get; set; }
    }
}
