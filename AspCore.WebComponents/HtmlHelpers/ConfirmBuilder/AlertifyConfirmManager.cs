namespace AspCore.WebComponents.HtmlHelpers.ConfirmBuilder
{
    public class AlertifyConfirmManager : BaseConfirmManager, IConfirmService
    {
        public override ConfirmType baseConfirmType => ConfirmType.Alertify;
    }
}
