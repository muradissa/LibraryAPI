using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LibraryAPI.Models
{
    public class Client
    {
        [Key]
        public int ClientID { get; set; }
        
        [Column(TypeName ="nvarchar(30)")]
        public string Name { get; set; }
        
        [Column(TypeName = "nvarchar(30)")]
        public string Email { get; set; }
        
        [Column(TypeName ="nvarchar(30)")]
        public string Phone { get; set; }
        
        [Column(TypeName ="nvarchar(30)")]
        public string Address { get; set; }

    }
}
