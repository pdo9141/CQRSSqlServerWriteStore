using System;
using AuctionAPI.Application;
using AuctionAPI.Dtos.User;
using AuctionDomainUser.Commands;
using AuctionDomainUser.Queries;

namespace AuctionAPI.Services {
    public class UserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserService()
        {
            _userRepository = new UserRepository();
        }

        #region Command Stack

        internal void RegisterUser(RegisterUserDto dto)
        {
            var registerUser = new RegisterUserCommand(Guid.NewGuid(), dto.FirstName, dto.LastName, dto.PhoneNumber, dto.EmailAddress, dto.Password);
            Dispatcher.Instance.Dispatch(registerUser);
        }

        internal void ChangePassword(ChangePasswordDto dto) {
            var changePasswordCommand = new ChangePasswordCommand(dto.UserID, dto.NewPassword);
            Dispatcher.Instance.Dispatch(changePasswordCommand);
        }

        #endregion

        #region Query Stack

        internal UserQueryModel GetUser(Guid id)
        {
            return _userRepository.GetUser(id);
        }
        
        #endregion
    }
}