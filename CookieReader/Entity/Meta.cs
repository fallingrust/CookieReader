using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace CookieReader.Entity
{
    [Keyless]
    [Table("meta")]
    public class Meta
    {

        [Column("key")]
        public string Key { get; set; } = "";

        [Column("value")]
        public string Value { get; set; } = "";
    }
}
