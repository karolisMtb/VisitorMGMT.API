using VisitorMGMT.API.BusinessLogic.Interfaces;
using VisitorMGMT.API.DataAccess.Entities;
using VisitorMGMT.API.DataAccess.Interfaces;
using VisitorMGMT.API.DataAccess.Models;

namespace VisitorMGMT.API.BusinessLogic.Services
{
    public class ProfileService : IProfileService
    {
        private readonly IVisitorService _visitorService;
        private readonly IProfileRepository _profileRepository;
        public ProfileService(IVisitorService visitorService, IProfileRepository profileRepository)
        {
            _profileRepository = profileRepository;
            _visitorService = visitorService;
        }

        public async Task UpdateNameAsync(Visitor currentVisitor, string newName)
        {
            Profile profileToUpdate = await _profileRepository.GetProfileByUserName(currentVisitor.UserName);       
            await _profileRepository.UpdateNameAsync(profileToUpdate, newName);
        }

        public async Task UpdateIdentityNumberAsync(Visitor currentVisitor, int identityNumber)
        {
            Profile profileToUpdate = await _profileRepository.GetProfileByUserName(currentVisitor.UserName);            
            await _profileRepository.UpdateIdentityNumberAsync(profileToUpdate, identityNumber);
        }

        public async Task UpdateAddressAsync(Visitor currentVisitor, AddressDTO addressDTO)
        {
            Profile profileToUpdate = await _profileRepository.GetProfileByUserName(currentVisitor.UserName);
            bool addressIsSet = await CheckIFAddressIsSet(currentVisitor);
            await _profileRepository.UpdateAddressAsync(profileToUpdate, addressDTO, addressIsSet);
        }

        public async Task UpdatePhoneNumberAsync(Visitor currentVisitor, string phoneNumber)
        {
            Profile profileToUpdate = await _profileRepository.GetProfileByUserName(currentVisitor.UserName);
            await _profileRepository.UpdatePhoneNumberAsync(profileToUpdate, phoneNumber);
        }

        private async Task<bool> CheckIFAddressIsSet(Visitor currentVisitor)
        {
            Visitor visitor = await _visitorService.GetVisitorByUserNameAsync(currentVisitor.UserName);
            return await _profileRepository.CheckIfAddressExists(visitor);
        }
    }
}
