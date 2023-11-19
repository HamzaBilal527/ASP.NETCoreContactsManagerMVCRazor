using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters
{
    public class SkipFilter : Attribute, IFilterMetadata
    {
    }
}
