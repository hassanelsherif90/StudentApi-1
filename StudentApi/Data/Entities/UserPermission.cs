﻿namespace StudentApi.Data.Entities;

public class UserPermission
{
    public int UserId { get; set; }

    public Permission PermissionId { get; set; }
}
