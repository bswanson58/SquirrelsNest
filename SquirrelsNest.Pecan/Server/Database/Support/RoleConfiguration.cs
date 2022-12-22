using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SquirrelsNest.Pecan.Shared.Constants;

namespace SquirrelsNest.Pecan.Server.Database.Support {
    public class RoleConfiguration : IEntityTypeConfiguration<IdentityRole> {
        public void Configure( EntityTypeBuilder<IdentityRole> builder ) {
            builder.HasData(
                new IdentityRole {
                    Name = ClaimValues.ClaimRoleUser,
                    NormalizedName = ClaimValues.ClaimRoleUser.ToUpper()
                },
                new IdentityRole {
                    Name = ClaimValues.ClaimRoleAdmin,
                    NormalizedName = ClaimValues.ClaimRoleAdmin.ToUpper()
                }
            );
        }
    }
}
