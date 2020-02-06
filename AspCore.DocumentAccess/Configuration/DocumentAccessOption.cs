namespace AspCore.DocumentAccess.Configuration
{
    public class DocumentAccessOption
    {
        /// <summary>
        /// uploader,viewer,signer için gidilecek api configuration key.
        /// </summary>
        public string apiKey { get; set; }

        /// <summary>
        /// uploader,viewer,signer için gidilecek api controller name.
        /// </summary>
        public string apiControllerRoute { get; set; }
    }
}
