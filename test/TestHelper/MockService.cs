using JF.Common;
using JF.Domain.Command;
using JF.Identity.Core.Application.Command;
using Moq;

namespace TestHelper
{
    public static class MockService
    {
        private static Mock<ICommandHandler<SignUpCommand>> _mockSignUpCommandHandler;
        public static Mock<ICommandHandler<SignUpCommand>> MockSignUpCommandHandler
        {
            get
            {
                if(_mockSignUpCommandHandler == null)
                {
                    var mockSignUpCommandHandler = new Mock<ICommandHandler<SignUpCommand>>();

                    _mockSignUpCommandHandler = mockSignUpCommandHandler;
                }
                return _mockSignUpCommandHandler;
            }
        }
    }
}
