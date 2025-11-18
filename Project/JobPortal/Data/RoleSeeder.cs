using Microsoft.AspNetCore.Identity;

namespace JobPortal.Data
{
	public static class RoleSeeder
	{
		public static async Task SeedAsync(RoleManager<IdentityRole> rm)
		{
			if (!await rm.RoleExistsAsync("Candidate"))
				await rm.CreateAsync(new IdentityRole("Candidate"));

			if (!await rm.RoleExistsAsync("Employer"))
				await rm.CreateAsync(new IdentityRole("Employer"));
		}
	}
}
