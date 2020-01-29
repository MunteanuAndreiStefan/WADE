using AspectInjector.Broker;
using System;
using System.Collections.Generic;
using System.Text;

namespace FakeNewsDetectionCache.Aspects
{
    [Aspect(Scope.PerInstance)]
    [Injection(typeof(Monitor))]
    public class Monitor:Attribute
    {


        [Advice(Kind.After, Targets = Target.Public | Target.Setter)]
        public void AfterSetter(
            [Argument(Source.Instance)] object source,
            [Argument(Source.Name)] string propName,
            [Argument(Source.Injections)] Attribute[] injections
            )
        {
            var val=source.GetType().GetProperty(propName).GetValue(source);
            if(propName=="CredibilityScore")
            {
                if ((int)val < 0)
                    throw new Exception("CredibilityScore cannot be a negative number");
            }

            if (propName == "Id")
            {
                if ((int)val <= 0)
                    throw new Exception("Id must be greater than 0");
            }

            if (propName == "Username")
            {
                if((string)val=="")
                    throw new Exception("Username cannot be null");
            }

        }
    }
}
