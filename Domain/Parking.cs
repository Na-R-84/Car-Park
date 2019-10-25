using System;

namespace CarPark.Domain.Models
{
    class Parking
    {
        public string FirstName { get; }
        public string LastName { get; }
        public string SocialSecurityNumber { get; }
        public string PhoneNumber { get; }
        public string Email { get; }
        public string RegistrationNumber { get; }
        public string Notes { get; }
        public DateTime ArrivedAt { get; }
        public DateTime? DepartedAt { get; }

        public Parking(string firstName, string lastName, string socialSecurityNumber, string phoneNumber, string email, string registrationNumber, string notes, DateTime arrivedAt)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = socialSecurityNumber;
            PhoneNumber = phoneNumber;
            Email = email;
            RegistrationNumber = registrationNumber;
            Notes = notes;
            ArrivedAt = arrivedAt;
        }

        public Parking(string firstName, string lastName, string socialSecurityNumber, string phoneNumber, string email, string registrationNumber, string notes, DateTime arrivedAt, DateTime? departedAt)
        {
            FirstName = firstName;
            LastName = lastName;
            SocialSecurityNumber = socialSecurityNumber;
            PhoneNumber = phoneNumber;
            Email = email;
            RegistrationNumber = registrationNumber;
            Notes = notes;
            ArrivedAt = arrivedAt;
            DepartedAt = departedAt;
        }
    }
}