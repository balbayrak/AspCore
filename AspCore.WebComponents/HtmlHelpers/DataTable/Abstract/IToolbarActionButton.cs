using AspCore.WebComponents.HtmlHelpers.Button.Abstract;
using AspCore.WebComponents.HtmlHelpers.General.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspCore.WebComponents.HtmlHelpers.DataTable.Abstract
{
    public interface IToolbarActionButton<T>
        where T : IActionButton<T>
    {
        T FormSide(EnumFormSide formSide);
    }
}
