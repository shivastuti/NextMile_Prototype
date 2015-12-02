using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NextMile02.Models
{
    public class CurrentUser : ICurrentUser
    {
        public virtual string UserId()
        {
            return (String)HttpContext.Current.Session["uid"];
        }
        //public virtual string UserId()
        //{
        //    return (String)HttpContext.Session["uid"];
        //    //return state.ToString();
        //    //return (String)state["uid"];
        //    //return (String)Session["uid"];
        //}
        //public virtual string UserId(IQueryable state)
        //{
        //    return state.ToString();
        //    //return (String)state["uid"];
        //    //return (String)Session["uid"];
        //}
    }
}