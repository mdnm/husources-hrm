using HRM.Model.Entities.Interfaces;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HRM.Model.Entities
{
    [Table("human_resources_employee")]
    public class HumanResourcesEmployee : IUser
    {
        [Column("id")]
        public Guid Id { get; set; }
        [Column("username")]
        public string Username { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }
        [Column("hierarchy_points")]
        public int HierarchyPoints { get; set; }
    }
}
