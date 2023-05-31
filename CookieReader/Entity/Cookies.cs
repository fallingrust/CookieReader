using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieReader.Entity
{
    [Keyless]
    [Table("cookies")]
    public class Cookies
    {
        [Column("creation_utc")]
        public long CreationUTC { get; set; }

        [Column("host_key")]
        public string HostKey { get; set; } = "";

        [Column("top_frame_site_key")]
        public string TopFrameSiteKey { get; set; } = "";

        [Column("name")]
        public string Name { get; set; } = "";

        [Column("value")]
        public string Value { get; set; } = "";

        [Column("encrypted_value")]
        public byte[] EncryptedValue { get; set; } = Array.Empty<byte>();

        [Column("path")]
        public string Path { get; set; } = "";

        [Column("expires_utc")]
        public long ExpiresUTC { get; set; }

        [Column("is_secure")]
        public long IsSecure { get; set; }

        [Column("is_httponly")]
        public long IsHttpOnly { get; set; }

        [Column("last_access_utc")]
        public long LastAccessUTC { get; set; }

        [Column("has_expires")]
        public long HasExpires { get; set; }

        [Column("is_persistent")]
        public long IsPersistent { get; set; }

        [Column("priority")]
        public long Priority { get; set; }

        [Column("samesite")]
        public long SameSite { get; set; }

        [Column("source_scheme")]
        public long SourceScheme { get; set; }

        [Column("source_port")]
        public long SourcePort { get; set; }

        [Column("is_same_party")]
        public long IsSameParty { get; set; }

        [Column("last_update_utc")]
        public long LastUpdateUTC { get; set; }

        [Column("is_edgelegacycookie")]
        public long IsedgelegacyCookie { get; set; }

        [Column("browser_provenance")]
        public long BrowserProvenance { get; set; }
    }
}
