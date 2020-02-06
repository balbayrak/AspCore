namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public class SweetConfirmManager : BaseConfirmManager, IConfirmService
    {
        public override ConfirmType baseConfirmType => ConfirmType.Sweet;
    }
}
