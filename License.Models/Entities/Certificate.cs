using License.Models.Enums;

namespace License.Models.Entities
{
    public class Certificate : BaseEntity
    {
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
        public string? MachineCode { get; set; }
        public ProductType ProductType { get; set; }
        public CertificateType CertificateType { get; set; }
        public DateTime? RegistrationDate { get; set; }
        public DateTime? ActivationDate { get; set; }
        public bool Active { get; set; }

        public Certificate Clone() { return (Certificate)this.MemberwiseClone(); }
    }
}
