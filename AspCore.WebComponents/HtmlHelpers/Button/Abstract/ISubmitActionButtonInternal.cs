namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface ISubmitActionButtonInternal : IActionButtonInternal
    {
        string submitformid { get; set; }

        bool closeParentModal { get; set; }
    }
}
