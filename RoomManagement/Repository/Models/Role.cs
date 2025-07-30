using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace RoomManagement.Repository.Models;

[Table("Role")]
public partial class Role
{
    [Key]
    public int Id { get; set; }

    [Column("Role")]
    [StringLength(30)]
    [Unicode(false)]
    public string? role{ get; set; }

    //[InverseProperty("Role")]
    public virtual ICollection<User> Users { get; set; } = new List<User>();
    public Role()
    {
        Users = new List<User>();
    }
    public Role(String rol) {
     role = rol;
     Users = new List<User>();
    }
   
}
