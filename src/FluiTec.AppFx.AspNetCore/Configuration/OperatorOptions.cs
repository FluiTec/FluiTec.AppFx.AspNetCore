using FluiTec.AppFx.Options;

namespace FluiTec.AppFx.AspNetCore.Configuration
{
    /// <summary>An operator options.</summary>
    [ConfigurationName("Operator")]
    public class OperatorOptions
    {
        /// <summary>Gets or sets the enterprise.</summary>
        /// <value>The enterprise.</value>
        public string Enterprise { get; set; }

        /// <summary>Gets or sets the street.</summary>
        /// <value>The street.</value>
        public string Street { get; set; }

        /// <summary>Gets or sets the zip code.</summary>
        /// <value>The zip code.</value>
        public string ZipCode { get; set; }

        /// <summary>Gets or sets the city.</summary>
        /// <value>The city.</value>
        public string City { get; set; }

        /// <summary>Gets or sets the phone.</summary>
        /// <value>The phone.</value>
        public string Phone { get; set; }

        /// <summary>Gets or sets the fax.</summary>
        /// <value>The fax.</value>
        public string Fax { get; set; }

        /// <summary>Gets or sets the homepage display.</summary>
        /// <value>The homepage display.</value>
        public string HomepageDisplay { get; set; }

        /// <summary>Gets or sets the homepage.</summary>
        /// <value>The homepage.</value>
        public string Homepage { get; set; }

        /// <summary>Gets or sets the mail address display.</summary>
        /// <value>The mail address display.</value>
        public string MailAddressDisplay { get; set; }

        /// <summary>Gets or sets the mail address.</summary>
        /// <value>The mail address.</value>
        public string MailAddress { get; set; }

        /// <summary>Gets or sets the privacy mail address display.</summary>
        /// <value>The privacy mail address display.</value>
        public string PrivacyMailAddressDisplay { get; set; }

        /// <summary>Gets or sets the privacy mail address.</summary>
        /// <value>The privacy mail address.</value>
        public string PrivacyMailAddress { get; set; }

        /// <summary>Gets or sets the commercial register.</summary>
        /// <value>The commercial register.</value>
        public string CommercialRegister { get; set; }

        /// <summary>Gets or sets the tax identifier number.</summary>
        /// <value>The tax identifier number.</value>
        public string TaxIdNumber { get; set; }

        /// <summary>Gets or sets the tax number.</summary>
        /// <value>The tax number.</value>
        public string TaxNumber { get; set; }

        /// <summary>Gets or sets the representatives.</summary>
        /// <value>The representatives.</value>
        public string Representatives { get; set; }

        /// <summary>Gets or sets a value indicating whether this object has complementary.</summary>
        /// <value>True if this object has complementary, false if not.</value>
        public bool HasComplementary { get; set; }

        /// <summary>Gets or sets the name of the complementary.</summary>
        /// <value>The name of the complementary.</value>
        public string ComplementaryName { get; set; }

        /// <summary>Gets or sets the complementary representatives.</summary>
        /// <value>The complementary representatives.</value>
        public string ComplementaryRepresentatives { get; set; }

        /// <summary>Gets or sets the complementary commercial register.</summary>
        /// <value>The complementary commercial register.</value>
        public string ComplementaryCommercialRegister { get; set; }
    }
}