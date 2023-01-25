using Xunit;

namespace VendingMachine.UnitTests.Helpers
{
    public class AssertException
    {
        public static void Throws<T>(Action action, string expectedMessage) where T : Exception
        {
            try
            {
                action.Invoke();
            }
            catch (T exc)
            {
                Assert.Equal(expectedMessage, exc.Message);

                return;
            }

            Assert.True(false, "Exception of type " + typeof(T) + " should be thrown.");
        }
    }
}
