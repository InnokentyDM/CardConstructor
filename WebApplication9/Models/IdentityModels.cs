using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication9.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {

        public bool ConfirmedEmail { get; set; }

    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
		public WebApplication9.Controllers.applicationsController m_applicationsController;
		public WebApplication9.Controllers.COUNTsController m_COUNTsController;
		public WebApplication9.Controllers.addCardsController m_addCardsController;
		public WebApplication9.Controllers.layoutsController m_layoutsController;
		public WebApplication9.Controllers.ITEM_TYPEController m_ITEM_TYPEController;
		public WebApplication9.Controllers.AspNetRolesController m_AspNetRolesController;
		public WebApplication9.Controllers.AspNetUsersController m_AspNetUsersController;

        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }
    }
}