using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    [MetadataType(typeof(PreferenceData))]
    public partial class Preference
    {
        /// <summary>
        /// CREATE TABLE [dbo].[UserProfileTest1] (
        /// [Id]         INT           NOT NULL,
        /// [userid]     NVARCHAR (50) NOT NULL,
        /// [truckname]  NVARCHAR (50) NOT NULL,
        /// [preference] INT           NULL,
        /// PRIMARY KEY CLUSTERED ([Id] ASC)
        /// </summary>
        public class PreferenceData
        {
            [ScaffoldColumn(false)]
            public int Id { get; set; }
            [Required()]
            public string userid { get; set; }
            [Required()]
            public string truckname { get; set; }
            public int? preference { get; set; }
        }
    }
}