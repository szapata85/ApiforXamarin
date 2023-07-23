using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DB.Data.Model
{
    public class Phone
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string PhoneNumber { get; set; }

        [ForeignKey("Id")]
        public virtual Client Client { get; set; }
    }
}
