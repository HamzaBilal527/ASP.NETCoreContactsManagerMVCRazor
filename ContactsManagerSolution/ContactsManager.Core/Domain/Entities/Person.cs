using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Entities
{
	public class Person
	{
        [Key]
		public Guid PersonId { get; set; }
        [StringLength(40)]
        public string? PersonName { get; set; }
        [StringLength(100)]
        public string? Email { get; set; }
        public DateTime? DateOfBirth { get; set; }
        [StringLength(40)]
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        [StringLength(400)]
        public string? Address { get; set; }
        public bool RevieveNewsLetter { get; set; }
        public string? TIN { get; set; }

        [ForeignKey("CountryId")]
        public virtual Country? Country { get; set; }

        public override string ToString()
        {
            return $"PersonId: {PersonId}, PersonName: {PersonName}, Email: {Email}," +
                $"Date of Birth: {DateOfBirth?.ToString("MM/dd/yyyy")}, Gender: {Gender}," +
                $"Country Id: {CountryId}, Country: {Country?.CountryName}, Address: {Address}," +
                $"Recieve News Letters: {RevieveNewsLetter}";
        }

    }
}

