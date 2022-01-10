using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.IO;

namespace Foundation
{
    public static class Wiring
    {
        /// <Summary>
        /// wireTo is an extension method on the type object
        /// Important method that wires and connects instances of classes that have ports by matching interfaces (with optional port names).
        /// If object A (this) has a private field of an interface, and object B implements the interface, then wire them together using reflection.
        /// Returns this for fluent style programming.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A">first object being wired</param>
        /// <param name="B">second object being wired</param>
        /// <param name="APortName">port fieldname in the A object (optional)</param>
        /// <returns>A</returns>
        /// ------------------------------------------------------------------------------------------------------------------
        /// WireTo method understanding what it does:
        /// <param name="A">
        /// The object on which the method is called is the object being wired from. It must have a private field of the interface type.
        /// </param> 
        /// <param name="B">The object being wired to. It must implement the interface)</param> 
        /// <returns>this to support fluent programming style which allows multiple wiring to the same A object with .WireTo operators</returns>
        /// <remarks>
        /// 1. only wires compatible interfaces, A uses the interface and B implements the interface
        /// 2. interface field must be private (all publics are for use by the higher layer. This prevents confusion in the higher layer when when creating an instance of an abstraction - the ports should not be visible) 
        /// 3. can only wire a single matching interface per call (wires the first one it finds in class A that is not yet assigned)
        /// 4. skips ports in class A that are already wired
        /// 5. you can overide the above order, or specify the port more explicitly, by specifying the port field name in wireTo method
        /// 6. looks for list as well (be careful of a list of interface blocking other fields of the same interfaces type lower down from ever being wired)
        public static T WireTo<T>(this T A, object B, string APortName = null)
        {
            // achieve the following via reflection
            // A.field = B; 
            // if 1) field is private, 2) field's type matches one of the implemented interfaces of B, and 3) field is not yet assigned

            if (A == null)
            {
                throw new ArgumentException("A is null ");
            }
            if (B == null)
            {
                throw new ArgumentException("B is null ");
            }



            bool wired = false;
            var BType = B.GetType();
            var AfieldInfos = A.GetType().GetFields(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance) // do the reflection once
                .Where(f => (APortName == null || f.Name == APortName)).Where(f => f.GetValue(A) == null).ToList(); // filter to for given portname (if any) and not yet assigned 
            var BinterfaceTypes = BType.GetInterfaces(); // do the reflection once

            foreach (var AfieldInfo in AfieldInfos)
            {
                var BimplementedInterface = BinterfaceTypes.FirstOrDefault(interfaceType => AfieldInfo.FieldType == interfaceType);
                if (BimplementedInterface != null)  // there is a matching interface
                {
                    AfieldInfo.SetValue(A, B);  // do the wiring
                    wired = true;
                    break;
                }
            }

            if (!wired) // throw exception
            {
                var AinstanceName = A.GetType().GetProperties().FirstOrDefault(f => f.Name == "InstanceName")?.GetValue(A);
                var BinstanceName = B.GetType().GetProperties().FirstOrDefault(f => f.Name == "InstanceName")?.GetValue(B);

                if (APortName != null)
                {
                    // a specific port was specified - see if the port was already wired
                    var AfieldInfo = AfieldInfos.FirstOrDefault();
                    if (AfieldInfo?.GetValue(A) != null) throw new Exception($"Port already wired {A.GetType().Name}[{AinstanceName}].{APortName} to {BType.Name}[{BinstanceName}]");
                }
                throw new Exception($"Failed to wire {A.GetType().Name}[{AinstanceName}].\"{APortName}\" to {BType.Name}[{BinstanceName}]");
            }
            return A;
        }

        /// <summary>
        /// Same as WireTo except that it return the second object to support composing a chain of instances of abstractions without nested syntax
        /// e.g. new A().WireIn(new B()).WireIn(new C());
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="A">first object being wired</param>
        /// <param name="B">second object being wired</param>
        /// <param name="APortName">port fieldname in the A object (optional)</param>
        /// <returns>B to support fluent programming style which allows wiring a chain of objects within .WireIn operators</returns>
        public static object WireIn<T>(this T A, object B, string APortName = null)
        {
            WireTo(A, B, APortName);
            return B;
        }
    }
}
