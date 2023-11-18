using System;
using System.Collections.Generic;

namespace amazonCloneWebAPI.Models;

public partial class User
{
    public int UsrUserId { get; set; }

    public string UsrLastName { get; set; } = null!;

    public string UsrFirstName { get; set; } = null!;

    public string UsrUserName { get; set; } = null!;

    public string UsrPassword { get; set; } = null!;

    public string? UsrGender { get; set; }

    public DateTime? UsrDateOfBirth { get; set; }

    public int? UsrAge { get; set; }

    public string? UsrEmail { get; set; }

    public string? UsrPhoneNumber { get; set; }

    public string? UsrCountry { get; set; }

    public int? UsrRoleId { get; set; }

    public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();

    public virtual Role? UsrRole { get; set; }
}
