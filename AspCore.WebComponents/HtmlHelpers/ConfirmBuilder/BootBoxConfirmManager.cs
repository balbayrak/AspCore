namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public class BootBoxConfirmManager : BaseConfirmManager, IConfirmService
    {
        public override ConfirmType baseConfirmType => ConfirmType.BootBox;
    }
}
