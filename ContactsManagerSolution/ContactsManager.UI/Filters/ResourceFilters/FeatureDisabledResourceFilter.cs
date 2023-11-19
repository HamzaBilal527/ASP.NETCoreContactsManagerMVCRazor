﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDDemo.Filters.ResourceFilters
{
    public class FeatureDisabledResourceFilter : IAsyncResourceFilter
    {
        private readonly ILogger<FeatureDisabledResourceFilter> _logger;
        private readonly bool _isDiabled;

        public FeatureDisabledResourceFilter(ILogger<FeatureDisabledResourceFilter> logger, bool isDisabled = true)
        {
            _logger = logger;
            _isDiabled = isDisabled;
        }
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));
            if (_isDiabled)
            {
                //context.Result = new NotFoundResult();

                context.Result = new StatusCodeResult(501);
            }

            else
            {
                await next();

            }
            _logger.LogInformation("{FilterName}.{MethodName} - after", nameof(FeatureDisabledResourceFilter), nameof(OnResourceExecutionAsync));

        }
    }
}
