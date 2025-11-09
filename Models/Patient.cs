using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace amazonCloneWebAPI.Models
{
    public class Patient
    {
        [Key]
        [HiddenInput(DisplayValue = false)]
        public int PatientID { get; set; }
        [Required]
        public string? PatientName { get; set; }
        [MaxLength(ErrorMessage = "Exceeding the Maximum Length")]
        public string? Description { get; set; }
        public string? Password { get; set; }
        [Compare("Password", ErrorMessage = "Passwords Does not match")]
        public string? ConfirmPassword { get; set; }
        [Phone(ErrorMessage = "Invalid Phone Number")]
        public string? Phone { get; set; }
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string? Email { get; set; }
        [ForeignKey("AppointmentID")]
        public Appointments? AppointmentID { get; set; }
        [ReadOnly(true)]
        public string? Status { get; set; }
        [Range(1, 100)]
        public int Age { get; set; }
        [DisplayFormat(DataFormatString = "{0;c}")]
        public string? BillAmount { get; set; }
        [DataType(DataType.EmailAddress)]
        public string? NOKEmailAddress { get; set; }
    }
}