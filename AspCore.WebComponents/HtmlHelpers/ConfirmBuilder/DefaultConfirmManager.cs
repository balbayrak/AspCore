namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public class DefaultConfirmManager : BaseConfirmManager, IConfirmService
    {
        public override ConfirmType baseConfirmType => ConfirmType.Default;
    }
}

