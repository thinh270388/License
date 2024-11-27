using License.APIs.Datas;
using License.APIs.Helpers;
using License.APIs.Repositories.Constracts;
using License.Models;
using License.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata;

namespace License.APIs.Repositories.Implementations
{
    public class CertificateRepository : IGenericRepository<Certificate>
    {
        private readonly ApplicationDbContext _context;

        public CertificateRepository(ApplicationDbContext context) 
        { 
            _context = context;
        }

        public async Task<DtoResult<Certificate>> AddAsync(Certificate entity)
        {
            var exist = (await _context.Certificates.FirstOrDefaultAsync(x => x.Email!.ToLower().Equals(entity.Email!.ToLower()) &&
                        x.MachineCode!.Equals(entity.MachineCode))) != null;
            if (exist) return new DtoResult<Certificate>() { Success = false, Message = ConstantHelper.CONFLICT };

            await _context.Certificates.AddAsync(entity);
            await _context.SaveChangesAsync();
            return new DtoResult<Certificate>() { Success = true, Message = $"Add {ConstantHelper.SUCCESS}", Result = entity };
        }

        public async Task<DtoResult<Certificate>> DeleteAsync(Guid id)
        {
            var item = await _context.Certificates.FindAsync(id);
            if (item == null) return new DtoResult<Certificate>() { Success = false, Message = ConstantHelper.NOT_FOUND };

            _context.Certificates.Remove(item);
            await _context.SaveChangesAsync();
            return new DtoResult<Certificate>() { Success = true, Message = $"Delete {ConstantHelper.SUCCESS}" };
        }

        public async Task<DtoResult<Certificate>> GetAllAsync()
        {
            var results = await _context.Certificates.ToListAsync();
            return new DtoResult<Certificate>() { Success = true, Message = $"GetAll {ConstantHelper.SUCCESS}", Results = results };
        }

        public async Task<DtoResult<Certificate>> GetByIdAsync(Guid id)
        {
            var result = await _context.Certificates.FindAsync(id);
            return new DtoResult<Certificate>() { Success = true, Message = $"GetByID {ConstantHelper.SUCCESS}", Result = result };
        }

        public async Task<DtoResult<Certificate>> UpdateAsync(Certificate entity)
        {
            var item = await _context.Certificates.FindAsync(entity.Id);
            if (item == null) return new DtoResult<Certificate>() { Success = false, Message = ConstantHelper.NOT_FOUND };

            item.Name = entity.Name;
            item.Email = entity.Email;
            item.PhoneNumber = entity.PhoneNumber;
            item.MachineCode = entity.MachineCode;
            item.CertificateType = entity.CertificateType;
            item.RegistrationDate = entity.RegistrationDate;
            item.ActivationDate = entity.ActivationDate;
            item.ProductType = entity.ProductType;
            item.Description = entity.Description;

            await _context.SaveChangesAsync();
            return new DtoResult<Certificate>() { Success = true, Message = $"Update {ConstantHelper.SUCCESS}", Result = item };
        }
    }
}
