using JF.Common;
using JF.Domain.Command;
using JF.Identity.Core.Application.Command;
using Moq;

namespace TestHelper
{
    public static class MockService
    {
        private static Mock<ICommandHandler<SignUpCommand, XError>> _mockSignUpCommandHandler;
        public static Mock<ICommandHandler<SignUpCommand,XError>> MockSignUpCommandHandler
        {
            get
            {
                if(_mockSignUpCommandHandler == null)
                {
                    var mockSignUpCommandHandler = new Mock<ICommandHandler<SignUpCommand, XError>>();

                    _mockSignUpCommandHandler = mockSignUpCommandHandler;
                }
                return _mockSignUpCommandHandler;
            }
        }
    }
}
