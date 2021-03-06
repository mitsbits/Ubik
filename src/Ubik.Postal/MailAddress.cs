﻿namespace Ubik.Postal
{
    public class MailAddress
    {
        private readonly string _email;
        private readonly string _displayName;

        public MailAddress(string email)
        {
            if (!email.ValidEmail()) throw new EmailFormatException(email);
            _email = email;
        }

        public MailAddress(string email, string displayName):this(email)
        {
            _displayName = displayName;
        }

        public string Email { get { return _email; } }
        public string DisplayName { get { return _displayName; } }
    }
}