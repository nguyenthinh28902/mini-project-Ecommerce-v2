using System;
using System.Collections.Generic;

namespace Ecommerce.Infrastructure.Entities;

public partial class UserProfile
{
    public int UserId { get; set; }

    public string? IdentityDocumentType { get; set; }

    public string LastName { get; set; } = null!;

    public string MiddleName { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string FullName { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string Gender { get; set; } = null!;

    public string NationalityCode { get; set; } = null!;

    public string? CitizenId { get; set; }

    public DateOnly? CitizenIdIssuedDate { get; set; }

    public string? CitizenIdIssuedPlace { get; set; }

    public string? PlaceOfBirth { get; set; }

    public string? PlaceOfBirthProvince { get; set; }

    public string? PlaceOfBirthWard { get; set; }

    public string? PlaceOfBirthAddress { get; set; }

    public string? HometownProvince { get; set; }

    public string? HometownWard { get; set; }

    public string? HometownAddress { get; set; }

    public string? PermanentProvince { get; set; }

    public string? PermanentWard { get; set; }

    public string? PermanentAddress { get; set; }

    public string? AvatarUrl { get; set; }

    public string VerificationStatus { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PhoneNumber { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
