using System;
using System.Collections.Generic;

namespace amazonCloneWebAPI.Models;

public partial class Role
{
    public int RoleRoleId { get; set; }

    public string RoleRoleName { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
