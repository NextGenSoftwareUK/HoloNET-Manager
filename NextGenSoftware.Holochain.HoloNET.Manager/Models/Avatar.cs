using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using NextGenSoftware.Holochain.HoloNET.Client;
using NextGenSoftware.Holochain.HoloNET.Client.Interfaces;
using NextGenSoftware.Holochain.HoloNET.ORM.Entries;
using NextGenSoftware.Holochain.HoloNET.Manager.Interfaces;

namespace NextGenSoftware.Holochain.HoloNET.Manager.Models
{
    /// <summary>
    /// This example creates its own internal instance of the HoloNETClient, you should only use this if you will be extending only one HoloNETAuditEntryBaseClass/HoloNETEntryBaseClass otherwise use the AvatarShared Example.
    /// </summary>
    public class Avatar : HoloNETAuditEntryBase, IAvatar
    {
        // Example of how you can disable the 3 different audit options (you can of course only disable one of two of them).
        //public Avatar() : base("oasis", "get_entry_avatar", "create_entry_avatar", "update_entry_avatar", "delete_entry_avatar", false, false, false) { }
        //public Avatar(HoloNETConfig holoNETConfig) : base("oasis", "get_entry_avatar", "create_entry_avatar", "update_entry_avatar", "delete_entry_avatar", holoNETConfig, false, false, false) { }
        // Were get_avatar / create_avatar / update_avatar / delete_avatar: the Holochain 0.7 OASIS hApp has no get_avatar and its
        // update_avatar takes a different input, so the demo could neither load nor update. The *_entry_avatar functions
        // take exactly what HoloNETEntryBase sends.
        //public Avatar() : base("oasis", "get_avatar", "create_avatar", "update_avatar", "delete_avatar") { }
        //public Avatar(IHoloNETDNA holoNETDNA) : base("oasis", "get_avatar", "create_avatar", "update_avatar", "delete_avatar", true, holoNETDNA) { }

        // No connection of its own: a HoloNETObservableCollection creates one Avatar per loaded record with this constructor
        // (and the collection popup creates them to add), and each used to open (and auto-start) its own conductor connection.
        public Avatar() : base("oasis", "get_entry_avatar", "create_entry_avatar", "update_entry_avatar", "delete_entry_avatar", false) { }
        public Avatar(IHoloNETDNA holoNETDNA) : base("oasis", "get_entry_avatar", "create_entry_avatar", "update_entry_avatar", "delete_entry_avatar", true, holoNETDNA) { }

        private string _username;

        // The OASIS hApp's avatar entry requires a username (and an id, which HoloNETAuditEntryBase provides); without one every
        // save failed validation. Defaults to the email, or the id when there's no email.
        [HolochainRustFieldName("username")]
        public string Username
        {
            get { return !string.IsNullOrEmpty(_username) ? _username : !string.IsNullOrEmpty(Email) ? Email : Id.ToString(); }
            set { _username = value; }
        }


        [HolochainRustFieldName("first_name")]
        public string FirstName { get; set; }

        [HolochainRustFieldName("last_name")]
        public string LastName { get; set; }

        [HolochainRustFieldName("email")]
        public string Email { get; set; }

        [HolochainRustFieldName("dob")]
        public DateTime DOB { get; set; }

        public override Task<ZomeFunctionCallBackEventArgs> SaveAsync(Dictionary<string, string> customDataKeyValuePair = null, Dictionary<string, bool> holochainFieldsIsEnabledKeyValuePair = null, bool cachePropertyInfos = true, bool useReflectionToMapKeyValuePairResponseOntoEntryDataObject = true)
        {
            //Example of how to disable various holochain fields/ properties so the data is omitted from the data sent to the zome function.
            //if (holochainFieldsIsEnabledKeyValuePair == null)
            //    holochainFieldsIsEnabledKeyValuePair = new Dictionary<string, bool>();

            //holochainFieldsIsEnabledKeyValuePair["DOB"] = false;
            //holochainFieldsIsEnabledKeyValuePair["Email"] = false;

            ////Below is an example of how you can send custom data to the zome function:
            //if (customDataKeyValuePair == null)
            //    customDataKeyValuePair = new Dictionary<string, string>();

            //customDataKeyValuePair["dynamic data"] = "dynamic";
            //customDataKeyValuePair["some other data"] = "data";

            return base.SaveAsync(customDataKeyValuePair, holochainFieldsIsEnabledKeyValuePair, cachePropertyInfos, useReflectionToMapKeyValuePairResponseOntoEntryDataObject);
        }

        public override ZomeFunctionCallBackEventArgs Save(Dictionary<string, string> customDataKeyValuePair = null, Dictionary<string, bool> holochainFieldsIsEnabledKeyValuePair = null, bool cachePropertyInfos = true, bool useReflectionToMapKeyValuePairResponseOntoEntryDataObject = true)
        {
            //Example of how to disable various holochain fields/properties so the data is omitted from the data sent to the zome function.
            //if (holochainFieldsIsEnabledKeyValuePair == null)
            //    holochainFieldsIsEnabledKeyValuePair = new Dictionary<string, bool>();

            //holochainFieldsIsEnabledKeyValuePair["DOB"] = false;
            //holochainFieldsIsEnabledKeyValuePair["Email"] = false;

            //Below is an example of how you can send custom data to the zome function:
            //if (customDataKeyValuePair == null)
            //    customDataKeyValuePair = new Dictionary<string, string>();

            //customDataKeyValuePair["dynamic data"] = "dynamic";
            //customDataKeyValuePair["some other data"] = "data";

            return base.Save(customDataKeyValuePair, holochainFieldsIsEnabledKeyValuePair, cachePropertyInfos, useReflectionToMapKeyValuePairResponseOntoEntryDataObject);
        }
    }
}