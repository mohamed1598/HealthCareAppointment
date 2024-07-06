using Patient.Domain.ValueObjects;
using Shared.Primitives;

namespace Patient.Domain.Entities
{
    public record ContactDetails
    {
        internal ContactDetails(PhoneNumber phoneNumber, Email email, Address address)
        {
            PhoneNumber = phoneNumber;
            Email = email;
            Address = address;
        }

        public PhoneNumber PhoneNumber { get; set; }
        public Email Email { get; set; }
        public Address Address { get; set; }
        
        public static ContactDetails Create(PhoneNumber phoneNumber, Email email, Address address)
        {
            return new ContactDetails(phoneNumber, email, address);
        }

    }
}