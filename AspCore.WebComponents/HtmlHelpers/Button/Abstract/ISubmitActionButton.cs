namespace AspCore.WebComponents.HtmlHelpers.Button.Abstract
{
    public interface ISubmitActionButton : IActionButton<ISubmitActionButton>
    {
        ISubmitActionButton SubmitForm(string formid);
        ISubmitActionButton CloseParentModal(bool closeParentModal);
    }
}
