namespace nuap.Helpers
{
    using Plugin.Settings;
    using Plugin.Settings.Abstractions;

    public static class Settings
    {
        static ISettings AppSettings
        {
            get
            {
                return CrossSettings.Current;
            }
        }

        const string token = "Token";
        const string userType = "UserType";
        const string phone = "Phone";
        const string email = "Email";
        static readonly string stringDefault = string.Empty;

        public static string Token
        {
            get
            {
                return AppSettings.GetValueOrDefault(token, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(token, value);
            }
        }

        public static string UserType
        {
            get
            {
                return AppSettings.GetValueOrDefault(userType, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(userType, value);
            }
        }

        public static string Phone
        {
            get
            {
                return AppSettings.GetValueOrDefault(phone, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(phone, value);
            }
        }

        public static string Email
        {
            get
            {
                return AppSettings.GetValueOrDefault(email, stringDefault);
            }
            set
            {
                AppSettings.AddOrUpdateValue(email, value);
            }
        }
    }
}