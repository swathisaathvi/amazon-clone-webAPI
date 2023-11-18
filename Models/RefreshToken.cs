using System;
using System.Collections.Generic;

namespace amazonCloneWebAPI.Models;

public partial class RefreshToken
{
    public int TknRfrshTokenId { get; set; }

    public int? TknUserId { get; set; }

    public string? TknTokenId { get; set; }

    public string TknRfrshToken { get; set; } = null!;

    public bool? TknIsActive { get; set; }

    public virtual User? TknUser { get; set; }
}
