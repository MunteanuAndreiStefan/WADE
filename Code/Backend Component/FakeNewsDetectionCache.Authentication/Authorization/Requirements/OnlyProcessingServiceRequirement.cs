using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsDetectionCache.Authentication.Authorization.Requirements
{
    public class OnlyProcessingServiceRequirement: IAuthorizationRequirement
    {
    }
}
