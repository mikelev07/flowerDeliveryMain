using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FlowersDelivery.Models
{
    public class AdminViewModel
    {
        public class RoleViewModel
        {
            public string Id { get; set; }
            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Имя роли")]
            public string Name { get; set; }
        }

        public class EditUserViewModel
        {
            public string Id { get; set; }

            [Required(AllowEmptyStrings = false)]
            [Display(Name = "Почта")]
            [EmailAddress]
            public string Email { get; set; }

            public IEnumerable<SelectListItem> RolesList { get; set; }
        }
    }
}