using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Security;
using System.Net.Http;
using System.Net;
using GK.Utils;
namespace GoldKeyWebApi
{
    public class ApiSecurityAttribute : AuthorizeAttribute
    {
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            //从http请求的头里面获取身份验证信息，验证是否是请求发起方的ticket
            int exist = actionContext.Request.Headers.Count(t => t.Key == "X-Token");
            if(exist==0)
            {
                HandleUnauthorizedRequest(actionContext);
            }
            else
            {
                string token = actionContext.Request.Headers.GetValues("X-Token").FirstOrDefault();
                if (!string.IsNullOrEmpty(token))
                {
                    //解密用户ticket,并校验用户名密码是否匹配
                    if (ValidateTicket(token))
                    {
                        base.IsAuthorized(actionContext);
                    }
                    else
                    {
                        HandleUnauthorizedRequest(actionContext);
                    }
                }
                //如果取不到身份验证信息，并且不允许匿名访问，则返回未验证401
                else
                {
                    var attributes = actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>().OfType<AllowAnonymousAttribute>();
                    bool isAnonymous = attributes.Any(a => a is AllowAnonymousAttribute);
                    if (isAnonymous) base.OnAuthorization(actionContext);
                    else HandleUnauthorizedRequest(actionContext);
                } 
            }

        }

        //校验用户名密码（正式环境中应该是数据库校验）
        private bool ValidateTicket(string encryptTicket)
        {
            try
            {
                //解密Ticket
                Tool tool = new Tool();
                var strTicket = tool.Decrypt(encryptTicket,"apitoken");
                var index = strTicket.IndexOf("&");
                string struid = strTicket.Substring(0, index);
                string strPwd = strTicket.Substring(index + 1);
                string enpwd = tool.Md5(strPwd);
                int uid = 0;
                int.TryParse(struid, out uid);
                if (uid > 0)
                {
                    GK.Service.UserManager.UserService us = new GK.Service.UserManager.UserService();
                    GK.Model.public_db.sys_user user = us.Find(uid);
                    if(user.userpwd == enpwd)
                    { 
                    return true;
                    }
                    else
                    { return false; }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}