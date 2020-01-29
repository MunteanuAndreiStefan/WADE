using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsDetectionCache.Authentication.Authorization
{
    public static class Policies
    {
        public const string OnlyExtension = nameof(OnlyExtension);
        public const string OnlyDevelopers = nameof(OnlyDevelopers);
        public const string OnlyProcessingService= nameof(OnlyProcessingService);
    }
}
