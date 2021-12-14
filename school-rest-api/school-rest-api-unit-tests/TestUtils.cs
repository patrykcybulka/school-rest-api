using Microsoft.VisualStudio.TestTools.UnitTesting;
using school_rest_api.Enums;
using school_rest_api.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace school_rest_api_unit_tests
{
    public sealed class TestUtils
    {
        public static void CheckError(Func<Task> Action, EErrorCode errorCode)
        {
            try
            {
                var data = Action.Invoke();

                if (data.Exception != null)
                {
                    throw data.Exception.InnerException;
                }

                Assert.Fail();
            }
            catch (SchoolException exception)
            {
                Assert.AreEqual(errorCode, exception.ErrorCode);
            }
        }
    }
}
