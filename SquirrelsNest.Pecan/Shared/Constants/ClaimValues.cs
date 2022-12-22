namespace SquirrelsNest.Pecan.Shared.Constants {
    public static class ClaimValues {
        public static readonly string   ClaimEmail      = "email";
        public static readonly string   ClaimEntityId   = "entityId";
        public static readonly string   ClaimName       = "name";

//        public static readonly string   ClaimRole       = "role";
        public static readonly string   ClaimRoleAdmin  = cAdministrator;
        public static readonly string   ClaimRoleUser   = cUser;

        public const string             cAdministrator  = "administrator";
        public const string             cUser           = "user";
    }
}
