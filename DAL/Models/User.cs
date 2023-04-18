using System.ComponentModel.DataAnnotations;

namespace Backend.DAL.Models;

public class User
{
	[Required]
	public int Id { get; set; } = 0;
}